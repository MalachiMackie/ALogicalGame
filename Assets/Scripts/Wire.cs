using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public ObservableCollection<FloorFace> Path { get; private set; } = new ObservableCollection<FloorFace>();

    private ObservableCollection<LogicFace> _connectedOutputLogicFaces = new ObservableCollection<LogicFace>();

    private List<LogicFace> _connectedInputLogicFaces = new List<LogicFace>();

    private bool _wireState;

    private HashSet<IHaveOutput> _trueOutputs = new HashSet<IHaveOutput>();

    private GameObject _wireTemplate;

    private void Start()
    {
        _wireTemplate = Resources.Load<GameObject>("Prefabs/Wire");
        gameObject.name = "Wire";
    }

    private void OnConnectedOutputLogicFacesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            var logicFace = e.NewItems[0] as LogicFace;
            logicFace.HaveOutput.OutputUpdated += HaveOutput_OutputUpdated;
            if(logicFace.HaveOutput.Output)
            {
                HaveOutput_OutputUpdated(logicFace.HaveOutput, true);
            }
        }
        if(e.Action == NotifyCollectionChangedAction.Remove)
        {
            var logicFace = e.OldItems[0] as LogicFace;
            logicFace.HaveOutput.OutputUpdated -= HaveOutput_OutputUpdated;
        }
    }

    private void HaveOutput_OutputUpdated(object sender, bool e)
    {
        if (e)
        {
            _trueOutputs.Add(sender as IHaveOutput);
        }
        else if (_trueOutputs.Contains(sender as IHaveOutput))
        {
            _trueOutputs.Remove(sender as IHaveOutput);
        }

        UpdateInputs();
    }

    private void PathChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            if (e.NewItems[0] is LogicFace newLogicFace)
            {
                if (newLogicFace.Mode == LogicConnectorMode.Input)
                {
                    _connectedInputLogicFaces.Add(newLogicFace);
                }
                else
                {
                    _connectedOutputLogicFaces.Add(newLogicFace);
                }
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            if (e.OldItems[0] is LogicFace oldLogicFace)
            {
                if (oldLogicFace.Mode == LogicConnectorMode.Input)
                {
                    _connectedInputLogicFaces.Remove(oldLogicFace);
                    oldLogicFace.HaveInput.SetInput(false, oldLogicFace);
                }
                else
                {
                    _trueOutputs.Remove(oldLogicFace.HaveOutput);
                    _connectedOutputLogicFaces.Remove(oldLogicFace);
                    UpdateInputs();
                }
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            _connectedOutputLogicFaces.Clear();
            _connectedInputLogicFaces.Clear();
            _trueOutputs.Clear();
            UpdateInputs();
        }
    }

    private void UpdateInputs()
    {
        bool output = _trueOutputs.Count != 0;

        if (output != _wireState)
        {
            _wireState = output;
            foreach (LogicFace inputLogicFace in _connectedInputLogicFaces)
            {
                inputLogicFace.HaveInput.SetInput(_wireState, inputLogicFace);
            }
        }
    }

    public void AddFace(FloorFace face)
    {
        if(!Path.Contains(face))
        {
            Path.Add(face);
            face.AddWire(this);
            face.CheckForConnections();
            face.UpdateNeighbours();
            if (face is LogicFace logicFace && logicFace.Mode == LogicConnectorMode.Input && _trueOutputs.Count != 0)
            {
                logicFace.HaveInput.SetInput(true, logicFace);
            }
        }
    }

    public void RemoveFace(FloorFace face)
    {
        if (Path.Contains(face))
        {
            face.RemoveWire();
            Path.Remove(face);
        }
    }

    public void AddWire(Wire wire)
    {
        AddFaces(wire.Path);
        Destroy(wire.gameObject);
    }

    public IEnumerator RemoveWire(IEnumerable<FloorFace> pathToRemove)
    {
        var myFaces = pathToRemove.Where(x => Path.Contains(x));
        foreach(FloorFace face in myFaces)
        {
            RemoveFace(face);
        }
        if(!Path.Any())
        {
            Destroy(gameObject);
        }

        var newPaths = new List<IEnumerable<FloorFace>>();
        while(Path.Any())
        {
            var newPath = Path[0].GetNeighbourWires();
            foreach(FloorFace face in newPath)
            {
                RemoveFace(face);
            }
            newPaths.Add(newPath);
            yield return null;
        }


        foreach(IEnumerable<FloorFace> wirePath in newPaths)
        {
            GameObject newWireObj = Instantiate(_wireTemplate);
            Wire newWire = newWireObj.GetComponent<Wire>();
            newWire.Init(wirePath);
            yield return null;
        }

        Path.CollectionChanged -= PathChanged;
        _connectedOutputLogicFaces.CollectionChanged -= OnConnectedOutputLogicFacesChanged;

        Destroy(gameObject);
    }

    public void Init(IEnumerable<FloorFace> path)
    {
        Path.CollectionChanged += PathChanged;
        _connectedOutputLogicFaces.CollectionChanged += OnConnectedOutputLogicFacesChanged;

        AddFaces(path);
    }

    public void AddFaces(IEnumerable<FloorFace> path)
    {
        for (int i = path.Count() - 1; i >= 0; i--)
        {
            FloorFace face = path.ElementAt(i);
            AddFace(face);
        }
    }

    private IEnumerator IsPathValid(IEnumerable<FloorFace> path)
    {
        var isWireValid = true;

        var wires = new List<Wire>();
        foreach (FloorFace face in path)
        {
            if(face.HasWire)
            {
                if (!wires.Contains(face.ParentWire))
                {
                    wires.Add(face.ParentWire);
                }
                else
                {
                    Debug.LogWarning("Tried to connect wire to itself");
                    isWireValid = false;
                    break;
                }
            }
        }

        foreach (FloorFace face in path)
        {
            foreach (IHaveGridPosition neighbour in face.Neighbours)
            {
                if (neighbour is FloorFace neighbourFace && neighbourFace.HasWire)
                {
                    if (!wires.Contains(neighbourFace.ParentWire))
                    {
                        wires.Add(neighbourFace.ParentWire);
                    }
                }
            }
        }

        var occupants = new Dictionary<IHaveGridPosition, LogicConnectorMode>();
        foreach (FloorFace face in wires.SelectMany(x => x.Path).Concat(path).Distinct())
        {
            if (face is LogicFace logicFace)
            {
                IHaveGridPosition logicOccupant = logicFace.HaveInput as IHaveGridPosition ?? logicFace.HaveOutput as IHaveGridPosition;
                if (!occupants.ContainsKey(logicOccupant))
                {
                    occupants.Add(logicOccupant, logicFace.Mode);
                }
                else if (occupants[logicOccupant] != logicFace.Mode)
                {
                    isWireValid = false;
                    Debug.LogWarning("Tried to connect wire to both output and input of the same object");
                }
            }
        }
        yield return null;

        var neighbourWires = new List<Wire>();
        FloorFace[] neighbours = path.Where(x => !x.HasWire).SelectMany(x => x.Neighbours).OfType<FloorFace>().Distinct().ToArray();

        for (int i = 0; i < neighbours.Length; i++)
        {
            var neighbour = neighbours[i];
            if (neighbour.HasWire)
            {
                if (!neighbourWires.Contains(neighbour.ParentWire))
                {
                    neighbourWires.Add(neighbour.ParentWire);
                }
                else
                {
                    Debug.LogWarning("Tried to connect wire to itself");
                    isWireValid = false;
                    break;
                }
            }

            if (i % 30 == 0)
            {
                yield return null;
            }
        }

        yield return (isWireValid, wires);
    }

    private IEnumerator CheckForWires(IEnumerable<FloorFace> path)
    {

        var isWireValidCoroutine = new CoroutineWithData<(bool, List<Wire>)>(this, IsPathValid(path));
        yield return isWireValidCoroutine.Coroutine;

        bool isWireValid = isWireValidCoroutine.Result.Item1;
        List<Wire> wires = isWireValidCoroutine.Result.Item2;

        if (!isWireValid)
        {
            Destroy(gameObject);
            yield break;
        }

        if (wires.Any())
        {
            for (int i = 0; i < wires.Count; i++)
            {
                Wire wire = wires[i];
                if (i == 0)
                {
                    wire.AddFaces(path);
                }
                else
                {
                    wires.First().AddWire(wire);
                }
                yield return null;
            }
            Destroy(gameObject);
        }
        else
        {
            Init(path);
        }
    }
    
    public static void CreateWire(IEnumerable<FloorFace> path)
    {
        var newWire = Instantiate(Resources.Load<GameObject>("Prefabs/Wire")).GetComponent<Wire>();
        newWire.StartCoroutine(newWire.CheckForWires(path));
    }
}

using Assets.Scripts.Behaviours;
using Assets.Scripts.Helpers;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Wire : MonoBehaviour
    {
        public ObservableCollection<FloorFace> Path { get; private set; } = new ObservableCollection<FloorFace>();

        private ObservableCollection<OutputLogicFace> _connectedOutputLogicFaces = new ObservableCollection<OutputLogicFace>();

        private List<InputLogicFace> _connectedInputLogicFaces = new List<InputLogicFace>();

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
                var logicFace = e.NewItems[0] as OutputLogicFace;
                logicFace.OutputComponent.OutputUpdated += HaveOutput_OutputUpdated;
                if (logicFace.OutputComponent.Output)
                {
                    HaveOutput_OutputUpdated(logicFace.OutputComponent, true);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var logicFace = e.OldItems[0] as OutputLogicFace;
                logicFace.OutputComponent.OutputUpdated -= HaveOutput_OutputUpdated;
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
                if (e.NewItems[0] is InputLogicFace inputLogicFace)
                {
                    _connectedInputLogicFaces.Add(inputLogicFace);
                }
                else if (e.NewItems[0] is OutputLogicFace outputLogicFace)
                {
                    _connectedOutputLogicFaces.Add(outputLogicFace);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems[0] is InputLogicFace inputLogicFace)
                {
                    _connectedInputLogicFaces.Remove(inputLogicFace);
                    inputLogicFace.InputComponent.SetInput(false, inputLogicFace);
                }
                else if (e.OldItems[0] is OutputLogicFace outputLogicFace)
                {
                    _trueOutputs.Remove(outputLogicFace.OutputComponent);
                    _connectedOutputLogicFaces.Remove(outputLogicFace);
                    UpdateInputs();
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
                foreach (InputLogicFace inputLogicFace in _connectedInputLogicFaces)
                {
                    inputLogicFace.InputComponent.SetInput(_wireState, inputLogicFace);
                }
            }
        }

        public void AddFace(FloorFace face)
        {
            if (!Path.Contains(face))
            {
                Path.Add(face);
                face.SetParentWire(this);
                face.CheckForConnections();
                face.UpdateNeighbours();
                if (face is InputLogicFace logicFace && _trueOutputs.Count != 0)
                {
                    logicFace.InputComponent.SetInput(true, logicFace);
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
            IEnumerable<FloorFace> myFaces = pathToRemove.Where(x => Path.Contains(x));
            foreach (FloorFace face in myFaces)
            {
                RemoveFace(face);
            }
            if (!Path.Any())
            {
                Destroy(gameObject);
                yield break;
            }

            var newPaths = new List<IEnumerable<FloorFace>>();
            while (Path.Any())
            {
                IEnumerable<FloorFace> newPath = Path[0].GetNeighbourWires();
                foreach (FloorFace face in newPath)
                {
                    RemoveFace(face);
                }
                newPaths.Add(newPath);
                yield return null;
            }


            foreach (IEnumerable<FloorFace> wirePath in newPaths)
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
            bool isWireValid = true;

            var wires = new List<Wire>();
            foreach (FloorFace face in path)
            {
                if (face.HasWire)
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

            var occupants = new Dictionary<IHaveGridPosition, bool>();
            foreach (FloorFace face in wires.SelectMany(x => x.Path).Concat(path).Distinct())
            {
                (IHaveGridPosition occupant, bool isInput) = (default, default);
                if (face is InputLogicFace inputLogicFace)
                {
                    occupant = inputLogicFace.InputComponent;
                    isInput = true;
                }
                else if (face is OutputLogicFace outputLogicFace)
                {
                    occupant = outputLogicFace.OutputComponent;
                    isInput = false;
                }

                if (occupant != default)
                {
                    if (!occupants.ContainsKey(occupant))
                    {
                        occupants.Add(occupant, isInput);
                    }
                    else if (occupants[occupant] != isInput)
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
                FloorFace neighbour = neighbours[i];
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
            Wire newWire = Instantiate(Resources.Load<GameObject>("Prefabs/Wire")).GetComponent<Wire>();
            newWire.StartCoroutine(newWire.CheckForWires(path));
        }
    }
}
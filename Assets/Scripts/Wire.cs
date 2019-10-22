using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class Wire : MonoBehaviour
{
    private ObservableCollection<FloorFace> _path = new ObservableCollection<FloorFace>();

    private ObservableCollection<LogicFace> _connectedOutputLogicFaces = new ObservableCollection<LogicFace>();

    private List<LogicFace> _connectedInputLogicFaces = new List<LogicFace>();

    private bool _wireState;

    private HashSet<IHaveOutput> _trueOutputs = new HashSet<IHaveOutput>();

    public void Init(IEnumerable<FloorFace> path)
    {
        if(path.All(x => x.HasWire))
        {
            Destroy(gameObject);
            return;
        }

        _path.CollectionChanged += PathChanged;
        _connectedOutputLogicFaces.CollectionChanged += OnConnectedOutputLogicFacesChanged;


        foreach (FloorFace face in path)
        {
            AddFace(face);
        }

        foreach (FloorFace face in path)
        {
            face.CheckForConnections();
            face.UpdateNeighbours();
        }
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
                    _connectedInputLogicFaces.Add(oldLogicFace);
                }
                else
                {
                    _connectedOutputLogicFaces.Add(oldLogicFace);
                }
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            _connectedOutputLogicFaces.Clear();
            _connectedInputLogicFaces.Clear();
        }
    }

    private void AddFace(FloorFace face)
    {
        face.AddWire(this);
        if(!_path.Contains(face))
        {
            _path.Add(face);
        }
        if (face is LogicFace logicFace && logicFace.Mode == LogicConnectorMode.Input && _trueOutputs.Count != 0)
        {
            logicFace.HaveInput.SetInput(true, logicFace);
        }
    }

    public void AddWire(Wire wire)
    {
        if (_path != null)
        {
            foreach (FloorFace face in wire._path)
            {
                AddFace(face);
            }
            Destroy(wire.gameObject);
        }
    }
}

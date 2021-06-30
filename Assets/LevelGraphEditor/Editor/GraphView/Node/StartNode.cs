using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StartNode : BaseNode
{
    public PortSet portSet;

    //TODO: Enter name Code
    //TODO: Enter title style

    public StartNode() { }
    public StartNode(Vector2 _position, LevelEditorWindow _editorWindow, LevelFlowGraphView _graphView, PortSet _portSet)
    {
        title = "Enter";
        SetPosition(new Rect(_position, defaultNodeSize));

        portSet = _portSet;
        Port outputPort = AddOutputPort("Start", Port.Capacity.Single);
        outputPort.name = portSet.localOutGuid;

        RefreshExpandedState();
        RefreshPorts();
    }
    public StartNode(Vector2 _position, LevelEditorWindow _editorWindow, LevelFlowGraphView _graphView)
    {
        title = "Enter";
        SetPosition(new Rect(_position, defaultNodeSize));

        if (portSet == null)
        {
            portSet = new PortSet(nodeGuid, Vector2.zero, "Start");
        }

        Port outputPort = AddOutputPort("Start", Port.Capacity.Single);
        portSet.nodeGuid = nodeGuid;
        outputPort.name = portSet.localOutGuid;
        outputContainer.Add(outputPort);


        RefreshExpandedState();
        RefreshPorts();
    }

}


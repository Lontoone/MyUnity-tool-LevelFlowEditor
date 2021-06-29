using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

public class BaseNode : Node
{
    public static Dictionary<string, BaseNode> nodeMap = new Dictionary<string, BaseNode>();

    protected string nodeGuid;
    protected LevelFlowGraphView graphView;
    protected LevelEditorWindow editorWindow;

    protected Vector2 defaultNodeSize = new Vector2(200, 250);

    public string NodeGuid { get => nodeGuid; set => nodeGuid = value; }

    public BaseNode()
    {
        //StyleSheet styleSheet = Resources.Load<StyleSheet>("NodeStyleSheet");
        //styleSheets.Add(styleSheet);

        nodeGuid = Guid.NewGuid().ToString();  //Guid能產生獨特的id
        nodeMap.Add(nodeGuid, this);
    }

    public void AddOutputPort(string name, Port.Capacity capacity = Port.Capacity.Single)
    {
        Port outputPort = GetPortInstance(Direction.Output, capacity);
        outputPort.portName = name;
        outputContainer.Add(outputPort);
    }

    public void AddInputPort(string name, Port.Capacity capacity = Port.Capacity.Multi)
    {
        Port inputPort = GetPortInstance(Direction.Input, capacity);
        inputPort.portName = name;
        inputContainer.Add(inputPort);
    }

    public Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
    }

    public virtual void LoadValueIntoField()
    {

    }
}

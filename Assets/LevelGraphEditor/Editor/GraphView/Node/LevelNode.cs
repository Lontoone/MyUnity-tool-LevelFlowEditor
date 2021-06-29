using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelNode : BaseNode
{
    public const float SUB_MAP_VIEW_SIZE = 250;
    public List<PortSet> portSets = new List<PortSet>();
    //public static Dictionary<string, Port> portIds = new Dictionary<string, Port>();

    public List<Port> inPorts = new List<Port>();
    public List<Port> outPorts = new List<Port>();
    public List<LevelNodeData> portsData = new List<LevelNodeData>();

    public object scene;
    public Label subGraphView;
    private ObjectField sceneField;


    public LevelNode() { }
    public LevelNode(Vector2 _position, LevelEditorWindow _editorWindow, LevelFlowGraphView _graphView)
    {
        SetPosition(new Rect(_position, defaultNodeSize));

        //nodeGuid = Guid.NewGuid().ToString();  //Guid能產生獨特的id
        //AddOutputPort("Output", Port.Capacity.Single);      

        CreateSubGraphView();
        //Test---------------------------------
        //AddSubGraphViewPort(new Vector2(125, 125));
        //AddSubGraphViewPort(new Vector2(70, 60));
        //Test---------------------------------

        sceneField = new ObjectField
        {
            objectType = typeof(Scene),
            allowSceneObjects = false
        };

        sceneField.RegisterValueChangedCallback(ValueTuple =>
        {
            scene = ValueTuple.newValue;
            title = ValueTuple.newValue.name;
        });
        mainContainer.Add(sceneField);

        //新增節點須refresh
        RefreshExpandedState();
        RefreshPorts();
    }

    private void CreateSubGraphView()
    {
        subGraphView = new Label("Texts Box");
        subGraphView.AddToClassList("subViewBox");
        mainContainer.Add(subGraphView);
    }
    public void SetupSubPortSets()
    {
        //TODO:整理API
        if (portSets != null)
        {
            for (int i = 0; i < portSets.Count; i++)
            {
                Port[] _ports = CreateSubPorts(portSets[i].position, portSets[i].pointObjectName);
                _ports[0].name = portSets[i].localInGuid;
                _ports[1].name = portSets[i].localOutGuid;                
            }
        }
    }
    public Port[] CreateSubPorts(Vector2 _pos, string _title = "port")
    {
        Port _outPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        Port _inPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
        Label textLabel = new Label(_title);
        _inPort.portName = "in";
        _outPort.portName = "out";

        _inPort.Add(_outPort);
        _inPort.Add(textLabel);
        subGraphView.contentContainer.Add(_inPort);

        _inPort.SetPosition(new Rect(_pos.x, _pos.y, 50, 50));

        inPorts.Add(_inPort);
        outPorts.Add(_outPort);
        return new Port[] { _inPort, _outPort };
    }
    public void AddSubGraphViewLevelConnectPort(Vector2 _pos, string _objId, string _title = "port")
    {
        Port[] ports = CreateSubPorts(_pos, _title);

        //Save guid to name
        PortSet _portSet = new PortSet(_objId, nodeGuid, _pos,_title);
        ports[0].name = _portSet.localInGuid;
        ports[1].name = _portSet.localOutGuid;
        portSets.Add(_portSet);
    }

    public void ClearLevelConnectPort()
    {
        //TODO: 整理API
    }
    public override void LoadValueIntoField()
    {
        sceneField.SetValueWithoutNotify((UnityEngine.Object)scene);
    }

    public static LevelNode FindNodeByScene(Scene _targetScene)
    {
        foreach (KeyValuePair<string, BaseNode> _node in BaseNode.nodeMap)
        {
            if (_node.Value is LevelNode &&
               (_node.Value as LevelNode).scene != null)
            {
                if ((_node.Value as LevelNode).sceneField.value.name == _targetScene.name)
                {
                    return (LevelNode)_node.Value;
                }
            }
        }

        return null;
    }
}

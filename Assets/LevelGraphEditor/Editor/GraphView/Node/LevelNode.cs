using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
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

    //public List<Port> inPorts = new List<Port>();
    //public List<Port> outPorts = new List<Port>();

    public object scene;
    public string scenePath;
    public Label subGraphView;
    private ObjectField sceneField;


    public LevelNode() { }
    public LevelNode(Vector2 _position, LevelEditorWindow _editorWindow, LevelFlowGraphView _graphView)
    {
        SetPosition(new Rect(_position, defaultNodeSize));

        CreateSubGraphView();

        sceneField = new ObjectField
        {
            objectType = typeof(Scene),
            allowSceneObjects = false
        };

        sceneField.RegisterValueChangedCallback(ValueTuple =>
        {
            scene = ValueTuple.newValue;
            title = ValueTuple.newValue.name;
            scenePath = AssetDatabase.GetAssetPath(ValueTuple.newValue as SceneAsset);
            //SetScenePath();
        });
        mainContainer.Add(sceneField);

        //新增節點須refresh
        RefreshExpandedState();
        RefreshPorts();
    }

    private void CreateSubGraphView()
    {
        subGraphView = new Label("");
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
                Port[] _ports = CreatePortSet(portSets[i].position, portSets[i].pointObjectName);
                _ports[0].name = portSets[i].localInGuid;
                _ports[1].name = portSets[i].localOutGuid;
            }
        }

        Debug.Log("2. SetupSubPortSets  port count" + portSets.Count);
    }
    private Port[] CreatePortSet(Vector2 _pos, string _title = "port")
    {
        Port _outPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        Port _inPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
        Label textLabel = new Label(_title);
        _inPort.portName = "in";
        _outPort.portName = "out";

        _inPort.Add(_outPort);
        _inPort.Add(textLabel);
        subGraphView.contentContainer.Add(_inPort);

        _inPort.SetPosition(new Rect(_pos.x, _pos.y, 85, 50));

        inPorts.Add(_inPort);
        outPorts.Add(_outPort);
        return new Port[] { _inPort, _outPort };
    }
    public PortSet CreateLevelPort(Vector2 _pos, string _objSetId, string _title = "port")
    {
        //Check if _objId has port 檢查該對應物件是否已紀錄
        PortSet _portSet = portSets.Find(x => x.setGuid == _objSetId);
        Debug.Log("3. CreateLevelPort  port count" + portSets.Count);
        if (_portSet != null)
        {
            Debug.Log("port esisted " + _portSet.setGuid + " tilie " + _title + " pos " + _pos);
            Port[] ports = CreatePortSet(_pos, _title);
            ports[0].name = _portSet.localInGuid;
            ports[1].name = _portSet.localOutGuid;
            _portSet.pointObjectName = _title;
            _portSet.position = _pos;
        }
        else
        {
            //else =>create new 否則創造一組
            Port[] ports = CreatePortSet(_pos, _title);

            //Save guid to name
            _portSet = new PortSet(nodeGuid, _pos, _title);
            ports[0].name = _portSet.localInGuid;
            ports[1].name = _portSet.localOutGuid;
            portSets.Add(_portSet);

            Debug.Log("port created " + _portSet.setGuid);
        }
        return _portSet;
    }

    public void ClearLevelConnectPort()
    {
        //TODO: 整理API
        for (int i = 0; i < inPorts.Count; i++)
        {
            inPorts[i].RemoveFromHierarchy();
        }
        inPorts.Clear();
        outPorts.Clear();
        Debug.Log("ClearLevelConnectPort port count " + portSets.Count);
    }
    public override void LoadValueIntoField()
    {
        sceneField.SetValueWithoutNotify((UnityEngine.Object)(object)AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath));
        scene = SceneManager.GetSceneByPath(scenePath);
        title = Path.GetFileNameWithoutExtension(scenePath);
        SetupSubPortSets();

        //RefreshExpandedState();
        //RefreshPorts();
    }
    public void RemoveNoExistingPortSet(ConnectPoint[] _points)
    {
        if (portSets.Count < 1)
        {
            return;
        }
        for (int i = 0; i < portSets.Count; i++)
        {
            bool _isDiff = true;
            for (int j = 0; j < _points.Length; j++)
            {

                if (portSets[i].setGuid == _points[j].portSetId)
                {
                    _isDiff = false;
                    break;
                }
            }
            //刪除不存在的
            if (_isDiff)
            {
                Debug.Log("移除 " + portSets[i].pointObjectName);
                portSets.Remove(portSets[i]);
                //TODO: port刪除也要刪除連結線
            }
        }
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/LevelFlow")]
public class LevelMapSO: ScriptableObject
{
    //public List<NodeLinkData> nodeLinkDatas = new List<NodeLinkData>();
    //TODO: data...
    //public List<BaseNodeData> baseNodeDatas = new List<BaseNodeData>();
    public List<LevelNodeData> levelNodeDatas = new List<LevelNodeData>();
    public List<PortSet> portSets = new List<PortSet>();
    public List<LinkData> linkDatas = new List<LinkData>();
}

//節點node資料
[System.Serializable]
public class BaseNodeData
{
    //public string inNodeGuid;
    //public string outNodeGuid;
    public string guid;
    public Vector2 position;
}
[System.Serializable]
public class LevelNodeData : BaseNodeData
{
    public List<PortSet> portSets = new List<PortSet>();
    public object scene;
    //TODO: load type  

}
[System.Serializable]
public class PortSet
{
    public string setGuid;
    public string localInGuid;
    public string localOutGuid;
    public string pointObjectInstanceId;
    public string pointObjectName;
    public string nodeGuid;
    public Vector2 position;

    public PortSet() { }
    public PortSet(string _pointObjId, string _nodeId, Vector2 _pos,string _pointObjName)
    {
        setGuid = Guid.NewGuid().ToString();
        localInGuid = Guid.NewGuid().ToString();
        localOutGuid = Guid.NewGuid().ToString();
        pointObjectInstanceId = _pointObjId;
        nodeGuid = _nodeId;
        position = _pos;
        pointObjectName = _pointObjName;
    }
}

[System.Serializable]
public class LinkData
{
    public string inPortGuid;
    public string outPortGuid;
}
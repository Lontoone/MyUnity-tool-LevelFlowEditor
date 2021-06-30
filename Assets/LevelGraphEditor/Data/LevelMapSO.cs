using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/LevelFlow")]
public class LevelMapSO : ScriptableObject
{
    //public List<NodeLinkData> nodeLinkDatas = new List<NodeLinkData>();
    //TODO: data...
    //public List<BaseNodeData> baseNodeDatas = new List<BaseNodeData>();
    public List<LevelNodeData> levelNodeDatas = new List<LevelNodeData>();
    public List<PortSet> allPortSets
    {
        get
        {
            List<PortSet> _temp = new List<PortSet>();
            for (int i = 0; i < levelNodeDatas.Count; i++)
            {
                _temp.AddRange(levelNodeDatas[i].portSets);
            }
            return _temp;
        }
    }
    public List<LinkData> linkDatas = new List<LinkData>();

    public BaseNodeData GetNode(string _nodeId)
    {
        return levelNodeDatas.Find(x => x.guid == _nodeId);
    }
    public PortSet GetPortSet(string _setGuid)
    {
        return allPortSets.Find(x => x.setGuid == _setGuid);
    }
    public PortSet GetLinkedPort(string _outPortGuid)
    {
        LinkData _link = linkDatas.Find(x => x.outPortGuid == _outPortGuid);
        if (_link == null)
        {
            return null;
        }
        PortSet _inPortSet = allPortSets.Find(x => x.localInGuid == _link.inPortGuid);
        return _inPortSet;
    }
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
    //public object scene;
    public string scenePath;
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
    public PortSet(string _pointObjId, string _nodeId, Vector2 _pos, string _pointObjName)
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
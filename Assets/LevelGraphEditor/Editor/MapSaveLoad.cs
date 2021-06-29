using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MapSaveLoad
{
    private LevelFlowGraphView graphView;
    private List<Edge> edges => graphView.edges.ToList();
    private List<BaseNode> nodes => graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();

    public MapSaveLoad(LevelFlowGraphView _graphView)
    {
        graphView = _graphView;
    }

    public void Save(LevelMapSO _levelFlowSO)
    {
        SaveEdges(_levelFlowSO);
        SaveNodes(_levelFlowSO);

        //要存檔code修改過的scriptable Object，需要先setDirty告知此檔案被修改了，
        //再SaveAssets
        EditorUtility.SetDirty(_levelFlowSO);
        AssetDatabase.SaveAssets();
    }
    #region Save
    private void SaveEdges(LevelMapSO _levelFlowSO)
    {
        _levelFlowSO.linkDatas.Clear();
        Edge[] connectedEdges = edges.Where(edge => edge.input.node != null).ToArray();
        foreach (Edge _edge in connectedEdges)
        {
            LinkData _data = new LinkData();
            _data.inPortGuid = _edge.input.name;
            _data.outPortGuid = _edge.output.name;
            _levelFlowSO.linkDatas.Add(_data);
        }
    }
    private void SaveNodes(LevelMapSO _levelFlowSO)
    {
        _levelFlowSO.levelNodeDatas.Clear();
        foreach (BaseNode _node in nodes)
        {
            switch (_node)
            {
                case LevelNode _lvNode:
                    _levelFlowSO.levelNodeDatas.Add(SaveNodeData(_node));
                    break;
            }
        }
    }

    private LevelNodeData SaveNodeData(BaseNode _lvnode)
    {
        LevelNode _node = (LevelNode)_lvnode;
        LevelNodeData _data = new LevelNodeData
        {
            guid = _node.NodeGuid,
            position = _node.GetPosition().position,
            portSets = _node.portSets,
            scene = _node.scene,           
            
        };
        return _data;

    }
    #endregion

    public void Load(LevelMapSO _levelFlowSO)
    {
        ClearGraph();
        GenerateNodes(_levelFlowSO);
        ConnectNodes(_levelFlowSO);
    }
    #region Load

    #endregion

    private void ClearGraph()
    {
        edges.ForEach(edge => graphView.RemoveElement(edge));
        nodes.ForEach(node => graphView.RemoveElement(node));
    }
    private void GenerateNodes(LevelMapSO _levelFlowSO)
    {
        foreach (LevelNodeData _data in _levelFlowSO.levelNodeDatas) {
            LevelNode _lvNode = graphView.CreateLevelNode(_data.position);
            _lvNode.NodeGuid = _data.guid;
            _lvNode.scene = _data.scene;
            _lvNode.portSets = _data.portSets;

            _lvNode.LoadValueIntoField();
            _lvNode.SetupSubPortSets();

            graphView.AddElement(_lvNode);
        }

    }
    private void ConnectNodes(LevelMapSO _levelFlowSO)
    {
    }
}

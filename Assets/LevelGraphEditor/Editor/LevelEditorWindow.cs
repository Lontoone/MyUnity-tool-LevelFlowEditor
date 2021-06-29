using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelEditorWindow : EditorWindow
{
    public LevelMapSO flowData;
    private LevelFlowGraphView graphView;
    private ToolbarMenu toolbarMenu;
    private MapSaveLoad mapSaveLoad;

    [OnOpenAsset(1)] //當資料夾裡的檔案被點擊時的callback
    public static bool ShowWindowInfo(int _instanceID, int line) //每個project裡面的檔案都有自己的id
    {
        UnityEngine.Object item = EditorUtility.InstanceIDToObject(_instanceID);

        if (item is LevelMapSO) //點擊這類檔案的資料，開啟對應的編輯視窗
        {
            LevelEditorWindow window = (LevelEditorWindow)GetWindow(typeof(LevelEditorWindow));
            window.titleContent = new GUIContent("Level Flow Editor");
            window.flowData = item as LevelMapSO;
            window.minSize = new Vector2(500, 250);
            //window.Load();
        }
        return false;
    }
    public void OnEnable()
    {
        ConstructGraphView(); //產生注意順序
        GenerateToolBar();
        //Load();
    }

    //建立網格背景
    private void ConstructGraphView()
    {
        graphView = new LevelFlowGraphView(this);
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);

        mapSaveLoad = new MapSaveLoad(graphView);
    }

    private void GenerateToolBar()
    {
        StyleSheet styleSheet = Resources.Load<StyleSheet>("GraphViewStyleSheet");
        rootVisualElement.styleSheets.Add(styleSheet);

        Toolbar toolbar = new Toolbar();

        //存檔btn
        Button save_btn = new Button()
        {
            text = "Save"
        };
        save_btn.clicked += () =>
        {
            Save();
        };
        toolbar.Add(save_btn);

        //讀檔btn
        Button load_btn = new Button()
        {
            text = "Load"
        };
        load_btn.clicked += () =>
        {
            Load();
        };
        toolbar.Add(load_btn);

        //Upadte current Scene
        Button updateCurrentScene_btn = new Button()
        {
            text = "Update current scene"
        };
        updateCurrentScene_btn.clicked += () =>
        {
            SampleCurrentScene();
        };
        toolbar.Add(updateCurrentScene_btn);

        //Load All Scenes
        Button loadAllScene_btn = new Button()
        {
            text = "Load all scene"
        };
        loadAllScene_btn.clicked += () =>
        {
            //Load();
        };
        toolbar.Add(loadAllScene_btn);

        rootVisualElement.Add(toolbar);
    }

    private void Save()
    {
        if (flowData != null)
        {
            mapSaveLoad.Save(flowData);
        }
    }
    private void Load() {
        if (flowData != null)
        {
            mapSaveLoad.Load(flowData);
        }
    }

    private void SampleCurrentScene()
    {
        //get current Scene
        Scene _currentScene = SceneManager.GetActiveScene();

        //get current Scene's node
        LevelNode _currentNode = LevelNode.FindNodeByScene(_currentScene);

        //create one if not exist
        if (_currentNode == null)
        {
            /*
            _currentNode = graphView.CreateLevelNode(Vector2.zero);
            _currentNode.SetScene(_currentNode);
            graphView.AddElement(_currentNode);*/
            return;
        }
        //get current Scene's node's sub view

        //get all the enter point
        ConnectPoint[] points = FindObjectsOfType<ConnectPoint>();
        Vector3[] _projectedPoints = new Vector3[points.Length];

        Vector3 _widthProjectAxis = new Vector3(1, 0, 0);
        Vector3 _heightProjectAxis = new Vector3(0, 1, 0); //temp ignore z axis        
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 _projectPoint = points[i].transform.position;
            _projectPoint.x = Vector3.Dot(_projectPoint, _widthProjectAxis);
            _projectPoint.y = Vector3.Dot(_projectPoint, _heightProjectAxis);
            _projectedPoints[i] = _projectPoint;
        }

        float subMapViewSize = LevelNode.SUB_MAP_VIEW_SIZE - 100;

        float _maxHeight = _projectedPoints.ToList().Max(i => i.y);
        float _minHeight = _projectedPoints.ToList().Min(i => i.y);

        float _maxWidth = _projectedPoints.ToList().Max(i => i.x);
        float _minWidth = _projectedPoints.ToList().Min(i => i.x);

        float _widthGap = _maxWidth - _minWidth;
        float _heightGap = _maxHeight - _minHeight;

        //sample points' distance to sub view
        Vector3[] _subViewPoints = new Vector3[points.Length];
        for (int i = 0; i < _subViewPoints.Length; i++)
        {
            _subViewPoints[i] = new Vector2(
                    (_projectedPoints[i].x - _minWidth) / _widthGap * subMapViewSize,
                    (_projectedPoints[i].y - _minHeight) / _heightGap * subMapViewSize
                );

            //UI 需要翻轉y軸
            _subViewPoints[i].y = subMapViewSize - _subViewPoints[i].y;

            Debug.Log(points[i].name + "  " + points[i].transform.position + " " + _projectedPoints[i] + "  " + _subViewPoints[i]);
            //create port
            _currentNode.AddSubGraphViewLevelConnectPort(_subViewPoints[i], points[i].GetInstanceID().ToString(), points[i].name);
        }



    }
}

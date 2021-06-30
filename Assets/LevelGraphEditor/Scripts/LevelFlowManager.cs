using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFlowManager : MonoBehaviour
{
    public LevelMapSO flowData;
    public static LevelFlowManager instance;
    public static event Action<Transform> OnLevelEntered;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadNextScene(string _setGuid)
    {
        //Get this portSet
        PortSet _thisPortSet = flowData.GetPortSet(_setGuid);
        if (_thisPortSet == null)
        {
            //  => Not on the data : return
            Debug.Log("Port " + _setGuid + " is Null");
        }
        else
        {
            //  =>On Data:
            //      =>Get this port's next port in link data
            PortSet _nextPortSet = flowData.GetLinkedPort(_thisPortSet.localOutGuid);
            if (_nextPortSet != null)
            {
                //Get Next port's scene data and enterPoint object
                LevelNodeData _node = (LevelNodeData)flowData.GetNode(_nextPortSet.nodeGuid);

                //TODO: Apply scene load Mode
                SceneManager.GetSceneByPath(_node.scenePath);

                Transform enterPoint = FindObjectsOfType<ConnectPoint>().ToList().Find(x => x.portSetId == _nextPortSet.setGuid).transform;

                OnLevelEntered?.Invoke(enterPoint);
                //return enterPoint;
            }
            else
            {
                Debug.Log("Next Port " + " is Null");
            }

        }
    }


}

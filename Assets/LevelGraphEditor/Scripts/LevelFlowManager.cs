using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFlowManager : MonoBehaviour
{
    public LevelMapSO flowData;
    public static LevelFlowManager instance;

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

    public void LoadNextScene() {

    }


}

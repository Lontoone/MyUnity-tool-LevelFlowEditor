### See the code [here](https://github.com/Lontoone/MyUnityToolLab/tree/master/Level%20Flow%20Manager)

# MyUnity-tool-LevelFlowEditor

![image] (https://i.imgur.com/vYl1hAi.png)

Tutorial video:

<iframe  title="Demo content" width="480" height="390" src="https://youtu.be/YZwyQ-2FZbA" frameborder="0" allowfullscreen></iframe>

## API:

### Event:
- LevelFlowManager
-- public static event Action<string> OnConnectPointEntered;
    Called when finish loading scene. Return the `portSetGuid` of the matching enternce point gameObject.
	
    in demo: `RangeTriggerConnectPoint.cs`. 
	
	```
	public void OnEnable()
    {
        LevelFlowManager.OnConnectPointEntered += MovePlayerHere;
    }
	public void OnDisable()
    {
        LevelFlowManager.OnConnectPointEntered -= MovePlayerHere;
    }
	private void MovePlayerHere(string _enterPoint)
    {
        if (_enterPoint == portSetId)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
            leaveLock = true;
        }
    }
	
	```

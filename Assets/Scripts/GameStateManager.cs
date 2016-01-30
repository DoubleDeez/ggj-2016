using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {

    private bool GameIsPaused;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    
    public bool IsGamePaused()
    {
        return GameIsPaused;
    }
    
    private void ListenForPause()
    {
        GameIsPaused = Input.GetKey(KeyCode.Escape) || Input.GetButton("Pause");
    }
}

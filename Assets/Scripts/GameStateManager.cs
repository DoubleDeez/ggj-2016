using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {

    private bool GameIsPaused;

	// Use this for initialization
	void Start () {
	   GameIsPaused = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
	   ListenForPause();
	}
    
    public bool IsGamePaused()
    {
        return GameIsPaused;
    }
    
    private void ListenForPause()
    {
        bool pausePressed = Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause");
        if(pausePressed)
        {
            Debug.Log("Game Paused");
            GameIsPaused = !GameIsPaused;
            if(GameIsPaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
}

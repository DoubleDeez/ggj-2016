using UnityEngine;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour {

    private bool GameIsPaused;
    
    [System.Serializable]
    public class GameState {
        public string StateName;
        public List<string> GrandpaStateHints;
        public List<string> ChildStateHints;
    }
    
    public List<GameState> GameStates;
    public GameObject Grandpa;
    public GameObject Child;
    
    private GameState CurrentState;

	// Use this for initialization
	void Start () {
        GameIsPaused = false;
        CurrentState = GameStates[0];
        Player grandpaPlayer = Grandpa.GetComponent<Player>();
        grandpaPlayer.ClearHints();
        grandpaPlayer.SetHints(CurrentState.GrandpaStateHints);
        Player childPlayer = Child.GetComponent<Player>();
        childPlayer.ClearHints();
        childPlayer.SetHints(CurrentState.ChildStateHints);
	}
	
	// Update is called once per frame
	void Update () 
    {
	   ListenForPause();
	}
    
    void OnValidate() {
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

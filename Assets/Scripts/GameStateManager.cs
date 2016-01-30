using UnityEngine;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour {
    
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
    private bool GameIsPaused;
    private bool InputDisabled;

	// Use this for initialization
	void Start () {
        GameIsPaused = false;
        InputDisabled = false;
        CurrentState = GameStates[0];
        Player grandpaPlayer = Grandpa.GetComponent<Player>();
        grandpaPlayer.SetHints(CurrentState.GrandpaStateHints);
        Player childPlayer = Child.GetComponent<Player>();
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
    
    public bool IsInputDisabled()
    {
        return InputDisabled;
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

using UnityEngine;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour {
    
    [System.Serializable]
    public class GameState {
        public string StateName;
        public List<string> Hints;
    }
    
    [System.Serializable]
    public class LevelInteraction {
        [TooltipAttribute("This must match the name of the interaction object")]
        public string InteractionName;
        public bool IsPlayerColliding;
        public bool HasBeenInteracted;
    }
    
    [System.Serializable]
    public class GameLevel {
        public List<LevelInteraction> Interactions;
    }
    
    public List<GameLevel> GameLevels;
    public List<GameState> GrandpaStates;
    public List<GameState> ChildStates;
    public GameObject Grandpa;
    public GameObject Child;
    
    private bool GameIsPaused;
    private bool InputDisabled;
    private GameState CurrentGrandpaState;
    private GameState CurrentChildState;
    
    private Dictionary<string, bool> Flags;

	// Use this for initialization
	void Start () {
        Flags = new Dictionary<string, bool>();
        GameIsPaused = false;
        InputDisabled = false;
        CurrentGrandpaState = GrandpaStates[0];
        CurrentChildState = ChildStates[0];
        Player grandpaPlayer = Grandpa.GetComponent<Player>();
        grandpaPlayer.SetHints(CurrentGrandpaState.Hints);
        Player childPlayer = Child.GetComponent<Player>();
        childPlayer.SetHints(CurrentChildState.Hints);
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
    
    public LevelInteraction FindLevelInteraction(string interactionName) {
        foreach(GameLevel level in GameLevels) {
            foreach(LevelInteraction interaction in level.Interactions) {
                if(interaction.InteractionName.Equals(interactionName)) {
                    return interaction;
                }
            }
        }
        
        return null;
    }
    
    public void DoInteraction(LevelInteraction interaction) {
        
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

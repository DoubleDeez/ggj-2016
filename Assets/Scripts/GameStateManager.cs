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
    
<<<<<<< f06a844daa3b8a433f95d82c2cb29cb3fa0b65f1
    private GameState CurrentState;
    private bool GameIsPaused;
    private bool InputDisabled;
=======
    private GameState CurrentGrandpaState;
    private GameState CurrentChildState;
    
    private Dictionary<string, bool> Flags;
>>>>>>> Interactions are now tracked and able to be added via editor. Performing logic on interaction requires hard coding

	// Use this for initialization
	void Start () {
        Flags = new Dictionary<string, bool>();
        GameIsPaused = false;
<<<<<<< f06a844daa3b8a433f95d82c2cb29cb3fa0b65f1
        InputDisabled = false;
        CurrentState = GameStates[0];
=======
        CurrentGrandpaState = GrandpaStates[0];
        CurrentChildState = ChildStates[0];
>>>>>>> Interactions are now tracked and able to be added via editor. Performing logic on interaction requires hard coding
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

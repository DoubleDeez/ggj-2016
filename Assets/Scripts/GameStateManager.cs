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
        // Grandpa Interactions
        if(interaction.InteractionName.Equals("Football")) {
            // @TODO : Grandpa needs to say a message here
            // @TODO : Make heirloom glow
            Flags.Add("FOOTBALL_TELEPORT", true);
            SetCurrentGrandpaState("GlowingHeirloom");
        } else if(interaction.InteractionName.Equals("HeirloomGrandpa")) {
            // @TODO : Activate dpad UI with football - can now tp to backyard
            Flags.Add("GRANDPA_HEIRLOOM", true);
            SetCurrentGrandpaState("No hints");
        } else if(interaction.InteractionName.Equals("Diary")) {
            // @TODO : Activate dpad UI with diary - can now tp to bedroom
            Flags.Add("DIARY_TELEPORT", true);
            SetCurrentGrandpaState("No hints");
        } else if(interaction.InteractionName.Equals("CribBox")) {
            // @TODO : Activate dpad UI with diary - can now tp to baby room
            Flags.Add("BABY_TELEPORT", true);
            SetCurrentGrandpaState("No hints");
        } else if(interaction.InteractionName.Equals("BackyardFootball")) {
            // @TODO : play animation
            Flags.Add("CAN_TACKLE", true);
            SetCurrentGrandpaState("No hints");
        } else if(interaction.InteractionName.Equals("SexyTimeCloset")) {
            Flags.Add("BABY_CHANGE", true);
        } else if(interaction.InteractionName.Equals("BabyMonitor")) {
            if(QueryFlag("HAS_HEARING")) {
                Flags.Add("HEARD_FIGHTING", true);
            } else {
                Flags.Add("HEARD_STATIC", true);
            }
        } else if(interaction.InteractionName.Equals("BabyMobile")) {
            // @TODO save colour on right and set wallpaper for child
            Flags.Add("CHANGED_WALLPAPER", true);
        } else if(interaction.InteractionName.Equals("Infant")) {
            if(QueryFlag("NO_WALKER")) {
                Flags.Add("IS_BRAVE", true);
                SetCurrentGrandpaState("No hints");
            }
        }
        // Child's Interactions
        else if(interaction.InteractionName.Equals("HonorMedal")) {
            // @TODO : Activate dpad UI with Medal - can now tp to ww2
            Flags.Add("WW2_TELEPORT", true);
            SetCurrentChildState("TheBox");
        } else if(interaction.InteractionName.Equals("HeirloomKid")) {
            // @TODO : Activate dpad UI
            Flags.Add("CHILD_HEIRLOOM", true);
            SetCurrentChildState("No hints");
        } else if(interaction.InteractionName.Equals("BottleCaps")) {
            // @TODO : Activate dpad UI with bottle caps - can now tp to ww2bar
            Flags.Add("BAR_TELEPORT", true);
            SetCurrentChildState("No hints");
        } else if(interaction.InteractionName.Equals("MedicalBills")) {
            // @TODO : Activate dpad UI with med bills - can now tp to street
            Flags.Add("STREET_TELEPORT", true);
            SetCurrentGrandpaState("No hints");
        } else if(interaction.InteractionName.Equals("Grenade")) {
            if(QueryFlag("IS_BRAVE") && QueryFlag("VOLUNTEERED")) {
                Flags.Add("HAS_HEARING", true);
                SetCurrentGrandpaState("No hints");
            }
        } else if(interaction.InteractionName.Equals("Volunteer")) {
            // Trigger grenade throw, maybe captain says something
            Flags.Add("VOLUNTEERED", true);
        } else if(interaction.InteractionName.Equals("DrunkPa")) {
            // Tattoo change, fade to house after tattoo sounds
            Flags.Add("WHISPERED", true);
            SetCurrentGrandpaState("No hints");
        } else if(interaction.InteractionName.Equals("StreetPa")) {
            if(QueryFlag("CAN_TACKLE") && QueryFlag("STREET_TELEPORT")) {
                Flags.Add("NO_WALKER", true);
            }
        }
    }
    
    public bool QueryFlag(string name) {
        return Flags.ContainsKey(name) && Flags[name];
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
    
    private void SetCurrentGrandpaState(string stateName) {
        foreach(GameState state in GrandpaStates) {
            if(state.StateName.Equals(stateName)) {
                CurrentGrandpaState = state;
            }
        }
    }
    
    private void SetCurrentChildState(string stateName) {
        foreach(GameState state in ChildStates) {
            if(state.StateName.Equals(stateName)) {
                CurrentChildState = state;
            }
        }
    }
}

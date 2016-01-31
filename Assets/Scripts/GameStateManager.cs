using UnityEngine;
using XboxCtrlrInput;
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
    
    public IncomingCarScript Car;
    
    public float DelayToBegin = 3.0f;
    
    public GameObject SexyTimeDarkness;
    
    /** CHAT BUBBLES */
    public GameObject Grandpa_MomSaysWakeUp;
    public GameObject Grandpa_SexyTime;
    public GameObject Grandpa_Static;
    public GameObject Grandpa_Monitor;
    
    public GameObject Child_MomMessage;
    public GameObject Child_Captain;
    public GameObject Child_GrandpaWW2;
    public GameObject Child_Bar;
    /** END CHAT BUBBLES */
    
    private bool GameIsPaused;
    private bool InputDisabled;
    private GameState CurrentGrandpaState;
    private GameState CurrentChildState;
    
    private Dictionary<string, bool> Flags;
    
    private float StartTime;

	// Use this for initialization
	void Start () {
        Flags = new Dictionary<string, bool>();
        GameIsPaused = false;
        InputDisabled = true;
        CurrentGrandpaState = GrandpaStates[0];
        CurrentChildState = ChildStates[0];
        Player grandpaPlayer = Grandpa.GetComponent<Player>();
        grandpaPlayer.SetHints(CurrentGrandpaState.Hints);
        Player childPlayer = Child.GetComponent<Player>();
        childPlayer.SetHints(CurrentChildState.Hints);
        StartTime = Time.time + DelayToBegin;
	}
	
	// Update is called once per frame
	void Update () 
    {
	   ListenForPause();
       if(Time.time > StartTime) {
           StartTime = float.MaxValue;
           InputDisabled = false;
           Grandpa_MomSaysWakeUp.GetComponent<ChatBubbleController>().Trigger();
       }
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
            SetFlag("FOOTBALL_TELEPORT", true);
            SetCurrentGrandpaState("GlowingHeirloom");
            Grandpa.GetComponent<Player>().EnableGate("right");
        } else if(interaction.InteractionName.Equals("HeirloomGrandpa")) {
            SetFlag("GRANDPA_HEIRLOOM", true);
            Grandpa.GetComponent<Player>().EnableHeirloom(true);
            SetCurrentGrandpaState("No hints");
        } else if(interaction.InteractionName.Equals("Diary")) {
            SetFlag("DIARY_TELEPORT", true);
            SetCurrentGrandpaState("No hints");
            Grandpa.GetComponent<Player>().EnableGate("down");
        } else if(interaction.InteractionName.Equals("CribBox")) {
            SetFlag("BABY_TELEPORT", true);
            SetCurrentGrandpaState("No hints");
            Grandpa.GetComponent<Player>().EnableGate("left");
        } else if(interaction.InteractionName.Equals("BackyardFootball")) {
            // @TODO : play animation
            SetFlag("CAN_TACKLE", true);
            SetCurrentGrandpaState("No hints");
        } else if(interaction.InteractionName.Equals("SexyTimeCloset")) {
            SetFlag("BABY_CHANGE", true);
            SexyTimeDarkness.SetActive(false);
            Grandpa_SexyTime.GetComponent<ChatBubbleController>().Trigger();
        } else if(interaction.InteractionName.Equals("BabyMonitor")) {
            if(QueryFlag("HAS_HEARING")) {
                SetFlag("HEARD_FIGHTING", true);
            } else {
                SetFlag("HEARD_STATIC", true);
            }
        } else if(interaction.InteractionName.Equals("BabyMobile")) {
            // @TODO save colour on right of mobile and set wallpaper for child
            SetFlag("CHANGED_WALLPAPER", true);
        } else if(interaction.InteractionName.Equals("Infant")) {
            if(QueryFlag("NO_WALKER")) {
                SetFlag("IS_BRAVE", true);
                SetCurrentGrandpaState("No hints");
            }
        }
        // Child's Interactions
        else if(interaction.InteractionName.Equals("HonorMedal")) {
            SetFlag("WW2_TELEPORT", true);
            SetCurrentChildState("TheBox");
            Child.GetComponent<Player>().EnableGate("right");
        } else if(interaction.InteractionName.Equals("HeirloomKid")) {
            SetFlag("CHILD_HEIRLOOM", true);
            SetCurrentChildState("No hints");
            Child.GetComponent<Player>().EnableHeirloom(true);
        } else if(interaction.InteractionName.Equals("BottleCaps")) {
            SetFlag("BAR_TELEPORT", true);
            SetCurrentChildState("No hints");
            Child.GetComponent<Player>().EnableGate("down");
        } else if(interaction.InteractionName.Equals("MedicalBills")) {
            SetFlag("STREET_TELEPORT", true);
            SetCurrentGrandpaState("No hints");
            Child.GetComponent<Player>().EnableGate("left");
        } else if(interaction.InteractionName.Equals("Grenade")) {
            if(QueryFlag("IS_BRAVE") && QueryFlag("VOLUNTEERED")) {
                SetFlag("HAS_HEARING", true);
                SetCurrentGrandpaState("No hints");
            }
        } else if(interaction.InteractionName.Equals("Volunteer")) {
            // Trigger grenade throw
            SetFlag("VOLUNTEERED", true);
        } else if(interaction.InteractionName.Equals("DrunkPa")) {
            // Tattoo change, fade to house after tattoo sounds
            SetFlag("WHISPERED", true);
            SetCurrentGrandpaState("No hints");
        } else if(interaction.InteractionName.Equals("StreetPa")) {
            if(QueryFlag("CAN_TACKLE") && QueryFlag("STREET_TELEPORT")) {
                SetFlag("NO_WALKER", true);
                Car.RunHimDown = false;
                // Remove grandpa's walker
            }
        }
    }
    
    public bool QueryFlag(string name) {
        return Flags.ContainsKey(name) && Flags[name];
    }
    
    public void SetFlag(string key, bool val) {
        if(!Flags.ContainsKey(key)) {
            Flags.Add(key, val);
        } else {
            Flags[key] = val;
        }
    }
    
    public void PlaySoundEffect(AudioClip sound) {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = sound;
        audioSource.Play();
    }
    
    private void ListenForPause()
    {
        bool pausePressed = Input.GetKeyDown(KeyCode.Escape) || XCI.GetButtonDown(XboxButton.Start);
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
    
    public void SetCurrentGrandpaState(string stateName) {
        foreach(GameState state in GrandpaStates) {
            if(state.StateName.Equals(stateName)) {
                CurrentGrandpaState = state;
                Grandpa.GetComponent<Player>().SetHints(state.Hints);
            }
        }
    }
    
    public void SetCurrentChildState(string stateName) {
        foreach(GameState state in ChildStates) {
            if(state.StateName.Equals(stateName)) {
                CurrentChildState = state;
                Child.GetComponent<Player>().SetHints(state.Hints);
            }
        }
    }
}

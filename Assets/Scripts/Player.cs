﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public bool DEBUG_BypassTeleportRestictions = false;

    public int PlayerNumber;
    public string CurrentLevel = "House";
    public float HintDisplayTime = 2.0f;
    public GameObject PlayerHintBubble;

    public RuntimeAnimatorController PrimaryScheme;
    public RuntimeAnimatorController SecondaryScheme;

    public float TeleportFadeSpeed = 1.5f;
    public float TeleportFadeDuration = 2.0f;
    public Color TeleportFadeTint = Color.white;

    public Image FadeUI;

    public AudioClip TeleportSound;
    private AudioSource audioSource;

    public GameObject Heirloom;
    public GameObject UpSpawn;
    public GameObject DownSpawn;
    public GameObject LeftSpawn;
    public GameObject RightSpawn;

    public Sprite HeirloomStatic;
    public Sprite HeirloomTop;
    public Sprite HeirloomLeft;
    public Sprite HeirloomRight;
    public Sprite HeirloomDown;
    public GameObject GateUp;
    public GameObject GateLeft;
    public GameObject GateRight;
    public GameObject GateDown;

    public Sprite AlternateSprite;
    private Sprite MainSprite;

    private GameStateManager GameManager;
    private List<GameStateManager.LevelInteraction> LevelInteractionsColliding;
    private List<string> Hints;
    private Vector3 ChatPositionDelta;
    private float TimeToHideHint = 0.0f;

    private bool IsAnimatorSchemeAlternate=false;
    private bool HasTakenTeleport = false; //Take only one teleport at a time

    private bool IsPlayingWalkingSound = false;
    private bool IsPlayingRunningSound = false;

    private float FadeOpaqueStart = 0.0f;
    private float FadeOpaqueEnd = 0.0f;
    private float FadeTransparentStart = 0.0f;
    private float FadeTransparentEnd = 0.0f;

	// Use this for initialization
	void Start () {
        LevelInteractionsColliding = new List<GameStateManager.LevelInteraction>();
        PlayerHintBubble.SetActive(false);
        ChatPositionDelta = PlayerHintBubble.transform.position - transform.position;
        GameManager = FindObjectOfType<GameStateManager>();
        MainSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        EnableHeirloom(false);
        GateUp.SetActive(false);
        GateRight.SetActive(false);
        GateDown.SetActive(false);
        GateLeft.SetActive(false);
        
        if(PlayerNumber == 1) {
            GameManager.SetCurrentGrandpaState("GameStartGramp");
        } else {
            GameManager.SetCurrentChildState("Sleep");
        }

        audioSource = GetComponent<AudioSource>();
	}
    
    void OnEnable()
    {
        if(FadeUI==null)
        {
            Debug.LogError("Fading in and out might be broken. Check your player object to make sure you have a correct reference set!");
        }
    }

	// Update is called once per frame
	void Update () {
	   PlayerHintBubble.transform.position = transform.position + ChatPositionDelta;
       if(PlayerHintBubble.activeSelf && Time.time > TimeToHideHint) {
           HideHint();
       }
       
       // @TODO This causes NullReferenceException
       // foxtrot94: Could not confirm that it does. Must set FadeUI from editor though...
       Color color = FadeUI.color;
       if(Time.time < FadeOpaqueEnd) {
           color.a = Mathf.Lerp(FadeUI.color.a, 1.0f, (Time.time - FadeOpaqueStart) / (FadeOpaqueEnd - FadeOpaqueStart));
           FadeUI.color = color;
       } else if(Time.time > FadeTransparentStart && Time.time < FadeTransparentEnd) {
           color.a = Mathf.Lerp(FadeUI.color.a, 0.0f, (Time.time - FadeTransparentStart) / (FadeTransparentEnd - FadeTransparentStart));
           FadeUI.color = color;
       }
	}

    public void _playSound(AudioClip sound)
    {
        if (sound != audioSource.clip || !audioSource.isPlaying)
        {
            audioSource.clip = sound;
            audioSource.Play();
        }
    }

    public void ShowHint() {
        if(!PlayerHintBubble.activeSelf && Hints.Count > 0) {
            ChatBubbleController chatBubble = PlayerHintBubble.GetComponentInChildren<ChatBubbleController>();
            chatBubble.ChatText.Clear();
            chatBubble.ChatText.Add(Hints[Random.Range(0, Hints.Count)]);
            PlayerHintBubble.SetActive(true);
            TimeToHideHint = Time.time + HintDisplayTime;
        }
    }

    void HideHint() {
        PlayerHintBubble.SetActive(false);
    }

    public void AddHint(string hint) {
        Hints.Add(hint);
    }

    public void SetHints(List<string> hints) {
        Hints = new List<string>(hints);
    }

    public void ClearHints() {
        Hints = new List<string>();
    }

    public void AddCollidingInteraction(GameStateManager.LevelInteraction Interaction) {
        LevelInteractionsColliding.Add(Interaction);
    }

    public void RemoveCollidingInteraction(GameStateManager.LevelInteraction Interaction) {
        LevelInteractionsColliding.Remove(Interaction);
    }

    public List<GameStateManager.LevelInteraction> GetLevelInteractionsColliding() {
        return LevelInteractionsColliding;
    }
    
    public void EnableHeirloom(bool enable) {
        Heirloom.SetActive(enable);
    }
    
    public void EnableGate(string direction) {
        switch(direction) {
            case "up":
                GateUp.SetActive(true);
                break;
            case "right":
                GateRight.SetActive(true);
                break;
            case "down":
                GateDown.SetActive(true);
                break;
            case "left":
                GateLeft.SetActive(true);
                break;
        }
    }

    //@TODO : Confirm the order in which the check should go
    public void OnDPadUp()
    {
        Heirloom.GetComponent<Image>().sprite = HeirloomTop;
        //Teleport to the Footbal Spawn
        if(DEBUG_BypassTeleportRestictions || ((GameManager.QueryFlag("GRANDPA_HEIRLOOM") || GameManager.QueryFlag("CHILD_HEIRLOOM") ) && UpSpawn!=null))
        {
            FadeInOut(TeleportFadeDuration, TeleportFadeSpeed, TeleportFadeTint);
            UpSpawn.GetComponent<SpawnPoint>().TeleportPlayer(this);
            
            HasTakenTeleport = false;
            
            if(PlayerNumber == 1) {
                GameManager.SetCurrentGrandpaState("No hints");
            } else {
                GameManager.SetCurrentChildState("NoHints");
            }
            CurrentLevel = "House";
        }
    }

    public void OnDPadDown()
    {
        Heirloom.GetComponent<Image>().sprite = HeirloomDown;
        //Teleport to Main Hub
        if(DEBUG_BypassTeleportRestictions || ((GameManager.QueryFlag("DIARY_TELEPORT") || GameManager.QueryFlag("BAR_TELEPORT") ) && (GameManager.QueryFlag("GRANDPA_HEIRLOOM") || GameManager.QueryFlag("CHILD_HEIRLOOM") ) && DownSpawn!=null && !HasTakenTeleport))
        {
            FadeInOut(TeleportFadeDuration, TeleportFadeSpeed, TeleportFadeTint);
            DownSpawn.GetComponent<SpawnPoint>().TeleportPlayer(this);
            
            if(!DEBUG_BypassTeleportRestictions)
            {
                HasTakenTeleport = true;
            }
            
            if(PlayerNumber == 1) {
                GameManager.SetCurrentGrandpaState("SexyTime");
                CurrentLevel = "SexyTime";
            } else {
                GameManager.SetCurrentChildState("BarWhispers");
                CurrentLevel = "BarWhispers";
            }
        }
    }

    public void OnDPadLeft()
    {
        Heirloom.GetComponent<Image>().sprite = HeirloomLeft;
        //Teleport to Diary
        if(DEBUG_BypassTeleportRestictions || ((GameManager.QueryFlag("BABY_TELEPORT") || GameManager.QueryFlag("STREET_TELEPORT")) && (GameManager.QueryFlag("GRANDPA_HEIRLOOM") || GameManager.QueryFlag("CHILD_HEIRLOOM") ) && LeftSpawn!=null && !HasTakenTeleport))
        {
            FadeInOut(TeleportFadeDuration, TeleportFadeSpeed, TeleportFadeTint);
            LeftSpawn.GetComponent<SpawnPoint>().TeleportPlayer(this);
            
            if(!DEBUG_BypassTeleportRestictions)
            {
                HasTakenTeleport = true;
            }
            
            if(PlayerNumber == 1) {
                GameManager.SetCurrentGrandpaState("BabyRoom");
                CurrentLevel = "BabyRoom";
            } else {
                GameManager.SetCurrentChildState("CarStreet");
                CurrentLevel = "CarStreet";
            }
        }
    }

    public void OnDPadRight()
    {
        Heirloom.GetComponent<Image>().sprite = HeirloomRight;
        //Teleport to Backyard
        if(DEBUG_BypassTeleportRestictions || ((GameManager.QueryFlag("FOOTBALL_TELEPORT") || GameManager.QueryFlag("WW2_TELEPORT")) && (GameManager.QueryFlag("GRANDPA_HEIRLOOM") || GameManager.QueryFlag("CHILD_HEIRLOOM") ) && RightSpawn!=null && !HasTakenTeleport))
        {
            FadeInOut(TeleportFadeDuration, TeleportFadeSpeed, TeleportFadeTint);
            RightSpawn.GetComponent<SpawnPoint>().TeleportPlayer(this);
            
            if(!DEBUG_BypassTeleportRestictions)
            {
                HasTakenTeleport = true;
            }
            
            if(PlayerNumber == 1) {
                GameManager.SetCurrentGrandpaState("Backyard");
                CurrentLevel = "Backyard";
            } else {
                GameManager.SetCurrentChildState("WW2");
                CurrentLevel = "WW2";
            }
        }
    }

    public void OnDPadUpReleased() {
        Heirloom.GetComponent<Image>().sprite = HeirloomStatic;
    }

    public void OnDPadRightReleased() {
        Heirloom.GetComponent<Image>().sprite = HeirloomStatic;
    }

    public void OnDPadLeftReleased() {
        Heirloom.GetComponent<Image>().sprite = HeirloomStatic;
    }

    public void OnDPadDownReleased() {
        Heirloom.GetComponent<Image>().sprite = HeirloomStatic;
    }


    public void SwitchAnimatorController()
    {
        if(IsAnimatorSchemeAlternate)
        {
            gameObject.GetComponent<Animator>().runtimeAnimatorController = PrimaryScheme;
            gameObject.GetComponent<SpriteRenderer>().sprite = MainSprite;
        }
        else
        {
            gameObject.GetComponent<Animator>().runtimeAnimatorController = SecondaryScheme;
            gameObject.GetComponent<SpriteRenderer>().sprite = MainSprite;
        }
        IsAnimatorSchemeAlternate = !IsAnimatorSchemeAlternate;
    }

    public void FadeInOut(float duration, float fadeTime, Color tint) {
        FadeUI.color = tint;
        FadeOpaqueStart = Time.time;
        FadeOpaqueEnd = FadeOpaqueStart + fadeTime;
        FadeTransparentStart = FadeOpaqueEnd + duration;
        FadeTransparentEnd = FadeTransparentStart + fadeTime;
    }
    
    public void NotifyTeleportRecall(SpawnPoint point)
    {
        if(point!=null && HasTakenTeleport)
        {
            //We can now teleport to spawn points again
            HasTakenTeleport = false;
        }
    }
}

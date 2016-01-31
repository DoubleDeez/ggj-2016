using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public bool DEBUG_BypassTeleportRestictions = false;

    public int PlayerNumber;
    public float HintDisplayTime = 2.0f;
    public GameObject PlayerHintBubble;

    public RuntimeAnimatorController PrimaryScheme;
    public RuntimeAnimatorController SecondaryScheme;

    public float TeleportFadeSpeed = 1.5f;
    public float TeleportFadeDuration = 2.0f;
    public Color TeleportFadeTint = Color.white;

    public Image FadeUI;

    public AudioClip WalkingSound;
    public AudioClip RunningSound;

    public Image Heirloom;
    public GameObject UpSpawn;
    public GameObject DownSpawn;
    public GameObject LeftSpawn;
    public GameObject RightSpawn;

    public Sprite HeirloomStatic;
    public Sprite HeirloomTop;
    public Sprite HeirloomLeft;
    public Sprite HeirloomRight;
    public Sprite HeirloomDown;

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
	}
    
    void OnEnable()
    {
        if(FadeUI==null)
        {
            Debug.LogError("Fading in and out might be broken. Check your player object to make sure you have a correct reference set!")
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

    //@TODO : Confirm the order in which the check should go
    public void OnDPadUp()
    {
        Heirloom.sprite = HeirloomTop;
        //Teleport to the Footbal Spawn
        if(DEBUG_BypassTeleportRestictions || ((GameManager.QueryFlag("FOOTBALL_TELEPORT") || GameManager.QueryFlag("WW2_TELEPORT") ) && UpSpawn!=null && !HasTakenTeleport))
        {
            FadeInOut(TeleportFadeDuration, TeleportFadeSpeed, TeleportFadeTint);
            UpSpawn.GetComponent<SpawnPoint>().TeleportPlayer(this);
            
            if(DEBUG_BypassTeleportRestictions)
            {
                HasTakenTeleport = true;
            }
        }
    }

    public void OnDPadDown()
    {
        Heirloom.sprite = HeirloomDown;
        //Teleport to Main Hub
        if(DEBUG_BypassTeleportRestictions || ((GameManager.QueryFlag("GRANDPA_HEIRLOOM") || GameManager.QueryFlag("CHILD_HEIRLOOM") ) && DownSpawn!=null && !HasTakenTeleport))
        {
            FadeInOut(TeleportFadeDuration, TeleportFadeSpeed, TeleportFadeTint);
            DownSpawn.GetComponent<SpawnPoint>().TeleportPlayer(this);
            
            if(DEBUG_BypassTeleportRestictions)
            {
                HasTakenTeleport = true;
            }
        }
    }

    public void OnDPadLeft()
    {
        Heirloom.sprite = HeirloomLeft;
        //Teleport to Diary
        if(DEBUG_BypassTeleportRestictions || ((GameManager.QueryFlag("DIARY_TELEPORT") || GameManager.QueryFlag("BAR_TELEPORT")) && LeftSpawn!=null && !HasTakenTeleport))
        {
            FadeInOut(TeleportFadeDuration, TeleportFadeSpeed, TeleportFadeTint);
            LeftSpawn.GetComponent<SpawnPoint>().TeleportPlayer(this);
            
            if(DEBUG_BypassTeleportRestictions)
            {
                HasTakenTeleport = true;
            }
        }
    }

    public void OnDPadRight()
    {
        Heirloom.sprite = HeirloomRight;
        //Teleport to Backyard
        if(DEBUG_BypassTeleportRestictions || ((GameManager.QueryFlag("BABY_TELEPORT") || GameManager.QueryFlag("STREET_TELEPORT")) && RightSpawn!=null && !HasTakenTeleport))
        {
            FadeInOut(TeleportFadeDuration, TeleportFadeSpeed, TeleportFadeTint);
            RightSpawn.GetComponent<SpawnPoint>().TeleportPlayer(this);
            
            if(DEBUG_BypassTeleportRestictions)
            {
                HasTakenTeleport = true;
            }
        }
    }

    public void OnDPadUpReleased() {
        Heirloom.sprite = HeirloomStatic;
    }

    public void OnDPadRightReleased() {
        Heirloom.sprite = HeirloomStatic;
    }

    public void OnDPadLeftReleased() {
        Heirloom.sprite = HeirloomStatic;
    }

    public void OnDPadDownReleased() {
        Heirloom.sprite = HeirloomStatic;
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

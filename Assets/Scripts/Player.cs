using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    
    public int PlayerNumber;
    public float HintDisplayTime = 2.0f;
    public GameObject PlayerHintBubble;
    
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
    
    private GameStateManager GameManager;
    private List<GameStateManager.LevelInteraction> LevelInteractionsColliding;
    private List<string> Hints;
    private Vector3 ChatPositionDelta;
    private float TimeToHideHint = 0.0f;

	// Use this for initialization
	void Start () {
        LevelInteractionsColliding = new List<GameStateManager.LevelInteraction>();
        PlayerHintBubble.SetActive(false);
        ChatPositionDelta = PlayerHintBubble.transform.position - transform.position;
        GameManager = FindObjectOfType<GameStateManager>();
	}
	
	// Update is called once per frame
	void Update () {
	   PlayerHintBubble.transform.position = transform.position + ChatPositionDelta;
       if(PlayerHintBubble.activeSelf && Time.time > TimeToHideHint) {
           HideHint();
       }
	}
    
    public void ShowHint() {
        if(!PlayerHintBubble.activeSelf) {
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
        // if( (GameManager.QueryFlag("FOOTBALL_TELEPORT") || GameManager.QueryFlag("WW2_TELEPORT") ) && UpSpawn!=null)
        // {
            Debug.Log("Player "+this.PlayerNumber+" Calling Up");
            UpSpawn.GetComponent<SpawnPoint>().TeleportPlayer(this);
        // }
    }
    
    public void OnDPadDown()
    {
        Heirloom.sprite = HeirloomDown;
        //Teleport to Main Hub
        if( (GameManager.QueryFlag("GRANDPA_HEIRLOOM") || GameManager.QueryFlag("CHILD_HEIRLOOM") ) && DownSpawn!=null)
        {
            DownSpawn.GetComponent<SpawnPoint>().TeleportPlayer(this);
        }
    }
    
    public void OnDPadLeft()
    {
        Heirloom.sprite = HeirloomLeft;
        //Teleport to Diary
        if( (GameManager.QueryFlag("DIARY_TELEPORT") || GameManager.QueryFlag("BAR_TELEPORT")) && LeftSpawn!=null)
        {
            LeftSpawn.GetComponent<SpawnPoint>().TeleportPlayer(this);
        }
    }
    
    public void OnDPadRight()
    {
        Heirloom.sprite = HeirloomRight;
        //Teleport to Backyard
        if( (GameManager.QueryFlag("BABY_TELEPORT") || GameManager.QueryFlag("STREET_TELEPORT")) && RightSpawn!=null)
        {
            RightSpawn.GetComponent<SpawnPoint>().TeleportPlayer(this);
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
}

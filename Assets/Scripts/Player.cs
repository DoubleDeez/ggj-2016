using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    
    public int PlayerNumber;
    public float HintDisplayTime = 2.0f;
    public GameObject PlayerHintBubble;
    
    private List<string> Hints;
    private Vector3 ChatPositionDelta;
    private float TimeToHideHint = 0.0f;

	// Use this for initialization
	void Start () {
        PlayerHintBubble.SetActive(false);
        ChatPositionDelta = PlayerHintBubble.transform.position - transform.position;
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
}

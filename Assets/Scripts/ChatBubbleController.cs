using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChatBubbleController : MonoBehaviour {
    public List<string> ChatText = new List<string>();
    [TooltipAttribute("How many seconds to show each line of text")]
    public float ChatTextTime = 2.0f;
    public int FontSize = 50;
    public Vector2 Padding = new Vector2(25,40);
    public Text TextObject;
    public RectTransform Canvas;
    
    public bool IsTriggered = false;
    public bool LoopTrigger = false;
    
    private bool IsCycling = false;
    private float CycleNextLineTime;
    private int CycleChatIndex = 0;

	void Start () {
	   if(IsTriggered) {
           gameObject.SetActive(false);
       }
	}
    
    void OnValidate() {
        UpdateText();
    }
	
	void Update () {
        if(IsTriggered) {
            if(IsCycling) {
                TextObject.text = ChatText[CycleChatIndex];
                TextObject.fontSize = FontSize;
                GUIStyle style = new GUIStyle();
                style.fontSize = FontSize;
                Canvas.sizeDelta = (style.CalcSize(new GUIContent(ChatText[CycleChatIndex])) + Padding) / 100;
                if(Time.time > CycleNextLineTime) {
                    if(CycleChatIndex >= ChatText.Count) {
                        if(LoopTrigger) {
                            CycleChatIndex = 0;
                            CycleNextLineTime = Time.time + ChatTextTime;
                        } else {
                            IsCycling = false;
                            gameObject.SetActive(false);
                        }
                    } else {
                        CycleChatIndex++;
                        CycleNextLineTime = Time.time + ChatTextTime;
                    }
                }
            }
        } else {
            UpdateText();
        }
	}
    
    void UpdateText() {
        int chatIndex = ((int)Time.time % (int)(ChatTextTime * ChatText.Count)) / (int)ChatTextTime;
        TextObject.text = ChatText[chatIndex];
        TextObject.fontSize = FontSize;
        GUIStyle style = new GUIStyle();
        style.fontSize = FontSize;
        Canvas.sizeDelta = (style.CalcSize(new GUIContent(ChatText[chatIndex])) + Padding) / 100;
    }
    
    public void Trigger() {
        IsCycling = true;
        gameObject.SetActive(true);
        CycleNextLineTime = Time.time + ChatTextTime;
    }
}

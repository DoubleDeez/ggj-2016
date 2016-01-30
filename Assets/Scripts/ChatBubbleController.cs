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

	void Start () {
	
	}
    
    void OnValidate() {
        UpdateText();
    }
	
	void Update () {
        UpdateText();
	}
    
    void UpdateText() {
        int chatIndex = ((int)Time.time % (int)(ChatTextTime * ChatText.Count)) / (int)ChatTextTime;
        TextObject.text = ChatText[chatIndex];
        TextObject.fontSize = FontSize;
        GUIStyle style = new GUIStyle();
        style.fontSize = FontSize;
        Canvas.sizeDelta = (style.CalcSize(new GUIContent(ChatText[chatIndex])) + Padding) / 100;
    }
}

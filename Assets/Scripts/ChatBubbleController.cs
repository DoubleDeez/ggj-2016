using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChatBubbleController : MonoBehaviour {
    public List<string> ChatText = new List<string>();
    [TooltipAttribute("How many seconds to show each line of text")]
    public float ChatSpeed = 2.0f;
    public float AnimationSpeed = 1.0f;
    public Vector2 Padding = new Vector2(10,15);
    public Text TextObject;
    public RectTransform Canvas;

	void Start () {
	
	}
    
    void OnValidate() {
        TextObject.text = ChatText[0];
        GUIStyle style = new GUIStyle();
        style.fontSize = 50;
        Canvas.sizeDelta = (style.CalcSize(new GUIContent(ChatText[0])) + Padding) / 100;
    }
	
	void Update () {
	
	}
}

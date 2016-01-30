using UnityEngine;
using System.Collections;

public class InteractionObjectController : MonoBehaviour {
    
    public GameObject InteractionIcon;
    public GameObject InteractionObjectDefault;
    public GameObject InteractionIconHighlighted;
    
	void Start () {
        // Hide/Show child sprites as appropriate (no highlight)
        InteractionIcon.GetComponent<Renderer>().enabled = false;
        InteractionObjectDefault.GetComponent<Renderer>().enabled = true;
        InteractionIconHighlighted.GetComponent<Renderer>().enabled = false;
	}
    
    void OnValidate() {
        
    }
	
	void Update () {
	
	}
    
    void OnTriggerEnter2D(Collider2D Other) {
        Debug.Log(Other.gameObject.tag);
        if(Other.gameObject.tag.Equals("Player")) {
            InteractionIcon.GetComponent<Renderer>().enabled = true;
            InteractionObjectDefault.GetComponent<Renderer>().enabled = false;
            InteractionIconHighlighted.GetComponent<Renderer>().enabled = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D Other) {
        if(Other.gameObject.tag.Equals("Player")) {
            InteractionIcon.GetComponent<Renderer>().enabled = false;
            InteractionObjectDefault.GetComponent<Renderer>().enabled = true;
            InteractionIconHighlighted.GetComponent<Renderer>().enabled = false;
        }
    }
}

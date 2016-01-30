using UnityEngine;
using System.Collections;

public class InteractionObjectController : MonoBehaviour {
    
    public GameObject InteractionIcon;
    public GameObject InteractionObjectDefault;
    public GameObject InteractionIconHighlighted;
    
	void Start () {
	}
    
    void OnValidate() {
        
    }
	
	void Update () {
	
	}
    
    void OnTriggerEnter(Collider Other) {
        if(Other.gameObject.tag.Equals("Player")) {
            InteractionIcon.GetComponent<Renderer>().enabled = true;
            InteractionObjectDefault.GetComponent<Renderer>().enabled = false;
            InteractionIconHighlighted.GetComponent<Renderer>().enabled = true;
        }
    }
    
    void OnTriggerExit(Collider Other) {
        if(Other.gameObject.tag.Equals("Player")) {
            InteractionIcon.GetComponent<Renderer>().enabled = false;
            InteractionObjectDefault.GetComponent<Renderer>().enabled = true;
            InteractionIconHighlighted.GetComponent<Renderer>().enabled = false;
        }
    }
}

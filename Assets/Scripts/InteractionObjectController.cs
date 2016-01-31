using UnityEngine;
using System.Collections;

public class InteractionObjectController : MonoBehaviour {
    
    public GameStateManager GameState;
    public GameObject InteractionIcon;
    public GameObject InteractionObjectDefault;
    public GameObject InteractionObjectHighlighted;
    
	void Start () {
        // Hide/Show child sprites as appropriate (no highlight)
        InteractionIcon.GetComponent<Renderer>().enabled = false;
        InteractionObjectDefault.GetComponent<Renderer>().enabled = true;
        InteractionObjectHighlighted.GetComponent<Renderer>().enabled = false;
	}
    
    void OnValidate() {
        
    }
	
	void Update () {
	
	}
    
    void OnTriggerEnter2D(Collider2D Other) {
        if(Other.gameObject.tag.Equals("Player")) {
            InteractionIcon.GetComponent<Renderer>().enabled = true;
            if(InteractionObjectHighlighted != null) {
                InteractionObjectDefault.GetComponent<Renderer>().enabled = false;
                InteractionObjectHighlighted.GetComponent<Renderer>().enabled = true;
            }
            GameStateManager.LevelInteraction interaction = GameState.FindLevelInteraction(name);
            interaction.IsPlayerColliding = true;
            Other.GetComponent<Player>().AddCollidingInteraction(interaction);
        }
    }
    
    void OnTriggerExit2D(Collider2D Other) {
        if(Other.gameObject.tag.Equals("Player")) {
            InteractionIcon.GetComponent<Renderer>().enabled = false;
            if(InteractionObjectHighlighted != null) {
                InteractionObjectDefault.GetComponent<Renderer>().enabled = true;
                InteractionObjectHighlighted.GetComponent<Renderer>().enabled = false;
            }
            GameStateManager.LevelInteraction interaction = GameState.FindLevelInteraction(name);
            interaction.IsPlayerColliding = false;
            Other.GetComponent<Player>().RemoveCollidingInteraction(interaction);
        }
    }
}

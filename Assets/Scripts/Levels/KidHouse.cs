using UnityEngine;
using System.Collections;

public class KidHouse : MonoBehaviour {

    public GameObject Level;
    public Sprite OpenDoor;

    private SpriteRenderer LevelBackground;
    
	// Use this for initialization
	void Start () 
    {
	   LevelBackground = Level.GetComponent<SpriteRenderer>();
       if(LevelBackground==null)
       {
           Debug.Log("Error loading "+this.name);
           this.enabled = false;
       }
	}
	
	// Update is called once per frame
	void Update () 
    {
	   
	}
    
    public void DoScriptedAction()
    {
        
    }
}

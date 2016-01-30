using UnityEngine;
using System.Collections;

/// <summary>
/// The InputController has hardcoded
/// </summary>
public class InputController : MonoBehaviour {

    public string PlayerPrefix;


	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!GameIsPaused())
        {
            ReadPlayerInput();
        }
	}

    // Check and read Input
    

    // Has the game paused
    bool GameIsPaused()
    {
        // Read all the keys
        return false;
    }

    // 
    void ReadPlayerInput()
    {
        
    }
}

using UnityEngine;
using System.Collections;

/// <summary>
/// The InputController has hardcoded
/// </summary>
public class InputController : MonoBehaviour {

/// <summary>
/// Editor Variables
/// </summary>
    public GameObject PlayerObject;
    public float PlayerVelocity = 10.0f;
    public float PlayerJumpHeight = 1.0f;

/// <summary>
/// Private Variables
/// </summary>
    private Player MainPlayer;
    private Rigidbody2D PlayerPhysics;
    private string Prefix="P1";
    private string JumpButton="Jump";
    private string HorizontalAxis="Horizontal";
    private string InteractButton="Interact";
    private string ActionButton="Action";

	// Use this for initialization
	void Start () 
    {
	   if(PlayerObject==null)
       {
           Debug.Log("InputController has null PlayerToControl. Disabling!");
           this.enabled = false;
       }
       else
       {
           MainPlayer = PlayerObject.GetComponent<Player>();
           PlayerPhysics = PlayerObject.GetComponent<Rigidbody2D>();
           if(MainPlayer!=null && PlayerPhysics!=null)
           {
               Prefix = MainPlayer.PlayerNumber.ToString();
               Debug.Log("Setting up for input Player "+Prefix);
               
               JumpButton = string.Format("P{0}{1}",Prefix,JumpButton);
               HorizontalAxis = string.Format("P{0}{1}",Prefix,HorizontalAxis);
               InteractButton = string.Format("P{0}{1}",Prefix,InteractButton);
               ActionButton = string.Format("P{0}{1}",Prefix,ActionButton); 
           }
           else
           {
               Debug.Log("FAILED");
           }
           Debug.Log(InteractButton);
           
       }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!GameIsPaused())
        {
            ReadPlayerInput();
        }
	}

    // Has the game paused
    bool GameIsPaused()
    {
        // Read all the keys
        
        return false;
    }

    // Check and read Input
    void ReadPlayerInput()
    {
        if(Input.GetButton(InteractButton))
        {
            Debug.Log("Interacted!");
        }
        
        if(Input.GetButton(JumpButton) && PlayerPhysics.velocity.y < 9.8f)
        {
            //Give an additional force
            PlayerPhysics.AddForce(
                new Vector2(0, PlayerJumpHeight ),
                ForceMode2D.Impulse
            );
        }
        
        float horizontalIn = Input.GetAxis(HorizontalAxis);
        
        if(horizontalIn != 0)
        {
            Vector2 axisIn = new Vector2(horizontalIn,0);
            axisIn *= PlayerVelocity;
            PlayerPhysics.AddForce(axisIn);
            Debug.Log(axisIn);
        }
        
    }
}

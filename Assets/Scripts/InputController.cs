using UnityEngine;
using System.Collections;

/// <summary>
/// The InputController has hardcoded strings corresponding to Buttons and Joystick Axis in 
///  the Unity Input Manager. Changes there have to be reflected here...
/// </summary>
public class InputController : MonoBehaviour {

/// <summary>
/// Editor Variables
/// </summary>
    public float PlayerVelocity = 6.0f;
    public float PlayerJumpHeight = 6.0f;
    public bool InvertXAxis = false;

/// <summary>
/// Private Variables
/// </summary>
    private GameStateManager GameState;

    private Player MainPlayer;
    private Rigidbody2D PlayerPhysics;
    private BoxCollider2D PlayerCollider;
    private Animator PlayerAnimator;
    private float VariableVelocity;
    
    private int PlayerNumber=1;
    private bool IsMoving=false;
    private float TranslationMovement;
    
    private string JumpButton="Jump";
    private string HorizontalAxis="Horizontal";
    private string InteractButton="Interact";
    private string ActionButton="Action";
    private string Pause="Pause";
    private string Keyboard="Keyboard";
    private string HintButton="Hint";

	// Use this for initialization
	void Start () 
    {
       GameState = FindObjectOfType<GameStateManager>();
       
	   if(GameState==null)
       {
           Debug.Log("GameStateManager missing! Abort!");
           this.enabled = false;
           return;
       }
       else
       {
           MainPlayer = gameObject.GetComponent<Player>();
           PlayerPhysics = gameObject.GetComponent<Rigidbody2D>();
           PlayerCollider = gameObject.GetComponent<BoxCollider2D>();
           PlayerAnimator = gameObject.GetComponent<Animator>();
           if(MainPlayer!=null && PlayerPhysics!=null)
           {
               PlayerNumber = MainPlayer.PlayerNumber;
               Debug.Log("Setting up for input Player "+PlayerNumber);
               
               JumpButton = string.Format("P{0}{1}",PlayerNumber,JumpButton); //TODO: Fix problems with Jump buttons
               HorizontalAxis = string.Format("P{0}{1}",PlayerNumber,HorizontalAxis);
               InteractButton = string.Format("P{0}{1}",PlayerNumber,InteractButton);
               ActionButton = string.Format("P{0}{1}",PlayerNumber,ActionButton);
               HintButton = string.Format("P{0}{1}",PlayerNumber,HintButton);
               Keyboard = string.Format("P{0}{1}",PlayerNumber,Keyboard); 
           }
       }
       
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!GameState.IsGamePaused())
        {
            ReadPlayerInput();
            AnimatePlayer();
        }
	}

    // Check and read Input
    private void ReadPlayerInput()
    {
        if(Input.GetButton(InteractButton))
        {
            Debug.Log("Interacted!");
        }
        
        if(IsGrounded())
        {
            VariableVelocity = PlayerVelocity;
            
            if(Input.GetButtonDown(JumpButton))
            {
                PlayerPhysics.AddForce(
                    new Vector2(0, PlayerPhysics.mass * PlayerJumpHeight ),
                    ForceMode2D.Impulse
                );
            }
        }
        else
        {
            VariableVelocity -= Time.deltaTime*PlayerVelocity/2;
            
        }

        float horizontalJoystickIn = Input.GetAxis(HorizontalAxis);
        float keyboardIn = Input.GetAxis(Keyboard);

        TranslationMovement = Mathf.Abs(keyboardIn) > 0.01f ? keyboardIn : horizontalJoystickIn;
        TranslationMovement *= InvertXAxis ? -1 : 1;
        
        gameObject.transform.Translate(Time.deltaTime * VariableVelocity * TranslationMovement,0,0);
        
        
        if(Input.GetButton(HintButton)) {
            MainPlayer.ShowHint();
        }
    }
    
    private bool IsGrounded()
    {
        return PlayerPhysics.velocity.y < 0.001f && PlayerPhysics.velocity.y > -0.001f;
    }
    
    
    private void AnimatePlayer()
    {
        if(Mathf.Abs(TranslationMovement) < 0.1f)
        {
            PlayerAnimator.SetInteger("state",0);
        }
        else
        {
            PlayerAnimator.SetInteger("state",1);
        }
    }
    
}

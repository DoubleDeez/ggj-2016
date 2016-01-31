using UnityEngine;
using XboxCtrlrInput;

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
    public XboxController XboxInput;

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
    private bool IsInteracting=false;
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
               PlayerNumber = (int) XboxInput;
               Debug.Log("Setting up for input Player "+PlayerNumber);
               
            //    JumpButton = string.Format("P{0}{1}",PlayerNumber,JumpButton); //TODO: Fix problems with Jump buttons
            //    HorizontalAxis = string.Format("P{0}{1}",PlayerNumber,HorizontalAxis);
            //    InteractButton = string.Format("P{0}{1}",PlayerNumber,InteractButton);
            //    ActionButton = string.Format("P{0}{1}",PlayerNumber,ActionButton);
            //    HintButton = string.Format("P{0}{1}",PlayerNumber,HintButton);
            //    Keyboard = string.Format("P{0}{1}",PlayerNumber,Keyboard); 
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
            ReadDebug();
        }
	}

    // Check and read Input
    private void ReadPlayerInput()
    {
        IsInteracting = XCI.GetButton(XboxButton.A);
        if(IsInteracting) {
            foreach(GameStateManager.LevelInteraction interaction in MainPlayer.GetLevelInteractionsColliding()) {
                interaction.HasBeenInteracted = true;
                GameState.DoInteraction(interaction);
            }
        }
        
        if(IsGrounded())
        {
            VariableVelocity = PlayerVelocity;
            
            if(XCI.GetButtonDown(XboxButton.X,XboxInput))
            {
                Jump();
            }
        }
        else
        {
            VariableVelocity -= Time.deltaTime*PlayerVelocity/2;
        }
       
        TranslationMovement =  XCI.GetAxis(XboxAxis.LeftStickX,XboxInput);
       
        gameObject.transform.Translate(Time.deltaTime * VariableVelocity * TranslationMovement,0,0);
         
        if(XCI.GetButton(XboxButton.B,XboxInput)) {
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
    
    public bool Interacted() {
        return IsInteracting;
    }
    
    private void Jump()
    {
        PlayerPhysics.AddForce(
            new Vector2(0, PlayerPhysics.mass * PlayerJumpHeight ),
            ForceMode2D.Impulse
        );
    }
    
    //We want our game to support only Xbox Gamepad input.
    // However, we need hardcoded keyboard input for now...
    private void ReadDebug()
    {
        if(XboxInput == XboxController.First)
        {
            DebugP1();
        }
        else
        {
            DebugP2();
        }
    }
    
    private void DebugP1()
    {
        IsInteracting = Input.GetKeyDown(KeyCode.E);
        if(IsInteracting) {
            foreach(GameStateManager.LevelInteraction interaction in MainPlayer.GetLevelInteractionsColliding()) {
                interaction.HasBeenInteracted = true;
                GameState.DoInteraction(interaction);
            }
        }
        
        if(IsGrounded())
        {
            VariableVelocity = PlayerVelocity;
            
            if(Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }
        }
        else
        {
            VariableVelocity -= Time.deltaTime*PlayerVelocity/2;
        }
       
        
        if(Input.GetKey(KeyCode.A))
        {
            TranslationMovement = -1.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            TranslationMovement = 1.0f;
        }
        else
        {
            TranslationMovement = Mathf.Lerp(TranslationMovement,0.0f,Time.deltaTime);
        }
       
        gameObject.transform.Translate(Time.deltaTime * VariableVelocity * TranslationMovement,0,0);
        
         
        if(Input.GetKeyDown(KeyCode.Q)) {
            MainPlayer.ShowHint();
        }
    }
    
    private void DebugP2()
    {
        IsInteracting = Input.GetKeyDown(KeyCode.RightShift);
        if(IsInteracting) {
            foreach(GameStateManager.LevelInteraction interaction in MainPlayer.GetLevelInteractionsColliding()) {
                interaction.HasBeenInteracted = true;
                GameState.DoInteraction(interaction);
            }
        }
        
        if(IsGrounded())
        {
            VariableVelocity = PlayerVelocity;
            
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
        }
        else
        {
            VariableVelocity -= Time.deltaTime*PlayerVelocity/2;
        }
        
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            TranslationMovement = -1.0f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            TranslationMovement = 1.0f;
        }
        else
        {
            TranslationMovement = Mathf.Lerp(TranslationMovement,0.0f,Time.deltaTime);
        }
       
        gameObject.transform.Translate(Time.deltaTime * VariableVelocity * TranslationMovement,0,0);
         
        if(Input.GetKeyDown(KeyCode.Return)) {
            MainPlayer.ShowHint();
        }
    }
}

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
    
    // state int to be set to for changing animation
    private enum AnimStates {
        Idle=0,
        Charge=2,
        Grenade=3,
        Jump=4,
        Scared=5,
        Touch=6,
        Walk=1,
        Whisper=7
    }

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
               //No setup anymore!
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
        //Interactions
        IsInteracting = XCI.GetButton(XboxButton.A);
        if(IsInteracting) {
            foreach(GameStateManager.LevelInteraction interaction in MainPlayer.GetLevelInteractionsColliding()) {
                interaction.HasBeenInteracted = true;
                GameState.DoInteraction(interaction);
            }
        }
        
        //Jumping
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
       
       //Movement (Horizontal only)
        TranslationMovement =  XCI.GetAxis(XboxAxis.LeftStickX,XboxInput);
       
        gameObject.transform.Translate(Time.deltaTime * VariableVelocity * TranslationMovement,0,0);
         
        if(XCI.GetButton(XboxButton.B,XboxInput)) {
            MainPlayer.ShowHint();
        }
        
        //DPad - Let the player handle this logic 
        if(XCI.GetDPadDown(XboxDPad.Up,XboxInput))
        {
            MainPlayer.OnDPadUp();
        }
        else if( XCI.GetDPadDown(XboxDPad.Down,XboxInput))
        {
            MainPlayer.OnDPadDown();
        }
        else if( XCI.GetDPadDown(XboxDPad.Left,XboxInput))
        {
            MainPlayer.OnDPadLeft();
        }
        else if( XCI.GetDPadDown(XboxDPad.Right,XboxInput))
        {
            MainPlayer.OnDPadRight();
        }
    }
    
    public bool IsGrounded()
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

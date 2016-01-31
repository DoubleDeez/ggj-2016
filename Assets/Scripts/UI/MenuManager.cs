using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

public class MenuManager : MonoBehaviour {

    public GameObject IntroObject;
    public Button StartButton;
    public Button CreditsButton;
    public Button QuitButton;
    public float IntroTime = 8.0f;
    
    private MovieTexture IntroMovie;
    private float ElapsedTime=0.0f;
    
	// Use this for initialization
	void OnEnable () 
    {   
	   IntroMovie = (MovieTexture) IntroObject.GetComponent<RawImage>().texture;
       IntroMovie.Play();
	}
	
	// Update is called once per frame
	void Update () {
        //Wait some time for showing the buttons
        if(ElapsedTime > 40.0f)
        {
            ResetButtons();
            OnEnable();
            ElapsedTime = 0.0f;
        }
        
        if(ElapsedTime > IntroTime && ElapsedTime < 40.0f)
        {
            IncreaseAlpha(StartButton);
            IncreaseAlpha(CreditsButton);
            IncreaseAlpha(QuitButton);
            ReadInput();
        }
        if(StartButton.image.color.a > 0.75f)
        {
            StartButton.GetComponentInChildren<Text>().enabled = true;
            CreditsButton.GetComponentInChildren<Text>().enabled = true;
            QuitButton.GetComponentInChildren<Text>().enabled = true;
        }
        ElapsedTime+=Time.deltaTime;
        
	}
    
    private void ReadInput()
    {
       if(Input.GetButtonDown("Submit"))
       {
           OnPressStart();
       }
       
        if(XCI.GetButtonDown(XboxButton.A))
        {
            OnPressStart();
        }
        if(XCI.GetButtonDown(XboxButton.B))
        {
            OnPressQuit();
        }
        if(XCI.GetButtonDown(XboxButton.X))
        {
            OnPressCredits();
        }
    }
    
    private void IncreaseAlpha(Button aButton)
    {
        aButton.image.color = new Color(aButton.image.color.r,aButton.image.color.g,aButton.image.color.b,aButton.image.color.a+Time.deltaTime);
    }
    
    private void ResetAButton(Button aButton)
    {
        aButton.image.color = new Color(aButton.image.color.r,aButton.image.color.g,aButton.image.color.b,aButton.image.color.a+Time.deltaTime);
        aButton.GetComponentInChildren<Text>().enabled = false;
    }
    
    private void ResetButtons()
    {
        ResetAButton(StartButton);
        ResetAButton(CreditsButton);
        ResetAButton(QuitButton);
    }
    
    public void OnPressStart()
    {
        SceneManager.LoadScene("sandbox");
    }
    
    public void OnPressCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }
    
    public void OnPressQuit()
    {
        Application.Quit();
    }
}

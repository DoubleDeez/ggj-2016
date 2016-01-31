using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject IntroObject;
    public GameObject StartButton;
    public GameObject CreditsButton;
    public GameObject QuitButton;
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
        if(!IntroMovie.isPlaying)
        {
            OnEnable();
            ElapsedTime = 0.0f;
        }
        else if(ElapsedTime > IntroTime)
        {
            StartButton.SetActive(true);
            CreditsButton.SetActive(true);
            QuitButton.SetActive(true);
        }
        ElapsedTime+=Time.deltaTime;
        
	   if(Input.GetButtonDown("Submit"))
       {
           OnPressStart();
       }
	}
    
    private void ResetButtons()
    {
        
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

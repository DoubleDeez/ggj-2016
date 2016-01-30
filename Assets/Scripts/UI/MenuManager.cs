using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	   if(Input.GetButtonDown("Submit"))
       {
           OnPressStart();
       }
	}
    
    public void OnPressStart()
    {
        SceneManager.LoadScene("StartLevel");
    }
}

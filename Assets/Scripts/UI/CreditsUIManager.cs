using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

public class CreditsUIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	   ReadInput();
	}
    
    private void ReadInput()
    {
        if(XCI.GetButtonDown(XboxButton.B))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}

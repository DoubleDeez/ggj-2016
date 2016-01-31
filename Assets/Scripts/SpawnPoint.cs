using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

    public float TimeLimitInSeconds = 60.0f;
    public float TimeRemaining;
    
    private GameStateManager GameStateMan;
    private Vector3 PreviousPlayerPosition;
    private bool IsCountingTime;
    private float TimeStarted;
    private Player SpawnPlayer;
    //Somewhere to warp them back

	// Use this for initialization
	void Start () {
	   GameStateMan = FindObjectOfType<GameStateManager>();
	}
	
	// Update is called once per frame
	void Update () {
	   if(IsCountingTime)
       {
           if(TimeStarted + TimeLimitInSeconds < Time.time)
           {
               RecallPlayer();
           }
           else
           {
               TimeRemaining-=Time.deltaTime;
           }
       }
	}
    
    public void TeleportPlayer(Player aPlayer)
    {
        
        SpawnPlayer = aPlayer;
        if(aPlayer==null)
        {
            return;
        }
        
        PreviousPlayerPosition = new Vector3(aPlayer.transform.position.x,aPlayer.transform.position.y,aPlayer.transform.position.z);
        aPlayer.transform.position = this.transform.position;
        
        if(!aPlayer.DEBUG_BypassTeleportRestictions)
        {
            TimeStarted = Time.time;
            IsCountingTime = true;
            TimeRemaining = TimeLimitInSeconds;
        }
    }
    
    private void RecallPlayer()
    {
        SpawnPlayer.transform.position = PreviousPlayerPosition;
        IsCountingTime = false;
        Debug.Log("New Respawn Position: "+PreviousPlayerPosition);
        SpawnPlayer.NotifyTeleportRecall(this);
    }
}

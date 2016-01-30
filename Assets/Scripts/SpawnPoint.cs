using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

    public float TimeLimitInSeconds = 60.0f;
    
    private GameStateManager GameStateMan;
    private Transform PreviousPlayerPosition;
    private bool IsCountingTime;
    private float TimeStarted;
    private Player SpawnPlayer;
    //Somewhere to warp them back

	// Use this for initialization
	void Start () {
	   GameStateMan = FindObjectOfType<GameStateManager>();
       this.enabled = false; //Have it sleep until called.
	}
	
	// Update is called once per frame
	void Update () {
	   if(IsCountingTime)
       {
           if(TimeStarted + TimeLimitInSeconds < Time.time)
           {
               RecallPlayer();
           }
       }
	}
    
    public void TeleportPlayer(Player aPlayer)
    {
        SpawnPlayer = aPlayer;
        PreviousPlayerPosition = aPlayer.transform;
        TimeStarted = Time.time;
        IsCountingTime = true;
        this.enabled = true;
    }
    
    private void RecallPlayer()
    {
        SpawnPlayer.transform.Translate(PreviousPlayerPosition.transform.position);
        IsCountingTime = false;
    }
}

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    
    public Transform PlayerToFollow;
    public Vector2 CameraFollowDeadZone = new Vector2(3, 3);
    [TooltipAttribute("In units per second")]
    public float CatchUpSpeed = 2.0f;

    private bool CatchUp;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        FollowPlayer();
	}
    
    void OnValidate() {
        Vector3 position = transform.position;
        position.x = PlayerToFollow.position.x;
        transform.position = position;
    }
    
    void FollowPlayer() {
        Vector3 CamCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane));
        if(!CatchUp
            && (PlayerToFollow.position.x > CamCenter.x + CameraFollowDeadZone.x
            || PlayerToFollow.position.x < CamCenter.x - CameraFollowDeadZone.x
            || PlayerToFollow.position.y > CamCenter.y + CameraFollowDeadZone.y
            || PlayerToFollow.position.y < CamCenter.y - CameraFollowDeadZone.y)) {
            CatchUp = true;   
        }
        
        if(CatchUp) {
            Vector3 position = transform.position;
            position.x = Mathf.Lerp(position.x, PlayerToFollow.position.x, Time.deltaTime * CatchUpSpeed);
            position.y = Mathf.Lerp(position.y, PlayerToFollow.position.y, Time.deltaTime * CatchUpSpeed);
            transform.position = position;
        }
    }
}

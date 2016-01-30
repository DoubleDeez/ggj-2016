using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    
    public Transform PlayerToFollow;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        SetPositionToPlayer();
	}
    
    void OnValidate() {
        SetPositionToPlayer();
    }
    
    void SetPositionToPlayer() {
	   Vector3 position = transform.position;
       position.x = PlayerToFollow.position.x;
       transform.position = position;
    }
}

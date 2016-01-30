using UnityEngine;
using System.Collections;

public class FloatingScript : MonoBehaviour {
    
    public float FloatingAmplitude = 1.0f;
    public float FloatingSpeed = 1.0f;
    
    private bool GoingUp;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
       Vector3 position = transform.position;
       position.y += Mathf.Sin(Time.time * FloatingSpeed) * (FloatingAmplitude / 100.0f);
       transform.position = position;
	}
}

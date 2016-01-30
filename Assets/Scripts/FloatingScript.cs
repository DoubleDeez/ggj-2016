using UnityEngine;
using System.Collections;

public class FloatingScript : MonoBehaviour {
    
    public float FloatingAmplitude = 1.0f;
    public float FloatingSpeed = 1.0f;
    
    private float Origin;
    private bool GoingUp;
    
    private float FLOAT_TOLERANCE = 0.00005f;

	// Use this for initialization
	void Start () {
	   Origin = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
       Vector3 position = transform.position;
       position.y = Origin + Mathf.Sin(Time.time * FloatingSpeed) * FloatingAmplitude;
       transform.position = position;
	}
}

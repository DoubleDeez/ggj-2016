using UnityEngine;
using System.Collections;

public class IncomingCarScript : MonoBehaviour {
    
    public Vector3 StartScale = new Vector3(1,1,1);
    public Vector3 MaxScale = new Vector3(3,3,3);
    public float StartY = 10.0f;
    public float EndY = 0.0f;
    public float ScaleSpeed = 2.0f;
    
    public bool RunHimDown = false;

	// Use this for initialization
	void Start () {
	    transform.localScale = StartScale;
        Vector3 position = transform.position;
        position.y = StartY;
        transform.position = position;
	}
	
	// Update is called once per frame
	void Update () {
	   if(RunHimDown) {
           transform.localScale = Vector3.Lerp(transform.localScale, MaxScale, Time.deltaTime * ScaleSpeed);
           Vector3 position = transform.position;
           position.y = Mathf.Lerp(position.y, EndY, Time.deltaTime * ScaleSpeed * 2.0f);
           transform.position = position;
       }
	}
}

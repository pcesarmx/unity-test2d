using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    //public static CameraFollow sharedInstance;
    public Transform target;

    public Vector3 offset = new Vector3(0.2f,0f,-10.0f);

    public float dampTime = 0.03f;

    public Vector3 velocity = Vector3.zero;


	void Awake () {
        Application.targetFrameRate = 60;
        // sharedInstance = this;
	}

    public void ResetCameraPosition() {
        Camera c = GetComponent<Camera>();

        Vector3 point = c.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - c.ViewportToWorldPoint(new Vector3(offset.x, offset.y, point.z));

        Vector3 destination = point + delta;

        // Saltar camara

        destination = new Vector3(target.position.x, offset.y, offset.z);
        this.transform.position = destination;
    }
	
	// Update is called once per frame
	void Update () {
        Camera c = GetComponent<Camera>();

        Vector3 point = c.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - c.ViewportToWorldPoint(new Vector3( offset.x,offset.y,point.z));

        Vector3 destination = point + delta;

        // Saltar camara

        destination = new Vector3(target.position.x,offset.y,offset.z);
        this.transform.position = Vector3.SmoothDamp(this.transform.position,destination,ref velocity, dampTime);
	}
}

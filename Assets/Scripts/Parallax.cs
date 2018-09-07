using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    private Rigidbody2D _rigidbody;
    private Rigidbody2D rigidbodyPlayer;
    private float lastX;
    private static float palyerCurX;
    private static float VEL_TEST;


    public float speed = 0.0f;
    public const float TMP_CONST_L = -34.2f;
    //public const float TMP_CONST_R = -TMP_CONST_L;



    private void Awake()
    {
        this._rigidbody = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
        this.lastX = PlayerController.sharedInstance.transform.position.x;
        this.rigidbodyPlayer = PlayerController.sharedInstance.GetComponent<Rigidbody2D>();
    }

    private void Update(){
        palyerCurX = PlayerController.sharedInstance.transform.position.x;
        Parallax.VEL_TEST = this.rigidbodyPlayer.velocity.x >= 0 ? 1 : -1; // this.rigidbodyPlayer.velocity.x;//2.5f; //Mathf.Abs(this.rigidbodyPlayer.velocity.x);
    }

    private void FixedUpdate()
    {
        if ( Mathf.Approximately(palyerCurX, lastX) ) { // Player is not moving
            _rigidbody.velocity = new Vector2(0, 0);
        } else {
            _rigidbody.velocity = new Vector2(Parallax.VEL_TEST * speed, 0);
        }

        lastX = Parallax.palyerCurX;
        float parentPossition = this.transform.parent.transform.position.x;

        // float TMP_CONST_R = 34.66f;

        //if (this.transform.position.x - parentPossition >= TMP_CONST ) {
        if (this.transform.position.x - parentPossition <= TMP_CONST_L ) {
            this.transform.position = new Vector3(parentPossition-TMP_CONST_L,this.transform.position.y, this.transform.position.z);
        } 
        /*
        if (this.transform.position.x - parentPossition >= TMP_CONST_R) {
            this.transform.position = new Vector3(parentPossition - TMP_CONST_R, this.transform.position.y, this.transform.position.z);
        }*/
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float runningSpped = 1.5f;
    public int damage;

    private Rigidbody2D _rigidbody;

    private bool turnAround;

    // public Enemy sharedInstance;


    private void Awake()
    {
        // this.sharedInstance = this;
        TurnAround = true;
        this._rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        float currentRunningSpeed = runningSpped + (GameManager.sharedInstance.GetScore() / 10);
        // Debug.Log(currentRunningSpeed);
        if ( TurnAround ) {
            this.transform.eulerAngles = new Vector3(0,180.0f,0);
            currentRunningSpeed = -runningSpped;
        } else {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
            currentRunningSpeed = runningSpped;
        }


        if ( GameManager.sharedInstance.currentGameState == GameManager.GameState.inGame  ) {
            //if ( this.rigidbody.velocity.x < runningSpped ) {
                this._rigidbody.velocity = new Vector2(currentRunningSpeed, this._rigidbody.velocity.y);
            //}
        } else {
            this._rigidbody.velocity = new Vector2(0,0);
        }
    }


    public bool TurnAround
    {
        get
        {
            return turnAround;
        }

        set
        {
            turnAround = value;
        }
    }

}

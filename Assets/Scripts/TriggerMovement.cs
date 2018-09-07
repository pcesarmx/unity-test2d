using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMovement : MonoBehaviour {
    private bool movingForward;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("OnTriggerEnter2D => " + collision.tag);
        if (collision.tag == "Collectable" ) {
            return;
        }

        Enemy p = GetComponentInParent<Enemy>();



        if (movingForward) {
            p.TurnAround = true;
            // Enemy.turnAround = true;
        } else {
            p.TurnAround = false;
            // Enemy.turnAround = false;
        }

        movingForward = !movingForward;
    }
}

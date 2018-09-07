using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    private Transform origPossition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player" && other is CompositeCollider2D)
        {
            this.origPossition = PlayerController.sharedInstance.transform.parent;
            //Debug.Log("this.origPossition => " + this.origPossition);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "Player" && other is CompositeCollider2D  ) {
            PlayerController.sharedInstance.transform.parent = this.transform;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player" && (other is CompositeCollider2D)) {
            // Debug.Log("Leaving =>" + other);
            PlayerController.sharedInstance.transform.parent = this.origPossition;
        }
    }

}

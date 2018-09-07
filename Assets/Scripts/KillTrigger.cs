using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.tag == "Player" && collision is CapsuleCollider2D  ) {
            //Debug.Log("KILLL => " + GameManager.sharedInstance.currentGameState+ " c type => " + collision.GetType());
            PlayerController.sharedInstance.kill();
        }
    }
}

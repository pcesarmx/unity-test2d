using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveZone : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(" collision.GetType() => " + collision.GetType());
        //if ( collision.GetType() == new BoxCollider2D().GetType()  ) {
        //if (collision.GetType() == new CompositeCollider2D().GetType())
        if (collision is CompositeCollider2D) {
            //Debug.Log("Leaving zone: Collision TYPE" + collision.GetType());
            LevelGenerator.sharedInstance.AddLevelBlock();
            LevelGenerator.sharedInstance.RemoveOldestLevelBlock();
        }

    }
}

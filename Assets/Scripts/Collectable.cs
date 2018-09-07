using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType {
    healthPotion,
    manaPotion,
    money
}

public class Collectable : MonoBehaviour {
    //bool isCollected = false;
    public int value = 0;
    public CollectableType type = CollectableType.money;
    public AudioClip colletctSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision is CapsuleCollider2D) {
            Collect();
        }
    }

    void Show () {
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<CircleCollider2D>().enabled = true;
    }

	void Hide () {
        this.GetComponent<SpriteRenderer>().enabled = false;

        CircleCollider2D c = this.GetComponent<CircleCollider2D>();
        if (c != null){
            c.enabled = false;
        } else {
            var cc = this.GetComponent<CapsuleCollider2D>();
            cc.enabled = false;
        }


        if (this.transform.childCount > 0 ) {
            this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

    }

    void Collect() {
        //isCollected = true;
        Hide();



        AudioSource _audio = GetComponent<AudioSource>();

        if ( _audio != null && this.colletctSound != null ) {
            _audio.PlayOneShot(this.colletctSound);
        }
            

        switch(this.type) {
            case CollectableType.money:
                GameManager.sharedInstance.CollectObject(this.value);
                break;
            case CollectableType.healthPotion:
                PlayerController.sharedInstance.CollectHealth(this.value);
                break;
            case CollectableType.manaPotion:
                PlayerController.sharedInstance.CollectMana(this.value);
                break;
        }

    }
}

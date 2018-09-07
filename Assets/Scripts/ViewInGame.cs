using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewInGame : MonoBehaviour {
    public Text collectableLabel;
    public Text scoreLabel;
    public Text maxscoreLabel;
    public Text globalmaxscoreLabel;
    public Text globalCollectableLabel;
    //private float tmppoints = 0f;


    // Use this for initialization
    void Start () {
        //this.maxscoreLabel.text = "max score\n" + GameManager.sharedInstance.GetMaxScore().ToString("f2");
        //this.scoreLabel.text = "max score\n" + 0f.ToString("f2") ;
    }
	
	// Update is called once per frame
	void Update () {
        if(GameManager.sharedInstance.currentGameState == GameManager.GameState.inGame ||
           GameManager.sharedInstance.currentGameState == GameManager.GameState.gameOver) {
            int currentObjects = GameManager.sharedInstance.collectedObjects;
            this.collectableLabel.text = currentObjects.ToString();
        }

        // Debug.Log(travelleddistance);
        if (GameManager.sharedInstance.currentGameState == GameManager.GameState.inGame ) {
            //float travelleddistance = PlayerController.sharedInstance.GetDistance();
            this.scoreLabel.text = "Distance\n"+ GameManager.sharedInstance.GetScore().ToString("f2");
        }
        if (GameManager.sharedInstance.currentGameState == GameManager.GameState.gameOver && this.globalmaxscoreLabel != null) {
            this.globalmaxscoreLabel.text = "max score\n" + GameManager.sharedInstance.GetGlobalMaxScore().ToString("f2");
            this.globalCollectableLabel.text = "x " + GameManager.sharedInstance.GetCollectedItems().ToString();
        }
    }

    private void FixedUpdate()
    {
        float tmp = GameManager.sharedInstance.GetMaxScore() + GameManager.sharedInstance.GetScore();
        this.maxscoreLabel.text = "your score\n" + tmp.ToString("f2");
        try {
            if (GameManager.sharedInstance.currentGameState == GameManager.GameState.inGame)
            {
                GameObject.Find("TextLives").GetComponent<Text>().text = new string('♥', GameManager.sharedInstance.GetLivesCount()).Replace("♥", "♥ ");
            }
        } catch  {
            Debug.Log("OKK");
        }

    }
}

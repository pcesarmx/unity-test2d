using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;
    public Canvas menuCanvas, gameCanvas, gameOverCanvas, gameInfoCanvas;
    public enum GameState
    {
        menu,
        inGame,
        gameOver
    }

    public GameState currentGameState = GameState.menu;
    public int collectedObjects = 0;
    public float currentpoints = 0f;
    private float totalpoints = 0f;
    private float globalmaxscore = 0f;
    private int globalmaxscollectables = 0;
    //private float tmppoints = 0;
    private int lives = 2;
    private int _lives;
    private bool menuFlag;
    private bool pauseFlag;
    // private string livesstr = "♥ ♥ ♥ ♥";


    public void Awake()
    {
        sharedInstance = this;
        _lives = lives;
    }

    public void Start()
    {
        //test
        StartGame();
        //
        //setGameState(GameState.menu);
        PlayerPrefs.SetFloat("maxscore", 0);
        PlayerPrefs.SetFloat("maxscollectables", 0);
        this.globalmaxscore = PlayerPrefs.GetFloat("maxscore", 0);
        this.globalmaxscollectables = PlayerPrefs.GetInt("maxscollectables", 0);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Start") && currentGameState != GameState.inGame)
        {
            StartGame();
        }

        if (Input.GetButtonDown("Pause"))
        {
            PauseGame();
        }

        //if (Input.GetButtonDown("Info"))
        //{
        //    ToggleControlInfo();
        //}

        if (Input.GetButtonDown("Menu"))
        {
            if (!menuFlag)
                ToggleMainMenu();
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKey(KeyCode.LeftShift) )
        {
            ExitGame();
        }
#endif

        float travelleddistance = PlayerController.sharedInstance.GetDistance();
        // Debug.Log(travelleddistance);
        if (currentGameState == GameState.inGame && travelleddistance > currentpoints)
        {
            currentpoints = travelleddistance;
            //Debug.Log("Current => " + this.currentpoints);
        }
    }
    /*
    public void StartGame(Event e) {
        Debug.Log("XXXXXXXXXXXXX");
    }*/

    public void StartGame()
    {
         if (currentGameState == GameState.inGame ) {
            menuCanvas.enabled = false;
            return;
        } else if ( pauseFlag && hasLives()) {
            pauseFlag = false;
            setGameState(GameState.inGame);
            return;
        }

        setGameState(GameState.inGame);

        GameObject cameraO = GameObject.FindGameObjectWithTag("MainCamera");
        CameraFollow cameraFollow = cameraO.GetComponent<CameraFollow>();
        cameraFollow.ResetCameraPosition();

        if (PlayerController.sharedInstance.transform.position.x > 12.5)
        {
            LevelGenerator.sharedInstance.RemoveAllTheBlocks();
            LevelGenerator.sharedInstance.GenerateInitialBlocks();
        }

        PlayerController.sharedInstance.StartGame();
        // CameraFollow.sharedInstance.ResetCameraPosition();  
        //EventSystem.current.SetSelectedGameObject(null);
        //collectedObjects = 0;

        if (!hasLives())
        {
            lives = _lives;
            totalpoints = 0;
            collectedObjects = 0;
        } else {
            totalpoints += currentpoints;
        }

        currentpoints = 0;
    }

    public bool hasLives() {
        return lives >= 0;
    }

    public void PauseGame()
    {
        // pauseFlag = !pauseFlag;
        //setGameState(GameState.menu);
        Debug.Log("TODO Method");
    }

    public void GameOver()
    {
        setGameState(GameState.gameOver);
    }

    public void ExitGame()
    {
        Debug.Log("Saliendo .......");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ToggleMainMenu()
    {

        setGameState(GameState.menu);
        //menuCanvas.enabled = !menuCanvas.enabled;
    }

    public void ToggleControlInfo()
    {
        //GameObject go = gameInfoCanvas.GetComponentInChildren<>();
        //RectTransform rt = gameInfoCanvas.transform.GetChild(0).GetComponentInChildren <RectTransform>(); ;
        //Debug.Log(" w => " + " TAG =>" + rt.tag);
        //rt.sizeDelta = new Vector2(1920f, rt.rect.height);
        gameInfoCanvas.enabled = !gameInfoCanvas.enabled;
    }

    public void ShowControlInfo()
    {
        menuFlag = true;
        gameInfoCanvas.enabled = true;
        menuCanvas.enabled = false;
    }

    public void HideControlInfo()
    {
        gameInfoCanvas.enabled = false;
        if (menuFlag) {
            menuCanvas.enabled = true;
            menuFlag = false;
        }
    }

    public void setGameState(GameState state) {
        this.currentGameState = state;
        if (state == GameState.menu) {
            pauseFlag = true;
            menuCanvas.enabled = true;
            gameCanvas.enabled = false;
            gameOverCanvas.enabled = false;
        } else if (state == GameState.inGame) {
            menuCanvas.enabled = false;
            gameCanvas.enabled = true;
            gameOverCanvas.enabled = false;
            gameInfoCanvas.enabled = false;
            pauseFlag = false;
        } else if (state == GameState.gameOver) {
            //menuCanvas.enabled = false;
            //gameCanvas.enabled = false;
            //gameOverCanvas.enabled = true;
            //Debug.Log("TOTAL Points => " + totalpoints);
            float playerMaxScore = PlayerPrefs.GetFloat("maxscore", 0);

            if ( playerMaxScore < (totalpoints + currentpoints) ) {
                globalmaxscore = (totalpoints + currentpoints);
                globalmaxscollectables = collectedObjects;
                PlayerPrefs.SetFloat("maxscore", globalmaxscore);
                PlayerPrefs.SetInt("maxscollectables", globalmaxscollectables);
            }

            lives--;

            if (hasLives())
            {
                //Debug.Log("ZZ");
                StartGame();
            }
            else
            {
                menuCanvas.enabled = false;
                gameCanvas.enabled = false;
                gameInfoCanvas.enabled = false;
                gameOverCanvas.enabled = true;
            }

            /* Deprecated by autostart on kill
             * if (GameObject.Find("PlayButton") != null) {
                if (hasLives())
                {
                    // Hide button play again
                    GameObject.Find("PlayButton").GetComponent<UnityEngine.UI.Button>().interactable = false;
                }
                else
                {
                    GameObject.Find("PlayButton").GetComponent<UnityEngine.UI.Button>().interactable = true;
                    // Show button play again
                }
            }*/


        }

        //this.currentGameState = state;
    }

    public void CollectObject(int objectValue) {
        this.collectedObjects += objectValue;
        //Debug.Log("TOTAL => " + collectedObjects);
    }

    public float GetScore()
    {
        return this.currentpoints;
    }

    public float GetMaxScore()
    {
        return this.totalpoints;
    }

    public float GetGlobalMaxScore()
    {
        return this.globalmaxscore;
    }

    public int GetLivesCount()
    {
        return this.lives;
    }

    public int GetCollectedItems()
    {
        return this.globalmaxscollectables;
    }



}

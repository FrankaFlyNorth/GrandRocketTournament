using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // because we need e.g. the text

public class GM : MonoBehaviour {

    // number of lives
    public int lives = 3;
    // number of bricks
    public int bricks = 20;
    // time in seconds after the game is ended, before we reset it
    public float resetDelay = 1f;

    // the text that shows the lives
    public Text livesText;
    // reference to our "game over"/ "you won" text objects
    public GameObject gameOver;
    public GameObject youWon;

    // to instantiate new bricks
    public GameObject bricksPrefab;
    // to instantiate a new paddle when it gets destroyed because the player lost a live
    public GameObject paddle;
    public GameObject deathParticles;
    // to make an instance from a class (so we can access properties of GM via the class)
    // theres always one instance of the game manager (GM) available (singleton pattern)
    // this is accessable from another script
    public static GM instance = null;

    private GameObject clonePaddle;

	void Awake () {
        // do we have a GM yet? if not, this it is!
		if(instance == null) {
            instance = this;
        }
        // if theres already another GM instance, destroy it (prevents us from having 2 GMs on accident)
        else if (instance != this) {
            Destroy(gameObject);
        }

        Setup();
	}

    /**
     * Setup our game wit a paddle instance and brick instances.
     */
    public void Setup() {
        SetupPaddle();
        // create bricks from the bricks prefab
        Instantiate(bricksPrefab, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
    }

    /**
     * Instantiate a new paddle from
     */
    private void SetupPaddle() {
        // creates a paddle from the paddle prefab at position of the GM (0,0,0) without rotation
        clonePaddle = Instantiate(paddle, transform.position, Quaternion.identity) as GameObject;
    }

    /**
     * Check if the game is ended after we lost a life or a brick is destroyed.
     * Take action.
     */
    private void CheckGameOver() {
        if(bricks < 1 ) {
            // make youWon text object visible
            youWon.SetActive(true);
            // cosmetic! slow motion 
            Time.timeScale = .25f;
            // wait for a reset after a little delay (1 sec)
            Invoke("Reset", resetDelay);
        }

        if(lives < 1) {
            // make gameOber text object visible
            gameOver.SetActive(true);
            Time.timeScale = .25f; // slow motion
            Invoke("Reset", resetDelay);
        }
    }

    /**
     * Reloads the scene.
     */
    private void Reset() {
        // go back to normal time
        Time.timeScale = 1f;
        // reload the last loaded level
        Application.LoadLevel(Application.loadedLevel);
    }

    /**
     * Reduces lives, updates the text and creates a new paddle.
     */
    public void LoseLife() {
        // reduce the live from 1
        lives--;
        // update the text 
        livesText.text = "Lives: " + lives;
        // instantiate death particles where the paddle was
        Instantiate(deathParticles, clonePaddle.transform.position, Quaternion.identity);
        // destroy the old paddle
        Destroy(clonePaddle);
        // instantiate a new paddle after a little delay
        Invoke("SetupPaddle", resetDelay);
        // check if game is over because we lost a live
        CheckGameOver();
    }

    /**
     * Called from the brick game Object if it is destroyed.
     * Reduces the number of bricks, checks if player won.
     */
    public void DestroyBrick() {
        // reduce number of bricks
        bricks--;
        // check if game is over after a brick is destroyed
        CheckGameOver();
    }
}

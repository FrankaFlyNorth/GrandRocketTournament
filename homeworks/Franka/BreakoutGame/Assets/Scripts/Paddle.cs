using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {

    // the speed with which the paddle moves from left to right
    public float paddleSpeed = 1;
    // preset paddle position to the one we gave him in the editor
    private Vector3 playerPos = new Vector3(0, -9.5f, 0);
    
	// Update is called once per frame
	void Update () {
        // calculate our x position
        // where the paddle currently is on the x axis
        // +  horizontal is already defined in input, listens to the a/d/left/right buttons on our keyboard 
        // * paddle speed
        float xPos = transform.position.x + (Input.GetAxis("Horizontal") * paddleSpeed);
        // new player position
        // clamp: limits how far our paddle can move along the x axis (so it cant get outside of the arc)
        // starting from the -9.5f position we have at game start
        playerPos = new Vector3 (Mathf.Clamp(xPos, -8f, 8f), -9.5f, 0f); 
        // apply position
        transform.position = playerPos;
	}
}

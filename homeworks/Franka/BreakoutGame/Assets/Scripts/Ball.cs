using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public float ballInitialVelocity = 600f;

    private Rigidbody rb;
    private bool ballInPlay;
    // public GameObject ballTrailParticles; -> deactivated for now

    // when the game starts 
    void Awake () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        // this is done only once (for the initial state of ballInPlay=false, not repeatedly
		if(Input.GetButtonDown("Fire1") && !ballInPlay) {
            // unparent the ball from the paddle to fly around the level
            transform.parent = null;
            // ball is now in play and this code is not executed in update() again
            ballInPlay = true;
            // remove isKinematic from the rigidbody
            rb.isKinematic = false;
            // launch ball into the initial direction
            rb.AddForce(new Vector3(ballInitialVelocity, ballInitialVelocity, 0));
            // start showing trail of stars -> taken out for now
            // StartTrail();
        }
	}

    /**
     * Initialize the ball trail.
     * Note: I wasnt able to get the desired effect, so I took it out for now.
     */
    /*void StartTrail() {
        // instantiate the trail particles (makes it a root object though!)
        GameObject particle = Instantiate(ballTrailParticles, rb.transform.position, Quaternion.identity) as GameObject;
        // parent it again to the ball so the particles will follow the ball
        particle.transform.parent = rb.transform;
    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour {

    public GameObject brickParticle;

    /**
     * Gets called every time the ball collides with another object (brick)
     */
    private void OnCollisionEnter(Collision other)
    {
        // create the particles for a brick being hit
        Instantiate(brickParticle, transform.position, Quaternion.identity);
        // call the destroyBrick() function from the game manager
        GM.instance.DestroyBrick();
        // destroy this brick object
        Destroy(gameObject);
    }

}

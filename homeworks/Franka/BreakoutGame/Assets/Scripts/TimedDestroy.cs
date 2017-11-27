using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour {

    public float destroyTime = 2f;

	/**
     * to destroy particles (death, brick) after two seconds, so they will disappear from the game and 
     * wont hang there forever, thus cluttering the game scene.
     */
	void Start () {
        Destroy(gameObject, destroyTime);
	}
	
}

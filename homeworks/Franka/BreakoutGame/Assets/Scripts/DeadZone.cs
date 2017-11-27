using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour {

    /**
     * Gets triggered when the object is entered
     */
	void OnTriggerEnter (Collider col)
    {
        GM.instance.LoseLife();
    }
}

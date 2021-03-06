﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class Player : NetworkBehaviour {

	[SyncVar]
	private bool _isDead = false;
	public bool isDead
	{
		get { return _isDead; }
		protected set { _isDead = value; }
	}

	[SerializeField]
	private int maxHealth = 100;

	[SyncVar]
	private int currentHealth;

	public float GetHealthPercentage(){
		return (float) currentHealth / maxHealth;
	}

	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;


	public void Setup()
	{
		wasEnabled = new bool[disableOnDeath.Length];

		for (int i = 0;  i < wasEnabled.Length; i++) {
			wasEnabled[i] = disableOnDeath[i].enabled;			
		}

		SetDefaults ();
	}


	/*
	void Update(){
		if (!isLocalPlayer) {
			return;
		}

		if (Input.GetKeyDown (KeyCode.K)) {
			RpcTakeDamage (9999);
		}
	}
	*/


	// auf allen CLients aufgerufen
	[ClientRpc]
	public void RpcTakeDamage(int amount)
	{
		if (isDead) {
			return;
		}
		
		currentHealth -= amount;
		Debug.Log (transform.name + " hat jetzt " + currentHealth + " Leben");

		if (currentHealth <= 0) {
			Die ();
		}
	}

	private void Die()
	{
		isDead = true;

		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath [i].enabled = false;
		}

		Collider col = GetComponent<Collider> ();
		if (col != null) {
			col.enabled = false;
		}

			
		// DISABLE COMPONENTS
		Debug.Log(transform.name + " is DEAD!");

		// CALL RESPAWN METHOD
		StartCoroutine(Respawn());


	}

	private IEnumerator Respawn(){
		yield return new WaitForSeconds (GameManager.instance.matchSettings.respawnTime);
		SetDefaults ();
		Transform spawnPoint = NetworkManager.singleton.GetStartPosition ();
		transform.position = spawnPoint.position;
		transform.rotation = spawnPoint.rotation;

		Debug.Log (transform.name + " respawned.");
	}


	public void SetDefaults()
	{
		isDead = false;
		currentHealth = maxHealth;

		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath[i].enabled = wasEnabled[i];
		}

		Collider col = GetComponent<Collider> ();
		if (col != null) {
			col.enabled = true;
		}
	}

}

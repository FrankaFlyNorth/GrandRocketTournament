using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	private const string PLAYERTAG = "Player";

	[SerializeField]
	private PlayerWeapon weapon;

	[SerializeField]
	private GameObject[] weaponGFX;
	[SerializeField]
	private string weaponLayerName = "Weapon";


	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	void Start()
	{
		if (cam == null) {
			Debug.LogError ("PlayerShoot: No camera referenced!");
			this.enabled = false;
		}

		foreach (GameObject weaponComponent in weaponGFX) {
			weaponComponent.layer = LayerMask.NameToLayer (weaponLayerName);	
		}

	}

	void Update()
	{
		if (Input.GetButtonDown ("Fire1")) {
			Shoot ();
		}
	}

	// Client Methoden werden nur auf dem Client aufgerufen, nie auf dem Server (lokale Methode)
	[Client]
	void Shoot ()
	{
		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask)) {
			// Wir treffen etwas
			if (hit.collider.tag == PLAYERTAG){
				CmdPlayerShot (hit.collider.name, weapon.damage);
			}
		}
	}

	// Methoden die nur auf dem Server aufgerufen werden
	[Command]
	void CmdPlayerShot(string playerID, int damage)
	{
		Debug.Log (playerID + " has been shot.");

		Player player = GameManager.GetPlayer (playerID);
		player.RpcTakeDamage (damage);
	}

}

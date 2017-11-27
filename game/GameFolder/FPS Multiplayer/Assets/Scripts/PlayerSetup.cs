using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof (Player))]
[RequireComponent(typeof (PlayerController))]
public class PlayerSetup : NetworkBehaviour {

	// Enthaelt alle Komponenten die vom Player deaktiviert werden sollen, wenn es sich nicht um den lokalen Player handelt
	// Ist notwendig, damit z.B. der PlayerController des lokalen Players nicht auch die Bewegung des Players des anderen Clients beeinflusst
	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	[SerializeField]
	private GameObject playerUIPrefab;
	private GameObject playerUIInstance;

	Camera sceneCamera;

	void Start()
	{
		// deaktiviert unnötige Komponenten des nicht lokalen Spielers
		if (!isLocalPlayer) {
			DisableComponents ();
			AssignRemoteLayer ();
		} 
		// wenn lokaler Player erzeugt wurde, dann deaktiviere die sceneCamera
		else {

			//Cursor-Lock (Cursor unsichtbar machen)
			Cursor.lockState = CursorLockMode.Locked;

			sceneCamera = Camera.main;
			if (sceneCamera != null) {
				sceneCamera.gameObject.SetActive (false);
			}

			// PlayerUI erstellen
			playerUIInstance = Instantiate(playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;

			// PlayerUI konfigurieren
			PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
			if (ui == null) {
				Debug.LogError ("Keine PlayerUI Komponente auf dem PlayerUI Prefab.");
			}
			ui.SetPlayer(GetComponent<Player> ());
		}

		GetComponent<Player>().Setup();

	}

	public override void OnStartClient()
	{
		base.OnStartClient ();
		string netID = GetComponent<NetworkIdentity> ().netId.ToString();
		Player player = GetComponent<Player> ();
		GameManager.RegisterPlayer (netID, player);
	}
		

	void AssignRemoteLayer()
	{
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}

	void DisableComponents()
	{
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable [i].enabled = false;
		}
	}

	// wenn der Spieler deaktiviert (oder gelöscht) wurde, dann zeige wieder die sceneCamera
	void OnDisable(){

		// Cursor wieder sichtbar machen
		Cursor.lockState = CursorLockMode.None;

		Destroy (playerUIInstance);

		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive (true);
		}
		GameManager.UnRegisterPlayer (transform.name);

	}

}

using UnityEngine;

public class PlayerUI : MonoBehaviour {


	[SerializeField]
	RectTransform thrusterFuelFill;

	[SerializeField]
	RectTransform healthBarFill;


	private Player player;
	private PlayerController playerController;


	public void SetPlayer(Player player){
		this.player = player;
		playerController = player.GetComponent<PlayerController> ();
	}

	void Update(){
		SetFuelAmount (playerController.GetThrusterFuelAmount ());
		SetHealthAmount (player.GetHealthPercentage());
	}

	void SetFuelAmount(float amount){
		thrusterFuelFill.localScale = new Vector3(1f, amount, 1f);
	}

	void SetHealthAmount(float amount){
		healthBarFill.localScale = new Vector3 (1f, amount, 1f);
	}


}

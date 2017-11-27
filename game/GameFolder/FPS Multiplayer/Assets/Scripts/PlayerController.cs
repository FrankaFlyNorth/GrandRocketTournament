using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	// [SerializeField] macht die Variable im Inspector sichtbar, obwohl sie auf private gestellt ist
	[SerializeField] 
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 3f;

	[SerializeField]
	private float thrusterForce = 1000;
	[SerializeField]
	private float thrusterFuelBurnSpeed = 1f;
	[SerializeField]
	private float thrusterFuelRegenSpeed = 0.3f;
	private float thrusterFuelAmount = 1f;

	public float GetThrusterFuelAmount(){
		return thrusterFuelAmount;
	}


	[Header("Spring Settings:")]
	[SerializeField]
	private float jointSpring = 10f;
	[SerializeField]
	private float jointMaxForce = 30f;

	private PlayerMotor motor;
	private ConfigurableJoint joint;


	void Start()
	{
		motor = GetComponent<PlayerMotor>();
		joint = GetComponent<ConfigurableJoint> ();
		SetJointSettings (jointSpring);
	}

	void Update()
	{
		/*
		 * Bewegungsgeschwindigkeit als 3D Vektor berechnen: 
		*/

		// Unterschied GetAxis() und GetAxisRaw():
		// GetAxis() kann alle Werte zwischen -1 und 1 annehmen, je nach Sensitivität der Eingabe (Eingabe wird smoother)
		// GetAxisRaw() nimmt nur genau die Werte -1, 0 oder 1 an.
		// Hier wird getAxisRaw() verwendet um später selbst die volle Kontrolle über das Smoothing zu haben
		float xMov = Input.GetAxisRaw("Horizontal");
		float zMov = Input.GetAxisRaw ("Vertical");

		Vector3 movHorizontal = transform.right * xMov; //transform.right = [1,0,0]
		Vector3 movVertical = transform.forward * zMov; //transform.forward = [0,0,1]
		// Beispielberechnung movHorizontal:
		// Für xMov = [1,0,0] (Bewegung nach rechts) --> movHorizonzal = [1,0,0]*[1,0,0] = [1,0,0]
		// Für xMov = [0,0,0] (keine Bewegung) --> movHorizonzal = [1,0,0]*[0,0,0] = [0,0,0]
		// Für xMov = [-1,0,0] (Bewegung nach links) --> movHorizonzal = [1,0,0]*[-1,0,0] = [-1,0,0]
		// Analog für movVertical

		// Finaler Bewegungsvektor
		// (movHorizontal + movVertical).normalize liefert Vektor der Länge 1
		// dient praktisch nur als Richtungsvektor
		Vector3 velocity = (movHorizontal + movVertical).normalized * speed;

		/*
		 * Bewegung anwenden:
		*/
		motor.Move(velocity);


		/*
		 * Rotation als 3D Vektor berechnen (Drehung um die y-Achse)
		*/

		float yRot = Input.GetAxisRaw ("Mouse X");
		Vector3 rotation = new Vector3 (0f, yRot, 0f) * lookSensitivity;

		/*
		 * Rotation anwenden
		*/
		motor.Rotate (rotation);


		/*
		 * Kamerarotation als 3D Vektor berechnen (Drehung um die x-Achse)
		*/
		float xRot = Input.GetAxisRaw ("Mouse Y");
		float cameraRotationX = xRot * lookSensitivity;


		/*
		 * Kamerarotation anwenden
		*/
		motor.RotateCamera (cameraRotationX);


		// thrusterForce berechnen je nach Player-Input
		Vector3 thrusterForce = Vector3.zero;
		if (Input.GetButton ("Jump") && thrusterFuelAmount > 0f) {
			thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

			if (thrusterFuelAmount >= 0.01f) {
				thrusterForce = Vector3.up * this.thrusterForce;
				SetJointSettings (0f);
			}

		} else {
			thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
			SetJointSettings (jointSpring);
		}

		thrusterFuelAmount = Mathf.Clamp (thrusterFuelAmount, 0f, 1f);
			

		/*
		 * thrusterForce anwenden
		*/
		motor.ApplyThruster (thrusterForce);



		// Cursor toggle
		if (Input.GetKeyDown (KeyCode.Escape)) {
			
			if(Cursor.lockState == CursorLockMode.Locked){
				Cursor.lockState = CursorLockMode.None;	
			} else {
				Cursor.lockState = CursorLockMode.Locked;
			}
		}

	}

	private void SetJointSettings(float jointSpring){
		joint.yDrive = new JointDrive{
			positionSpring = jointSpring,
			maximumForce = jointMaxForce
		};
	}
}

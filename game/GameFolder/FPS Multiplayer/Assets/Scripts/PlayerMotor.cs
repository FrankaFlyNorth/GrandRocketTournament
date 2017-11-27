using UnityEngine;


[RequireComponent (typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float cameraRotationX = 0f;
	private float currentCameraRotationX = 0f;
	private Vector3 thrusterForce = Vector3.zero;

	[SerializeField]
	private float cameraRotationLimit = 85f;

	private Rigidbody rb;


	void Start()
	{
		rb = GetComponent<Rigidbody> ();
	}


	// Bekommt den Bewegungsvektor aus dem PlayerController
	public void Move (Vector3 velocity)
	{
		this.velocity = velocity;
	}

	// Bekommt den Rotationsvektor aus dem PlayerController
	public void Rotate (Vector3 rotation)
	{
		this.rotation = rotation;
	}

	// Bekommt den Kamerarotationsvektor aus dem PlayerController
	public void RotateCamera (float cameraRotationX)
	{
		this.cameraRotationX = cameraRotationX;
	}

	// Bekommt einen Force-Vector für unseren Thruster
	public void ApplyThruster(Vector3 thrusterForce){
		this.thrusterForce = thrusterForce;
	}


	// Läuft bei jedem Physics Durchlauf
	void FixedUpdate()
	{
		PerformMovement ();
		PerformRotation ();
	}

	// Führt die Bewegung aus, basierend auf dem Bewegungsvektor velocity
	void PerformMovement()
	{
		// wenn wir uns wirklich bewegen wollen
		if (velocity != Vector3.zero)
		{
			rb.MovePosition (rb.position + velocity * Time.fixedDeltaTime);
		}

		if (this.thrusterForce != Vector3.zero) {
			rb.AddForce (this.thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
		}
	}

	// Führt die Rotation aus, basierend auf dem Rotationsvektor und der Kamerarotation
	void PerformRotation()
	{
		rb.MoveRotation (rb.rotation * Quaternion.Euler (rotation));

		if (cam != null)
		{
			// Rotation setzen und begrenzen
			currentCameraRotationX -= cameraRotationX;
			currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
			// Rotation auf die Transformation unserer Kamera anwenden
			cam.transform.localEulerAngles = new Vector3 (currentCameraRotationX, 0f, 0f);
		}
	}

}

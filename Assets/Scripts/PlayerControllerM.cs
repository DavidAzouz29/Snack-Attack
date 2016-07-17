using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControllerM : MonoBehaviour {

	// Public variables
	// Movement
	public float moveSpeed; // Acceleration speed
	public float maxSpeed; // Max movement speed
	public float rotSpeed = 0.4f; // Rotation speed
	// Private variables
	private float deadzone = 0.25f; // Analog stick deadzone
	private Rigidbody rb; // Player's rigidbody component
	private Vector3 movementInput; // Input for movement
	private Vector3 rotationInput; // Input for rotation
	private Vector3 rotation; // The current rotation of the player

	public int playerNumber = 1; // Player number
	private string movementXName; // Name of the left stick horizontal movement
	private string movementYName; // Name of the left stick vertical movement
	private string rotationXName; // Name of the right stick horizontal movement
	private string rotationYName; // Name of the right stick vertical movement

	// Use this for initialization
	void Start () {

//		boostLife = boostMax;

		rb = GetComponent<Rigidbody> (); // Setting rb as player's rigidbody component

		movementXName = "LSX" + playerNumber; // Combining LSX and the player number to assign the right controls to the right players
		movementYName = "LSY" + playerNumber; // ""
		rotationXName = "RSX" + playerNumber; // ""
		rotationYName = "RSY" + playerNumber; // ""

	}

	// Physics update (fixed at 60fps)
	void FixedUpdate () {

		Movement ();

	}

	void Movement () {

		movementInput = new Vector3 (Input.GetAxis (movementXName), 0, Input.GetAxis (movementYName)); // Setting up movement input (left stick)
		rotationInput = new Vector3 (Input.GetAxis (rotationXName), 0, Input.GetAxis (rotationYName)); // Setting up rotation input (right stick)

		if (movementInput.magnitude > 1) { // Will check if movement input length is greater than 1
			movementInput.Normalize (); // If so, it will be set back to 1, this will still keep the same direction that the player is moving the left stick
		}
		if (movementInput.magnitude < deadzone) { // Will check if movement input length is lower than the deadzone amount
			movementInput = Vector3.zero; // If so, it will do nothing (this is just to avoid joystick drift or whatever you would like to call it, may fix some other small problems too!)
		}
		if (rotationInput.magnitude < deadzone) { // Same deal here as above, except for rotation
			rotationInput = Vector3.zero;
		}

		if (rb.velocity.magnitude < maxSpeed) { // Checks if the velocity of the player is lower the maxSpeed amount
			rb.AddForce (movementInput * moveSpeed); // If it is, it will continue to add force (which is the length of movement input multiplied by moveSpeed) in the movement input direction
		}
		if (rotationInput.magnitude > deadzone) { // Checks if rotation input length is greater than the deadzone, and if it is...
			rotation = transform.rotation.eulerAngles; // ...Checks current rotation of the player
			transform.rotation = Quaternion.LookRotation (rotationInput); // ...Will set player's rotation to look direction of the player to rotation input direction (desired rotation)
			transform.rotation = Quaternion.Lerp (Quaternion.Euler (rotation), transform.rotation, rotSpeed); //...Will smoothly rotate from the player's current rotation, to the desired rotation
		}

	}
}
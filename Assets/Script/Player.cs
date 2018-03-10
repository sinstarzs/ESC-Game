using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	private float inputDirection;	// x-value of MoveVector
	private float verticalVelocity;	// y-value of MoveVector
	private float zBalance;			// z-value of MoveVector (to stay at z=0)
	private float speed = 8.0f;
	private float gravity = 1.0f;
	private float jumpForce = 12.0f;
	private bool secondJumpAvail = false;

	private Vector3 moveVector;		// (float,float,float) value
	private CharacterController controller;


	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		inputDirection = Input.GetAxis ("Horizontal")*speed;

		if (isControllerGrounded())	// while grounded
		{
			verticalVelocity = -0.1f;
			//Debug.Log ("grounded");
		
			if (Input.GetKeyDown (KeyCode.Space)) {
				Debug.Log ("jump!");
				verticalVelocity = jumpForce;
				secondJumpAvail = true;
			}
		} else	// while not grounded
		{
			//Debug.Log ("falling");
			verticalVelocity -= gravity;
			if (Input.GetKeyDown (KeyCode.Space) && secondJumpAvail)
			{
				verticalVelocity = jumpForce;	// make player jump only when grounded
				secondJumpAvail = false;
				Debug.Log ("2nd Jump!");
			}
		}

		zBalance = -this.transform.position.z; // to correct any deviations in z-axis
		moveVector = new Vector3 (inputDirection , verticalVelocity, zBalance);
		controller.Move (moveVector * Time.deltaTime);
	}

	private bool isControllerGrounded ()
	{
		Vector3 leftRayStart;
		Vector3 rightRayStart;
		Vector3 buffer;

		buffer = new Vector3 (0, -0.1f, 0);

		leftRayStart = controller.bounds.center;
		rightRayStart = controller.bounds.center;

		leftRayStart.x -= controller.bounds.extents.x;	// extend ray to left of player
		rightRayStart.x += controller.bounds.extents.x;	// extend ray to right of player

		leftRayStart.y -= controller.bounds.extents.y;	// extend ray to bottom of player
		rightRayStart.y -= controller.bounds.extents.y;	// extend ray to bottom of player

		Debug.DrawRay (leftRayStart, buffer, Color.red);
		Debug.DrawRay (rightRayStart, buffer, Color.green);

		if (Physics.Raycast (leftRayStart, Vector3.down, 0.1f) ) // origin, direction, distance
			return true;

		if (Physics.Raycast (rightRayStart, Vector3.down, 0.1f)) 
			return true;

		return false;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)	// hit is the object collided
	{

		// Objects
		switch (hit.gameObject.tag)
		{
		case "Star":
			Debug.Log ("star collected");
			LevelManager.Instance.CollectStar ();
			Destroy (hit.gameObject);
			break;

		case "Spikes":
			LevelManager.Instance.TakeDamage (2);
			break;

		case "Finish":
			LevelManager.Instance.Finish ();
			break;

		default:
			break;
		}
	}
}
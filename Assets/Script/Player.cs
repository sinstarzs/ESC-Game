using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	private float inputDirection;	// x-value of MoveVector
	private float verticalVelocity;	// y-value of MoveVector
	private float speed = 8.0f;
	private float gravity = 1.0f;
	private float jumpForce = 12.0f;
	private bool secondJumpAvail = false;

	private Vector2 moveVector; // (float,float) value
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
			verticalVelocity = 0;
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
			
		moveVector = new Vector2 (inputDirection , verticalVelocity);
		controller.Move (moveVector * Time.deltaTime);
	}

	private bool isControllerGrounded ()
	{
		Vector3 leftRayStart;
		Vector3 rightRayStart;

		leftRayStart = controller.bounds.center;
		rightRayStart = controller.bounds.center;

		leftRayStart.x -= controller.bounds.extents.x;	// extend ray to left of cube
		rightRayStart.x += controller.bounds.extents.x;	// extend ray to right of cube

		//Debug.DrawRay (leftRayStart, Vector3.down, Color.red);
		//Debug.DrawRay (rightRayStart, Vector3.down, Color.green);

		if (Physics.Raycast (leftRayStart, Vector3.down, (controller.height / 2) + 0.1f)) 
			return true;

		if (Physics.Raycast (rightRayStart, Vector3.down, (controller.height / 2) + 0.1f)) 
			return true;

		return false;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)	// hit is the object collided
	{

		// Collectibles
		switch (hit.gameObject.tag)
		{
		case "Star":
			Debug.Log ("star collected");
			LevelManager.Instance.CollectStar ();
			Destroy (hit.gameObject);
			break;
		default:
			break;
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float speed;
	private Rigidbody2D myRigidbody;
	private bool moving;

	public float timeBetweenMovement;
	private float timeBetweenMovementCounter;
	public float timeToMovement;
	private float timeToMovementCounter;

	private Vector3 movementDirection;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();

		timeBetweenMovementCounter = timeBetweenMovement;
		timeToMovementCounter = timeToMovement;
	}
	
	// Update is called once per frame
	void Update () {
		if (moving) 
		{
			timeToMovementCounter -= Time.deltaTime;
			myRigidbody.velocity = movementDirection;

			if (timeToMovementCounter < 0f) 
			{
				moving = false;
				timeBetweenMovementCounter = timeBetweenMovement;
			}

		} else {
			timeBetweenMovementCounter -= Time.deltaTime;
			myRigidbody.velocity = Vector2.zero;

			if (timeBetweenMovementCounter < 0f) 
			{
				moving = true;
				timeToMovementCounter = timeToMovement;
				movementDirection = new Vector3 (Random.Range (-1f, 1f) * speed, Random.Range (-1f, 1f) * speed, 0f);
			}
		}

	}
}

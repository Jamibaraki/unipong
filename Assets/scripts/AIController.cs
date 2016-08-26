using UnityEngine;
using System.Collections;


//Unipong AI bat controller
public class AIController : MonoBehaviour {

	public GameObject ball;
	public float speed;
	private float topBound = 8.8f;
	private float bottomBound = -6.8f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		float verticalMovement;
		if( ball.transform.position.y < transform.position.y )
		{
			verticalMovement = -0.1f;
		}
		else
		{
			verticalMovement = 0.1f;
		}

		Vector3 movement = new Vector3 (0, verticalMovement, 0) * speed * Time.deltaTime;
		transform.Translate( movement );




		//check bounds
		if (transform.position.y > topBound) 
		{
			Vector3 newPosition = transform.position;
			newPosition.y = topBound;
			transform.position = newPosition;
		} 
		else if(transform.position.y < bottomBound)
		{
			Vector3 newPosition = transform.position;
			newPosition.y = bottomBound;
			transform.position = newPosition;
		}

	}
}

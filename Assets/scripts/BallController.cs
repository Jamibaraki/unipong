using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	public GUIText p1Text;
	public GUIText p2Text;
	public int speed;             
	public GUIText debugText;
	public AudioClip plik;
	public AudioClip ping;

	public AudioClip point;

	private Vector2 direction;
	private int startSpeed;
	private int p1Score = 0;
	private int p2Score = 0;
	private int maxScore = 5;

	//bat hits only take place if transform position difference is above this.
	//this is to prevent hits after ball moves behind front face of bat.
	private const float batHitThreshold = 0.55f;


	//we want the ball to bounce of the thin end of the bats sometimes,
	//but don't want the physics to be arsed up when the ball gets trapped
	//between the bat and the walls, so set a threshold to ignore the effect
	private const float topBound = 8f;
	private const float bottomBound = -6f;

	// Use this for initialization
	void Start () 
	{
		direction = new Vector2(1,1);
		//debugText.text = "start";
		if( (ApplicationModel.VERSION == ApplicationModel.DOMAIN_LOCKED_WEB_VERSION) ||
		   ApplicationModel.VERSION == ApplicationModel.ANDROID_VERSION ||
		   (ApplicationModel.VERSION == ApplicationModel.LIVE_WEB_VERSION ))
		{
			debugText.enabled = false;
		}

		startSpeed = speed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 currentPosition = transform.position;
		currentPosition.x = currentPosition.x + ( speed * direction.x * Time.deltaTime);
		currentPosition.y = currentPosition.y + ( speed * direction.y * Time.deltaTime);

		transform.position = currentPosition;


		//calculate bounce
	}

	/*
	void OnCollisionEnter( Collision collision )
	{
		debugText.text = "colision";


		ContactPoint contact = collision.contacts[0];
		Vector3 normal = contact.normal;
		Vector3 inverseNormal = transform.InverseTransformDirection(normal);
		Vector3 roundNormal = RoundVector3(inverseNormal);

		ReturnSide(roundNormal);
	}

	Vector3 RoundVector3(Vector3 target)
	{
		int x = (int) Mathf.Round(target.x);
		int y = (int) Mathf.Round(target.y);
		int z = (int) Mathf.Round(target.z);
		Vector3 returnVector = new Vector3(x,y,z);
		return returnVector;
	}

	void ReturnSide (Vector3 side) {
		string output = "null";

		if( Vector3.Equals( side, Vector3.down ))
		{
			output = "Down";
			direction.y = -1;
			AudioSource.PlayClipAtPoint(plik, transform.position);
		}

		if( Vector3.Equals( side, Vector3.up ))
		{
			output = "Up";
			direction.y = 1;
			AudioSource.PlayClipAtPoint(plik, transform.position);
		}
			
		//if( Vector3.Equals( side, Vector3.back ))
		//	output = "Back";

		if( Vector3.Equals( side, Vector3.left ))
		{
			output = "left";
			speed++;
			direction.x *= -1;

		}

		if( Vector3.Equals( side, Vector3.right ))
		{
			output = "right";
			speed++;
			direction.x *= -1;
		}

		debugText.text = (output+" was hit.");
		//Debug.Log(output+" was hit.");

	}
	*/


	bool checkCollisionThreshold(Collider other)
	{
		debugText.text = other.transform.position.y.ToString();
		//debugText.text = (transform.position.x - other.gameObject.transform.position.x).ToString();
		if( Mathf.Abs(transform.position.x - other.gameObject.transform.position.x) > batHitThreshold)
		{
			return true;
		}

		return false;
	}

	bool checkBatHeightThreshold(Collider other)
	{
		if ( (other.gameObject.transform.position.y < topBound) && (other.gameObject.transform.position.y > bottomBound) )
		{
			return true;
		}

		return false;
	}

	//set trigger status of objects in editor if we want to restore this
	void OnTriggerEnter(Collider other) 
	{
		//if (other.gameObject.tag == "PickUp") 
		if (true) 
		{
			//debugText.text = other.gameObject.name;


			switch( other.gameObject.name )
			{
			case "WallTop":
			case "WallBottom":
				direction.y *= -1;
				AudioSource.PlayClipAtPoint(plik, transform.position);
				break;
			case "PlayerBat":
				if( checkCollisionThreshold(other))
				{
					direction.x = 1;
					speed++;
					AudioSource.PlayClipAtPoint(ping, transform.position);
				}
				else if( checkBatHeightThreshold(other))
				{
					direction.y *= -1;
					AudioSource.PlayClipAtPoint(ping, transform.position);
				}
				//checkSide( other );
				break;
			case "AIBat":
				if( checkCollisionThreshold(other))
				{
					direction.x = -1;
					speed++;
					AudioSource.PlayClipAtPoint(ping, transform.position);
				}
				else if( checkBatHeightThreshold(other))
				{
					direction.y *= -1;
					AudioSource.PlayClipAtPoint(ping, transform.position);
				}
				break;
			case "WallRight":
				//player one scores a point when ball hits the right wall
				speed = startSpeed;
				p1Score++;
				p1Text.text = p1Score.ToString();
				newRound();
				break;
			case "WallLeft":
				//player two scores a point when ball hits the right wall
				speed = startSpeed;
				p2Score++;
				p2Text.text = p2Score.ToString();
				newRound();
				break;
			default:
				print ("BallController.onTriggerEnter unrecognized collision object");
				break;
			}

			//other.gameObject.SetActive( false );

			//count = count+1;
			//SetCountText();
		}

	}

	//figure out which side of object has cause collision
	void checkSide(Collider other)
	{

	}

	//after a point scored.. start a new round (or end game)
	void newRound()
	{
		//check if there is a winner
		if( p1Score >= maxScore )
		{
			ApplicationModel.winner = 1;
			homeScreen ();
			return;
		}

		if( p2Score >= maxScore )
		{
			ApplicationModel.winner = 2;
			homeScreen ();
			return;
		}

		//restart round (play point sound)
		AudioSource.PlayClipAtPoint(point, transform.position);
		//reset ball position and speed, direction will be unchanged
		transform.position = new Vector3(-2.2f,4.85f,6.85f);
		speed = startSpeed;
	}

	//head back to the home screen
	void homeScreen()
	{
		Application.LoadLevel("pongWelcomeScreen");
		this.gameObject.SetActive( false );
	}

}

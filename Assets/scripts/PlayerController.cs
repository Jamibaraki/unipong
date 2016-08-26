using UnityEngine;
using System.Collections;

//Unipong player bat controller
public class PlayerController : MonoBehaviour {

	public float speed;
	private float topBound = 8.8f;
	private float bottomBound = -6.8f;
	public GUIText debugText;
	public GUITexture pause;

	//private Rect windowRect = new Rect( (Screen.width/2) - 110, (Screen.height/2)-75, 220, 150);
	private Rect windowRect;
	private int pausePopupWidth;
	private int pausePopupHeight;

	private int midpoint;

	private bool showGUI = false;

	void Start()
	{
		//setup pause window based on version
		switch( ApplicationModel.VERSION )
		{
		case ApplicationModel.ANDROID_VERSION:
			pausePopupWidth = 420;
			pausePopupHeight = 300;
			break;
		case ApplicationModel.DOMAIN_LOCKED_WEB_VERSION:
		case ApplicationModel.LIVE_WEB_VERSION:
		case ApplicationModel.TEST_WEB_VERSION:
			pausePopupWidth = 220;
			pausePopupHeight = 150;
			break;
		}

		windowRect = new Rect( (Screen.width/2) - (pausePopupWidth/2), (Screen.height/2)-(pausePopupHeight/2), pausePopupWidth, pausePopupHeight);
		midpoint = Screen.height / 2;
	}

	//pause was touched or clicked
	void pauseHit()
	{

		Time.timeScale = 0;
		showGUI = true;
		debugText.text = "pause was hit";
	}

	void DoMyWindow(int windowID) 
	{
		GUIStyle bstyle = new GUIStyle("button");
		bstyle.fontSize = 60;

		//continue button
		if (GUI.Button(new Rect(10, 55, 400, 100), "Continue", bstyle) )
		{
			//print("Got a click");
			showGUI = false;
			Time.timeScale = 1;
		}
		//quit button
		if (GUI.Button(new Rect(10, 180, 400, 100), "Quit", bstyle ))
		{
			ApplicationModel.winner = 0;
			Application.LoadLevel("pongWelcomeScreen");
		}
			
		
	}

	void OnGUI()
	{
		if( showGUI )
		{
			//
			GUIStyle wstyle = new GUIStyle("window");
			wstyle.fontSize = 35;

			//this.useGUILayout = true;
			windowRect = GUI.Window(0, windowRect, DoMyWindow, "Game Paused",wstyle);
		}

	}


	// Update is called once per frame
	void FixedUpdate () 
	{
		//look for joypad
		float moveVertical = Input.GetAxis ("Vertical");
		//debugText.text = moveVertical.ToString ();


		//check for clicks
		if(Input.GetMouseButtonDown(0))
		{
			if( pause.HitTest(Input.mousePosition) )
			{
				pauseHit ();
				return;
			}
		}

		//look for touch input
		if(Input.touchCount > 0)
		{



			Touch touch = Input.GetTouch(0);


			//check if touch has hit ui..
			if( ( pause.HitTest( touch.position ) == true ) && (touch.phase == TouchPhase.Began) )
			{
				pauseHit();
				return;
			}


			//confirm/convert this to percentage of screen height
			float yTouch = touch.position.y;
			if( yTouch > midpoint )
			{
				moveVertical = 0.6f;
			}
			else
			{
				moveVertical = -0.6f;
			}
			//debugText.text = "touch"+touch.position.y.ToString();
		}

		Vector3 movement = new Vector3 (0, moveVertical, 0) * speed * Time.deltaTime;



		//rigidbody.AddForce (movement);
		//rigidbody.MovePosition (movement * speed * Time.deltaTime);
		//rigidbody.AddExplosionForce (10, movement, 5, 3);
		//rigidbody.AddRelativeForce (movement * speed * Time.deltaTime);
		//rigidbody.AddRelativeTorque (movement * speed * Time.deltaTime);
		transform.Translate( movement );

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

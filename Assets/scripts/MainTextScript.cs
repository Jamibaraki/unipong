using UnityEngine;
using System.Collections;
//using GoogleMobileAds;

public class MainTextScript : MonoBehaviour {

	// Use this for initialization
	//public AudioClip win;
	public GUITexture soundon;
	public GUITexture soundoff;
	//public GUIText debugText;

	public AudioClip drumLoop;
	public AudioClip endSound;

	private string gameTitle = "Unipong";

	void Start () 
	{
		//domain locking
		if( ApplicationModel.VERSION == ApplicationModel.DOMAIN_LOCKED_WEB_VERSION)
		{
			//Application.ExternalEval("if(document.location.host != 'www.flashgamelicense.com') { document.location='www.flashgamelicense.com'; }");
			Application.ExternalEval("if(document.location.host != 'www.fgl.com') { document.location='www.flashgamelicense.com'; }");
			/*
			string urladdress = Application.absoluteURL;
			if( (urladdress != "https://www.flashgamelicense.com") && (urladdress != "https://www.fgl.com")){
				Application.ExternalEval("if(true) { document.location='www.flashgamelicense.com'; }");
			}
			*/
		}

		/*
		if( (ApplicationModel.VERSION == ApplicationModel.DOMAIN_LOCKED_WEB_VERSION) ||
		   (ApplicationModel.VERSION == ApplicationModel.LIVE_WEB_VERSION ))
		{
			debugText.enabled = false;
		}

		//debugText.text = Application.absoluteURL;
		debugText.text = "test";
		*/

		//setting timescale to 1, in case we got back to this screen via quitting
		Time.timeScale = 1;

		//retain ui between scenes
		//DontDestroyOnLoad(soundon);
		//DontDestroyOnLoad(soundoff);

		switch(ApplicationModel.winner)
		{
			case 0:
				this.GetComponent<GUIText>().text = "Welcome to "+gameTitle+".\nTap to Start";
				//audio.loop = true;
				//audio.clip = drumLoop;
				GetComponent<AudioSource>().pitch = 0.7f;
				//audio.Play();
				break;
			case 1:
				this.GetComponent<GUIText>().text = "Player 1 wins.\nTap to Start";
				//AudioSource.PlayClipAtPoint(win, transform.position);
				GetComponent<AudioSource>().Play();
				break;
			case 2:
				this.GetComponent<GUIText>().text = "Player 2 wins.\nTap to Start";
				GetComponent<AudioSource>().Play();
				//AudioSource.PlayClipAtPoint(win, transform.position);
				break;
			default:
				this.GetComponent<GUIText>().text = "Welcome to "+gameTitle+".\nTap to Start";
				break;
		}

		//update the ui to reflect the current audio state
		if( AudioListener.pause == true )
		{
			soundOff ();
		}
		else
		{
			soundOn ();
		}


		//this.guiText.text = ApplicationModel.winner.ToString();
	}

	void soundOn()
	{
		soundon.enabled = true;
		soundoff.enabled = false;
		AudioListener.pause = false;
	}

	void soundOff()
	{
		soundoff.enabled = true;
		soundon.enabled = false;
		AudioListener.pause = true;
	}

	
	void Update() 
	{
		//check for ui touch and flip sound on/off if so


		foreach (Touch t in Input.touches)
		{

			if( ( soundon.HitTest( t.position ) == true ) && (t.phase == TouchPhase.Began) && (soundon.enabled == true ) )
			{
				soundOff();
				return;
			}

			if( ( soundoff.HitTest( t.position ) == true ) && (t.phase == TouchPhase.Began) )
			{
				soundOn ();
				return;
			}
		}


		if (Input.GetMouseButtonDown(0))
		{

			//check if we hit sound on or sound off with mouse
			if( soundon.HitTest(Input.mousePosition) && (soundon.enabled == true ) )
			{
				soundOff ();
				return;
			}

			if( soundoff.HitTest(Input.mousePosition) )
			{
				soundOn ();
				return;
			}

			//this.guiText.text="Pressed left click.";
			Application.LoadLevel ("pongGameScene");
			//debugText.text = "mbd";
		}
			
		/*
		if (Input.GetMouseButtonDown(1))
			this.guiText.text="Pressed right click.";
		
		if (Input.GetMouseButtonDown(2))
			this.guiText.text="Pressed middle click.";
		*/	
		
	}
}

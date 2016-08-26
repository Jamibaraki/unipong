using UnityEngine;
using System.Collections;

public class ApplicationModel : MonoBehaviour {

	static public int winner = 0;

	public const int DOMAIN_LOCKED_WEB_VERSION = 0;
	public const int TEST_WEB_VERSION = 1;
	public const int LIVE_WEB_VERSION = 2;
	public const int ANDROID_VERSION = 3;


	//static public int VERSION = ApplicationModel.DOMAIN_LOCKED_WEB_VERSION;
	static public int VERSION = ApplicationModel.ANDROID_VERSION;
	
}

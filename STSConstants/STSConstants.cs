//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

#if UNITY_MENU_IDEMOBI
using UnityMenuIdemobi;
#endif

//=====================================================================================================================
namespace SceneTransitionSystem
{
	public partial class STSConstants
	{//-------------------------------------------------------------------------------------------------------------
		// Menu Strings
		#if UNITY_MENU_IDEMOBI
		public const string K_MENU_BASE = UMIConstants.K_MENU_IDEMOBI;
		#else
		public const string K_MENU_BASE = "Window";
		#endif
		public const string K_MENU_IDEMOBI = 						K_MENU_BASE+"/Scene Transition System by ideMobi";
		//-------------------------------------------------------------------------------------------------------------
		// Idemobi alert Strings
		public const string K_ALERT_IDEMOBI_TITLE = 				"SceneTransitionSystem";
		public const string K_ALERT_IDEMOBI_MESSAGE = 				"SceneTransitionSystem is an idéMobi module to  Scene Transition System in your game.";
		public const string K_ALERT_IDEMOBI_OK = 					"Thanks!";
		public const string K_ALERT_IDEMOBI_SEE_DOC = 				"See online docs";
		public const string K_ALERT_IDEMOBI_DOC_HTTP = 				"http://www.idemobi.com/scenetransitionsystem";

		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
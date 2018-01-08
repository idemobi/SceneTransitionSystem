//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace SceneTransitionSystem
{
	/// <summary>
	/// STS editor menu.
	/// </summary>
	public class STSEditorMenu 
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Idemobis the editor info show.
		/// </summary>
		[MenuItem (STSConstants.K_MENU_IDEMOBI,false, 100)]
		public static void IdemobiInfoShow()
		{
			if (EditorUtility.DisplayDialog (STSConstants.K_ALERT_IDEMOBI_TITLE,
				STSConstants.K_ALERT_IDEMOBI_MESSAGE,
				STSConstants.K_ALERT_IDEMOBI_OK,
				STSConstants.K_ALERT_IDEMOBI_SEE_DOC)) {
			} else {
				Application.OpenURL (STSConstants.K_ALERT_IDEMOBI_DOC_HTTP);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif
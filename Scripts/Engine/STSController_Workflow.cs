//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;

//=====================================================================================================================
namespace SceneTransitionSystem
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class STSController : MonoBehaviour, STSTransitionInterface, STSIntermediateInterface
	{
		//-------------------------------------------------------------------------------------------------------------
		public void New_AddScene(string sNextActiveScene, string sIntermediate = null)
		{
			if (string.IsNullOrEmpty(sIntermediate) == false)
			{
			}
			else
			{
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void New_AddScenes(string sNextActiveScene, string[] sAdditionalScenes, string sIntermediate = null)
		{

		}
		//-------------------------------------------------------------------------------------------------------------
		public void New_RemoveScenes(string sNextActiveScene, string[] sRemoveScenes, string sIntermediate = null)
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		public void New_AddScenes(string sNextActiveScene, string[] sAdditionalScenes, string sIntermediate = null)
		{

		}
		//-------------------------------------------------------------------------------------------------------------
		private void New_ChangeScenes(
			string sActualActiveScene,
			string sNextActiveScene,
			string[] sAdditionalScenes,
			string[] sRemovableScenes)
		{

		}
		//-------------------------------------------------------------------------------------------------------------
		private void New_ChangeScenesWithIntermediate(
			string sIntermediateScene,
			string sActualActiveScene,
			string sNextActiveScene,
			string[] sAdditionalScenes,
			string[] sRemovableScenes)
		{

		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
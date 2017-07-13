using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using IdemobiTransitionEngine;

public class SceneControllerScript : MonoBehaviour {

	public string m_ThisSceneTitle = "Scene …";
	public string m_NextSceneName = "";

	public Text m_TitleLabel;
	public Text m_SubTitleLabel;
	public Text m_PercentLabel;

	// Public method for buttons 
	public void LoadNextSceneAdditiveWithTransition () {
		IDETransitionDataScript tTransitionDataScript = new IDETransitionDataScript("payload test");
		tTransitionDataScript.Title = "Hello World";
		tTransitionDataScript.Subtitle = "I do transition Additive";
		IDETransitionControllerScript.LoadSceneByName (m_NextSceneName , LoadSceneMode.Additive, "SceneIntermediate", tTransitionDataScript);
	}

	public void LoadNextSceneWithTransition () {
		IDETransitionDataScript tTransitionDataScript = new IDETransitionDataScript("payload test");
		tTransitionDataScript.Title = "Hello World";
		tTransitionDataScript.Subtitle = "I do transition Single";
		IDETransitionControllerScript.LoadSceneByName (m_NextSceneName , LoadSceneMode.Single, "SceneIntermediate", tTransitionDataScript);
	}


	public void LoadNextSceneAdditive () {
		IDETransitionDataScript tTransitionDataScript = new IDETransitionDataScript("payload test");
		tTransitionDataScript.Title = "Hello World";
		tTransitionDataScript.Subtitle = "I do transition Additive";
		IDETransitionControllerScript.LoadSceneByName (m_NextSceneName , LoadSceneMode.Additive, null, tTransitionDataScript);
	}

	public void LoadNextScene () {
		IDETransitionDataScript tTransitionDataScript = new IDETransitionDataScript("payload test");
		tTransitionDataScript.Title = "Hello World";
		tTransitionDataScript.Subtitle = "I do transition Single";
		IDETransitionControllerScript.LoadSceneByName (m_NextSceneName , LoadSceneMode.Single, null, tTransitionDataScript);
	}


	// Public methods for the Actions callback

	public void FadeInStart(IDETransitionDataScript sTransitionDataScript) {
		Debug.Log (m_ThisSceneTitle + "GOOD FadeInStart with data named ' " + sTransitionDataScript.InternalName + "'");
	}

	public void FadeInFinish(IDETransitionDataScript sTransitionDataScript) {
		Debug.Log (m_ThisSceneTitle + "GOOD FadeInFinish with data named ' " + sTransitionDataScript.InternalName + "'");
	}

	public void SceneEnable(IDETransitionDataScript sTransitionDataScript) {
		Debug.Log (m_ThisSceneTitle + "GOOD SceneEnable with data named ' " + sTransitionDataScript.InternalName + "'");
	}

	public void FadeOutStart(IDETransitionDataScript sTransitionDataScript) {
		Debug.Log (m_ThisSceneTitle + "GOOD FadeOutStart with data named ' " + sTransitionDataScript.InternalName + "'");
	}

	public void FadeOutFinish(IDETransitionDataScript sTransitionDataScript) {
		Debug.Log (m_ThisSceneTitle + "GOOD FadeOutFinish with data named ' " + sTransitionDataScript.InternalName + "'");
	}

	public void SceneDisable(IDETransitionDataScript sTransitionDataScript) {
		Debug.Log (m_ThisSceneTitle + "GOOD SceneDisable with data named ' " + sTransitionDataScript.InternalName + "'");
	}

	public void SceneLoaded(IDETransitionDataScript sTransitionDataScript) {
		Debug.Log (m_ThisSceneTitle + "GOOD SceneLoaded with data named ' " + sTransitionDataScript.InternalName + "'");
		if (m_TitleLabel != null) {
			m_TitleLabel.text = sTransitionDataScript.Title;
		}
		if (m_SubTitleLabel != null) {
			m_SubTitleLabel.text = sTransitionDataScript.Subtitle;
		}

	}

	public void SceneWillUnloaded(IDETransitionDataScript sTransitionDataScript) {
		Debug.Log (m_ThisSceneTitle + "GOOD SceneWillUnloaded with data named ' " + sTransitionDataScript.InternalName + "'");
	}


	// Public methods for the Actions callback for Transition Scene

	public void LoadNextSceneStart(IDETransitionDataScript sTransitionDataScript, float sPercent)
	{
		Debug.Log (m_ThisSceneTitle + "GOOD LoadNextSceneStart with data named ' " + sTransitionDataScript.InternalName + "'");
		m_TitleLabel.text = sTransitionDataScript.Title;
		m_SubTitleLabel.text = sTransitionDataScript.Subtitle;
		m_PercentLabel.text = "" + sPercent.ToString("P") + "%";
	}

	public void LoadingNextScenePercent(IDETransitionDataScript sTransitionDataScript, float sPercent)
	{
		Debug.Log (m_ThisSceneTitle + "GOOD LoadingNextScenePercent with data named ' " + sTransitionDataScript.InternalName + "' " + sPercent.ToString("P") + " %");
		m_TitleLabel.text = sTransitionDataScript.Title;
		m_SubTitleLabel.text = sTransitionDataScript.Subtitle;
		m_PercentLabel.text = "" + sPercent.ToString("P") + "%";
	}

	public void LoadNextSceneFinish(IDETransitionDataScript sTransitionDataScript, float sPercent)
	{
		Debug.Log (m_ThisSceneTitle + "GOOD LoadNextSceneFinish with data named ' " + sTransitionDataScript.InternalName + "'");
		m_TitleLabel.text = sTransitionDataScript.Title;
		m_SubTitleLabel.text = sTransitionDataScript.Subtitle;
		m_PercentLabel.text = "" + sPercent.ToString("P") + "%";
	}

}

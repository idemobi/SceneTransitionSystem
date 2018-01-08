//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;

namespace SceneTransitionSystem
{
	public class STSTransitionAnimation : MonoBehaviour
	{
		//private STSTransitionParameters PreviewParams;
		//private STSTransitionParameters NextParams;

		private void InitInstance ()
		{
			Debug.Log ("STSTransitionAnimation InitInstance");
		}

		//Awake is always called before any Start functions
		private void Awake ()
		{
			Debug.Log ("STSTransitionAnimation Awake");
		}

		private void OnDestroy ()
		{
			Debug.Log ("STSTransitionAnimation OnDestroy");
		}

		// Instance method

		// Fade effect method
		private STSAnimationStyle m_AnimationStyle = STSAnimationStyle.None;
        private Texture2D m_AnimationTexture = null;
		public Color m_AnimationColor = Color.yellow;
		public Color m_AnimationPreviewColor = Color.yellow;
		private float m_AnimationSpeed = 0.8f;
		// the fadding speed

		private int m_DrawDepth = -1000;
		// Animation counter
		private float m_AnimationCounter = 0.0f;
		private int m_AnimationDirection = -1;

		private bool m_AnimationFinish = false;
		private bool m_AnimationInProgress = false;


		private bool AnimationProgress ()
		{
			return m_AnimationInProgress;
		}

		private bool AnimationFinished ()
		{
			return m_AnimationFinish;
		}

		// override method

		void Update ()
		{
			//Debug.Log ("Update m_AnimationCounter = "+ m_AnimationCounter.ToString());
		}
		void FixedUpdate ()
		{
			//Debug.Log ("FixedUpdate m_AnimationCounter = "+ m_AnimationCounter.ToString());
		}
		void OnGUI ()
		{
			//Debug.Log ("OnGUI m_AnimationCounter = "+ m_AnimationCounter.ToString());

				switch (m_AnimationStyle) {
				case STSAnimationStyle.FadeIn:
					{
						Animation_FadeIn ();
					}
					break;
				case STSAnimationStyle.FadeOut:
					{
						Animation_FadeOut ();
					}
					break;
				case STSAnimationStyle.ShutterBottomIn:
					{
						Animation_ShutterCutting (1, 1, 10, 1, 0, 1);
					}
					break;
				case STSAnimationStyle.ShutterTopIn:
					{
						Animation_ShutterCutting (1, 1, 10, 1, 0, -1);
					}
					break;
				case STSAnimationStyle.ShutterRightIn:
					{
						Animation_ShutterCutting (1, 1, 1, 10, 1, 0);
					}
					break;
				case STSAnimationStyle.ShutterLeftIn:
					{
						Animation_ShutterCutting (1, 1, 1, 10, -1, 0);
					}
					break;
				case STSAnimationStyle.ShutterBottomOut:
					{
						Animation_ShutterCutting (1, 1, 10, 1, 0, 1);
					}
					break;
				case STSAnimationStyle.ShutterTopOut:
					{
						Animation_ShutterCutting (1, 1, 10, 1, 0, -1);
					}
					break;
				case STSAnimationStyle.ShutterRightOut:
					{
						Animation_ShutterCutting (1, 1, 1, 10, 1, 0);
					}
					break;
				case STSAnimationStyle.ShutterLeftOut:
					{
						Animation_ShutterCutting (1, 1, 1, 10, -1, 0);
					}
					break;
				}
		}



		void Animation_Next ()
		{
			//Debug.Log ("Animation_Next m_AnimationInProgress =" + m_AnimationInProgress);
			if (m_AnimationInProgress == true) {
				m_AnimationCounter += m_AnimationDirection * m_AnimationSpeed * Time.deltaTime;
				m_AnimationCounter = Mathf.Clamp01 (m_AnimationCounter);
				if (m_AnimationCounter == 0 && m_AnimationDirection == -1) {
					m_AnimationFinish = true;
					m_AnimationInProgress = false;
				} else if (m_AnimationCounter == 1 && m_AnimationDirection == 1) {
					m_AnimationFinish = true;
					m_AnimationInProgress = false;
				}
			}
		}

		private void Animation_FadeIn ()
		{
			Animation_Fade (-1);
		}

		private void Animation_FadeOut ()
		{
			Animation_Fade (1);
		}

		private void Animation_Fade (float sDirection)
		{
			float tAlpha = m_AnimationCounter;
//			if (sDirection < 0) {
//				tAlpha = 1 - tAlpha;
//			}
			//Debug.Log ("Animation_Fade direction = " + sDirection.ToString() + " alpha value = " + tAlpha.ToString() + " m_AnimationCounter = " + m_AnimationCounter.ToString());
			// Draw the animation
			Color tfadeColor = Color.Lerp (m_AnimationColor, m_AnimationPreviewColor, m_AnimationCounter * 2.0f);
			//Color tfadeColor = m_AnimationColor;
			Color tfadeColorAlpha = new Color (tfadeColor.r, tfadeColor.g, tfadeColor.b, tAlpha);
			if (m_AnimationTexture == null) {
				DrawQuad (new Rect (0, 0, Screen.width, Screen.height), tfadeColorAlpha);
			} else {
				GUI.color = tfadeColorAlpha;	// set the alpha value
				GUI.depth = m_DrawDepth; 	// make the black texture render on top (draw last)
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), m_AnimationTexture); // draw the texture to fit the entire screen area
			}

			// calculate the next animation
			Animation_Next ();
		}

		void Animation_Shutter (float sWidth, float sHeight, float sWidthCutting, float sHeightCutting, float sXDirection, float sYDirection)
		{
			Color tfadeColor = Color.Lerp (m_AnimationColor, m_AnimationPreviewColor, m_AnimationCounter * 2.0f);
			float tX = (1 - m_AnimationCounter) * sXDirection;
			float tY = (1 - m_AnimationCounter) * sYDirection;

			if (sYDirection == 0.0F) {
				tY = 0;
			}
			if (sXDirection == 0.0F) {
				tX = 0;
			}
			Debug.Log ("tX = " + tX.ToString () + "  tY = " + tY.ToString ());
			Rect tRect = new Rect (Screen.width * tX, Screen.height * tY, Screen.width, Screen.height);
			if (m_AnimationTexture == null) {
				DrawQuad (tRect, tfadeColor);
			} else {
				GUI.color = tfadeColor;	// set the alpha value
				GUI.depth = m_DrawDepth; 	// make the black texture render on top (draw last)
				GUI.DrawTexture (tRect, m_AnimationTexture); // draw the texture to fit the entire screen area
			}
			// animation 
			Animation_Next ();
		}


		void Animation_ShutterCutting (float sWidth, float sHeight, float sWidthCutting, float sHeightCutting, float sXDirection, float sYDirection)
		{
			Color tfadeColor = Color.Lerp (m_AnimationColor, m_AnimationPreviewColor, m_AnimationCounter * 2.0f);
			float tX = (1 - m_AnimationCounter) * sXDirection;
			float tY = (1 - m_AnimationCounter) * sYDirection;

			if (sYDirection == 0) {
				tY = 0;
			}
			if (sXDirection == 0) {
				tX = 0;
			}
			float tIncrX = Screen.width / sWidthCutting;
			for (int i = 0; i < sWidthCutting; i++) {
				float Tyy = Screen.height * tY - i * tIncrX;

				//Debug.Log ("tX = " + tX.ToString () + "  tY = " + tY.ToString ());
				Rect tRect = new Rect (Screen.width * tX + tIncrX * i, Tyy, tIncrX, Screen.height);
				if (m_AnimationTexture == null) {
					DrawQuad (tRect, tfadeColor);
				} else {
					GUI.color = tfadeColor;	// set the alpha value
					GUI.depth = m_DrawDepth; 	// make the black texture render on top (draw last)
					GUI.DrawTexture (tRect, m_AnimationTexture); // draw the texture to fit the entire screen area
				}
			}
			// animation 
			Animation_Next ();
		}







		// graphic tool box

		void DrawQuad (Rect position, Color color)
		{
			Texture2D tTexture = new Texture2D (1, 1);
			tTexture.SetPixel (0, 0, color);
			tTexture.Apply ();
			GUI.depth = m_DrawDepth; 
			GUI.skin.box.normal.background = tTexture;
			GUI.Box (position, GUIContent.none);
		}

		// end class
	}

	// end namespace
}
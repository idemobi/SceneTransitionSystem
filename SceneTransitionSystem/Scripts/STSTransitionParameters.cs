﻿using UnityEngine;

namespace SceneTransitionSystem
{
	[System.Serializable]
	public enum STSAnimationStyle
	{
		FadeIn,
		FadeOut,
		ShutterRightIn,
		ShutterLeftIn,
		ShutterTopIn,
		ShutterBottomIn,
		ShutterRightOut,
		ShutterLeftOut,
		ShutterTopOut,
		ShutterBottomOut,

		// add none for the start of root scene
		None
	}

	[System.Serializable]
	public class STSAnimationParameters : System.Object
	{
		[Header ("Type of transition")]
		// the Transition In Style
		public STSAnimationStyle Style = STSAnimationStyle.FadeIn;
		[Header ("Parameters for transition")]
		// the Transition In color
		public Color Color = Color.black;
		// the Transition In texture
		public Texture2D Texture = null;
		// the Transition In speed
		[Range (0.05f, 10.0f)]
		public float Seconds = 1.0f;
		// the Transition In call back
		[Header ("Callbacks for transition events")]
		public STSTransitionEventEstimatedSeconds Start;
		public STSTransitionEvent Finish;
	}



	public class STSTransitionParameters : MonoBehaviour
	{
	
		//[Header ("Animation In (enter this scene ) ")]
		public STSAnimationParameters AnimationIn = new STSAnimationParameters();
//		// the Transition In Style
//		public AnimationStyle AnimationInStyle = AnimationStyle.FadeIn;
//		// the Transition In color
//		public Color AnimationInColor = Color.black;
//		// the Transition In texture
//		public Texture2D AnimationInTexture = null;
//		// the Transition In speed
//		[Range (0.05f, 10.0f)]
//		public float AnimationInSeconds = 1.0f;
//		// the Transition In call back
//		public TransitionEventEstimatedSecondsScript AnimationInStart;
//		public TransitionEventScript AnimationInFinish;

		//[Header ("Animation Out (exit this scene ) ")]
		public STSAnimationParameters AnimationOut = new STSAnimationParameters();
//		// the Transition Out Style
//		public AnimationStyle AnimationOutStyle = AnimationStyle.FadeOut;
//		// the Transition Out color
//		public Color AnimationOutColor = Color.black;
//		// the Transition Out texture
//		public Texture2D AnimationOutTexture = null;
//		// the Transition Out speed
//		[Range (0.05f, 10.0f)]
//		public float AnimationOutSeconds = 1.0f;
//		// the Transition Out call back
//		public TransitionEventEstimatedSecondsScript AnimationOutStart;
//		public TransitionEventScript AnimationOutFinish;

		[Header ("This Scene state call back")]
		public STSTransitionEvent ThisSceneLoaded;
		public STSTransitionEvent ThisSceneEnable;
		public STSTransitionEvent ThisSceneDisable;
		public STSTransitionEvent ThisSceneWillUnloaded;

		// Use this for initialization
		void Awake ()
		{
            // test if Transition controller exist
            STSTransitionController.Singleton ();
//			AnimationIn.Style = AnimationStyle.FadeIn;
//			AnimationOut.Style = AnimationStyle.FadeOut;
		}

		// Use this for initialization
		void Start ()
		{
		
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}

		public void CopyIn (STSTransitionParameters sDestination)
		{
			sDestination.AnimationIn.Style = this.AnimationIn.Style;
			sDestination.AnimationIn.Color = this.AnimationIn.Color;
			sDestination.AnimationIn.Texture = this.AnimationIn.Texture;
			sDestination.AnimationIn.Seconds = this.AnimationIn.Seconds;
			sDestination.AnimationIn.Start = this.AnimationIn.Start;
			sDestination.AnimationIn.Finish = this.AnimationIn.Finish;

			sDestination.AnimationOut.Style = this.AnimationOut.Style;
			sDestination.AnimationOut.Color = this.AnimationOut.Color;
			sDestination.AnimationOut.Texture = this.AnimationOut.Texture;
			sDestination.AnimationOut.Seconds = this.AnimationOut.Seconds;
			sDestination.AnimationOut.Start = this.AnimationOut.Start;
			sDestination.AnimationOut.Finish = this.AnimationOut.Finish;

			sDestination.ThisSceneLoaded = this.ThisSceneLoaded;
			sDestination.ThisSceneEnable = this.ThisSceneEnable;
			sDestination.ThisSceneDisable = this.ThisSceneDisable;
			sDestination.ThisSceneWillUnloaded = this.ThisSceneWillUnloaded;
		
		}

	}
}
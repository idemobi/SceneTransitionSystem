using UnityEngine;

namespace SceneTransitionSystem
{
	public class STSTransitionStandBy : MonoBehaviour
	{
		[Header ("Intermediate Scene Parameters")]

		public float StandBySeconds = 3.0f;
		public bool AutoLoadNextScene = true;

		public STSTransitionLoading LoadNextSceneStart;
		public STSTransitionLoading LoadingNextScenePercent;
		public STSTransitionLoading LoadNextSceneFinish;

		public STSTransitionEvent StandByStart;
		public STSTransitionEvent StandByFinish;

		// Use this for initialization
		void Awake ()
		{
			// test if Transition controller exist
			STSTransitionController.Singleton ();
		}

		// Use this for initialization
		void Start ()
		{
		
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}

		public void CopyIn (STSTransitionStandBy sDestination)
		{
			sDestination.StandBySeconds = this.StandBySeconds;
			sDestination.AutoLoadNextScene = this.AutoLoadNextScene;
			sDestination.LoadNextSceneStart = this.LoadNextSceneStart;
			sDestination.LoadingNextScenePercent = this.LoadingNextScenePercent;
			sDestination.LoadNextSceneFinish = this.LoadNextSceneFinish;
			sDestination.StandByStart = this.StandByStart;
			sDestination.StandByFinish = this.StandByFinish;
		}
	}
}

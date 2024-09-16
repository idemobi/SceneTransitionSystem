using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;


namespace SceneTransitionSystem
{
    /// <summary>
    /// The STSPerformance class provides methods for performing computationally intensive tasks and timing their execution.
    /// </summary>
    public static class STSPerformance
    {
        /// <summary>
        /// Stores the current start time for measuring the duration of performance-intensive operations.
        /// This variable is set when <see cref="StartTimer"/> is called and used to calculate elapsed time
        /// when <see cref="EndTimer"/> is called.
        /// </summary>
        private static float Timer = 0.0F;

        /// <summary>
        /// Executes a performance-intensive computation multiple times.
        /// </summary>
        /// <param name="timesToRepeat">The number of times the computation should be repeated.</param>
        public static void PerformanceIntensiveMethod(int timesToRepeat)
        {
            float value = 0f;
            for (int i = 0; i < timesToRepeat; i++)
            {
                value = math.exp10(math.sqrt(value));
            }
        }

        /// <summary>
        /// Executes a performance-intensive task asynchronously.
        /// </summary>
        /// <param name="timesToRepeat">The number of iterations to perform the intensive computation.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static Task PerformanceIntensiveTask(int timesToRepeat)
        {
            return Task.Run(() =>
            {
                float value = 0f;
                for (int i = 0; i < timesToRepeat; i++)
                {
                    value = math.exp10(math.sqrt(value));
                }
            });
        }

        /// <summary>
        /// Starts the timer by recording the current real-time since startup.
        /// </summary>
        public static void StartTimer()
        {
            Timer = Time.realtimeSinceStartup;
        }

        /// <summary>
        /// Ends the timer that measures elapsed time since the last call to StartTimer,
        /// and logs the elapsed time in milliseconds to the Unity console.
        /// </summary>
        public static void EndTimer()
        {
            float tTime = (Time.realtimeSinceStartup - Timer) * 1000f;
            Debug.Log($"{tTime} ms");
        }
    }
}
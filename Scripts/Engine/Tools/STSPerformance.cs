//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Author	    Dolwen (Jérôme DEMYTTENAERE) 
//  Email		jerome.demyttenaere@gmail.com
//  Project 	SceneTransitionSystem for Unity3D
//  Source      https://blog.logrocket.com/performance-unity-async-await-tasks-coroutines-c-job-system-burst-compiler/
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

//=====================================================================================================================
namespace SceneTransitionSystem
{
    public static class STSPerformance
    {
        private static float Timer = 0.0F;

        public static void PerformanceIntensiveMethod(int timesToRepeat)
        {
            // Represents a Performance Intensive Method like some pathfinding or really complex calculation.
            float value = 0f;
            for (int i = 0; i < timesToRepeat; i++)
            {
                value = math.exp10(math.sqrt(value));
            }
        }

        public static Task PerformanceIntensiveTask(int timesToRepeat)
        {
            return Task.Run(() =>
            {
                // Represents a Performance Intensive Method like some pathfinding or really complex calculation.
                float value = 0f;
                for (int i = 0; i < timesToRepeat; i++)
                {
                    value = math.exp10(math.sqrt(value));
                }
            });
        }

        public static void StartTimer()
        {
            Timer = Time.realtimeSinceStartup;
        }

        public static void EndTimer()
        {
            float tTime = (Time.realtimeSinceStartup - Timer) * 1000f;
            Debug.Log($"{tTime} ms");
        }
    }
}
//=====================================================================================================================
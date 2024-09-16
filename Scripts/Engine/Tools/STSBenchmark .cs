using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace SceneTransitionSystem
{
    /// The STSBenchmark class provides functionality for benchmarking code execution time and tracking operation counts.
    /// It supports the ability to start and stop timers, set custom tags, set maximum thresholds for timers and operations,
    /// and increment operation counts.
    /// /
    public class STSBenchmark
    {
        /// <summary>
        /// A dictionary that stores the start times for various benchmarks,
        /// identified by a unique string key. This is used to keep track of the
        /// start time for performance measurements in the Scene Transition System.
        /// </summary>
        public static Dictionary<string, DateTime> cStartDico = new Dictionary<string, DateTime>();

        /// <summary>
        /// Dictionary that maintains counters associated with string keys.
        /// </summary>
        public static Dictionary<string, int> cCounterDico = new Dictionary<string, int>();

        /// <summary>
        /// A dictionary storing the maximum allowable float values for specific keys.
        /// Used to track and enforce maximum threshold values during benchmarking operations.
        /// </summary>
        public static Dictionary<string, float> cMaxDico = new Dictionary<string, float>();

        /// <summary>
        /// A dictionary that maps benchmark keys to their maximum allowable time per operation.
        /// </summary>
        /// <remarks>
        /// The keys in the dictionary are string identifiers for the benchmarks,
        /// and the values are float representing the maximum allowable time per operation in seconds.
        /// </remarks>
        public static Dictionary<string, float> cMaxGranDico = new Dictionary<string, float>();

        /// <summary>
        /// A static dictionary used to store and manage string tags associated with specific keys.
        /// This dictionary is part of the Scene Transition System benchmarking mechanism.
        /// Keys in this dictionary typically represent unique identifiers for different benchmarking
        /// operations, while the associated values are tags that can be used for additional categorization.
        /// </summary>
        public static Dictionary<string, string> cTagDico = new Dictionary<string, string>();

        /// <summary>
        /// Resets all benchmark dictionaries (cStartDico, cCounterDico, cTagDico) to new instances, clearing all stored data.
        /// </summary>
        public static void ResetAll()
        {
            cStartDico = new Dictionary<string, DateTime>();
            cCounterDico = new Dictionary<string, int>();
            cTagDico = new Dictionary<string, string>();
        }

        /// <summary>
        /// Retrieves the key of the method that invoked the benchmarking function.
        /// </summary>
        /// <returns>
        /// Returns a string representing the fully qualified name of the method (including type name) that called the benchmark function.
        /// </returns>
        protected static string GetKey()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(2);
            string tMethod = sf.GetMethod().DeclaringType.Name + " " + sf.GetMethod().Name;
            return tMethod;
        }

        /// <summary>
        /// Retrieves a Unity engine object related to the current stack frame.
        /// </summary>
        /// <returns>
        /// The UnityEngine.Object related to the current stack frame.
        /// </returns>
        protected static UnityEngine.Object GetObject()
        {
            //StackTrace st = new StackTrace();
            //StackFrame sf = st.GetFrame(2);
            UnityEngine.Object sObject = null;
            return sObject;
        }

        /// <summary>
        /// Initializes benchmarking for a specific key determined by the calling method.
        /// This method sets the start time, reset counter, and adjusts the relevant dictionaries
        /// to default values for the specific key.
        /// </summary>
        public static void Start()
        {
            Start(GetKey());
        }

        /// Represents the default maximum allowable time for benchmarking operations.
        static float kMaxDefault = 0.010f;

        /// <summary>
        /// Represents the default maximum allowed time per operation measured
        /// during the benchmarking process, specified as a float value.
        /// </summary>
        static float kMaxPerOperationDefault = 0.001f;

        /// <summary>
        /// Starts the benchmarking for the provided key.
        /// Initializes the timing and count for the specified operation.
        /// </summary>
        /// <param name="sKey">The key representing the operation to be benchmarked.</param>
        public static void Start(string sKey)
        {
            if (cStartDico.ContainsKey(sKey) == true)
            {
                cStartDico[sKey] = DateTime.Now;
                cCounterDico[sKey] = 0;
                cTagDico[sKey] = string.Empty;
                cMaxDico[sKey] = kMaxDefault;
                cMaxGranDico[sKey] = kMaxPerOperationDefault;
            }
            else
            {
                cStartDico.Add(sKey, DateTime.Now);
                cCounterDico.Add(sKey, 0);
                cTagDico.Add(sKey, string.Empty);
                cMaxDico.Add(sKey, kMaxDefault);
                cMaxGranDico.Add(sKey, kMaxPerOperationDefault);
            }

            UnityEngine.Debug.Log("benchmark : '" + sKey + " start now!");
        }

        /// <summary>
        /// Adds a tag to the benchmarking data for the given key.
        /// </summary>
        /// <param name="sTag">The tag to be added.</param>
        public static void Tag(string sTag)
        {
            Tag(GetKey(), sTag);
        }

        /// <summary>
        /// Adds or updates a tag for the given key in the benchmark dictionary.
        /// </summary>
        /// <param name="sKey">The key for which the tag is being set.</param>
        /// <param name="sTag">The tag to associate with the key.</param>
        public static void Tag(string sKey, string sTag)
        {
            if (cStartDico.ContainsKey(sKey) == true)
            {
                cTagDico[sKey] = sTag;
            }
        }

        /// <summary>
        /// Sets the maximum allowed duration for the benchmark associated with the current calling method.
        /// </summary>
        /// <param name="sMax">The maximum duration in seconds.</param>
        public static void Max(float sMax)
        {
            Max(GetKey(), sMax);
        }

        /// <summary>
        /// Sets the maximum allowed execution time for a given key.
        /// </summary>
        /// <param name="sKey">The identifier for which the maximum execution time is set.</param>
        /// <param name="sMax">The maximum execution time allowed.</param>
        public static void Max(string sKey, float sMax)
        {
            if (cStartDico.ContainsKey(sKey) == true)
            {
                cMaxDico[sKey] = sMax;
            }
        }

        /// <summary>
        /// Sets the maximum allowed time per operation for the current benchmarking context.
        /// </summary>
        /// <param name="sMax">The maximum time allowed per operation, in seconds.</param>
        public static void MaxPerOperation(float sMax)
        {
            MaxPerOperation(GetKey(), sMax);
        }

        /// <summary>
        /// Sets the maximum allowed time per operation for a given benchmark key.
        /// </summary>
        /// <param name="sKey">The benchmark key for which to set the maximum allowed time per operation.</param>
        /// <param name="sMax">The maximum allowed time per operation in seconds.</param>
        public static void MaxPerOperation(string sKey, float sMax)
        {
            if (cStartDico.ContainsKey(sKey) == true)
            {
                cMaxGranDico[sKey] = sMax;
            }
        }

        /// Increments the counter associated with the current method by a specified value.
        /// <param name="sVal">The value to increment the counter by. Default is 1.</param>
        public static void Increment(int sVal = 1)
        {
            Increment(GetKey(), sVal);
        }

        /// <summary>
        /// Increments the counter associated with a specific key by a given value.
        /// </summary>
        /// <param name="sKey">The key corresponding to the counter to be incremented.</param>
        /// <param name="sVal">The value by which to increment the counter. The default is 1.</param>
        public static void Increment(string sKey, int sVal = 1)
        {
            if (cStartDico.ContainsKey(sKey) == true)
            {
                cCounterDico[sKey] = cCounterDico[sKey] + sVal;
            }
        }

        /// <summary>
        /// Completes the benchmark for the calling method, performs calculations, and logs debug information if specified.
        /// </summary>
        /// <param name="sWithDebug">If true, logs debug information. Defaults to true.</param>
        /// <returns>The duration of the benchmark in double precision.</returns>
        public static double Finish(bool sWithDebug = true)
        {
            return Finish(GetKey(), sWithDebug);
        }

        /// <summary>
        /// Finishes the benchmark measurement for the given key and optionally logs the result.
        /// </summary>
        /// <param name="sKey">The key associated with the benchmark measurement.</param>
        /// <param name="sWithDebug">Indicates whether to log the result for debugging purposes.</param>
        /// <returns>The time elapsed in seconds since the benchmark started.</returns>
        public static double Finish(string sKey, bool sWithDebug = true)
        {
            double rDelta = 0;
            if (cStartDico.ContainsKey(sKey) == true)
            {
                double tStart = STSDateHelper.ConvertToTimestamp(cStartDico[sKey]);
                int tCounter = cCounterDico[sKey];
                string tTag = cTagDico[sKey];
                if (string.IsNullOrEmpty(tTag) == false)
                {
                    tTag = " (tag : " + tTag + ")";
                }

                float tMax = cMaxDico[sKey];
                float tMaxGranule = cMaxGranDico[sKey];
                cStartDico.Remove(sKey);
                cCounterDico.Remove(sKey);
                cTagDico.Remove(sKey);
                cMaxDico.Remove(sKey);
                cMaxGranDico.Remove(sKey);
                double tFinish = STSDateHelper.ConvertToTimestamp(DateTime.Now);
                rDelta = tFinish - tStart;
                string tMaxColor = "black";
                if (rDelta >= tMax)
                {
                    tMaxColor = "red";
                }

                if (sWithDebug == true)
                {
                    if (tCounter == 1)
                    {
                        UnityEngine.Debug.Log("benchmark : '" + sKey + "'" + tTag + " execute " + tCounter +
                                              " operation in <color=" + tMaxColor + ">" +
                                              rDelta.ToString("F3") + " seconds </color>");
                    }
                    else if (tCounter > 1)
                    {
                        double tGranule = rDelta / tCounter;
                        string tMaxGranuleColor = "black";
                        if (tGranule >= tMaxGranule)
                        {
                            tMaxGranuleColor = "red";
                        }

                        UnityEngine.Debug.Log("benchmark : '" + sKey + "'" + tTag + " execute " + tCounter +
                                              " operations in <color=" + tMaxColor + ">" + rDelta.ToString("F3") +
                                              " seconds </color>(<color=" + tMaxGranuleColor + ">" + tGranule.ToString("F5") +
                                              " seconds per operation</color>)");
                    }
                    else
                    {
                        UnityEngine.Debug.Log("benchmark : '" + sKey + "'" + tTag + " execute in <color=" + tMaxColor + ">" +
                                              rDelta.ToString("F3") + " seconds </color>");
                    }
                }
            }
            else
            {
                if (sWithDebug == true)
                {
                    UnityEngine.Debug.Log("benchmark : error '" + sKey + "' has no start value");
                }
            }

            return rDelta;
        }
    }
}
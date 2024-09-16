using System;
using System.Collections.Generic;

namespace SceneTransitionSystem
{
    /// <summary>
    /// Represents data related to a scene transition.
    /// </summary>
    public partial class STSTransitionData
    {
        /// <summary>
        /// Represents the level or stage within the scene transition system.
        /// </summary>
        public string Level;
    }

    /// <summary>
    /// Represents the data required for handling scene transitions.
    /// </summary>
    public partial class STSTransitionData
    {
        /// <summary>
        /// Holds additional information for scene transition effects such as start and finish points.
        /// This information is used to provide more detailed parameters for transition effects.
        /// </summary>
        public STSEffectMoreInfos EffectMoreInfos;

        /// <summary>
        /// Represents the internal name used to uniquely identify a specific transition data instance in the Scene Transition System.
        /// </summary>
        public string InternalName;

        /// <summary>
        /// The title for the transition, usually representing the name or identifier
        /// of the scene or state being transitioned to.
        /// </summary>
        public string Title;

        /// <summary>
        /// Represents the subtitle associated with the scene transition.
        /// </summary>
        public string Subtitle;

        /// <summary>
        /// Stores a dictionary of key-value pairs to be used as a payload during scene transitions.
        /// </summary>
        private Dictionary<string, object> DictionaryAsPayload = new Dictionary<string, object>();

        /// <summary>
        /// Represents the data required for a scene transition within the Scene Transition System.
        /// </summary>
        public STSTransitionData()
        {
            // Empty!
        }

        /// <summary>
        /// Represents data related to scene transitions.
        /// </summary>
        public STSTransitionData(string sInternalName)
        {
            InternalName = sInternalName;
        }

        /// <summary>
        /// Represents data associated with scene transitions in the Scene Transition System.
        /// </summary>
        public STSTransitionData(string sInternalName, string sTitle, string sSubtitle, string sLevel)
        {
            InternalName = sInternalName;
            Title = sTitle;
            Subtitle = sSubtitle;
            Level = sLevel;
        }

        /// <summary>
        /// Represents transition data for the scene transition system. It contains information
        /// about levels, internal names, titles, subtitles, and additional key-value pair payload data.
        /// </summary>
        public STSTransitionData(string sInternalName, string sTitle, string sSubtitle, string sLevel, Dictionary<string, object> sDictionaryAsPayload)
        {
            InternalName = sInternalName;
            Title = sTitle;
            Subtitle = sSubtitle;
            Level = sLevel;
            DictionaryAsPayload = sDictionaryAsPayload;
        }

        /// <summary>
        /// Clears all entries in the transition data payload dictionary.
        /// </summary>
        /// <remarks>
        /// The payload dictionary is used to store various key-value pairs relevant to a scene transition.
        /// This method removes all key-value pairs from the dictionary, effectively resetting the payload to an empty state.
        /// </remarks>
        public void ClearPayLoad()
        {
            DictionaryAsPayload.Clear();
        }

        /// <summary>
        /// Adds an object to the payload dictionary with the specified key.
        /// If the key already exists, the associated object will be replaced.
        /// </summary>
        /// <param name="sKey">The key under which the object will be stored in the payload dictionary.</param>
        /// <param name="sObject">The object to be stored in the payload dictionary.</param>
        public void AddObjectForKeyInPayload(string sKey, object sObject)
        {
            if (DictionaryAsPayload == null)
            {
                DictionaryAsPayload = new Dictionary<string, object>();
            }

            if (DictionaryAsPayload.ContainsKey(sKey))
            {
                DictionaryAsPayload[sKey] = sObject;
            }
            else
            {
                DictionaryAsPayload.Add(sKey, sObject);
            }
        }

        /// <summary>
        /// Checks if the payload contains a specified key.
        /// </summary>
        /// <param name="sKey">The key to check for existence in the payload dictionary.</param>
        /// <returns>Returns true if the key exists in the payload, otherwise false.</returns>
        public bool HasKey(string sKey)
        {
            bool rValue = false;
            if (DictionaryAsPayload.ContainsKey(sKey))
            {
                rValue = true;
            }

            return rValue;
        }

        /// <summary>
        /// Retrieves an object associated with the specified key from the payload dictionary.
        /// </summary>
        /// <param name="sKey">The key for the object to retrieve.</param>
        /// <returns>The object associated with the specified key, or null if the key does not exist.</returns>
        public object GetObject(string sKey)
        {
            object tValue = null;
            if (DictionaryAsPayload.TryGetValue(sKey, out tValue))
            {
                return tValue;
            }

            return null;
        }

        /// <summary>
        /// Tries to retrieve a boolean value from the payload dictionary for the specified key.
        /// </summary>
        /// <param name="sKey">The key associated with the boolean value.</param>
        /// <param name="sDefault">The default value to return if the key does not exist in the dictionary.</param>
        /// <returns>The boolean value associated with the specified key, or the default value if the key does not exist.</returns>
        public bool GetBool(string sKey, bool sDefault = false)
        {
            object tValue;
            if (DictionaryAsPayload.TryGetValue(sKey, out tValue))
            {
                return Convert.ToBoolean(tValue);
            }

            return sDefault;
        }

        /// <summary>
        /// Retrieves the value associated with the specified key from the payload as a string.
        /// </summary>
        /// <param name="sKey">The key whose value to get.</param>
        /// <param name="sDefault">The default value to return if the key is not found.</param>
        /// <returns>The value associated with the specified key, or the default value if the key is not found.</returns>
        public string GetString(string sKey, string sDefault = "")
        {
            object tValue;
            if (DictionaryAsPayload.TryGetValue(sKey, out tValue))
            {
                return Convert.ToString(tValue);
            }

            return sDefault;
        }

        /// <summary>
        /// Retrieves an integer value associated with the specified key from the payload dictionary.
        /// If the key does not exist, a default value is returned.
        /// </summary>
        /// <param name="sKey">The key for the integer value to retrieve.</param>
        /// <param name="sDefault">The default value to return if the key does not exist.</param>
        /// <return>The integer value associated with the specified key, or the default value if the key does not exist.</return>
        public int GetInt(string sKey, int sDefault = -1)
        {
            object tValue;
            if (DictionaryAsPayload.TryGetValue(sKey, out tValue))
            {
                return Convert.ToInt32(tValue);
            }

            return sDefault;
        }
    }
}
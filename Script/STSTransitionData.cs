﻿//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// Transition scene system
    /// </summary>
    //-----------------------------------------------------------------------------------------------------------------
	public class STSTransitionData
	{
        //-------------------------------------------------------------------------------------------------------------
		public string InternalName;
		public string Title;
		public string Subtitle;
		public string Level;
        public List<object> ListAsPayload = new List<object>();
        public Dictionary<string, object> DictionaryAsPayload = new Dictionary<string, object>();
        //-------------------------------------------------------------------------------------------------------------
		public STSTransitionData ()
		{
            // Empty!
	    }
        //-------------------------------------------------------------------------------------------------------------
		public STSTransitionData (string sInternalName)
		{
			InternalName = sInternalName;
		}
        //-------------------------------------------------------------------------------------------------------------
		public STSTransitionData (string sInternalName, string sTitle, string sSubtitle, string sLevel)
		{
			InternalName = sInternalName;
			Title = sTitle;
			Subtitle = sSubtitle;
			Level = sLevel;
		}
        //-------------------------------------------------------------------------------------------------------------
		public STSTransitionData (string sInternalName, string sTitle, string sSubtitle, string sLevel, List<object> sListAsPayload, Dictionary<string, object> sDictionaryAsPayload)
		{
			InternalName = sInternalName;
			Title = sTitle;
			Subtitle = sSubtitle;
			Level = sLevel;
			ListAsPayload = sListAsPayload;
			DictionaryAsPayload = sDictionaryAsPayload;
		}
        //-------------------------------------------------------------------------------------------------------------
        public void ClearPayLoad()
        {
            DictionaryAsPayload.Clear();
        }
        //-------------------------------------------------------------------------------------------------------------
		public void AddObjectForKeyInPayload (string sKey, object sObject)
		{
			if (DictionaryAsPayload == null)
            {
				DictionaryAsPayload = new Dictionary<string, object> ();
			}
			DictionaryAsPayload.Add (sKey, sObject);
		}
        //-------------------------------------------------------------------------------------------------------------
        public void AddObjectInPayload(object sObject)
        {
            if (ListAsPayload == null)
            {
                ListAsPayload = new List<object>();
            }
            ListAsPayload.Add(sObject);
        }
        //-------------------------------------------------------------------------------------------------------------
        public object GetObject(string sKey)
        {
            object value;
            if (DictionaryAsPayload.TryGetValue(sKey, out value))
            {
                return value;
            }
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool GetBool(string sKey, bool sDefault = false)
        {
            object value;
            if (DictionaryAsPayload.TryGetValue(sKey, out value))
            {
                return Convert.ToBoolean(value);
            }
            return sDefault;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetString(string sKey)
        {
            object value;
            if (DictionaryAsPayload.TryGetValue(sKey, out value))
            {
                return Convert.ToString(value);
            }
            return "";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================

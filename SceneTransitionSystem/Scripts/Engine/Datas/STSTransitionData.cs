using System;
using System.Collections.Generic;

//=====================================================================================================================
namespace SceneTransitionSystem
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class STSTransitionData
    {
        //-------------------------------------------------------------------------------------------------------------
		public string InternalName;
		public string Title;
		public string Subtitle;
		public string Level;
		public List<object> ListAsPayload;
        public Dictionary<string, object> DictionaryAsPayload;
        //-------------------------------------------------------------------------------------------------------------
		public STSTransitionData ()
		{
        }
        //-------------------------------------------------------------------------------------------------------------
		public STSTransitionData (string sInternalName)
		{
			this.InternalName = sInternalName;
        }
        //-------------------------------------------------------------------------------------------------------------
		public STSTransitionData (string sInternalName, string sTitle, string sSubtitle, string sLevel)
		{
			this.InternalName = sInternalName;
			this.Title = sTitle;
			this.Subtitle = sSubtitle;
			this.Level = sLevel;
        }
        //-------------------------------------------------------------------------------------------------------------
		public STSTransitionData (string sInternalName, string sTitle, string sSubtitle, string sLevel, List<object> sListAsPayload, Dictionary<string, object> sDictionaryAsPayload)
		{
			this.InternalName = sInternalName;
			this.Title = sTitle;
			this.Subtitle = sSubtitle;
			this.Level = sLevel;
			this.ListAsPayload = sListAsPayload;
			this.DictionaryAsPayload = sDictionaryAsPayload;
        }
        //-------------------------------------------------------------------------------------------------------------
		public void AddObjectForKeyInPayload (object sObject, string sKey)
		{
			if (DictionaryAsPayload == null) {
				DictionaryAsPayload = new Dictionary<string, object> ();
			}
			DictionaryAsPayload.Add (sKey, sObject);
        }
        //-------------------------------------------------------------------------------------------------------------
        public object GetObjectForKeyInPayload(string sKey)
        {
            object rObject = null;
            if (DictionaryAsPayload != null)
            {
                rObject = DictionaryAsPayload[sKey];
            }
            return rObject;
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
		private void intialization ()
		{
			
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
using System.Collections.Generic;

namespace SceneTransitionSystem
{
	public class STSTransitionData
	{
		public string InternalName;
		public string Title;
		public string Subtitle;
		public string Level;
		public List<object> ListAsPayload;
		public Dictionary<string, object> DictionaryAsPayload;

		public STSTransitionData ()
		{
		}

		public STSTransitionData (string sInternalName)
		{
			this.InternalName = sInternalName;
		}

		public STSTransitionData (string sInternalName, string sTitle, string sSubtitle, string sLevel)
		{
			this.InternalName = sInternalName;
			this.Title = sTitle;
			this.Subtitle = sSubtitle;
			this.Level = sLevel;
		}

		public STSTransitionData (string sInternalName, string sTitle, string sSubtitle, string sLevel, List<object> sListAsPayload, Dictionary<string, object> sDictionaryAsPayload)
		{
			this.InternalName = sInternalName;
			this.Title = sTitle;
			this.Subtitle = sSubtitle;
			this.Level = sLevel;
			this.ListAsPayload = sListAsPayload;
			this.DictionaryAsPayload = sDictionaryAsPayload;
			}

		public void addObjectForKeyInPayload (object sObject, string sKey)
		{
			if (DictionaryAsPayload == null) {
				DictionaryAsPayload = new Dictionary<string, object> ();
			}
			DictionaryAsPayload.Add (sKey, sObject);
		}

		public void addObjectInPayload (object sObject)
		{
			if (ListAsPayload == null) {
				ListAsPayload = new List<object> ();
			}
			ListAsPayload.Add(sObject);
		}

		private void intialization ()
		{
			
		}
	}
}

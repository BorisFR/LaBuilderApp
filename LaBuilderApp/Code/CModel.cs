using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace LaBuilderApp
{
	public class CModel<T> : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged ([CallerMemberName] string caller = "")
		{
			if (PropertyChanged != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (caller));
			}
		}


		public static IEnumerable<T> LoadData (string data)
		{
			List<T> all = null;
			try {
				all = JsonConvert.DeserializeObject<List<T>> (data.Replace ("&amp;", "&"));
			} catch (Exception err) {
				Tools.Trace ("LoadData-Error: " + err.Message);
			}
			all.TrimExcess ();
			return all;
		}

	}
}
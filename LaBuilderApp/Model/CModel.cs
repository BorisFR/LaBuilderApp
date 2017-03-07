using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		public static List<T> Whole = new List<T> ();
		public static void LoadData (string data)
		{
			List<T> all = null;
			try {
				all = JsonConvert.DeserializeObject<List<T>> (data.Replace ("&amp;", "&"));
				all.TrimExcess ();
			} catch (Exception err) {
				Tools.Trace ("LoadData-Error: " + err.Message);
			}
			Whole = all;
			//return all;
			var ignore = Tools.DelayedGCAsync ();
		}


		private static ObservableCollection<T> toto = new ObservableCollection<T> ();
		public static ObservableCollection<T> All { get { return toto; } set { toto = value; } }

	}
}
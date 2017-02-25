using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public class ThingsType : CModel<ThingsType>
	{
		private int thingType; public int ThingType { get { return thingType; } set { thingType = value; RaisePropertyChanged (); } }
		private string name; public string Name { get { return name; } set { name = value; RaisePropertyChanged (); } }

		public static string GetName (int id)
		{
			if (dict.ContainsKey (id)) return dict [id].Name;

			IDataServer thingstype = new IDataServer ("thingstype");
			thingstype.IgnoreLocalData = true;
			thingstype.DataRefresh += (sender, status, result) => {
				IDataServer x = sender as IDataServer;
				if (status) {
					Tools.Trace ($"DataRefresh {x.FileName}: {result}");
					ThingsType.LoadData (result);
					ThingsType.PopulateData ();
				}
			};
			DataServer.AddToDo (thingstype);
			DataServer.Launch ();

			return id.ToString ();
		}

		private static Dictionary<int, ThingsType> dict = new Dictionary<int, ThingsType> ();

		public static void PopulateData ()
		{
			dict.Clear ();
			foreach (ThingsType t in Whole) {
				try {
					dict.Add (t.ThingType, t);
				} catch (Exception err) {
					Tools.Trace ($"ThingType.PopulateData error adding {t.Name}. Error: {err.Message}");
				}
			}
		}

	}
}
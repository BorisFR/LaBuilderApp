using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public class Country : CModel<Country>
	{
		private int countryCode; public int CountryCode { get { return countryCode; } set { countryCode = value; RaisePropertyChanged (); } }
		private string name; public string Name { get { return name; } set { name = value; RaisePropertyChanged (); } }

		public static string GetName (int id)
		{
			if (dict.ContainsKey (id)) return dict [id].Name;

			IDataServer obj = new IDataServer ("country");
			obj.IgnoreLocalData = true;
			obj.DataRefresh += (sender, status, result) => {
				IDataServer x = sender as IDataServer;
				if (status) {
					Tools.Trace ($"DataRefresh {x.FileName}: {result}");
					Country.LoadData (result);
					Country.PopulateData ();
				}
			};
			DataServer.AddToDo (obj);
			DataServer.Launch ();

			return id.ToString ();
		}

		public static ImageSource CountryImage (int id)
		{
			return ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/boris/Content/flags/{id}.png"));
		}


		private static Dictionary<int, Country> dict = new Dictionary<int, Country> ();

		public static void PopulateData ()
		{
			dict.Clear ();
			foreach (Country t in Whole) {
				try {
					dict.Add (t.CountryCode, t);
				} catch (Exception err) {
					Tools.Trace ($"Country.PopulateData error adding {t.Name}. Error: {err.Message}");
				}
			}
		}

	}
}
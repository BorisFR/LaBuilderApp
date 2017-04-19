using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public class Cards : CModel<Cards>
	{
		private string id; public string Id { get { return id; } set { id = value; RaisePropertyChanged (); } }
		private int builderCode; public int BuilderCode { get { return builderCode; } set { builderCode = value; RaisePropertyChanged (); } }

		public ImageSource GetImage { get { return ImageSource.FromUri (new Uri ($"{Global.DataUrl}images/cards/{builderCode}_o{id}.png")); } }
		public ImageSource GetSmallImage { get { return ImageSource.FromUri (new Uri ($"{Global.DataUrl}images/cards/{builderCode}_v{id}.jpg")); } }


		private string builder;
		public string Builder {
			get {
				if (builder != null && builder.Length > 0) return builder;
				builder = LaBuilderApp.Builder.GetById (builderCode).Username;
				return builder;

			}
		}

		public static ObservableCollection<Cards> GetForBuilder (int builderCode)
		{
			ObservableCollection<Cards> l = new ObservableCollection<Cards> ();
			foreach (Cards c in Whole) {
				if (c.BuilderCode == builderCode)
					l.Add (c);
			}
			return l;
		}

		public static Cards GetById (string id)
		{
			if (dictCards.ContainsKey (id))
				return dictCards [id];
			return new Cards () {

			};
		}

		private static Dictionary<string, Cards> dictCards = new Dictionary<string, Cards> ();

		public static void PopulateData ()
		{
			All.Clear ();
			dictCards.Clear ();
			foreach (Cards c in Whole) {
				All.Add (c);
				dictCards.Add (c.Id, c);
			}
		}

		public static void AddOne (string id, int builderId)
		{
			Cards c = new Cards ();
			c.Id = id;
			c.BuilderCode = builderId;
			All.Add (c);
			dictCards.Add (c.Id, c);
			Whole.Add (c);
		}

		public static void RemoveOne (string id)
		{
			foreach (Cards c in Whole)
				if (c.Id == id) {
					Whole.Remove (c);
					break;
				}
			foreach (Cards c in All)
				if (c.Id == id) {
					Whole.Remove (c);
					break;
				}
			if (dictCards.ContainsKey (id))
				dictCards.Remove (id);
		}

		public Cards ()
		{
		}

	}
}
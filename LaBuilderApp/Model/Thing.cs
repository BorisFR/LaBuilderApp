using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Plugin.AppInfo;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public class ThingsGroup : ObservableCollection<Thing>
	{

		public ThingsGroup (string title)
		{
			Title = title;
		}

		public string Title { get; private set; }

	}

	public class Thing : CModel<Thing>
	{
		private string id; public string Id { get { return id; } set { id = value; RaisePropertyChanged (); } }
		private int thingType; public int ThingType { get { return thingType; } set { thingType = value; RaisePropertyChanged (); } }
		private string name; public string Name { get { return name; } set { name = value; RaisePropertyChanged (); } }
		private string picture; public string Picture { get { return picture; } set { picture = value; RaisePropertyChanged (); } }
		private int builderCode; public int BuilderCode { get { return builderCode; } set { builderCode = value; RaisePropertyChanged (); } }
		private string description; public string Description { get { return description; } set { description = value; RaisePropertyChanged (); } }
		private int duree; public int Duree { get { return duree; } set { duree = value; RaisePropertyChanged (); } }

		public ImageSource PictureImage { get { return ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/boris/data/images/things/{builderCode}/{picture}")); } }

		private string builder; // = string.Empty;
		public string Builder {
			get {
				if (builder != null && builder.Length > 0) return builder;
				builder = LaBuilderApp.Builder.GetById (builderCode).Username;
				return builder;
			}
		}

		public ImageSource BuilderAvatarImage { get { return ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/forum/download/file.php?avatar={LaBuilderApp.Builder.GetById (builderCode).Avatar}")); } }

		public static ObservableCollection<ThingsGroup> AllGroup = new ObservableCollection<ThingsGroup> ();

		private static string typeText (int t)
		{
			switch (t) {
			case 100: return "Astromech";
			case 200: return "Sabre laser";
			case 300: return "Mouse Droïd";
			case 5000: return "Autre";
			default: return t.ToString ();
			}
		}

		public static Thing GetById (string id)
		{
			if (dictThings.ContainsKey (id))
				return dictThings [id];
			return new Thing () { Name = id.ToString () };
		}

		private static Dictionary<string, Thing> dictThings = new Dictionary<string, Thing> ();

		public static void PopulateData ()
		{
			Device.BeginInvokeOnMainThread (() => {
				All.Clear ();
				AllGroup.Clear ();
				dictThings.Clear ();
				ThingsGroup tg = null;
				int tt = -42;
				foreach (Thing t in Whole) {
					dictThings.Add (t.id, t);
					if (tt != t.ThingType) {
						tt = t.ThingType;
						if (tg == null) {
							tg = new ThingsGroup (typeText (tt));
						} else {
							AllGroup.Add (tg);
							tg = new ThingsGroup (typeText (tt));
						}
					}
					tg.Add (t);
				}
				if (tg != null)
					AllGroup.Add (tg);
			});
		}

		static Thing ()
		{
			if (!CrossAppInfo.Current.DisplayName.Equals ("XamarinFormsPreviewer")) return;
			List<ThingsGroup> list = new List<ThingsGroup> ();
			ThingsGroup tg = new ThingsGroup ("Design");
			Thing t = new Thing ();
			t.Name = "My splendid thing";
			t.ThingType = 1;
			t.BuilderCode = 2436;
			t.Duree = 7;
			t.Description = "This is the best thing ever done, 'cause, it is mine! Bla bla bla and bla bla bla and bla bla bla :)";
			tg.Add (t);
			list.Add (tg);
			DesignData = list;
		}

		public static IEnumerable<ThingsGroup> DesignData { get; set; }

	}
}
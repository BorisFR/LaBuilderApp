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
		private string [] pictureList; public string [] PictureList { get { return pictureList; } set { pictureList = value; RaisePropertyChanged (); } }

		public ImageSource PictureImage { get { return ImageSource.FromUri (new Uri ($"{Global.BaseUrl}boris/data/images/things/{builderCode}/{picture}")); } }

		private string descriptionLabel = string.Empty;
		public string DescriptionLabel {
			get {
				return descriptionLabel;
				//description.Replace ("\\n", "\r\n"); 
			}
		}

		private string builder; // = string.Empty;
		public string Builder {
			get {
				if (builder != null && builder.Length > 0) return builder;
				builder = LaBuilderApp.Builder.GetById (builderCode).Username;
				return builder;
			}
		}

		private int allPicturesCount = 0;
		public int AllPicturesCount {
			get { return allPicturesCount; }
		}

		public ObservableCollection<ImageSource> AllPictures {
			get {
				allPicturesCount = 0;
				ObservableCollection<ImageSource> temp = new ObservableCollection<ImageSource> ();
				if (pictureList != null) {
					foreach (string img in pictureList) {
						if (img.Length == 0) continue;
						if (img.Contains ("BADFORMAT")) continue;
						allPicturesCount++;
						temp.Add (new UriImageSource { Uri = new Uri ($"{Global.BaseUrl}boris/data/images/things/{builderCode}/{img}"), CachingEnabled = true, CacheValidity = new TimeSpan (5, 0, 0, 0) });
						//temp.Add (ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/boris/data/images/things/{builderCode}/{img}")));
					}
				}
				return temp;
			}
		}

		private int allPicturesPosition;
		public int AllPicturesPosition { get { return allPicturesPosition; } set { allPicturesPosition = value; RaisePropertyChanged (); } }

		public ImageSource BuilderAvatarImage { get { return ImageSource.FromUri (new Uri ($"{Global.BaseUrl}forum/download/file.php?avatar={LaBuilderApp.Builder.GetById (builderCode).Avatar}")); } }

		public static ObservableCollection<ThingsGroup> AllGroup = new ObservableCollection<ThingsGroup> ();

		private static string typeText (int t)
		{
			return LaBuilderApp.ThingsType.GetName (t);
			/*
			switch (t) {
			case 100: return "Astromech";
			case 200: return "Sabre laser";
			case 300: return "Mouse Droïde";
			case 400: return "Accessoires";
			case 500: return "Cosplay";
			case 600: return "Radio commande";
			case 700: return "Application";
			case 800: return "BB-8";
			case 5000: return "Autre";
			default: return t.ToString ();
			}*/
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
			//Device.BeginInvokeOnMainThread (() => {
			All.Clear ();
			AllGroup.Clear ();
			dictThings.Clear ();
			ThingsGroup tg = null;
			int tt = -42;
			foreach (Thing t in Whole) {
				if (t.description != null)
					t.descriptionLabel = Tools.PrettyLabel (t.description);
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
				Global.IsFirstTimeError = true;
				var ignore = Tools.DelayedGCAsync ();
			}
			if (tg != null)
				AllGroup.Add (tg);
			//});
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
			t.descriptionLabel = "This is the best thing ever done, 'cause, it is mine! Bla bla bla and bla bla bla and bla bla bla :)";
			tg.Add (t);
			list.Add (tg);
			DesignData = list;
		}

		public static IEnumerable<ThingsGroup> DesignData { get; set; }

	}
}
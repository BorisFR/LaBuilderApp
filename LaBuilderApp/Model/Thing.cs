using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public class Thing : CModel<Thing>
	{
		private string id; public string Id { get { return id; } set { id = value; RaisePropertyChanged (); } }
		private int thingType; public int ThingType { get { return thingType; } set { thingType = value; RaisePropertyChanged (); } }
		private string name; public string Name { get { return name; } set { name = value; RaisePropertyChanged (); } }
		private string picture; public string Picture { get { return picture; } set { picture = value; RaisePropertyChanged (); } }
		private int builderCode; public int BuilderCode { get { return builderCode; } set { builderCode = value; RaisePropertyChanged (); } }
		private string description; public string Description { get { return description; } set { description = value; RaisePropertyChanged (); } }
		private int duree; public int Duree { get { return duree; } set { duree = value; RaisePropertyChanged (); } }

		public ImageSource PictureImage { get { return ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/boris/data/images/things/{picture}")); } }

		private string builder = string.Empty;
		public string Builder {
			get {
				if (builder.Length > 0) return builder;

				return builder;
			}
		}


		static Thing ()
		{
			List<Thing> list = new List<Thing> ();
			Thing t = new Thing ();
			t.Name = "My splendid thing";
			t.ThingType = 1;
			t.BuilderCode = 2436;
			t.Duree = 7;
			t.Description = "This is the best thing ever done, 'cause, it is mine!";
			list.Add (t);
			DesignData = list;
		}

		public static IEnumerable<Thing> DesignData { get; set; }

	}
}
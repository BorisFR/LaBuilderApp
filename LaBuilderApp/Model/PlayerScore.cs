using System;
using System.Collections.Generic;
using Plugin.AppInfo;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public class PlayerScore : CModel<PlayerScore>
	{
		private int i; public int I { get { return i; } set { i = value; RaisePropertyChanged (); } }
		private int s; public int S { get { return s; } set { s = value; RaisePropertyChanged (); } }

		private string builder; // = string.Empty;
		public string Builder {
			get {
				if (builder != null && builder.Length > 0) return builder;
				builder = LaBuilderApp.Builder.GetById (i).Username;
				return builder;
			}
		}

		public ImageSource AvatarImage { get { return LaBuilderApp.Builder.GetById (i).AvatarImage; } }

		public string Score {
			get {
				TimeSpan ts = TimeSpan.FromSeconds (s);
				return ts.ToString ("c");
			}
		}


		static PlayerScore ()
		{
			if (!CrossAppInfo.Current.DisplayName.Equals ("XamarinFormsPreviewer")) return;
			List<PlayerScore> list = new List<PlayerScore> ();
			PlayerScore t = new PlayerScore ();
			t.I = 2634;
			t.S = 32;
			list.Add (t);
			DesignData = list;
		}

		public static IEnumerable<PlayerScore> DesignData { get; set; }


	}
}
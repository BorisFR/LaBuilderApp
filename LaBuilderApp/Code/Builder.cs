using System;
using System.Collections.Generic;
using Plugin.AppInfo;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public class Builder : CModel<Builder>
	{

		// {"username":"Boris","avatar":"2634_1366807872.gif","userId":2634,"groupId":9,"location":"Villeneuve d'Ascq, Lille, Nord, France",
		// "interest":"Test du champs centres d'intérêt.","occupation":"Fier d'être dev - http://fierdetredeveloppeur.org/",
		// "website":"https://www.facebook.com/Boris360","facebook":"R2D2ByBoris","twitter":"Boris360","youtube":"BorisFR","from":"20130224"}

		private string username; public string Username { get { return username; } set { username = value; RaisePropertyChanged (); } }
		private string avatar; public string Avatar { get { return avatar; } set { avatar = value; RaisePropertyChanged (); } }
		private int userId; public int UserId { get { return userId; } set { userId = value; RaisePropertyChanged (); } }
		private int groupId; public int GroupId { get { return groupId; } set { groupId = value; RaisePropertyChanged (); } }
		private string location; public string Location { get { return location; } set { location = value; RaisePropertyChanged (); } }
		private string interest; public string Interest { get { return interest; } set { interest = value; RaisePropertyChanged (); } }
		private string occupation; public string Occupation { get { return occupation; } set { occupation = value; RaisePropertyChanged (); } }
		private string website; public string Website { get { return website; } set { website = value; RaisePropertyChanged (); } }
		private string facebook; public string Facebook { get { return facebook; } set { facebook = value; RaisePropertyChanged (); } }
		private string twitter; public string Twitter { get { return twitter; } set { twitter = value; RaisePropertyChanged (); } }
		private string youtube; public string Youtube { get { return youtube; } set { youtube = value; RaisePropertyChanged (); } }
		private string from; public string From { get { return from; } set { from = value; RaisePropertyChanged (); } }

		public ImageSource AvatarImage { get { return ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/forum/download/file.php?avatar={avatar}")); } }
		public ImageSource CountryImage { get { return ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/boris/Content/flags/33.png")); } }

		private string communication = string.Empty;
		public string Communication {
			get {
				if (communication.Length > 0) return communication;

				if (website != null && website.Length > 0)
					communication = $"Site Internet :\n {website}";
				if (facebook != null && facebook.Length > 0)
					if (communication.Length > 0)
						communication = communication + $"\nFacebook :\n https://fb.com/{facebook}";
					else
						communication = $"Facebook :\n https://fb.com/{facebook}";
				if (youtube != null && youtube.Length > 0)
					if (communication.Length > 0)
						communication = communication + $"\nChaine youtube :\n https://youtube.com/{youtube}";
					else
						communication = $"Chaine youtube :\n https://youtube.com/{youtube}";
				if (twitter != null && twitter.Length > 0)
					if (communication.Length > 0)
						communication = communication + $"\nTwitter : @{twitter}\n https://twitter.com/{twitter}";
					else
						communication = $"Twitter : @{twitter}\n https://twitter.com/{twitter}";

				return communication;
			}
		}
		private string since = string.Empty;
		public string Since {
			get {
				if (since.Length > 0) return since;
				DateTime now = DateTime.UtcNow;
				DateTime start = new DateTime (Convert.ToInt32 (from.Substring (0, 4)), Convert.ToInt32 (from.Substring (4, 2)), Convert.ToInt32 (from.Substring (6, 2)));
				TimeSpan ts = now - start;
				int cptMonths = (int)(ts.TotalDays / 30);
				int cptYears = cptMonths / 12;
				cptMonths = cptMonths - (cptYears * 12);
				int cptDays = (int)(ts.TotalDays - (((cptYears * 12) + cptMonths) * 30));
				// O à 15 jours => 0, 15 à 31 jours => 1 mois
				if (cptDays > 15) cptMonths++;
				cptDays = 0;
				int realMonths = cptMonths;
				// 0 à 3 => 0 / 4 à 8 => 6 mois / 9 à 12 => 1 an
				if (cptMonths < 4) {
					cptMonths = 0;
				} else {
					if (cptMonths > 8) {
						cptYears = cptYears + 1;
						cptMonths = 0;
					} else {
						cptMonths = 6;
					}
				}
				if (cptMonths == 0) {
					if (cptYears > 1) { since = $"{cptYears} ans"; return since; }
					if (cptYears == 1) { since = $"{cptYears} an"; return since; }
					if (realMonths > 0) { since = $"{realMonths} mois"; return since; }
					since = "15 jours";
					return since;
				}
				if (cptYears > 1) { since = $"{cptYears} ans et demi"; return since; }
				if (cptYears == 1) { since = $"{cptYears} an et demi"; return since; }
				since = "6 mois";
				return since;
			}
		}

		public static Builder GetById (int id)
		{
			if (dictBuilders.ContainsKey (id))
				return dictBuilders [id];
			return new Builder () { Username = id.ToString () };
		}

		private static Dictionary<int, Builder> dictBuilders = new Dictionary<int, Builder> ();
		public static void PopulateData ()
		{
			Tools.Trace ("Builder PopulateData");
			Device.BeginInvokeOnMainThread (() => {
				All.Clear ();
				dictBuilders.Clear ();
				try {
					//List<Exhibition> temp = new List<Exhibition> ();
					foreach (Builder ex in Whole) {
						All.Add (ex);
						dictBuilders.Add (ex.UserId, ex);
					}
				} catch (Exception err) {
					Tools.Trace ("Builder PopulateData-Error: " + err.Message);
				}
			});
		}

		static Builder ()
		{
			if (!CrossAppInfo.Current.DisplayName.Equals ("XamarinFormsPreviewer")) return;
			Tools.Trace ("Builder class");
			List<Builder> temp = new List<Builder> ();
			Builder b = new Builder ();
			b.Username = "Demo";
			b.Avatar = "2634_1366807872.gif";
			b.From = "20130224";
			temp.Add (b);
			DesignData = temp;
		}

		public static IEnumerable<Builder> DesignData { get; set; }

	}
}
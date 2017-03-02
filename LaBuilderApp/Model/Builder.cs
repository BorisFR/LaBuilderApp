using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Plugin.AppInfo;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public class BuilderGroup : ObservableCollection<Builder>
	{

		public BuilderGroup (string title)
		{
			Title = title;
		}

		public string Title { get; private set; }

	}

	public class Builder : CModel<Builder>, IComparable<Builder>
	{

		// {"username":"Boris","avatar":"2634_1366807872.gif","userId":2634,"groupId":9,"location":"Villeneuve d'Ascq, Lille, Nord, France",
		// "interest":"Test du champs centres d'intérêt.","occupation":"Fier d'être dev - http://fierdetredeveloppeur.org/",
		// "website":"https://www.facebook.com/Boris360","facebook":"R2D2ByBoris","twitter":"Boris360","youtube":"BorisFR","from":"20130224"}

		private string username; public string Username { get { return username; } set { username = value; RaisePropertyChanged (); } }
		private string avatar; public string Avatar { get { return avatar; } set { avatar = value; RaisePropertyChanged (); } }
		private int userId; public int UserId { get { return userId; } set { userId = value; RaisePropertyChanged (); } }
		private int groupId; public int GroupId { get { return groupId; } set { groupId = value; RaisePropertyChanged (); } }
		private string location; public string Location { get { return location; } set { location = value; RaisePropertyChanged (); } }
		//private string interest; public string Interest { get { return interest; } set { interest = value; RaisePropertyChanged (); } }
		private string description; public string Description { get { return description; } set { description = value; RaisePropertyChanged (); } }
		//private string occupation; public string Occupation { get { return occupation; } set { occupation = value; RaisePropertyChanged (); } }
		private string website; public string Website { get { return website; } set { website = value; RaisePropertyChanged (); } }
		private string facebook; public string Facebook { get { return facebook; } set { facebook = value; RaisePropertyChanged (); } }
		private string twitter; public string Twitter { get { return twitter; } set { twitter = value; RaisePropertyChanged (); } }
		private string youtube; public string Youtube { get { return youtube; } set { youtube = value; RaisePropertyChanged (); } }
		private string from; public string From { get { return from; } set { from = value; RaisePropertyChanged (); } }
		private string sceneName; public string SceneName { get { return sceneName; } set { sceneName = value; RaisePropertyChanged (); } }
		private int countryCode; public int CountryCode { get { return countryCode; } set { countryCode = value; RaisePropertyChanged (); } }
		private string picture; public string MainPicture { get { return picture; } set { picture = value; RaisePropertyChanged (); } }
		private Dictionary<string, string> pictureList; public Dictionary<string, string> PictureList { get { return pictureList; } set { pictureList = value; RaisePropertyChanged (); } }
		private string isPublicClubPicture; public string IsPublicClubPicture { get { return isPublicClubPicture; } set { isPublicClubPicture = value; RaisePropertyChanged (); } }
		private string firstLastName; public string FirstLastName { get { return firstLastName; } set { firstLastName = value; RaisePropertyChanged (); } }

		public ImageSource PictureImage { get { return ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/boris/data/images/builders/{userId}/{picture}")); } }
		public ImageSource OfficialPictureImage {
			get {
				if (IsPublicClub)
					return ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/boris/data/images/builders/{userId}.jpg"));
				return ImageSource.FromUri (new Uri ("http://www.r2builders.fr/boris/1x1.png"));
			}
		}
		public bool IsPublicClub {
			get {
				if (isPublicClubPicture != null && isPublicClubPicture.Equals ("True"))
					return true;
				if (Global.IsConnected)
					return true;
				return false;
			}
		}
		public ImageSource AvatarImage { get { return ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/forum/download/file.php?avatar={avatar}")); } }
		public ImageSource CountryImage {
			get {
				if (countryCode > 0)
					return Country.CountryImage (countryCode);

				return Country.CountryImage (33);
			}
		}
		//public string InterestLabel { get { return interest.Replace ("\\n", "\r\n"); } }
		//public string OccupationLabel { get { return occupation.Replace ("\\n", "\r\n"); } }
		public string DescriptionLabel { get { return description.Replace ("\\n", "\r\n"); } }

		public string FullName {
			get {
				if (Global.IsConnected) {
					if (firstLastName != null && firstLastName.Length > 0) {
						if (username.Equals (sceneName))
							return $"{username} - {firstLastName}";
						if (sceneName != null && sceneName.Length > 0)
							return $"{sceneName} ({username}) - {firstLastName}";
						return $"{username} - {firstLastName}";
					} else {
						if (username.Equals (sceneName))
							return username;
						if (sceneName != null && sceneName.Length > 0)
							return $"{sceneName} ({username})";
						return username;
					}
				}
				if (sceneName != null && sceneName.Length > 0)
					return sceneName;
				return username;
			}
		}

		public bool IsBuilderConnected {
			get {
				return Global.IsConnected;
			}
		}

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


		public ObservableCollection<ImageSource> AllPictures {
			get {
				ObservableCollection<ImageSource> temp = new ObservableCollection<ImageSource> ();
				if (pictureList != null) {
					foreach (string img in pictureList.Values) {
						temp.Add (ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/boris/data/images/builders/{userId}/{img}")));
					}
				}
				return temp;
			}
		}

		private int allPicturesPosition;
		public int AllPicturesPosition { get { return allPicturesPosition; } set { allPicturesPosition = value; RaisePropertyChanged (); } }



		//private ObservableCollection<Thing> things = null;
		public ObservableCollection<Thing> Things {
			get {
				ObservableCollection<Thing> things = null;
				if (things != null) return things;
				things = new ObservableCollection<Thing> ();
				foreach (Thing t in LaBuilderApp.Thing.Whole) {
					if (t.BuilderCode == userId)
						things.Add (t);
				}
				return things;
			}
		}

		private int groupCount = 0;
		private int itemCount = 0;

		public int AllGroupsAndItemsSize {
			// change 50 and 20 if you change the PageBuilder.xaml design
			get { return 50 * itemCount + 20 * groupCount; }
		}

		public ObservableCollection<ExhibitionGroup> Events {
			get {
				ObservableCollection<ExhibitionGroup> list = new ObservableCollection<ExhibitionGroup> ();
				groupCount = 0;
				itemCount = 0;
				ExhibitionGroup eg = null;
				int year = 0;
				string month = string.Empty;
				foreach (Exhibition ex in Exhibition.Whole) {
					bool isPresent = false;
					try {
						if (ex.BuilderList != null) {
							foreach (int x in ex.BuilderList) {
								if (x == userId) {
									isPresent = true;
									break;
								}
							}
						}
					} catch (Exception err) {
						System.Diagnostics.Debug.WriteLine (err.Message);
					}
					if (!isPresent) {
						//System.Diagnostics.Debug.WriteLine ($"Ignore event: {ex.Title}");
						continue;
					}
					//System.Diagnostics.Debug.WriteLine ($"Event: {ex.Title}");
					try {
						if (ex.YearEvent != year || ex.MonthEvent != month) {
							month = ex.MonthEvent;
							year = ex.YearEvent;
							if (eg == null) {
								eg = new ExhibitionGroup ($"{month} {year}");
							} else {
								list.Insert (0, eg);
								groupCount++;
								eg = new ExhibitionGroup ($"{month} {year}");
							}
							eg.Insert (0, ex);
							itemCount++;
						} else {
							eg.Insert (0, ex);
							itemCount++;
						}
					} catch (Exception err) {
						System.Diagnostics.Debug.WriteLine (err.Message);
					}
				}
				if (eg != null) {
					list.Insert (0, eg);
					groupCount++;
				}
				RaisePropertyChanged ("AllGroupsAndItemsSize");
				return list;
			}
		}


		public ObservableCollection<Exhibition> EventsDetail {
			get {
				ObservableCollection<Exhibition> list = new ObservableCollection<Exhibition> ();

				foreach (Exhibition ex in Exhibition.Whole) {
					bool isPresent = false;
					try {
						if (ex.BuilderList != null) {
							foreach (int x in ex.BuilderList) {
								if (x == userId) {
									isPresent = true;
									break;
								}
							}
						}
					} catch (Exception err) {
						System.Diagnostics.Debug.WriteLine (err.Message);
					}
					if (!isPresent) {
						//System.Diagnostics.Debug.WriteLine ($"Ignore event: {ex.Title}");
						continue;
					}
					//System.Diagnostics.Debug.WriteLine ($"Event: {ex.Title}");
					list.Insert (0, ex);
				}
				return list;
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
				//All.Clear ();
				AllGroup.Clear ();
				List<Builder> list = new List<Builder> ();
				dictBuilders.Clear ();
				try {
					//List<Exhibition> temp = new List<Exhibition> ();
					foreach (Builder ex in Whole) {
						//All.Add (ex);
						dictBuilders.Add (ex.UserId, ex);
						list.Add (ex);
					}
					list.Sort ();
					BuilderGroup bg = null;
					foreach (Builder b in list) {
						// affiche-t'on ce builder ?
						if (b.Things.Count == 0)
							continue;


						if (bg == null) {
							bg = new BuilderGroup (b.Username.Substring (0, 1).ToLower ());
						} else {
							if (b.Username.Substring (0, 1).ToLower () != bg.Title) {
								AllGroup.Add (bg);
								bg = new BuilderGroup (b.Username.Substring (0, 1).ToLower ());
							}
						}
						bg.Add (b);
						//All.Add (b);
					}
					if (bg != null)
						AllGroup.Add (bg);
				} catch (Exception err) {
					Tools.Trace ("Builder PopulateData-Error: " + err.Message);
				}

			});
		}

		int IComparable<Builder>.CompareTo (Builder other)
		{
			return -other.Username.ToLower ().CompareTo (this.Username.ToLower ());
		}

		public static ObservableCollection<BuilderGroup> AllGroup = new ObservableCollection<BuilderGroup> ();

		static Builder ()
		{
			if (!CrossAppInfo.Current.DisplayName.Equals ("XamarinFormsPreviewer")) return;
			Tools.Trace ("Builder class");
			List<BuilderGroup> lbg = new List<BuilderGroup> ();
			BuilderGroup bg = new BuilderGroup ("a");
			//List<Builder> temp = new List<Builder> ();
			Builder b = new Builder ();
			b.userId = 2634;
			b.Username = "Demo";
			b.SceneName = "Boris";
			b.FirstLastName = "Stéphane Fardoux";
			b.Avatar = "2634_1366807872.gif";
			b.IsPublicClubPicture = "True";
			b.From = "20130224";
			//temp.Add (b);
			bg.Add (b);
			lbg.Add (bg);
			DesignData = lbg; // temp;
		}

		public static IEnumerable<BuilderGroup> DesignData { get; set; }

	}
}
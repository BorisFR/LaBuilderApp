using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using Plugin.AppInfo;

namespace LaBuilderApp
{
	public class PhpDateTime
	{
		public string date;
		public string timezone_type;
		public string timezone;

		public DateTime Date {
			get { return DateTime.ParseExact (date, "yyyy-MM-dd HH:mm:ss.ffffff", Global.CultureFrench); }
		}
	}

	public class OpenHour
	{
		/* "OpenHourList":[
		{"StartHour":
			{"date":"2017-01-13 21:00:00.000000","timezone_type":3,"timezone":"Europe\/Paris"},
			"EndHour":
			{"date":"2017-01-13 23:30:00.000000","timezone_type":3,"timezone":"Europe\/Paris"
			}}]
		*/
		public PhpDateTime StartHour;
		public PhpDateTime EndHour;
	}

	public class ExhibitionGroup : ObservableCollection<Exhibition>
	{

		public ExhibitionGroup (string title)
		{
			Title = title;
		}

		public string Title { get; private set; }

	}


	public class Exhibition : CModel<Exhibition>
	{

		/* {
		x "Id":"F9A5FD48-2BF5-DEF6-1E01-DB54B611A2A0","BuilderCode":2634,"EventType":0,"CountryCode":49,
		x "Title":"Star Wars Celebration Europe 2",
		x "Location":"Messe Essen",
		x "Description":"",
		x "StartDate":{"date":"2013-07-26 00:00:00.000000","timezone_type":3,"timezone":"Europe/Paris"},
		x "EndDate":{"date":"2013-07-28 00:00:00.000000","timezone_type":3,"timezone":"Europe/Paris"},
		"OpenHourList":null,
		x "AdminList":{"0":55},
		x "BuilderList":{"0":2634},
		x "Logo":"200B9342-0F34-0FF9-3877-8C8E6AFFF391.jpeg",
		x "Flyer":null,
		x "PublicView":1,
		*/

		private string id; public string Id { get { return id; } set { id = value; RaisePropertyChanged (); } }
		private int builderCode; public int BuilderCode { get { return builderCode; } set { builderCode = value; RaisePropertyChanged (); } }
		private int eventType; public int EventType { get { return eventType; } set { eventType = value; RaisePropertyChanged (); } }
		private int countryCode; public int CountryCode { get { return countryCode; } set { countryCode = value; RaisePropertyChanged (); } }
		private string title; public string Title { get { return title; } set { title = value; RaisePropertyChanged (); } }
		private string location; public string Location { get { return location; } set { location = value; RaisePropertyChanged (); } }
		private string description; public string Description { get { return description; } set { description = value; RaisePropertyChanged (); } }
		private PhpDateTime startDate; public PhpDateTime StartDate { get { return startDate; } set { startDate = value; RaisePropertyChanged (); } }
		private PhpDateTime endDate; public PhpDateTime EndDate { get { return endDate; } set { endDate = value; RaisePropertyChanged (); } }
		private OpenHour [] openHourList; public OpenHour [] OpenHourList { get { return openHourList; } set { openHourList = value; RaisePropertyChanged (); } }
		private int [] adminList; public int [] AdminList { get { return adminList; } set { adminList = value; RaisePropertyChanged (); } }
		private int [] builderList; public int [] BuilderList { get { return builderList; } set { builderList = value; RaisePropertyChanged (); } }
		private string logo; public string Logo { get { return logo; } set { logo = value; RaisePropertyChanged (); } }
		private string flyer; public string Flyer { get { return flyer; } set { flyer = value; RaisePropertyChanged (); } }
		private bool publicView; public bool PublicView { get { return publicView; } set { publicView = value; RaisePropertyChanged (); } }

		public ImageSource LogoImage { get { return ImageSource.FromUri (new Uri ($"{Global.BaseUrl}boris/data/images/events/{logo}")); } }
		public ImageSource FlyerImage { get { return ImageSource.FromUri (new Uri ($"{Global.BaseUrl}boris/data/images/flyer/{flyer}")); } }
		public ImageSource CountryImage { get { return Country.CountryImage (countryCode); } }
		public string DescriptionLabel { get { return description.Replace ("\\n", "\r\n"); } }

		public Color ListBackgroundColor {
			get {
				switch (eventType) {
				case 0:
					return Color.White;
				case 1:
					return Color.FromHex ("DDDDDD");
				case 2:
					return Color.FromHex ("BBBBBB");
				case 3:
					return Color.FromHex ("999999");
				default:
					return Color.FromHex ("777777");
				}
			}
		}

		//private ObservableCollection<Builder> allBuilders;
		public ObservableCollection<Builder> AllBuilder {
			get {
				ObservableCollection<Builder> allBuilders = null;
				if (allBuilders != null) return allBuilders;
				allBuilders = new ObservableCollection<Builder> ();
				if (builderList == null) return allBuilders;
				foreach (int i in builderList) {
					allBuilders.Add (Builder.GetById (i));
				}
				return allBuilders;
			}
		}

		/*
		public void RefreshBuilders ()
		{
			allBuilders = null;
		}*/

		private string dateEvent = string.Empty; public string DateEvent {
			get {
				if (dateEvent.Length > 0) return dateEvent;
				populateComplementData ();
				return dateEvent;
			}
		}

		private string hourEvent = string.Empty; public string HourEvent {
			get {
				if (hourEvent.Length > 0) return hourEvent;
				populateComplementData ();
				return hourEvent;
			}
		}

		public string Ouverture {
			get {
				if (hourEvent.Length > 0)
					return dateEvent + '\n' + hourEvent;
				return dateEvent;
			}
		}

		private string monthEvent = "*** sans date";
		public string MonthEvent {
			get {
				return monthEvent;
			}
		}

		private int yearEvent = 0; public int YearEvent {
			get {
				if (yearEvent > 0) return yearEvent;
				populateComplementData ();
				return yearEvent;
			}
		}

		private void populateComplementData ()
		{
			if (StartDate == null) StartDate = new PhpDateTime ();
			if (EndDate == null) EndDate = new PhpDateTime ();
			try {
				try {
					if (StartDate.date != null) { // on a une date de début
						yearEvent = StartDate.Date.Year;
						monthEvent = StartDate.Date.ToString ("MMMMM", Global.CultureFrench);

						if ((EndDate.date == null) || (StartDate.Date == EndDate.Date)) { // sur 1 seule journée
							dateEvent = $"Le {StartDate.Date.ToString ("dddd d MMMMM yyyy", Global.CultureFrench)}";
						} else {
							// sur plusieurs jours
							if (StartDate.Date.Month == EndDate.Date.Month)
								dateEvent = $"Du {StartDate.Date.ToString ("dddd d", Global.CultureFrench)} au {EndDate.Date.ToString ("dddd d MMMMM yyyy", Global.CultureFrench)}";
							else
								dateEvent = $"Du {StartDate.Date.ToString ("dddd d MMMMM", Global.CultureFrench)} au {EndDate.Date.ToString ("dddd d MMMMM yyyy", Global.CultureFrench)}";
						}
					}
				} catch (Exception err) {
					Tools.Trace ("populateComplementData-Error date: " + err.Message);
				}
				try {
					if (OpenHourList != null) {
						foreach (OpenHour oh in OpenHourList) {
							if (oh.StartHour == null) oh.StartHour = new PhpDateTime ();
							if (oh.EndHour == null) oh.EndHour = new PhpDateTime ();
							if (oh.StartHour.date != null) {
								yearEvent = oh.StartHour.Date.Year;
								monthEvent = StartDate.Date.ToString ("MMMMM", Global.CultureFrench);
								if (oh.EndHour.date != null) {
									hourEvent = $"le {oh.StartHour.Date.ToString ("dddd d MMMMM", Global.CultureFrench)} de {oh.StartHour.Date.ToString ("HH:mm", Global.CultureFrench)} à {oh.EndHour.Date.ToString ("HH:mm", Global.CultureFrench)}";
								}
							}
						}
						if (OpenHourList.Count () == 1 && hourEvent.Length > 0) {
							dateEvent = hourEvent;
							hourEvent = string.Empty;
						}
					}
				} catch (Exception err) {
					Tools.Trace ("populateComplementData-Error hour: " + err.Message);
				}
				if (dateEvent.Length == 0) { dateEvent = hourEvent; hourEvent = string.Empty; }

				if (StartDate.date == null && OpenHourList == null) {
					yearEvent = currentYear;
				}
				//dateEvent = "Du samedi 10 au dimanche 11 juin";
				//yearEvent = 2017;
			} catch (Exception err) {
				Tools.Trace ("populateComplementData-Error: " + err.Message);
			}
		}



		// ***********************
		// GLOBAL STATIC functions
		// ***********************

		private static int comingEventsHeight = 0; public static int ComingEventsHeight { get { return comingEventsHeight; } }
		public static ObservableCollection<ExhibitionGroup> ComingEvents = new ObservableCollection<ExhibitionGroup> ();

		public static void PrepareComingEvents ()
		{
			comingEventsHeight = 0;
			ComingEvents.Clear ();
			DateTime now = DateTime.Now;
			DateTime finishDateForNews = new DateTime (now.Year, now.Month, 1, 0, 0, 0); // début du mois, ex 1/3/2017
			finishDateForNews = finishDateForNews.AddMonths (2).AddDays (-1); // on ajoute 2 mois moins 1 jours => 1/5/2017 -1 => 30/4/2017
			int maxDaysInFutur = (int)(finishDateForNews - now).TotalDays;
			DateTime startDateForNews = new DateTime (now.Year, now.Month, 1, 0, 0, 0); // début du mois, ex 1/3/2017
			startDateForNews = startDateForNews.AddMonths (-1); // on enleve 1 mois => 1/2/2017
			int maxDaysInPast = (int)(startDateForNews - now).TotalDays;
			TimeSpan ts;
			ExhibitionGroup eg = null;
			ExhibitionGroup eg1 = null;
			ExhibitionGroup eg2 = null;
			try {
				//List<Exhibition> temp = new List<Exhibition> ();
				foreach (Exhibition ex in Whole) {
					if (ex.StartDate == null) continue;
					if (ex.StartDate.date == null) continue;
					ts = ex.StartDate.Date - now;
					if (ts.TotalDays < maxDaysInPast) continue; // trop loin dans le passé
					if (ts.TotalDays > maxDaysInFutur) continue; // trop loin
					if (ts.TotalDays <= -1.0) {
						if (eg == null) {
							eg = new ExhibitionGroup ("Ca vient d'avoir lieu");
							comingEventsHeight += 20;
						}
						eg.Add (ex);
						comingEventsHeight += 50;
						continue;
					}
					if (ts.TotalDays < 0) {
						if (eg != null)
							ComingEvents.Add (eg);
						eg = null;
						if (eg1 == null) {
							eg1 = new ExhibitionGroup ("C'est aujourd'hui");
							comingEventsHeight += 20;
						}
						eg1.Add (ex);
						comingEventsHeight += 50;
						continue;
					}
					//					if (ComingEvents.Count () == 0) {
					if (eg1 != null) {
						ComingEvents.Add (eg1);
						eg1 = null;
						eg2 = new ExhibitionGroup ("Les prochains événements");
						comingEventsHeight += 20;
					}
					eg2.Add (ex);
					comingEventsHeight += 50;
					//Tools.Trace ($"Height: {comingEventsHeight}");
				}
				if (eg != null)
					ComingEvents.Add (eg);
				if (eg1 != null)
					ComingEvents.Add (eg1);
				if (eg2 != null)
					ComingEvents.Add (eg2);

			} catch (Exception err) {
				Tools.Trace ("PrepareComingEvents: " + err.Message);
			}
		}


		public static ObservableCollection<ExhibitionGroup> AllGroup = new ObservableCollection<ExhibitionGroup> ();

		private static int currentYear = 0;
		public static int CurrentYear { get { return currentYear; } }

		public static void ChangeToYear (int year)
		{
			Tools.Trace ("ChangeToYear");
			if (currentYear == year) return;
			currentYear = year;
			//Device.BeginInvokeOnMainThread (() => {
			//All.Clear ();
			AllGroup.Clear ();
			ExhibitionGroup eg = null;
			dictEvents.Clear ();
			try {
				//List<Exhibition> temp = new List<Exhibition> ();
				foreach (Exhibition ex in Whole) {
					if (ex.YearEvent == year) {
						//temp.Add (ex);
						if (eg == null) {
							eg = new ExhibitionGroup (ex.MonthEvent);
						} else {
							if (ex.MonthEvent != eg.Title) {
								AllGroup.Add (eg);
								eg = new ExhibitionGroup (ex.MonthEvent);
							}
						}
						eg.Add (ex);
						//All.Add (ex);
					}
					dictEvents.Add (ex.Id, ex);
				}
				if (eg != null)
					AllGroup.Add (eg);
				Tools.Trace ("ChangeToYear done.");
				Global.IsFirstTimeError = true;
				var ignore = Tools.DelayedGCAsync ();
			} catch (Exception err) {
				Tools.Trace ("ChangeToYear-Error: " + err.Message);
			}
			//});
		}

		public static Exhibition GetById (string id)
		{
			if (dictEvents.ContainsKey (id))
				return dictEvents [id];
			return new Exhibition () { Title = id.ToString () };
		}

		private static Dictionary<string, Exhibition> dictEvents = new Dictionary<string, Exhibition> ();


		public static void ClearData ()
		{
			IDataServer.ClearData ("events");
			AllGroup.Clear ();
			Whole.Clear ();
			ChangeToYear (0);
		}

		public static void PopulateData ()
		{
			Tools.Trace ("Exhibition PopulateData");
			if (currentYear == 0)
				ChangeToYear (DateTime.Now.Year);
			else {
				// correction bug de changement d'année sur refresh
				int save = currentYear;
				currentYear = 0;
				ChangeToYear (DateTime.Now.Year);
				ChangeToYear (save);
			}
			PrepareComingEvents ();
		}
		/*
		public static void BuildersNeedToBeRefresh ()
		{
			foreach (KeyValuePair<string, Exhibition> kvp in dictEvents) {
				kvp.Value.RefreshBuilders ();
			}
		}*/

		static Exhibition ()
		{
			if (!CrossAppInfo.Current.DisplayName.Equals ("XamarinFormsPreviewer")) return;
			Tools.Trace ("Exhibition class");
			//All = new ObservableCollection<Exhibition> ();
			//try {
			// DATA de démo pour la conception.
			List<ExhibitionGroup> leg = new List<ExhibitionGroup> ();
			ExhibitionGroup eg = new ExhibitionGroup ("janvier");
			//List<Exhibition> temp = new List<Exhibition> ();
			Exhibition ex = new Exhibition ();
			ex.BuilderCode = 2634;
			ex.CountryCode = 33;
			ex.StartDate = new PhpDateTime ();
			ex.StartDate.date = "2017-01-18 00:00:00.000000";
			ex.Description = "Bonjour le monde.";
			ex.Location = "Lille";
			ex.PublicView = true;
			ex.Title = "Démonstration 1";
			//temp.Add (ex);
			eg.Add (ex);
			Whole.Add (ex);

			//All.Add (ex);
			ex = new Exhibition ();
			ex.BuilderCode = 2634;
			ex.CountryCode = 33;
			ex.StartDate = new PhpDateTime ();
			ex.StartDate.date = "2017-01-20 00:00:00.000000";
			ex.EndDate = new PhpDateTime ();
			ex.EndDate.date = "2017-01-21 00:00:00.000000";
			ex.Description = "Hello world!";
			ex.Location = "Paris";
			ex.PublicView = true;
			ex.Title = "Démonstration 2";
			//temp.Add (ex);
			eg.Add (ex);
			Whole.Add (ex);

			leg.Add (eg);
			//All.Add (ex);
			//All = temp;
			DesignData = leg;
			//PopulateData ();
			//} catch (Exception err) {
			//	Tools.Trace ("Exhibition-Error: " + err.Message);
			//}
			PopulateData ();
			PrepareComingEvents ();
		}

		public static IEnumerable<ExhibitionGroup> DesignData { get; set; }


	}
}
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;

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

		public ImageSource LogoImage { get { return ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/boris/data/images/events/{logo}")); } }

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

						if ((EndDate.date == null) || (StartDate.Date == EndDate.Date)) { // sur 1 seule journée
							dateEvent = $"Le {StartDate.Date.ToString ("dddd dd MMMMM yyyy", Global.CultureFrench)}";
						} else {
							// sur plusieurs jours
							if (StartDate.Date.Month == EndDate.Date.Month)
								dateEvent = $"Du {StartDate.Date.ToString ("dddd dd", Global.CultureFrench)} au {EndDate.Date.ToString ("dddd dd MMMMM yyyy", Global.CultureFrench)}";
							else
								dateEvent = $"Du {StartDate.Date.ToString ("dddd dd MMMMM", Global.CultureFrench)} au {EndDate.Date.ToString ("dddd dd MMMMM yyyy", Global.CultureFrench)}";
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
								if (oh.EndHour.date != null) {
									hourEvent = $"le {oh.StartHour.Date.ToString ("dddd dd MMMMM", Global.CultureFrench)} de {oh.StartHour.Date.ToString ("HH:mm", Global.CultureFrench)} à {oh.EndHour.Date.ToString ("HH:mm", Global.CultureFrench)}";
								}
							}
						}
						if (OpenHourList.Count () == 1 && hourEvent.Length > 0)
							dateEvent = hourEvent;
					}
				} catch (Exception err) {
					Tools.Trace ("populateComplementData-Error hour: " + err.Message);
				}
				if (dateEvent.Length == 0) dateEvent = hourEvent;

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

		private static ObservableCollection<Exhibition> toto = new ObservableCollection<Exhibition> ();
		public static ObservableCollection<Exhibition> All { get { return toto; } set { toto = value; } }

		private static int currentYear = 0;
		public static int CurrentYear { get { return currentYear; } }

		public static void ChangeToYear (int year)
		{
			Tools.Trace ("ChangeToYear");
			if (currentYear == year) return;
			currentYear = year;
			All.Clear ();
			try {
				//List<Exhibition> temp = new List<Exhibition> ();
				foreach (Exhibition ex in Whole) {
					if (ex.YearEvent == year) {
						//temp.Add (ex);
						All.Add (ex);
					}
				}
				//All.;
				Tools.Trace ("ChangeToYear done.");
			} catch (Exception err) {
				Tools.Trace ("ChangeToYear-Error: " + err.Message);
			}
		}

		public static void PopulateData ()
		{
			Tools.Trace ("PopulateData");
			if (currentYear == 0)
				ChangeToYear (DateTime.Now.Year);
			else {
				currentYear = 0;
				ChangeToYear (DateTime.Now.Year);
			}
		}

		static IEnumerable<Exhibition> DesignData { get; set; }

		static Exhibition ()
		{
			Tools.Trace ("Exhibition");
			//All = new ObservableCollection<Exhibition> ();
			//try {
			// DATA de démo pour la conception.
			List<Exhibition> temp = new List<Exhibition> ();
			Exhibition ex = new Exhibition ();
			ex.BuilderCode = 2634;
			ex.CountryCode = 33;
			ex.StartDate = new PhpDateTime ();
			ex.StartDate.date = "2017-01-18 00:00:00.000000";
			ex.Description = "Bonjour le monde.";
			ex.Location = "Lille";
			ex.PublicView = true;
			ex.Title = "Démonstration 1";
			temp.Add (ex);
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
			temp.Add (ex);
			//All.Add (ex);
			//All = temp;
			DesignData = temp;
			//PopulateData ();
			//} catch (Exception err) {
			//	Tools.Trace ("Exhibition-Error: " + err.Message);
			//}
		}

	}
}
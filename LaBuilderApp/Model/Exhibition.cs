﻿using System;
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

		public ImageSource LogoImage { get { return ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/boris/data/images/events/{logo}")); } }
		public ImageSource FlyerImage { get { return ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/boris/data/images/flyer/{flyer}")); } }
		public ImageSource CountryImage { get { return ImageSource.FromUri (new Uri ($"http://www.r2builders.fr/boris/Content/flags/{countryCode}.png")); } }
		public string DescriptionLabel { get { return description.Replace ("\\n", "\r\n"); } }

		private ObservableCollection<Builder> allBuilders;
		public ObservableCollection<Builder> AllBuilder {
			get {
				if (allBuilders != null) return allBuilders;
				allBuilders = new ObservableCollection<Builder> ();
				if (builderList == null) return allBuilders;
				foreach (int i in builderList) {
					allBuilders.Add (Builder.GetById (i));
				}
				return allBuilders;
			}
		}

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
								monthEvent = StartDate.Date.ToString ("MMMMM", Global.CultureFrench);
								if (oh.EndHour.date != null) {
									hourEvent = $"le {oh.StartHour.Date.ToString ("dddd dd MMMMM", Global.CultureFrench)} de {oh.StartHour.Date.ToString ("HH:mm", Global.CultureFrench)} à {oh.EndHour.Date.ToString ("HH:mm", Global.CultureFrench)}";
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

		public static ObservableCollection<ExhibitionGroup> AllGroup = new ObservableCollection<ExhibitionGroup> ();

		private static int currentYear = 0;
		public static int CurrentYear { get { return currentYear; } }

		public static void ChangeToYear (int year)
		{
			Tools.Trace ("ChangeToYear");
			if (currentYear == year) return;
			currentYear = year;
			Device.BeginInvokeOnMainThread (() => {
				//All.Clear ();
				AllGroup.Clear ();
				ExhibitionGroup eg = null;
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
					}
					if (eg != null)
						AllGroup.Add (eg);
					Tools.Trace ("ChangeToYear done.");
				} catch (Exception err) {
					Tools.Trace ("ChangeToYear-Error: " + err.Message);
				}
			});
		}

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
				currentYear = 0;
				ChangeToYear (DateTime.Now.Year);
			}
		}

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
			leg.Add (eg);
			//All.Add (ex);
			//All = temp;
			DesignData = leg;
			//PopulateData ();
			//} catch (Exception err) {
			//	Tools.Trace ("Exhibition-Error: " + err.Message);
			//}
		}

		public static IEnumerable<ExhibitionGroup> DesignData { get; set; }


	}
}
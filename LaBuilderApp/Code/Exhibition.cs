using System;
using System.Collections.Generic;

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

		public static IEnumerable<Exhibition> All { get; set; }

		static Exhibition ()
		{
			// DATA de démo pour la conception.
			List<Exhibition> temp = new List<Exhibition> ();
			Exhibition ex = new Exhibition ();
			ex.BuilderCode = 2634;
			ex.CountryCode = 33;
			ex.Description = "Bonjour le monde.";
			ex.Location = "Lille";
			ex.PublicView = true;
			ex.Title = "Démonstration 1";
			temp.Add (ex);
			ex = new Exhibition ();
			ex.BuilderCode = 2634;
			ex.CountryCode = 33;
			ex.Description = "Hello world!";
			ex.Location = "Paris";
			ex.PublicView = true;
			ex.Title = "Démonstration 2";
			temp.Add (ex);
			All = temp;
		}

	}
}
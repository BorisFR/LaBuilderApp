using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaBuilderApp
{
	public class JA
	{
		public string Object;
		public string X;
	}

	public class Exhibition
	{

		/* {
		"Id":"F9A5FD48-2BF5-DEF6-1E01-DB54B611A2A0","BuilderCode":2634,"EventType":0,"CountryCode":49,
		"Title":"Star Wars Celebration Europe 2",
		"Location":"Messe Essen",
		"Description":"",
		"StartDate":{"date":"2013-07-26 00:00:00.000000","timezone_type":3,"timezone":"Europe/Paris"},
		"EndDate":{"date":"2013-07-28 00:00:00.000000","timezone_type":3,"timezone":"Europe/Paris"},
		"OpenHourList":null,
		"AdminList":{"0":55},
		"BuilderList":{"0":2634},
		"Logo":"200B9342-0F34-0FF9-3877-8C8E6AFFF391.jpeg",
		"Flyer":null,
		"PublicView":1,
*/
		public string Id;
		public int BuilderCode;
		public int EventType;
		public int CountryCode;
		public string Title;
		public string Location;
		public string Description;

		public List<JA> AdminList;
		public List<JA> BuilderList;
		public string Logo;
		public string Flyer;
		public bool PublicView;

	}
}
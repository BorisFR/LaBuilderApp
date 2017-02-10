using System;
using Xamarin.Forms;
using System.Net;
using System.Globalization;

namespace LaBuilderApp
{

	public delegate void JobDone (bool status, string result);


	public static class Global
	{
		public static string BaseUrl = "http://r2builders.fr/";
		public static CultureInfo CultureFrench = new CultureInfo ("fr-FR");

		public static IFiles Files = null;

		public static Random Random;

		public static async void DoInit (string from)
		{
			if (Files != null) {
				Tools.Trace ($"> {from}: Global.DoInit() already done.");
				return;
			}
			Files = DependencyService.Get<IFiles> ();
			Random = new Random (DateTime.Now.Millisecond);

			IDataServer events = new IDataServer ("events");
			//events.IgnoreLocalData = true;
			events.DataRefresh += (status, result) => {
				Tools.Trace ("DataRefresh: " + result);
				Exhibition.All = Exhibition.LoadData (result);
				//Exhibition.LoadData (result);
			};
			DataServer.AddToDo (events);

			DataServer.Launch ();
			Tools.Trace ($"> {from}: Global.DoInit() done.");
		}

	}
}
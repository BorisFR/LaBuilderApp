using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LaBuilderApp
{

	public static class Tools
	{

		public static event JobDone JobDone;
		public static event JobDone DoneBatch;


		public static void Trace (string text)
		{
			System.Diagnostics.Debug.WriteLine (text);
		}

		private static HttpClient httpClient = null;

		private static HttpClient theHttpClient {
			get {
				if (httpClient != null)
					return httpClient;
				httpClient = new HttpClient ();
				Trace ("----------Max response size: " + httpClient.MaxResponseContentBufferSize.ToString ());
				httpClient.Timeout = new TimeSpan (0, 0, 0, 10, 500);
				httpClient.DefaultRequestHeaders.ExpectContinue = false;
				return httpClient;
			}
		}


		public static async Task DoDownload (object sender, string fileName)
		{
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format ($"{Global.BaseUrl}boris/getData.php?data={fileName}");
				if (Global.CurrentToken.Length > 0)
					url = string.Format ($"{Global.BaseUrl}boris/getData.php?data={fileName}&token={Global.CurrentToken}");
				if (fileName == "login")
					url = string.Format ($"{Global.BaseUrl}boris/dologin.php?login={Global.CurrentLogin}&password={Global.CurrentPassword}");
				Trace ("Url: " + url);
				result = await theHttpClient.GetStringAsync (url);
				status = true;
			} catch (Exception err) {
				Trace ("ERROR: " + err.Message);
			}
			if (JobDone != null)
				JobDone (sender, status, result);
			if (DoneBatch != null)
				DoneBatch (sender, status, result);
		}

		public static void test ()
		{
			//Global.Files.
		}

		public static async Task DelayedGCAsync ()
		{
			await Task.Delay (2000);
			GC.Collect ();
		}

	}
}
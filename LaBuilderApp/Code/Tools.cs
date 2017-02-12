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

	}
}
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CreateiBeacon
{

	public delegate void JobDone (string state, bool status, string data);

	public class InfoWeb
	{
		public event JobDone JobDone;
		private HttpClient httpClient = null;

		public InfoWeb ()
		{

		}

		private HttpClient theHttpClient {
			get {
				if (httpClient != null)
					return httpClient;
				httpClient = new HttpClient ();
				//Trace ("----------Max response size: " + httpClient.MaxResponseContentBufferSize.ToString ());
				httpClient.Timeout = new TimeSpan (0, 0, 0, 10, 500);
				httpClient.DefaultRequestHeaders.ExpectContinue = false;
				return httpClient;
			}
		}

		public async Task DoDownload (string state, string url)
		{
			string result = string.Empty;
			bool status = false;
			try {
				result = await theHttpClient.GetStringAsync (url);
				status = true;
			} catch (Exception err) {
				result = err.Message;
			}
			if (JobDone != null)
				JobDone (state, status, result);
		}

		public string GetValueFrom (string key, string data)
		{
			int p = data.IndexOf (key + "\"");
			if (p < 0) return string.Empty;
			p = data.IndexOf (":", p);
			int e = data.IndexOf ("\"", p + 2);
			string temp = data.Substring (p + 2, e - (p + 2));
			return temp;
		}
	}
}
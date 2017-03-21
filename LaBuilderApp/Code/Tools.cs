using System;
using System.Net.Http;
using System.Threading.Tasks;
using Plugin.Settings;

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
				//Trace ("----------Max response size: " + httpClient.MaxResponseContentBufferSize.ToString ());
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
				string url = string.Format ($"{Global.AppUrl}getData.php?data={fileName}");
				if (Global.CurrentToken.Length > 0)
					url = string.Format ($"{Global.AppUrl}getData.php?data={fileName}&token={Global.CurrentToken}");
				if (fileName == "login")
					url = string.Format ($"{Global.AppUrl}dologin.php?login={Global.CurrentLogin}&password={Global.CurrentPassword}");
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

		private static event ScoreDone ScoreDone;
		private static bool justLoadScores = false;

		private static async Task DoScore (bool isFirst, int game, int level, int score)
		{
			string result = string.Empty;
			bool status = false;
			try {
				string url = string.Format ($"{Global.AppUrl}doscore.php?game={game}&level={level}&score={score}");
				if (!justLoadScores && Global.CurrentToken.Length > 0)
					url = string.Format ($"{Global.AppUrl}doscore.php?game={game}&level={level}&score={score}&token={Global.CurrentToken}");
				Trace ("Url: " + url);
				result = await theHttpClient.GetStringAsync (url);
				status = true;
			} catch (Exception err) {
				Trace ("DoScore ERROR: " + err.Message);
			}
			if (ScoreDone != null)
				ScoreDone (isFirst, game, level, score, status, result);
		}

		static async void Tools_ScoreDone (bool isFirst, int game, int level, int score, bool status, string result)
		{
			if (status) {
				ScoreDone -= Tools_ScoreDone;
				string fileName = $"game_{game.ToString ()}_{level.ToString ()}";
				if (result.StartsWith ("{") || result.StartsWith ("[")) {
					await Global.Files.SaveFile (fileName, result);
					CrossSettings.Current.AddOrUpdateValue<DateTime> ($"cache_{fileName}", DateTime.UtcNow);
				}
				TheWinners = result;
				if (JobDone != null)
					JobDone (null, status, result);
				return;
			}
			if (isFirst) {
				DoScore (false, game, level, score);
				return;
			}
			if (JobDone != null)
				JobDone (null, status, result);
		}

		public static void PostScore (int game, int level, int score)
		{
			TheWinners = string.Empty;
			ScoreDone += Tools_ScoreDone;
			DoScore (true, game, level, score);
		}

		public static string TheWinners = string.Empty;

		public static async Task LoadWinners (int game, int level)
		{
			TheWinners = string.Empty;
			string fileName = $"game_{game.ToString ()}_{level.ToString ()}";
			if (await Global.Files.IsExit (fileName)) {
				TheWinners = await Global.Files.ReadFile (fileName);
			} else {
				JobDone += Tools_JobDone_Score;
				justLoadScores = true;
				PostScore (game, level, 0);
			}
		}

		static void Tools_JobDone_Score (object sender, bool status, string result)
		{
			JobDone -= Tools_JobDone_Score;
			justLoadScores = false;
			if (status)
				TheWinners = result;
		}

		public static async Task DelayedGCAsync ()
		{
			await Task.Delay (2000);
			GC.Collect ();
		}


		public static string PrettyLabel (string source)
		{
			return source.Replace ("\\n", "\r\n")
				  .Replace ("&quot;", "\"")
				  .Replace ("&gt;", ">")
				  .Replace ("&lt;", "<");
		}
	}
}
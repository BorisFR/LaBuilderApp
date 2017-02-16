using System;
using System.Threading.Tasks;
using Plugin.Settings;

namespace LaBuilderApp
{
	public class IDataServer
	{
		public event Trigger StartWorking;
		public event JobDone DataRefresh;

		public bool IgnoreLocalData = false;
		public bool ForceFreshData = false;

		private string fileName = string.Empty;
		public string FileName { get { return fileName; } }

		public IDataServer (string name)
		{
			fileName = name;
		}

		public async Task<string> OldData ()
		{
			Tools.Trace ("Read data from file: " + fileName);
			string x = await Global.Files.ReadFile (fileName);
			return x;
		}

		public void DoDownload ()
		{
			if (StartWorking != null) StartWorking ();
			Tools.DoneBatch += DoneBatch;
			Tools.DoDownload (this, fileName);
		}

		private async void DoneBatch (object sender, bool status, string result)
		{
			Tools.DoneBatch -= DoneBatch;
			isExistAlreadyTest = false;
			if (status) { // on sauvegarde en cache
				if (result.StartsWith ("{\"state\":false")) { // pas d'erreur de transfert, mais pas de bonnes data récupérées
					status = false;
					Tools.Trace ($"Data error: {result}");
				} else {
					await Global.Files.SaveFile (fileName, result);
					CrossSettings.Current.AddOrUpdateValue<DateTime> ($"cache_{fileName}", DateTime.UtcNow);
				}
			}
			DataRefresh (this, status, result);
		}

		private bool isExist = false;
		private bool isExistAlreadyTest = false;

		public async Task<bool> HasOldData ()
		{
			if (IgnoreLocalData) {
				Tools.Trace ("Ignore local for: " + fileName);
				return false;
			}
			if (isExistAlreadyTest)
				return isExist;
			isExistAlreadyTest = true;
			//Tools.Trace ("HasOldData testing fileExist: " + fileName);
			DateTime dataTime = CrossSettings.Current.GetValueOrDefault<DateTime> ($"cache_{fileName}", new DateTime (2000, 1, 1));
			DateTime now = DateTime.Now;
			if ((now - dataTime).TotalHours > 24) { // ignore les data plus anciennes que 24 heures
				isExist = false;
				Tools.Trace ($"FileExist {fileName}: exist, but too old");
				return isExist;
			} else {
				isExist = await Global.Files.IsExit (fileName);
			}
			Tools.Trace ($"FileExist {fileName}: {isExist}");
			return isExist;
		}

		public async Task<bool> FileIsPresent ()
		{
			return await Global.Files.IsExit (fileName);
		}

		public DateTime FileDate ()
		{
			return CrossSettings.Current.GetValueOrDefault<DateTime> ($"cache_{fileName}", new DateTime (2000, 1, 1)).ToUniversalTime ();
		}

		public void TriggerData (bool status, string result)
		{
			DataRefresh (this, status, result);
		}


		public static async void ClearData (string fileName)
		{
			if (await Global.Files.IsExit (fileName)) {
				await Global.Files.DeleteFile (fileName);
				CrossSettings.Current.Remove ($"cache_{fileName}");
			}
		}
	}
}
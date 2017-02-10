using System;
using System.Threading.Tasks;

namespace LaBuilderApp
{
	public class IDataServer
	{
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
			Tools.DoneBatch += DoneBatch;
			Tools.DoDownload (fileName);
		}

		private async void DoneBatch (bool status, string result)
		{
			Tools.DoneBatch -= DoneBatch;
			if (status) {
				await Global.Files.SaveFile (fileName, result);
			}
			DataRefresh (status, result);
		}

		private bool isExit = false;
		private bool isTest = false;

		public async Task<bool> HasOldData ()
		{
			if (IgnoreLocalData) {
				Tools.Trace ("Ignore local for: " + fileName);
				return false;
			}
			if (isTest)
				return isExit;
			isTest = true;
			Tools.Trace ("HasOldData testing fileExist: " + fileName);
			isExit = await Global.Files.IsExit (fileName);
			return isExit;
		}

		public void TriggerData (bool status, string result)
		{
			DataRefresh (status, result);
		}

	}
}
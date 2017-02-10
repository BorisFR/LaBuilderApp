using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaBuilderApp
{
	public static class DataServer
	{
		private static List<IDataServer> toDo = new List<IDataServer> ();
		private static bool inProgress = false;
		private static IDataServer current = null;
		private static int count = 0;
		private static IDataServer temp = null;

		public static async Task AddToDo (IDataServer element)
		{
			var z = await element.HasOldData ();
			if (z) {
				string x = await element.OldData ();
				element.TriggerData (true, x);
				if (!element.ForceFreshData)
					return;
			}
			toDo.Add (element);
			//DoJob ();
			if (element.IgnoreLocalData)
				DoJob ();
		}


		public static void Launch ()
		{
			Tools.Trace ("************* Launching batch download");
			DoJob ();
		}

		private static void DoJob ()
		{
			if (inProgress)
				return;
			if (toDo.Count == 0)
				return;
			inProgress = true;
			current = toDo [0];
			Tools.Trace ("Processing: " + current.FileName);
			current.JobDone += Current_JobDone;
			current.DoDownload ();
		}

		static void Current_JobDone (bool status, string result)
		{
			Tools.Trace ("Job done: " + current.FileName);
			current.JobDone -= Current_JobDone;
			current.TriggerData (status, result);
			current = null;
			if (!status) {
				// on error, try again
				count++;
				// but just 3 times
				if (count > 2) {
					// abandon
					toDo.RemoveAt (0);
					count = 0;
				}
			} else {
				// ok, we've done it
				toDo.RemoveAt (0);
				count = 0;
			}
			inProgress = false;
			DoJob ();
		}


	}
}
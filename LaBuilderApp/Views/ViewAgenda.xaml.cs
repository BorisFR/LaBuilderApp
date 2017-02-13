using Plugin.AppInfo;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewAgenda : ContentView
	{
		public ViewAgenda ()
		{
			InitializeComponent ();
			if (!CrossAppInfo.Current.DisplayName.Equals ("XamarinFormsPreviewer"))
				lvExhibition.ItemsSource = Exhibition.All;

			btPreviousYear.Clicked += (sender, e) => {
				Exhibition.ChangeToYear (Exhibition.CurrentYear - 1);
				lYear.Text = $"Agenda {Exhibition.CurrentYear.ToString ()}";
			};
			btNextYear.Clicked += (sender, e) => {
				Exhibition.ChangeToYear (Exhibition.CurrentYear + 1);
				lYear.Text = $"Agenda {Exhibition.CurrentYear.ToString ()}";
			};

			lvExhibition.ItemSelected += (sender, e) => {
				if (lvExhibition.SelectedItem == null) return;
				ChooseIsDone ();
			};

			lvExhibition.Refreshing += (sender, e) => {
				IDataServer events = new IDataServer ("events");
				events.IgnoreLocalData = true;
				events.DataRefresh += (obj, status, result) => {
					IDataServer x = obj as IDataServer;
					if (status) {
						Tools.Trace ($"DataRefresh {x.FileName}: {result}");
						Exhibition.LoadData (result);
						Exhibition.PopulateData ();
					} else {
						Tools.Trace ($"DataRefresh ERROR {x.FileName}: {result}");
					}
				};
				DataServer.AddToDo (events);

				DataServer.QueueEmpty += () => {
					lvExhibition.EndRefresh ();
					DataServer.QueueEmpty = null;
				};
				DataServer.Launch ();

			};
		}

		private async void ChooseIsDone ()
		{
			Global.SelectedExhibition = lvExhibition.SelectedItem as Exhibition;
			lvExhibition.SelectedItem = null;
			await Navigation.PushModalAsync (new PageAgenda (), true);
		}

	}
}
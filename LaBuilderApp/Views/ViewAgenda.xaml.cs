using Plugin.AppInfo;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewAgenda : ContentView
	{
		public ViewAgenda ()
		{
			InitializeComponent ();
			if (!CrossAppInfo.Current.DisplayName.Equals ("XamarinFormsPreviewer")) {
				Device.BeginInvokeOnMainThread (() => {
					lvExhibition.ItemsSource = Exhibition.AllGroup;
				});
			}

			if (Exhibition.AllGroup.Count == 0) {
				Device.BeginInvokeOnMainThread (() => {
					lvExhibition.IsRefreshing = true;
					DoRefresh ();
				});
			}

			btPreviousYear.Clicked += (sender, e) => {
				Device.BeginInvokeOnMainThread (() => {
					Exhibition.ChangeToYear (Exhibition.CurrentYear - 1);
					lYear.Text = $"Agenda {Exhibition.CurrentYear.ToString ()}";
				});
			};
			btNextYear.Clicked += (sender, e) => {
				Device.BeginInvokeOnMainThread (() => {
					Exhibition.ChangeToYear (Exhibition.CurrentYear + 1);
					lYear.Text = $"Agenda {Exhibition.CurrentYear.ToString ()}";
				});
			};

			lvExhibition.ItemSelected += (sender, e) => {
				Device.BeginInvokeOnMainThread (() => {
					if (lvExhibition.SelectedItem == null) return;
					ChooseIsDone ();
				});
			};

			lvExhibition.Refreshing += (sender, e) => {
				Device.BeginInvokeOnMainThread (() => {
					DoRefresh ();
				});
			};
		}

		private void DoRefresh ()
		{
			IDataServer events = new IDataServer ("events");
			events.IgnoreLocalData = true;
			events.DataRefresh += (obj, status, result) => {
				IDataServer x = obj as IDataServer;
				if (status) {
					Tools.Trace ($"DataRefresh {x.FileName}: {result}");
					Device.BeginInvokeOnMainThread (() => {
						Exhibition.LoadData (result);
						Exhibition.PopulateData ();
					});
				} else {
					Tools.Trace ($"DataRefresh ERROR {x.FileName}: {result}");
				}
			};
			DataServer.AddToDo (events);

			DataServer.QueueEmpty += () => {
				Device.BeginInvokeOnMainThread (() => {
					lvExhibition.EndRefresh ();
				});
				DataServer.QueueEmpty = null;
			};
			DataServer.Launch ();
		}

		private void ChooseIsDone ()
		{
			Global.SelectedExhibition = lvExhibition.SelectedItem as Exhibition;
			Device.BeginInvokeOnMainThread (() => {
				lvExhibition.SelectedItem = null;
			});
			Navigation.PushModalAsync (new PageAgenda (), true);
		}

	}
}
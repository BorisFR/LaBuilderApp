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
				ChooseIsDone (this);
			};
		}

		private async void ChooseIsDone (ViewAgenda instance)
		{
			Global.SelectedExhibition = lvExhibition.SelectedItem as Exhibition;
			lvExhibition.SelectedItem = null;
			await Navigation.PushModalAsync (new PageAgenda (), true);
			//Navigation.PopModalAsync (true);
		}

	}
}
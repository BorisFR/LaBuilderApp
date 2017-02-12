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
		}

	}
}
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent ();
			Global.DoInit ("App");
			//MainPage = new LaBuilderAppPage ();
			MainPage = new MainAppPage ();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
			Global.DoInit ("OnStart");
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
			Tools.Trace ("OnSleep...");
			Global.OnSleep = true;
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
			Global.IsResume = true;
			Global.DoInit ("OnResume");
		}

	}
}
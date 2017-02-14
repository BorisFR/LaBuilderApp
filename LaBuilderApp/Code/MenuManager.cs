using System;
using System.Collections.ObjectModel;

namespace LaBuilderApp
{

	public class MenuGroup : ObservableCollection<Menu>
	{

		public MenuGroup (string title)
		{
			Title = title;
		}

		public string Title { get; private set; }

	}


	public class MenuManager
	{

		public static ObservableCollection<MenuGroup> All { get; set; }

		public MenuManager ()
		{
			All = new ObservableCollection<MenuGroup> ();
		}

		private static string T (string source)
		{
			return source;
			//return Translation.GetString (source);
		}

		public static void Refresh ()
		{
			All.Clear ();

			MenuGroup mg = new MenuGroup (T (" "));
			mg.Add (new Menu () {
				Page = MyPage.News,
				Title = T ("La Gazette"),
				Detail = T ("Les actus de l'association"),
				Icon = "news"
			});
			mg.Add (new Menu () {
				Page = MyPage.Events,
				Title = T ("L'Agenda"),
				Detail = T ("Les R2 Builders en balade"),
				Icon = "planner"
			});
			mg.Add (new Menu () {
				Page = MyPage.Builders,
				Title = T ("Les Builders"),
				Detail = T ("Mais qui sont'ils ?"),
				Icon = "builders"
			});
			mg.Add (new Menu () {
				Page = MyPage.About,
				Title = T ("A propos de"),
				Detail = T ("Informations"),
				Icon = "about"
			});
			/*
			mg.Add (new Menu () {
				Page = MyPage.None,
				Title = T ("MenuTheDroids"),
				Detail = T ("MenuTheDroidsDetail"),
				Icon = "robots"
			});
			//mg.Add (new Menu (){ Page = MyPage.None, Title = "AstroDex", 			Detail = "L'anuaire des unités Astro", 		Icon = "IA" });
			mg.Add (new Menu () {
				Page = MyPage.None,
				Title = T ("MenuMyCards"),
				Detail = T ("MenuMyCardsDetail"),
				Icon = "cards"
			});
			mg.Add (new Menu () {
				Page = MyPage.None,
				Title = T ("MenuQrCode"),
				Detail = T ("MenuQrCodeDetail"),
				Icon = "qrcode"
			});
			//mg.Add (new Menu (){ Page = MyPage.None, Title = "Vos cartes", 			Detail = "Votre collection de cartes", 		Icon = "IA" });
			//mg.Add (new Menu (){ Page = MyPage.None, Title = "Vos récompenses", 	Detail = "Vos récompenses obtenues", 		Icon = "IA" });
			mg.Add (new Menu () {
				Page = MyPage.Account,
				Title = T ("MenuMyAccount"),
				Detail = T ("MenuMyAccountDetail"),
				Icon = "user"
			});
			//mg.Add (new Menu (){ Page = MyPage.None, Title = "Aide", 				Detail = "Comment fonctionne l'application", Icon = "IA" });
			*/
			All.Add (mg);

			mg = new MenuGroup (T ("Builder"));
			mg.Add (new Menu () {
				Page = MyPage.Connect,
				Title = T ("Se connecter"),
				Detail = T ("Privilège des Builders..."),
				Icon = "login"
			});
			All.Add (mg);


			/*
			if (Global.IsConnected) {
				mg = new MenuGroup (T ("MenuMenuBuilders"));
				if (Global.ConnectedUser.IsBuilder) {
					mg.Add (new Menu () {
						Page = MyPage.MyBuilder,
						Title = T ("MenuBuilderPresentation"),
						Detail = T ("MenuBuilderPresentationDetail"),
						Icon = "builder"
					});
					mg.Add (new Menu () {
						Page = MyPage.None,
						Title = T ("MenuBuilderDroids"),
						Detail = T ("MenuBuilderDroidsDetail"),
						Icon = "myrobots"
					});
					mg.Add (new Menu () {
						Page = MyPage.MyExhibitions,
						Title = T ("MenuBuilderEvents"),
						Detail = T ("MenuBuilderEventsDetail"),
						Icon = "planner"
					});
					if (Global.ConnectedUser.IsNewser) {
						mg.Add (new Menu () {
							Page = MyPage.None,
							Title = T ("MenuAdminNews"),
							Detail = T ("MenuAdminNewsDetail"),
							Icon = "mynews"
						});
					}
				}
				if (mg.Count > 0)
					All.Add (mg);
				mg = new MenuGroup (T ("MenuMenuAdmin"));
				if (Global.ConnectedUser.IsModo) {
					mg.Add (new Menu () {
						Page = MyPage.None,
						Title = T ("MenuAdminAllNews"),
						Detail = T ("MenuAdminAllNewsDetail"),
						Icon = "adminnews"
					});
				}
				if (Global.ConnectedUser.IsAdmin) {
					mg.Add (new Menu () {
						Page = MyPage.AdminUsers,
						Title = T ("MenuAdminUsers"),
						Detail = T ("MenuAdminUsersDetail"),
						Icon = "adminusers"
					});
				}
				if (Global.ConnectedUser.NickName.Equals ("Boris")) {
					mg.Add (new Menu () {
						Page = MyPage.AdminBuilders,
						Title = "Admin builders",
						Detail = "Gérer qui est builder",
						Icon = "IA"
					});
				}
				if (mg.Count > 0)
					All.Add (mg);
			}
*/
		}

	}
}


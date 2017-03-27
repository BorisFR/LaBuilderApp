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
				Icon = "events"
			});
			mg.Add (new Menu () {
				Page = MyPage.Builders,
				Title = T ("Les Builders"),
				Detail = T ("Mais qui sont'ils ?"),
				Icon = "builders"
			});
			mg.Add (new Menu () {
				Page = MyPage.Things,
				Title = T ("Constructions"),
				Detail = T ("Ce qu'ils ont construit !"),
				Icon = "things"
			});
			mg.Add (new Menu () {
				Page = MyPage.Medias,
				Title = T ("Medias"),
				Detail = T ("Vu dans les médias"),
				Icon = "medias"
			});
			/*
			mg.Add (new Menu () {
				Page = MyPage.GameR2Finder,
				Title = T ("R2 Finder"),
				Detail = T ("Un petit jeu avec R2 !"),
				Icon = "game1"
			});*/
			mg.Add (new Menu () {
				Page = MyPage.Culture,
				Title = T ("Univers"),
				Detail = T ("L'univers Star Wars"),
				Icon = "univers"
			});
			/*
			mg.Add (new Menu () {
				Page = MyPage.About,
				Title = T ("A propos de"),
				Detail = T ("Informations"),
				Icon = "about"
			});
			mg.Add (new Menu () {
				Page = MyPage.Legal,
				Title = T ("Mentions légales"),
				Detail = T ("Parce qu'il en faut"),
				Icon = "info"
			});
			*/

			mg.Add (new Menu () {
				Page = MyPage.Radar,
				Title = T ("Radar"),
				Detail = T ("Radar"),
				Icon = "univers"
			});
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


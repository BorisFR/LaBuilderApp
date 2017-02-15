﻿using System;
using System.Collections.Generic;
using Plugin.DeviceInfo;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class DetailPage : ContentPage
	{
		public DetailPage ()
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar (this, false);

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				try {
					Global.MainAppPage.IsPresented = true;
				} catch (Exception) {
				}
			};
			btMenu.GestureRecognizers.Add (tapGestureRecognizer);

			Menu m = new Menu ();
			m.Detail = string.Empty;
			m.Title = "Chargement des donnees";
			m.Page = MyPage.FirstLoading;
			ShowPage (m);
		}

		public void ShowPage (Menu menu)
		{
			Device.BeginInvokeOnMainThread (() => {
				if (CrossDeviceInfo.Current.Model.Equals ("iPad"))
					theTitle.ChangeText (menu.Title.ToLower () + " - " + menu.Detail.ToLower ());
				else
					theTitle.ChangeText (menu.Title.ToLower ());
				switch (menu.Page) {
				case MyPage.FirstLoading:
					theFrame.Content = null;
					theFrame.Content = new ViewFirstLoading ();
					break;
				case MyPage.News:
					theFrame.Content = null;
					theFrame.Content = new ViewNews ();
					break;
				case MyPage.Events:
					theFrame.Content = null;
					theFrame.Content = new ViewAgenda ();
					break;
				case MyPage.Builders:
					theFrame.Content = null;
					theFrame.Content = new ViewBuilders ();
					break;
				case MyPage.Media:
					theFrame.Content = null;
					theFrame.Content = new ViewMedia ();
					break;
				case MyPage.About:
					theFrame.Content = null;
					theFrame.Content = new ViewAbout ();
					break;
				case MyPage.Connect:
					theFrame.Content = null;
					theFrame.Content = new ViewConnect ();
					break;
					/*			
								case MyPage.Account:
									theFrame.Content = null;
									theFrame.Content = new ViewAccount ();
									break;
								case MyPage.MyBuilder:
									theFrame.Content = null;
									theFrame.Content = new ViewMyBuilder ();
									break;
								case MyPage.MyExhibitions:
									theFrame.Content = null;
									theFrame.Content = new ViewMyExhibitions ();
									break;
								case MyPage.AdminUsers:
									theFrame.Content = null;
									theFrame.Content = new ViewAdminUsers ();
									break;
								case MyPage.AdminBuilders:
									theFrame.Content = null;
									theFrame.Content = new ViewAdminBuilders ();
									break;*/
				}
			});
		}

	}
}
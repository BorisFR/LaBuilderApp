#define FIX_WINDOWS_DOUBLE_TAPS         // Double-taps don't work well on Windows Runtime as of 2.3.0
#define FIX_WINDOWS_PHONE_NULL_CONTENT  // Set Content of Frame to null doesn't work in Windows as of 2.3.0

using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{
	enum GameR2FinderTileStatus
	{
		Hidden,
		Flagged,
		Exposed,
		ExposedDoubleTap
	}

	class GameR2FinderTile : Frame
	{
		GameR2FinderTileStatus tileStatus = GameR2FinderTileStatus.Hidden;
		Label label;
		Image flagImage, bugImage;
		Color fullBackGround = Color.FromHex ("5AA9D3");
		Color astroBackGround = Color.White;
		Color labelBackGround = Color.FromHex ("11252D");

		static List<string> astroList = new List<string> ();
		static ImageSource flagImageSource {
			get {
				string s = astroList [Global.Random.Next (astroList.Count)];
				return ImageSource.FromResource ("LaBuilderApp.Images." + s + ".png");
			}
		}
		static ImageSource bugImageSource;
		bool doNotFireEvent;

		public event EventHandler<GameR2FinderTileStatus> TileStatusChanged;

		static GameR2FinderTile ()
		{
			astroList.Add ("r2a6-2-125");
			astroList.Add ("r2b1-2-125");
			astroList.Add ("r2d2-2-125");
			astroList.Add ("r2r9-2-125");
			astroList.Add ("r2v2-2-125");
			astroList.Add ("r3d2-2-125");
			astroList.Add ("r3t7-2-125");
			astroList.Add ("r4g9-2-125");
			astroList.Add ("r4i9-2-125");
			astroList.Add ("r4m9-2-125");
			astroList.Add ("r5a7-2-125");
			astroList.Add ("r5d4-2-125");
			astroList.Add ("r6t6-2-125");
			astroList.Add ("r7s1-2-125");
			astroList.Add ("r9-1-125");

			astroList.Add ("r2a6-1-125");
			astroList.Add ("r2b1-1-125");
			astroList.Add ("r2d2-1-125");
			astroList.Add ("r2r9-1-125");
			astroList.Add ("r2v2-1-125");
			astroList.Add ("r3d2-1-125");
			astroList.Add ("r3t7-1-125");
			astroList.Add ("r4g9-1-125");
			astroList.Add ("r4i9-1-125");
			astroList.Add ("r4m9-1-125");
			astroList.Add ("r5a7-1-125");
			astroList.Add ("r5d4-1-125");
			astroList.Add ("r6t6-1-125");
			astroList.Add ("r7s1-1-125");


			//flagImageSource = ImageSource.FromResource ("LaBuilderApp.Images.gamer2d2white.png");
			bugImageSource = ImageSource.FromResource ("LaBuilderApp.Images.darth-vader.png");
		}

		public GameR2FinderTile (int row, int col)
		{
			this.Row = row;
			this.Col = col;

			this.BackgroundColor = Color.FromHex ("5AA9D3");
			this.OutlineColor = Color.FromHex ("11252D");
			this.Padding = 2;

			label = new Label {
				Text = " ",
				TextColor = Color.FromHex ("5AA9D3"),
				BackgroundColor = Color.FromHex ("11252D"),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
			};

			flagImage = new Image {
				Source = flagImageSource,
			};

			bugImage = new Image {
				Source = bugImageSource
			};

			TapGestureRecognizer singleTap = new TapGestureRecognizer {
				NumberOfTapsRequired = 1
			};
			singleTap.Tapped += OnSingleTap;
			this.GestureRecognizers.Add (singleTap);

#if FIX_WINDOWS_DOUBLE_TAPS

			if (Device.OS != TargetPlatform.Windows && Device.OS != TargetPlatform.WinPhone) {

#endif

				TapGestureRecognizer doubleTap = new TapGestureRecognizer {
					NumberOfTapsRequired = 2
				};
				doubleTap.Tapped += OnDoubleTap;
				this.GestureRecognizers.Add (doubleTap);

#if FIX_WINDOWS_DOUBLE_TAPS

			}

#endif

		}

		public int Row { private set; get; }

		public int Col { private set; get; }

		public bool IsBug { set; get; }

		public int SurroundingBugCount { set; get; }

		public GameR2FinderTileStatus Status {
			set {
				if (tileStatus != value) {
					tileStatus = value;

					switch (tileStatus) {
					case GameR2FinderTileStatus.Hidden:
						this.Content = null;
						this.BackgroundColor = fullBackGround;

#if FIX_WINDOWS_PHONE_NULL_CONTENT

						if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows) {
							this.Content = new Label { Text = " " };
						}

#endif
						break;

					case GameR2FinderTileStatus.Flagged:
						this.Content = flagImage;
						this.BackgroundColor = astroBackGround;
						break;

					case GameR2FinderTileStatus.Exposed:
						if (this.IsBug) {
							this.Content = bugImage;
							this.BackgroundColor = astroBackGround;
						} else {
							this.Content = label;
							this.BackgroundColor = labelBackGround;
							label.Text =
									(this.SurroundingBugCount > 0) ?
										this.SurroundingBugCount.ToString () : " ";
						}
						break;
					}

					if (!doNotFireEvent && TileStatusChanged != null) {
						TileStatusChanged (this, tileStatus);
					}
				} else if (tileStatus == GameR2FinderTileStatus.Exposed) {
					if (!this.IsBug) {
						// double click sur un nombre
						if (!doNotFireEvent && TileStatusChanged != null) {
							TileStatusChanged (this, GameR2FinderTileStatus.ExposedDoubleTap);
						}
					}
				}
			}
			get {
				return tileStatus;
			}
		}

		// Does not fire TileStatusChanged events.
		public void Initialize ()
		{
			doNotFireEvent = true;
			this.Status = GameR2FinderTileStatus.Hidden;
			this.IsBug = false;
			this.SurroundingBugCount = 0;
			doNotFireEvent = false;
		}

#if FIX_WINDOWS_DOUBLE_TAPS

		bool lastTapSingle;
		DateTime lastTapTime;

#endif

		void OnSingleTap (object sender, object args)
		{

#if FIX_WINDOWS_DOUBLE_TAPS

			if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone) {
				if (lastTapSingle && DateTime.Now - lastTapTime < TimeSpan.FromMilliseconds (500)) {
					OnDoubleTap (sender, args);
					lastTapSingle = false;
				} else {
					lastTapTime = DateTime.Now;
					lastTapSingle = true;
				}
			}

#endif

			switch (this.Status) {
			case GameR2FinderTileStatus.Hidden:
				this.Status = GameR2FinderTileStatus.Flagged;
				break;

			case GameR2FinderTileStatus.Flagged:
				this.Status = GameR2FinderTileStatus.Hidden;
				break;

			case GameR2FinderTileStatus.Exposed:
				// Do nothing
				break;
			}
		}

		void OnDoubleTap (object sender, object args)
		{
			this.Status = GameR2FinderTileStatus.Exposed;
		}
	}
}
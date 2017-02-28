using System;
using Android.Views;
using Xamarin.Forms.Platform.Android;

namespace LaBuilderApp.Droid
{
	public class CustomListViewRenderer : ListViewRenderer
	{
		private int _mPosition;

		public override bool DispatchTouchEvent (MotionEvent e)
		{
			if (e.ActionMasked == MotionEventActions.Down) {
				// Record the position the list the touch landed on
				_mPosition = this.Control.PointToPosition ((int)e.GetX (), (int)e.GetY ());
				return base.DispatchTouchEvent (e);
			}

			if (e.ActionMasked == MotionEventActions.Move) {
				// Ignore move eents
				return true;
			}

			if (e.ActionMasked == MotionEventActions.Up) {
				// Check if we are still within the same view
				if (this.Control.PointToPosition ((int)e.GetX (), (int)e.GetY ()) == _mPosition) {
					base.DispatchTouchEvent (e);
				} else {
					// Clear pressed state, cancel the action
					Pressed = false;
					Invalidate ();
					return true;
				}
			}

			return base.DispatchTouchEvent (e);
		}

	}
}
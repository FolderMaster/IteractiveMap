using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using UIKit;

using IteractiveMap.View;
using IteractiveMap.Droid;

[assembly: ExportRenderer(typeof(MainPage), typeof(MainPageRenderer))]
namespace IteractiveMap.Droid
{
    public class MainPageRenderer : PageRenderer
    {
        private MainPage _thePage;
        private UITapGestureRecognizer _tapGestureRecognizer;

        public MainPageRenderer()
        {
            _tapGestureRecognizer = new UITapGestureRecognizer(a => UITapGestureRecognizerHandler(a))
            {
                ShouldRecognizeSimultaneously = (recognizer, gestureRecognizer) => true,
                ShouldReceiveTouch = (recognizer, touch) => true,
            };
            _tapGestureRecognizer.DelaysTouchesBegan = _tapGestureRecognizer.DelaysTouchesEnded = _tapGestureRecognizer.CancelsTouchesInView = false;
        }

        private void UITapGestureRecognizerHandler(UITapGestureRecognizer gestureRecog)
        {
            if (gestureRecog.State == UIGestureRecognizerState.Ended)
            {
                var endPos = gestureRecog.LocationOfTouch(0, gestureRecog.View);

                _thePage.TriggerTouchDown(endPos.X, endPos.Y);
            }
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            if (e.OldElement != null)
            {
                View.RemoveGestureRecognizer(_tapGestureRecognizer);
                _thePage = null;
            }

            if (e.NewElement != null)
            {
                _thePage = e.NewElement as MainPage;
                View.AddGestureRecognizer(_tapGestureRecognizer);
            }

            base.OnElementChanged(e);
        }
}
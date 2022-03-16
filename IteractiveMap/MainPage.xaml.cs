using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
namespace IteractiveMap
{
    public partial class MainPage : ContentPage
    {
        double x, y;
        double scale;
        double xOffset, yOffset, currentScale, startScale;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
        }

        protected override void OnDisappearing()
        {
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }

        private void PinchGestureRecognizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = Content.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = Content.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

                // Apply translation based on the change in origin.

                // Apply scale factor.
                Content.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                // Store the translation delta's of the wrapped user interface element.
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
            }
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            double X = 0, Y = 0;
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    X = Math.Max(Math.Min(0, x + e.TotalX), - Content.Width);
                    Y = Math.Max(Math.Min(0, y + e.TotalY), - Content.Height);
                    break;
                case GestureStatus.Completed:
                    x = X;
                    y = Y;
                    break;
            }
            PlaceSearchBar.Text = "x:" + X.ToString() + "\ty:" + Y.ToString();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
        }

        private void PlaceSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            Random rand = new Random();
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = rand.Next(0, 25)
            };
            canvas.DrawCircle(info.Width / 2, info.Height / 2, 300, paint);
            paint.Style = SKPaintStyle.Fill;
            paint.Color = SKColors.Blue;
            canvas.DrawCircle(args.Info.Width / 2, args.Info.Height / 2, 300, paint);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
namespace IteractiveMap
{
    public partial class MainPage : ContentPage
    {
        double _x = 0, _y = 0;
        double _scale = 1;
        ObservableCollection<Place> _places = new ObservableCollection<Place>();

        ListView _listView = null;
        StackLayout _infoStackLayout = null;

        public MainPage()
        {
            InitializeComponent();

            _places.Add(new Place()
            {
                Name = "Дом Колотушкина",
                Adress = "улица Пушкина, дом Колотушкина",
                Point1 = new Point()
                {
                    X = 0,
                    Y = 0
                },
                Point2 = new Point()
                {
                    X = 100,
                    Y = 200
                },
            });
            _places.Add(new Place()
            {
                Name = "Дом Пушкина",
                Adress = "улица Пушкина, дом Пушкина",
                Point1 = new Point()
                {
                    X = 300,
                    Y = 400
                },
                Point2 = new Point()
                {
                    X = 600,
                    Y = 600
                },
            });
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
            switch (e.Status)
            {
                case GestureStatus.Started:
                    break;
                case GestureStatus.Running:
                    _scale += (e.Scale - 1) * _scale;
                    _scale = Math.Max(0.00000001, _scale);
                    _canvasView.InvalidateSurface();
                    _searchBar.Text = "scale:" + _scale.ToString();
                    break;
                case GestureStatus.Completed:
                    _searchBar.Text = null;
                    break;
            }
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            double X = 0, Y = 0;
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    break;
                case GestureStatus.Running:
                    X = e.TotalX * _scale;
                    Y = e.TotalY * _scale;
                    _x += X;
                    _y += Y;
                    _canvasView.InvalidateSurface();
                    _searchBar.Text = "x:" + X.ToString() + "\ty:" + Y.ToString();
                    break;
                case GestureStatus.Completed:
                    _searchBar.Text = null;
                    break;
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
        }

        private void _searchBar_Focused(object sender, FocusEventArgs e)
        {
            _listView = new ListView()
            {
                BackgroundColor = Color.White,
                ItemsSource = _places,
                ItemTemplate = new DataTemplate(() =>
                {
                    StackLayout SearchStackLayout = new StackLayout()
                    {
                        BackgroundColor = Color.DodgerBlue
                    };
                    Label NameSearchLabel = new Label()
                    {
                        TextColor = Color.Black,
                        FontAttributes = FontAttributes.Bold
                    };
                    Label AdressSearchLabel = new Label()
                    {
                        TextColor = Color.Black,
                        HorizontalTextAlignment = TextAlignment.End
                    };
                    NameSearchLabel.SetBinding(Label.TextProperty, "Name");
                    AdressSearchLabel.SetBinding(Label.TextProperty, "Adress");
                    SearchStackLayout.Children.Add(NameSearchLabel);
                    SearchStackLayout.Children.Add(AdressSearchLabel);
                    return new ViewCell() { View = SearchStackLayout };
                })
            };
            _listView.ItemSelected += _listView_ItemSelected;
            _listView.ItemTapped += _listView_ItemTapped;
            _searchstackLayout.Children.Add(_listView);
        }

        private void _searchBar_Unfocused(object sender, FocusEventArgs e)
        {
            _searchstackLayout.Children.Remove(_listView);
        }

        private void _searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(_listView != null)
            {
                _listView.ItemsSource = _places.Where(p => p.Name.Contains(e.NewTextValue));
            }
        }

        private void _searchBar_SearchButtonPressed(object sender, EventArgs e)
        {
        }

        private void _listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _infoStackLayout = new StackLayout();
            Label NameInfoLabel = new Label()
            {
                TextColor = Color.Black,
                FontAttributes = FontAttributes.Bold
            };
            Label AdressInfoLabel = new Label()
            {
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.End
            };
            NameInfoLabel.SetBinding(Label.TextProperty, "Name");
            AdressInfoLabel.SetBinding(Label.TextProperty, "Adress");
            _infoStackLayout.Children.Add(NameInfoLabel);
            _infoStackLayout.Children.Add(AdressInfoLabel);
            _stackLayout.Children.Add(_infoStackLayout);
        }

        private void _listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            _infoStackLayout = new StackLayout()
            {
                BackgroundColor = Color.White
            };
            Label NameInfoLabel = new Label()
            {
                TextColor = Color.Black,
                FontAttributes = FontAttributes.Bold
            };
            Label AdressInfoLabel = new Label()
            {
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.End
            };
            NameInfoLabel.SetBinding(Label.TextProperty, "Name");
            AdressInfoLabel.SetBinding(Label.TextProperty, "Adress");
            _infoStackLayout.Children.Add(NameInfoLabel);
            _infoStackLayout.Children.Add(AdressInfoLabel);
            _stackLayout.Children.Add(_infoStackLayout);
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            Random rand = new Random();
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Blue.ToSKColor(),
                StrokeWidth = (float)_scale * 10
            };
            canvas.Clear();
            for(int n = 0; n < _places.Count; ++n)
            {
                canvas.DrawRect(
                    (float)((_places[n].Point1.X - _x) * _scale),
                    (float)((_places[n].Point1.Y - _y) * _scale),
                    (float)((_places[n].Point2.X - _places[n].Point1.X) * _scale),
                    (float)((_places[n].Point2.Y - _places[n].Point1.Y) * _scale),
                    paint);
            }
        }
    }
}
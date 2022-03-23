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

using IteractiveMap.Model;
namespace IteractiveMap.View
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
            _places.Add(new Place("Дом Колотушкина", "улица Пушкина, дом Колотушкина", "+877777777", null, new SKPath()));
            _places.Add(new Place("Дом Пушкина", "улица Пушкина, дом Пушкина", "+877777777", new string[2] {"VK", "KALL"}, new SKPath()));
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
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    break;
                case GestureStatus.Running:
                    _x += e.TotalX * _scale;
                    _y += e.TotalY * _scale;
                    _canvasView.InvalidateSurface();
                    _searchBar.Text = "x:" + _x.ToString() + "\ty:" + _y.ToString();
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
            _listView = null;
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
            if(_infoStackLayout == null)
            {
                Create_infoStackLayout((Place)e.SelectedItem);
                _stackLayout.Children.Add(_infoStackLayout);
            }
            else
            {
                _stackLayout.Children.Remove(_infoStackLayout);
                Create_infoStackLayout((Place)e.SelectedItem);
                _stackLayout.Children.Add(_infoStackLayout);
            }
            _searchBar.Unfocus();
        }

        private void _listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (_infoStackLayout == null)
            {
                Create_infoStackLayout((Place)e.Item);
                _stackLayout.Children.Add(_infoStackLayout);
            }
            else
            {
                _stackLayout.Children.Remove(_infoStackLayout);
                Create_infoStackLayout((Place)e.Item);
                _stackLayout.Children.Add(_infoStackLayout);
            }
            _searchBar.Unfocus();
        }

        private void Create_infoStackLayout(Place place)
        {
            _infoStackLayout = new StackLayout()
            {
                BackgroundColor = Color.White,
            };
            _infoStackLayout.Children.Add(new Label()
            {
                TextColor = Color.Black,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = place.Name
            });
            _infoStackLayout.Children.Add(new Label()
            {
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.End,
                Text = place.Adress
            });
            _infoStackLayout.Children.Add(new Label()
            {
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = place.PhoneNumber
            });
            if(place.Links != null)
            {
                for (int n = 0; n < place.Links.Length; n++)
                {
                    _infoStackLayout.Children.Add(new Label()
                    {
                        TextColor = Color.Black,
                        HorizontalTextAlignment = TextAlignment.Start,
                        Text = place.Links[n]
                    });
                }
            }
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
                canvas.DrawPath(_places[n].Region, paint);
            }
        }
    }
}
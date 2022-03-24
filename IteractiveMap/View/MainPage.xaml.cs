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

using Xamarin.Forms.Maps;

using IteractiveMap.Model;
namespace IteractiveMap.View
{
    public partial class MainPage : ContentPage
    {
        double _x = 0, _y = 0;
        double _scale = 1;

        ObservableCollection<Place> _places = new ObservableCollection<Place>();

        ListView _listView = null;
        
        public MainPage()
        {
            InitializeComponent();
            _places.Add(new Place("Дом Колотушкина", "улица Пушкина, дом Колотушкина", PlaceType.Creativity, "+877777777", null, new SKPoint[] { new SKPoint(0, 0), new SKPoint(0, 200), new SKPoint(200, 200), new SKPoint(200, 0) }));
            _places.Add(new Place("Дом Пушкина", "улица Пушкина, дом Пушкина", PlaceType.Sport, "+877777777", new string[] {"VK", "KALL"}, new SKPoint[] { new SKPoint(400, 400), new SKPoint(400, 800), new SKPoint(500, 800), new SKPoint(500, 400) }));
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

        private void _contentView_Tapped(object sender, MR.Gestures.TapEventArgs e)
        {
            if (e.Touches != null && e.Touches.Length > 0)
            {
                Point Touch = e.Touches.First();
                _searchBar.Text = "x:" + Touch.X.ToString() + "\ty:" + Touch.Y.ToString();
            }
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
            Create_infoStackLayout((Place)e.SelectedItem);
            _searchBar.Unfocus();
        }

        private void _listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Create_infoStackLayout((Place)e.Item);
            _searchBar.Unfocus();
        }

        private void Create_infoStackLayout(Place place)
        {
            _infoStackLayout.Children.Clear();
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
                HorizontalTextAlignment = TextAlignment.Start,
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

        private void Map_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
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
                Style = SKPaintStyle.Fill,
                StrokeWidth = 1
            };
            canvas.Clear();
            for(int n = 0; n < _places.Count; ++n)
            {
                SKPoint[] points = new SKPoint[_places[n].Region.Length];
                for(int h = 0; h < points.Length; ++h)
                {
                    points[h] = new SKPoint((float)((_x + _places[n].Region[h].X) * _scale), (float)((_y + _places[n].Region[h].Y) * _scale));
                }
                SKPath region = new SKPath();
                region.AddPoly(points);
                switch(_places[n].Type)
                {
                    case PlaceType.None:
                        paint.Color = Color.White.ToSKColor();
                        break;
                    case PlaceType.Creativity:
                        paint.Color = Color.Green.ToSKColor();
                        break;
                    case PlaceType.Self_development:
                        paint.Color = Color.Yellow.ToSKColor();
                        break;
                    case PlaceType.Sport:
                        paint.Color = Color.Aqua.ToSKColor();
                        break;
                }
                canvas.DrawPath(region, paint);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace IteractiveMap.Model
{
    class Place
    {
        public Place(string name, string adress, PlaceType type, string phonenumber, string[] links, SKPoint[] region)
        {
            Name = name;
            Adress = adress;
            Type = type;
            Links = links;
            PhoneNumber = phonenumber;
            Region = region;
        }

        public string Name
        {
            get;
            set;
        }

        public string Adress
        {
            get;
            set;
        }

        public PlaceType Type
        {
            get;
            set;
        }

        public string PhoneNumber
        {
            get;
            set;
        }

        public string[] Links
        {
            get;
            set;
        }

        public SKPoint[] Region
        {
            get;
            set;
        }
    }
}
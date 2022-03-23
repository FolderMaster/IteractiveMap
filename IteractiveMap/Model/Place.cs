using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace IteractiveMap.Model
{
    class Place
    {
        public Place(string name, string adress, string phonenumber, string[] links, SKPath region)
        {
            Name = name;
            Adress = adress;
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

        public SKPath Region
        {
            get;
            set;
        }
    }
}

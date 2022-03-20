using System;
using System.Collections.Generic;
using System.Text;

namespace IteractiveMap
{
    class Place
    {
        public override string ToString()
        {
            return Name;
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

        public Point Point1
        {
            get;
            set;
        }
        public Point Point2
        {
            get;
            set;
        }
    }
    class Point
    {
        public double X
        {
            get;
            set;
        }

        public double Y
        {
            get;
            set;
        }
    }
}

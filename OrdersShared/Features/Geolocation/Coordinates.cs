﻿namespace OrdersShared.Features.Geolocation
{
    public class Coordinates
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Coordinates() 
        {
            Latitude = 0;
            Longitude = 0;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;


namespace MerchantRPG.Models.Engine
{
    public class LocationService
    {
        //i'm lazy and .net core doesn't support System.Device.Locations apparently.
        //https://stackoverflow.com/questions/60700865/find-distance-between-2-coordinates-in-net-core
        public double CalculateDistance(Location point1, Location point2)
        {
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        public virtual Location CurrentLocation()
        {
            //do magic to get current gps coords
            var response = new Location()
            {
                Latitude = 0,
                Longitude = 0
            };
            return response; 
        }
    }


    public class LocationTestingService  : LocationService, ILocationService
    {
        public override Location CurrentLocation()
        {
            var response = new Location();
            return response; 
        }
    }

    public interface ILocationService
    {
        double CalculateDistance(Location point1, Location point2);
        Location CurrentLocation();
    }
}

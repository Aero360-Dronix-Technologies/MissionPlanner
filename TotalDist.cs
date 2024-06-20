using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectShowLib.SBE;
using GMap.NET.MapProviders;
using MissionPlanner.Utilities;
using SharpDX.Mathematics.Interop;


namespace MissionPlanner
{
    internal class TotalDist
    {
        public static double totaltime;
        public static double CalculateTotalDistance(List<Locationwp> locationwps)
        {
            double totalDistance = 0.0;
            List<Locationwp> speedline = locationwps.Where(wp => wp.id == 178).ToList();

            double calcspeed = speedline[0].p2;
            
            List<Locationwp> filteredWaypoints = locationwps.Where(wp => wp.id == 16).ToList();
            for (int i = 0; i < filteredWaypoints.Count - 1; i++)
            {
                PointLatLngAlt p1 = new PointLatLngAlt { Lat = filteredWaypoints[i].lat, Lng = filteredWaypoints[i].lng, Alt = filteredWaypoints[i].alt };
                PointLatLngAlt p2 = new PointLatLngAlt { Lat = filteredWaypoints[i + 1].lat, Lng = filteredWaypoints[i + 1].lng, Alt = filteredWaypoints[i + 1].alt };

                totalDistance += GMapProviders.EmptyProvider.Projection.GetDistance(p1, p2);
            }

            PointLatLngAlt first = new PointLatLngAlt { Lat = filteredWaypoints[0].lat, Lng = filteredWaypoints[0].lng, Alt = filteredWaypoints[0].alt };
            int lastind = filteredWaypoints.Count - 1;
            PointLatLngAlt second = new PointLatLngAlt { Lat = filteredWaypoints[lastind].lat, Lng = filteredWaypoints[lastind].lng, Alt = filteredWaypoints[lastind].alt };


            double homedist = second.GetDistance(MainV2.comPort.MAV.cs.PlannedHomeLocation) + first.GetDistance(MainV2.comPort.MAV.cs.PlannedHomeLocation);

            double finaldist = (totalDistance * 1000) + homedist;
            
            totaltime = (finaldist / (calcspeed * 0.8));

            return totaltime;

        }

    }
}

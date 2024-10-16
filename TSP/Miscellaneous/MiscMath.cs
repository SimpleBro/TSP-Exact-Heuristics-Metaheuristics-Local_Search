using TSP.InitialSolition;
using TSP.Metaheuristics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Numerics;

namespace TSP.Miscellaneous
{
    internal class MiscMath
    {
        /// <summary>
        /// Calculates the distance from a point (x3,y3) to a line ((x1,y1),(x2,y2)).
        /// </summary>
        /// <returns>The distance from the point to the line.</returns>
        public static double DistanceFromPointToLine(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            // Calculate the distance
            double distance = Math.Abs((x2 - x1)*(y1 -y3) - (x1 - x3)*(y2 - y1)) /
                              Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

            return distance;

        }

        /// <summary>
        /// Calculate the height (distance) value of point C from the hypotenuse in a given triangle [A, B, C].
        /// All distances of the triangle edges are known in advance.
        /// a^2 = p*c | b^2 = q*c | h^2 = p*q
        /// </summary>
        public static double HeightOfCFromHypotenuse(double a, double b, double c)
        {
            double p = Math.Pow(a, 2) / c;
            double q = Math.Pow(b, 2) / c;
            double h = Math.Sqrt(p * q);

            return h;
        }
        /// <summary>
        /// Calculate the acceleration value between two speed values. km/h -> m/s^2
        /// </summary>
        public static double KmToAcceleration(double v_1, double v_2)
        {
            return ((v_1 / 3.6) - (v_2 / 3.6)) / 300;
        }

        /// <summary>
        /// Calculate the Euclidian distance between two points (x_1,y_1), (x_2,y_2).
        /// </summary>
        public static double EuclidianDistance(double x_1, double y_1, double x_2, double y_2)
        {
            return Math.Sqrt(Math.Pow(x_1 - x_2, 2) + Math.Pow(y_1 - y_2, 2));
        }

        /// <summary>
        /// Return the minimal value of two values a,b.
        /// </summary>
        public static double Min(double a, double b)
        {
            return Math.Min(a, b);
        }

        /// <summary>
        /// Return the maximal value of two values a,b.
        /// </summary>
        public static double Max(double a, double b)
        {
            return Math.Max(a, b);
        }

        /// <summary>
        /// Return the angle [°] between two points x,y.
        /// </summary>
        public static double AngleDeg(double x, double y)
        {
            double res = Math.Atan(y / x);

            if (x < 0)
                res += Math.PI;

            if (res < 0)
                res += 2 * Math.PI;

            return res * 180 / Math.PI;
        }

        /// <summary>
        /// Round the double value to the closest integer value.
        /// </summary>
        public static int RoundToInt(double value)
        {
            return Convert.ToInt32(Math.Round(value));
        }

        /// <summary>
        /// The haversine formula determines the great-circle distance between two points on a sphere given their longitudes and latitudes.
        /// </summary>
        public static double AerialDistanceHaversine(double x_1, double y_1, double x_2, double y_2)
        {
            double R = 6371000; // metres
            double phi1 = y_1 * Math.PI / 180; // φ, λ in radians
            double phi2 = y_2 * Math.PI / 180;
            double deltaPhi = (y_2 - y_1) * Math.PI / 180;
            double deltaLambda = (x_2 - x_1) * Math.PI / 180;

            double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                      Math.Cos(phi1) * Math.Cos(phi2) *
                      Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double d = R * c;
            return d;
        }

        /// <summary>
        /// Transform Longitude and Latitude to Picture Graph scale.
        /// </summary>
        public static int[] LonLatToXY(double x, double y)
        {
            int[] coordinates = new int[2];
            double xMin = 15.93785;
            double xMax = 16.185188;
            double yMin = 45.768888;
            double yMax = 45.919225;
            double xv = (x - xMin) / (xMax - xMin) * (Properties.Settings.Default.graphWidth);
            coordinates[0] = Convert.ToInt32(Math.Round(xv));
            double yv = (y - yMin) / (yMax - yMin) * (Properties.Settings.Default.graphHeight);
            coordinates[1] = Convert.ToInt32(Math.Round(yv));

            return coordinates;
        }

        /// <summary>
        /// Calculate the central x,y location of a given set of points.
        /// </summary>
        public static GeoLoc GetCentroid(List<GeoLoc> listOfPoints)
        {
            return new GeoLoc { latX = listOfPoints.Average(x => x.latX), longY = listOfPoints.Average(y => y.longY) };
        }

        /// <summary>
        /// The random value between[0,1> is used as a base in an exponential function, while the value of k_w is used as the exponent.
        /// Partially adopted from Source: Worst removal determinism factor - https://repozitorij.fpz.unizg.hr/islandora/object/fpz:2603 page: 92.
        /// Higher k_w value decreases elements with smaller probability value and increases those with higher values.
        /// </summary>
        public static List<double> DeterminismFactorScaling(List<double> targetList, int customerCount, double k_w = 2.5)
        {
            double sumOfValues = targetList.Sum();

            for (int i = 0; i < targetList.Count; i++)
            {
                targetList[i] /= sumOfValues;
                targetList[i] = 1 - targetList[i];  // The objective function is to minimize, thus the lowest value should be the one with the highest probability.            
                targetList[i] = Math.Pow(targetList[i], k_w);// * customerCount;
            }

            // Sum of new set of values
            sumOfValues = targetList.Sum();

            for (int i = 0; i < targetList.Count; i++)
            {
                targetList[i] /= sumOfValues;
                targetList[i] = Math.Floor(targetList[i] * 100);
            }

            return targetList;
        }

        /// <summary>
        /// The Hamming Distance measures the minimum number of substitutions required to change one path (list of vertices) into the other.
        /// The Hamming distance between two paths of equal length is the number of positions at which the corresponding vertices are different.
        /// </summary>
        public static int GetHammingDistance(List<Vertex> path_1, List<Vertex> path_2)
        {
            //string p1 = string.Join(" ", path_1.Select(x => x.index).ToArray());
            //string p2 = string.Join(" ", path_2.Select(x => x.index).ToArray());

            if (path_1.Count != path_2.Count)
            {
                return 1000000;
            }

            int distance = 0;

            for (int i = 0; i < path_1.Count - 1; i++)
            {
                if (path_1[i].index != path_2[i].index)
                    distance++;
            }

            //int distance =
            //    p1.ToCharArray()
            //    .Zip(p2.ToCharArray(), (c1, c2) => new { c1, c2 })
            //    .Count(m => m.c1 != m.c2);

            return distance;
        }
    }
}

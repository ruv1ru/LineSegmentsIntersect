using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;

namespace LineSegmentsIntersect
{
    public class Point
    {
        public float x;
        public float y;

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

    };

    public class IntersectCals
    {

        // Given three collinear points p, q, r, the function checks if
        // point q lies on line segment 'pr'
        static Boolean onSegment(Point p, Point q, Point r)
        {
            if (q.x <= Math.Max(p.x, r.x) && q.x >= Math.Min(p.x, r.x) &&
                q.y <= Math.Max(p.y, r.y) && q.y >= Math.Min(p.y, r.y))
                return true;

            return false;
        }

        // To find orientation of ordered triplet (p, q, r).
        // The function returns following values
        // 0 --> p, q and r are collinear
        // 1 --> Clockwise
        // 2 --> Counterclockwise
        static float orientation(Point p, Point q, Point r)
        {
            // See https://www.geeksforgeeks.org/orientation-3-ordered-points/
            // for details of below formula.
            float val = (q.y - p.y) * (r.x - q.x) -
                    (q.x - p.x) * (r.y - q.y);

            if (val == 0) return 0; // collinear

            return (val > 0) ? 1 : 2; // clock or counterclock wise
        }

        // The main function that returns true if line segment 'p1q1'
        // and 'p2q2' intersect.
        static Boolean doIntersect(Point p1, Point q1, Point p2, Point q2)
        {
            // Find the four orientations needed for general and
            // special cases
            float o1 = orientation(p1, q1, p2);
            float o2 = orientation(p1, q1, q2);
            float o3 = orientation(p2, q2, p1);
            float o4 = orientation(p2, q2, q1);

            // General case
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases
            // p1, q1 and p2 are collinear and p2 lies on segment p1q1
            if (o1 == 0 && onSegment(p1, p2, q1)) return true;

            // p1, q1 and q2 are collinear and q2 lies on segment p1q1
            if (o2 == 0 && onSegment(p1, q2, q1)) return true;

            // p2, q2 and p1 are collinear and p1 lies on segment p2q2
            if (o3 == 0 && onSegment(p2, p1, q2)) return true;

            // p2, q2 and q1 are collinear and q1 lies on segment p2q2
            if (o4 == 0 && onSegment(p2, q1, q2)) return true;

            return false; // Doesn't fall in any of the above cases
        }

        [Benchmark]
        public void CalcPointIntersectPolyline()
        {

            var polyline = new List<Point>();



            polyline.Add(new Point((float)-37.8159, (float)145.0657171));
            polyline.Add(new Point((float)-38.034292448735016, (float)145.22770146281098));
            polyline.Add(new Point((float)-38.10418125787285, (float)145.20252398216266));
            polyline.Add(new Point((float)-38.22633935308575, (float)145.1400779404142));
            
            Point p2 = new Point((float)-38.005253084591644, (float)145.32815947558436);
            Point q2 = new Point((float)-37.86398080271746, (float)145.20039376549212);


            for(int y = 0; y < 10000; y++)
            {
                for (int i = 0; i < polyline.Count - 1; i++)
                {

                    doIntersect(polyline[i], polyline[i + 1], p2, q2);

                }
            }

        }

    }
    class Program
    {

       

        // Driver code
        public static void Main(String[] args)
        {



            var summery = BenchmarkRunner.Run<IntersectCals>();
            /*
            p1 = new Point(10, 1); q1 = new Point(0, 10);
            p2 = new Point(0, 0); q2 = new Point(10, 10);
            if (doIntersect(p1, q1, p2, q2))
                Console.WriteLine("Yes");
            else
                Console.WriteLine("No");

            p1 = new Point(-5, -5); q1 = new Point(0, 0);
            p2 = new Point(1, 1); q2 = new Point(10, 10); ;
            if (doIntersect(p1, q1, p2, q2))
                Console.WriteLine("Yes");
            else
                Console.WriteLine("No");*/
        }



    }
}


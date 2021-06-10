using System;
using System.Collections.Generic;
using System.Linq;

using Arc = Objects.Geometry.Arc;
using Box = Objects.Geometry.Box;
using Circle = Objects.Geometry.Circle;
using ControlPoint = Objects.Geometry.ControlPoint;
using Curve = Objects.Geometry.Curve;
using Ellipse = Objects.Geometry.Ellipse;
using Interval = Objects.Primitive.Interval;
using Line = Objects.Geometry.Line;
using Mesh = Objects.Geometry.Mesh;
using Plane = Objects.Geometry.Plane;
using Point = Objects.Geometry.Point;
using Polycurve = Objects.Geometry.Polycurve;
using Polyline = Objects.Geometry.Polyline;
using Surface = Objects.Geometry.Surface;
using Vector = Objects.Geometry.Vector;

using TsBox = TopSolid.Kernel.G.D3.Box;
using TsPlane = TopSolid.Kernel.G.D3.Plane;
using TsPoint = TopSolid.Kernel.G.D3.Point;
using TsLineCurve = TopSolid.Kernel.G.D3.Curves.LineCurve;
using TsSplineCurve = TopSolid.Kernel.G.D3.Curves.BSplineCurve;
using TsVector = TopSolid.Kernel.G.D3.Vector;
using TsUVector = TopSolid.Kernel.G.D3.UnitVector;
using TsEntity = TopSolid.Kernel.DB.Entities.Entity; // TopSolid.Kernel.DB.D3.Entities.GeometricEntity;
using TsSurface = TopSolid.Kernel.DB.D3.Surfaces.SurfaceEntity;
//using TsLine = TopSolid.Kernel.DB.D2.L;
using TsInterval = TopSolid.Kernel.G.D1.Generic.Interval<double>;
using TsGeometry = TopSolid.Kernel.G.IGeometry;
using TsPointList = TopSolid.Kernel.G.D3.PointList;

namespace Objects.Converter.TopSolid
{
    public partial class ConverterTopSolid
    {
        // tolerance for geometry:
        public double tolerance = 0.000;

        // Convenience methods:
        // TODO: Deprecate once these have been added to Objects.sln
        public double[] PointToArray(TsPoint pt)
        {
            return new double[] { pt.X, pt.Y, pt.Z };
        }
        public TsPoint[] PointListToNative(IEnumerable<double> arr, string units)
        {
            var enumerable = arr.ToList();
            if (enumerable.Count % 3 != 0) throw new Speckle.Core.Logging.SpeckleException("Array malformed: length%3 != 0.");

            TsPoint[] points = new TsPoint[enumerable.Count / 3];
            var asArray = enumerable.ToArray();
            for (int i = 2, k = 0; i < enumerable.Count; i += 3)
                points[k++] = new TsPoint(
                  ScaleToNative(asArray[i - 2], units),
                  ScaleToNative(asArray[i - 1], units),
                  ScaleToNative(asArray[i], units));

            return points;
        }
        public double[] PointsToFlatArray(IEnumerable<TsPoint> points)
        {
            return points.SelectMany(pt => PointToArray(pt)).ToArray();
        }

        // Points
        public Point PointToSpeckle(TsPoint point, string units = null)
        {
            var u = units ?? ModelUnits;
            return new Point(point.X, point.Y, point.Z, u);
        }
        public TsPoint PointToNative(Point point)
        {
            var _point = new TsPoint(ScaleToNative(point.x, point.units),
              ScaleToNative(point.y, point.units),
              ScaleToNative(point.z, point.units));
            return _point;
        }

        public List<List<ControlPoint>> ControlPointsToSpeckle(TsSurface surface, string units = null)
        {
            var u = units ?? ModelUnits;

            // TODO: Update converstion
            var points = new List<List<ControlPoint>>();
            //int count = 0;
            //for (var i = 0; i < surface.NumControlPointsInU; i++)
            //{
            //  var row = new List<ControlPoint>();
            //  for (var j = 0; j < surface.NumControlPointsInV; j++)
            //  {
            //    var point = surface.ControlPoints[count];
            //    double weight = 1;
            //    try
            //    {
            //      weight = surface.Weights[count];
            //    }
            //    catch { }
            //    row.Add(new ControlPoint(point.X, point.Y, point.Z, weight, u));
            //    count++;
            //  }
            //  points.Add(row);
            //}
            return points;
        }

        // Vectors
        public Vector VectorToSpeckle(TsVector vector)
        {
            return new Vector(vector.X, vector.Y, vector.Z, ModelUnits);
        }
        public TsUVector VectorToNative(Vector vector)
        {
            return new TsUVector(
              ScaleToNative(vector.x, vector.units),
              ScaleToNative(vector.y, vector.units),
              ScaleToNative(vector.z, vector.units));
        }

        // Interval
        public Interval IntervalToSpeckle(TsInterval interval)
        {
            return new Interval(interval.Start, interval.End);
        }
        public TsInterval IntervalToNative(Interval interval)
        {
            return new TsInterval((double)interval.start, (double)interval.end);
        }


        // Plane
        public Plane PlaneToSpeckle(TsPlane plane, string units = null)
        {
            var u = units ?? ModelUnits;
            return new Plane(PointToSpeckle(plane.Po), VectorToSpeckle(plane.Vz), VectorToSpeckle(plane.Vx), VectorToSpeckle(plane.Vy), u);
        }
        public TsPlane PlaneToNative(Plane plane)
        {
            return new TsPlane(PointToNative(plane.origin), VectorToNative(plane.normal));
        }

        // LineCurve 
        public Line LineToSpeckle(TsLineCurve line)
        {
            return new Line(PointToSpeckle(line.Ps), PointToSpeckle(line.Pe));
        }
        public TsLineCurve LineToNative(Line line)
        {
            return new TsLineCurve(PointToNative(line.start), PointToNative(line.end));
        }

        // Box
        public Box BoxToSpeckle(TsBox box, string units = null)
        {
            try
            {

                var u = units ?? ModelUnits;

                Box _box = null;
                _box.volume = box.Volume;

                TsPlane tsPlane = box.Frame.Pxy;
                tsPlane.Po = new TsPoint(tsPlane.Po.X + box.Hx, tsPlane.Po.Y + box.Hy, tsPlane.Po.Z + box.Hz);

                _box = new Box(PlaneToSpeckle(tsPlane), new Interval(tsPlane.Po.X, tsPlane.Po.X + 2 * box.Hx), new Interval(tsPlane.Po.Y, tsPlane.Po.Y + 2 * box.Hy), new Interval(tsPlane.Po.Z, tsPlane.Po.Z + 2 * box.Hz), u);
                _box.area = (box.Hx * 2 * box.Hy * 2 * 2) + (box.Hx * 2 * box.Hz * 2 * 2) + (box.Hz * 2 * box.Hy * 2 * 2);
                _box.volume = box.Volume;

                return _box;

            }
            catch
            {
                return null;
            }
        }



    }
}

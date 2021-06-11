using System.Collections.Generic;
using System.Linq;
using TopSolid.Kernel.G.D3.Surfaces;
using Box = Objects.Geometry.Box;
using ControlPoint = Objects.Geometry.ControlPoint;
using Interval = Objects.Primitive.Interval;
using Line = Objects.Geometry.Line;
using Plane = Objects.Geometry.Plane;
using Point = Objects.Geometry.Point;
using TsBox = TopSolid.Kernel.G.D3.Box;
//using TsLine = TopSolid.Kernel.DB.D2.L;
using TsInterval = TopSolid.Kernel.G.D1.Generic.Interval<double>;
using TsLineCurve = TopSolid.Kernel.G.D3.Curves.LineCurve;
using TsPlane = TopSolid.Kernel.G.D3.Plane;
using TsPoint = TopSolid.Kernel.G.D3.Point;
using TsUVector = TopSolid.Kernel.G.D3.UnitVector;
using TsVector = TopSolid.Kernel.G.D3.Vector;
using Vector = Objects.Geometry.Vector;

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

        public List<List<ControlPoint>> ControlPointsToSpeckle(BSplineSurface surface, string units = null)
        {
            var u = units ?? ModelUnits;

            // TODO: Update converstion - Done by AHW
            var points = new List<List<ControlPoint>>();
            int count = 0;
            for (var i = 0; i < surface.UCptsCount; i++)
            {
                var row = new List<ControlPoint>();
                for (var j = 0; j < surface.VCptsCount; j++)
                {
                    var point = surface.CPts[count];
                    double weight = 1;
                    try
                    {
                        weight = surface.CWts[count];
                    }
                    catch { }
                    row.Add(new ControlPoint(point.X, point.Y, point.Z, weight, u));
                    count++;
                }
                points.Add(row);
            }
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
            return new Plane(PointToSpeckle(plane.Po), VectorToSpeckle(plane.Vx), VectorToSpeckle(plane.Vy), VectorToSpeckle(plane.Vz), u);
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

                /*
                 {
            //var u = units ?? ModelUnits;
            //Get the Center of the box
            //double X= 0, Y = 0, Z= 0;
            //foreach (TSPoint p in tsBox.Corners)
            //{
            //    X += p.X / tsBox.Corners.Count;
            //    Y += p.Y / tsBox.Corners.Count;
            //    Z += p.Z / tsBox.Corners.Count;
            //}

            //TSPoint Obox = new TSPoint(X, Y, Z);


            TsPlane tsPlane = tsBox.Frame.Pxy;
            tsPlane.Po = new TSPoint(tsPlane.Po.X + tsBox.Hx, tsPlane.Po.Y + tsBox.Hy, tsPlane.Po.Z + tsBox.Hz);


            var speckleBox = new SpBox(PlaneToSpeckle(tsPlane), new Interval(tsPlane.Po.X, tsPlane.Po.X + 2 * tsBox.Hx), new Interval(tsPlane.Po.Y, tsPlane.Po.Y + 2 * tsBox.Hy), new Interval(tsPlane.Po.Z, tsPlane.Po.Z + 2 * tsBox.Hz));
            speckleBox.area = (tsBox.Hx * 2 * tsBox.Hy * 2 * 2) + (tsBox.Hx * 2 * tsBox.Hz * 2 * 2) + (tsBox.Hz * 2 * tsBox.Hy * 2 * 2);
            speckleBox.volume = tsBox.Volume;

            return speckleBox;
                 */

            }
            catch
            {
                return null;
            }
        }

        public Geometry.Surface SurfaceToSpeckle(BSplineSurface surface, string units = null)
        {
            var u = units ?? ModelUnits;
            var result = new Geometry.Surface
            {
                degreeU = surface.UDegree,
                degreeV = surface.VDegree,
                rational = surface.IsRational,
                closedU = surface.IsUClosed,
                closedV = surface.IsVClosed,
                domainU = new Interval(surface.Us, surface.Ue),
                domainV = new Interval(surface.Vs, surface.Ve),
                knotsU = surface.UBs.ToList(),
                knotsV = surface.VBs.ToList()
            };
            result.units = u;

            result.SetControlPoints(ControlPointsToSpeckle(surface));
            //result.bbox = BoxToSpeckle(new RH.Box(surface.GetBoundingBox(true)), u);


            return result;
        }

        private List<double> GetCorrectKnots(List<double> knots, int controlPointCount, int degree)
        {
            var correctKnots = knots;
            if (knots.Count == controlPointCount + degree + 1)
            {
                correctKnots.RemoveAt(0);
                correctKnots.RemoveAt(correctKnots.Count - 1);
            }

            return correctKnots;



        }
    }
}

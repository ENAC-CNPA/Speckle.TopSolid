using Objects.Geometry;
using System.Collections.Generic;
using System.Linq;
using TopSolid.Kernel.G.D3;
using TopSolid.Kernel.G.D3.Curves;
using SpeckleCurve = Objects.Geometry.Curve;
using SpeckleLine = Objects.Geometry.Line;
using SpecklePoint = Objects.Geometry.Point;
using TSPoint = TopSolid.Kernel.G.D3.Point;

namespace EPFL.SpeckleTopSolid.UI
{
    static class ConvertersSpeckleTopSolid
    {
        #region Converters TS to Speckle
        public static SpecklePoint PointToSpeckle(TopSolid.Kernel.G.D3.Point tsPoint)
        {
            SpecklePoint SpPoint = new SpecklePoint(tsPoint.X, tsPoint.Y, tsPoint.Z);
            return SpPoint;
        }

        public static SpeckleLine LineToSpeckle(TopSolid.Kernel.G.D3.Curves.LineCurve tsLine)
        {
            SpeckleLine SpLine = new SpeckleLine(PointToSpeckle(tsLine.Ps), PointToSpeckle(tsLine.Pe));
            return SpLine;
        }

        public static Polyline PolylineToSpeckle(TopSolid.Kernel.G.D3.Curves.LineCurve TSLine)
        {
            return new Polyline();
        }

        //Inspired from Autocad Connector
        public static Objects.Geometry.Curve CurveToSpeckle(BSplineCurve tsCurve)
        {
            var curve = new SpeckleCurve();

            // get nurbs and geo data 
            //var data = spline.NurbsData;
            //var _spline = spline.GetGeCurve() as NurbCurve3d;

            // hack: check for incorrectly closed periodic curves (this seems like acad bug, has resulted from receiving rhino curves)
            //bool periodicClosed = false;
            //if (_spline.Knots.Count < _spline.NumberOfControlPoints + _spline.Degree + 1 && spline.IsPeriodic)
            //    periodicClosed = true;

            // handle the display polyline
            //try
            //{
            //    var poly = tsCurve.ToPolyline(false, true);
            //    Polyline displayValue = ConvertToSpeckle(poly) as Polyline;
            //    curve.displayValue = displayValue;
            //}
            //catch { }

            // get points
            // NOTE: for closed periodic splines, autocad does not track last #degree points. Add the first #degree control points to the list if so.


            //var points = tsCurve.CPts.ToList();


            //if (periodicClosed)
            //    points.AddRange(points.GetRange(0, spline.Degree));

            // get knots
            // NOTE: for closed periodic splines, autocad has #control points + 1 knots. Add #degree extra knots to beginning and end with #degree - 1 multiplicity for first and last
            //var knots = data.GetKnots().OfType<double>().ToList();
            //if (periodicClosed)
            //{
            //    double interval = knots[1] - knots[0]; //knot interval

            //    for (int i = 0; i < data.Degree; i++)
            //    {
            //        if (i < 2)
            //        {
            //            knots.Insert(knots.Count, knots[knots.Count - 1] + interval);
            //            knots.Insert(0, knots[0] - interval);
            //        }
            //        else
            //        {
            //            knots.Insert(knots.Count, knots[knots.Count - 1]);
            //            knots.Insert(0, knots[0]);
            //        }
            //    }
            //}

            // get weights
            // NOTE: autocad assigns unweighted points a value of -1, and will return an empty list in the spline's nurbsdata if no points are weighted
            // NOTE: for closed periodic splines, autocad does not track last #degree points. Add the first #degree weights to the list if so.
            //var weights = new List<double>();
            //for (int i = 0; i < spline.NumControlPoints; i++)
            //{
            //    double weight = spline.WeightAt(i);
            //    if (weight <= 0)
            //        weights.Add(1);
            //    else
            //        weights.Add(weight);
            //}
            //if (periodicClosed)
            //    weights.AddRange(weights.GetRange(0, spline.Degree));

            //// set nurbs curve info
            //curve.points = PointsToFlatArray(points).ToList();
            //curve.knots = knots;
            //curve.weights = weights;
            //curve.degree = spline.Degree;
            //curve.periodic = spline.IsPeriodic;
            //curve.rational = spline.IsRational;
            //curve.closed = (periodicClosed) ? true : spline.Closed;
            //curve.length = _spline.GetLength(_spline.StartParameter, _spline.EndParameter, tolerance);
            //curve.domain = IntervalToSpeckle(_spline.GetInterval());
            //curve.bbox = BoxToSpeckle(spline.GeometricExtents, true);
            //curve.units = ModelUnits;

            //start of Our Code
            //Create a list of speckle points


            List<TSPoint> tsPoints = tsCurve.CPts.ToList();

            //Weights
            List<double> ptWeights = new List<double>();
            try
            {
                if (tsCurve.CWts.Count != 0)
                {
                    foreach (double weight in tsCurve.CWts)
                    {
                        ptWeights.Add(weight);
                    }
                }
            }
            catch { }

            try
            {
                double range = (tsCurve.Te - tsCurve.Ts);
                PointList polyPoints = new PointList();
                for (int i = 0; i < 100; i++)
                {
                    polyPoints.Add(tsCurve.GetPoint((range / 100) * i));
                }
                TopSolid.Kernel.G.D3.Curves.PolylineCurve tspoly = new PolylineCurve(false, polyPoints);
                Polyline displayValue = new Polyline(PointsToFlatArray(polyPoints));


                curve.displayValue = displayValue;
            }
            catch { }

            //set speckle curve info
            curve.points = PointsToFlatArray(tsCurve.CPts).ToList();
            //curve.knots = knots;
            if (tsCurve.CWts.Count != 0)
                curve.weights = ptWeights;
            curve.degree = tsCurve.Degree;
            curve.periodic = tsCurve.IsPeriodic;
            curve.rational = tsCurve.IsRational;
            curve.closed = tsCurve.IsClosed();
            curve.length = tsCurve.GetLength();
            //curve.domain = IntervalToSpeckle(_spline.GetInterval());
            //curve.bbox = BoxToSpeckle(spline.GeometricExtents, true);
            //curve.units = ModelUnits;

            return curve;
        }

        private static double[] PointsToFlatArray(IEnumerable<TSPoint> points)
        {
            return points.SelectMany(pt => PointToArray(pt)).ToArray();
        }

        private static double[] PointToArray(TSPoint pt)
        {
            return new double[] { pt.X, pt.Y, pt.Z };
        }


        //TODO Create a Speckle brep out of a TS shape : First trial ongoing
        //public static Objects.Geometry.Brep ShapeToSpeckle(Shape shape)
        //{
        //    Objects.Geometry.Brep spBrep = new Brep();
        //    Objects.Geometry.BrepFace Bface;
        //    foreach (Face face in shape.Faces)
        //    {
        //        foreach (Vertex V in face.Vertices)
        //        {

        //        }
        //    }
        //    return spBrep;
        //}

        #endregion

        #region Converters Speckle To TS

        public static TopSolid.Kernel.G.D3.Point PointToTS(Objects.Geometry.Point spPoint)
        {
            TopSolid.Kernel.G.D3.Point tPoint = new TopSolid.Kernel.G.D3.Point(spPoint.x, spPoint.y, spPoint.z);
            return tPoint;
        }

        public static LineCurve LinetoTS(Line sLine)
        {
            LineCurve tLine = new LineCurve(PointToTS(sLine.start), PointToTS(sLine.end));
            return tLine;
        }

        public static PolylineCurve PolyLinetoTS(Polyline sPolyLine)
        {

            PointList tPointsList = new PointList();

            foreach (Objects.Geometry.Point p in sPolyLine.points)
            {
                TopSolid.Kernel.G.D3.Point tPoint = PointToTS(p);
                tPointsList.Add(tPoint);
            }

            PolylineCurve tPolyLine = new PolylineCurve(sPolyLine.closed, tPointsList);
            return tPolyLine;

        }

        #endregion 
    }
}

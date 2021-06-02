using Objects.Geometry;
using TopSolid.Kernel.G.D3;
using TopSolid.Kernel.G.D3.Curves;
using SpeckleLine = Objects.Geometry.Line;
using SpecklePoint = Objects.Geometry.Point;

namespace EPFL.SpeckleTopSolid.UI
{
    static class ConvertersSpeckleTopSolid
    {
        #region Converters TS to Speckle
        public static SpecklePoint PointToSpeckle(TopSolid.Kernel.G.D3.Point TSPoint)
        {
            SpecklePoint SpPoint = new SpecklePoint(TSPoint.X, TSPoint.Y, TSPoint.Z);
            return SpPoint;
        }

        public static SpeckleLine LineToSpeckle(TopSolid.Kernel.G.D3.Curves.LineCurve TSLine)
        {
            SpeckleLine SpLine = new SpeckleLine(PointToSpeckle(TSLine.Ps), PointToSpeckle(TSLine.Pe));
            return SpLine;
        }

        public static Polyline PolylineToSpeckle(TopSolid.Kernel.G.D3.Curves.LineCurve TSLine)
        {
            return new Polyline();
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

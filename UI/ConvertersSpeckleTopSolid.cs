using Objects.Geometry;
using TopSolid.Kernel.G.D3.Shapes;
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

        //TODO Create a Speckle brep out of a TS shape : First trial ongoing
        public static Objects.Geometry.Brep ShapeToSpeckle(Shape shape)
        {
            Objects.Geometry.Brep spBrep = new Brep();
            Objects.Geometry.BrepFace Bface;
            foreach (Face face in shape.Faces)
            {
                foreach (Vertex V in face.Vertices)
                {

                }
            }
            return spBrep;
        }

        #endregion
    }
}

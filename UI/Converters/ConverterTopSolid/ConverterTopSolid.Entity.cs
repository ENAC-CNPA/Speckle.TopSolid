using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Arc = Objects.Geometry.Arc;
using Box = Objects.Geometry.Box;
using BlockInstance = Objects.Other.BlockInstance;
using BlockDefinition = Objects.Other.BlockDefinition;
using Brep = Objects.Geometry.Brep;
using BrepEdge = Objects.Geometry.BrepEdge;
using BrepFace = Objects.Geometry.BrepFace;
using BrepLoop = Objects.Geometry.BrepLoop;
using BrepLoopType = Objects.Geometry.BrepLoopType;
using BrepTrim = Objects.Geometry.BrepTrim;
using Circle = Objects.Geometry.Circle;
using ControlPoint = Objects.Geometry.ControlPoint;
using Curve = Objects.Geometry.Curve;
using Plane = Objects.Geometry.Plane;
using Ellipse = Objects.Geometry.Ellipse;
using Interval = Objects.Primitive.Interval;
using Line = Objects.Geometry.Line;
using Mesh = Objects.Geometry.Mesh;
using Surface = Objects.Geometry.Surface;
using Point = Objects.Geometry.Point;
using Polycurve = Objects.Geometry.Polycurve;
using Polyline = Objects.Geometry.Polyline;
using Vector = Objects.Geometry.Vector;
using Speckle.Core.Models;
using Speckle.Core.Kits;

using TsBox = TopSolid.Kernel.G.D3.Box;
using TsPlane = TopSolid.Kernel.G.D3.Plane;
using TsPoint = TopSolid.Kernel.G.D3.Point;
using TsVector = TopSolid.Kernel.G.D3.Vector;
using TsUVector = TopSolid.Kernel.G.D3.UnitVector;
using TsEntity = TopSolid.Kernel.DB.Entities.Entity;
using TsGeometry = TopSolid.Kernel.G.IGeometry;

namespace Objects.Converter.TopSolid
{
    public partial class ConverterTopSolid
    {

        

        // Plane
        public Plane PlaneToSpeckle(TsPlane plane, string units = null)
        {
            var u = units ?? ModelUnits;
            return new Plane(PointToSpeckle(plane.Po), VectorToSpeckle(plane.Vz), VectorToSpeckle(plane.Vx), VectorToSpeckle(plane.Vy), u);
        }


        // Vector
        private Vector VectorToSpeckle(TsVector vector, string units = null)
        {
            var u = units ?? ModelUnits;
            return new Vector(vector.X, vector.Y, vector.Z, u);
        }

    }
}

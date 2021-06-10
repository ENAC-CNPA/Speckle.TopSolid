using System;
using System.Collections.Generic;
using System.Linq;

using Speckle.Core.Kits;
using Speckle.Core.Models;
using Arc = Objects.Geometry.Arc;
using BlockInstance = Objects.Other.BlockInstance;
using BlockDefinition = Objects.Other.BlockDefinition;
using Brep = Objects.Geometry.Brep;
using Circle = Objects.Geometry.Circle;
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
using Box = Objects.Geometry.Box;

using TsBox = TopSolid.Kernel.G.D3.Box;
using TsPlane = TopSolid.Kernel.G.D3.Plane;
using TsPoint = TopSolid.Kernel.G.D3.Point;
using TsVector = TopSolid.Kernel.G.D3.Vector;
using TsUVector = TopSolid.Kernel.G.D3.UnitVector;
using TsEntity = TopSolid.Kernel.DB.Entities.Entity;
using TsGeometry = TopSolid.Kernel.G.IGeometry;



#if (TOPSOLID15)
#endif


namespace Objects.Converter.TopSolid
{
    public partial class ConverterTopSolid // TODO: link to > ": ISpeckleConverter"
    {
#if TOPSOLID15
        public static string TopSolidAppName = "TopSolid15"; // Applications.TopSolid15;
#endif

        #region ISpeckleConverter props

        public string Description => "Default Speckle Kit for TopSolid";
        public string Name => nameof(ConverterTopSolid);
        public string Author => "Speckle";
        public string WebsiteOrEmail => "https://speckle.systems";

        public IEnumerable<string> GetServicedApplications() => new string[] { TopSolidAppName };

        public HashSet<Exception> ConversionErrors { get; private set; } = new HashSet<Exception>();

        #endregion ISpeckleConverter props


        public List<ApplicationPlaceholderObject> ContextObjects { get; set; } = new List<ApplicationPlaceholderObject>();

        public void SetContextObjects(List<ApplicationPlaceholderObject> objects) => ContextObjects = objects;

        public void SetPreviousContextObjects(List<ApplicationPlaceholderObject> objects) => throw new NotImplementedException();

        public void SetContextDocument(object doc)
        {
            // TODO: if documnent init is necessary for TopSolid
        }

        public Base ConvertToSpeckle(object @object)
        {
            switch (@object)
            {
                case TsBox o:
                    return BoxToSpeckle(o);

                case TsPlane o:
                    return PlaneToSpeckle(o);

                case TsPoint o:
                    return PointToSpeckle(o);

                case TsVector o:
                    return VectorToSpeckle(o);

                default:
                    throw new NotSupportedException();
            }
        }

        private Base ObjectToSpeckleBuiltElement(TsEntity o)
        {
            throw new NotImplementedException();
        }

        public List<Base> ConvertToSpeckle(List<object> objects)
        {
            return objects.Select(x => ConvertToSpeckle(x)).ToList();
        }

        // TODO: ????
        //public object ConvertToNative(Base @object)
        //{
        //    switch (@object)
        //    {
        //        case Box o:
        //            return BoxToNativeDB(o);

        //        case Plane o:
        //            return PlaneToNativeDB(o);

        //        case Point o:
        //            return PointToNativeDB(o);

        //        case Vector o:
        //            return VectorToNativeDB(o);

        //        default:
        //            throw new NotSupportedException();
        //    }
        //}

        public List<object> ConvertToNative(List<Base> objects)
        {
            // TODO: Fix List functions !??
            //return objects.Select(x => ConvertToNative(x)).ToList();
            return null;
        }

        /// <summary>
        /// Converts a TopSolid Entity <see cref="TsGeometry"/> instance to a Speckle <see cref="Base"/>
        /// </summary>
        /// <param name="obj">TopSolid Entity to be converted.</param>
        /// <returns></returns>
        public Base ObjectToSpeckle(TsGeometry obj)
        {
            switch (obj)
            {

                case TsBox o:
                    return BoxToSpeckle(o);

                case TsPlane o:
                    return PlaneToSpeckle(o);

                case TsPoint o:
                    return PointToSpeckle(o);

                // TODO: using multi type (TsGeometry isn't compatible)
                //case TsVector o:
                //    return VectorToSpeckle(o);

                default:
                    return null;
            }
        }

        public bool CanConvertToSpeckle(object @object)
        {
            switch (@object)
            {
                case TsGeometry o:
                    switch (o)
                    {
                        case TsBox _:
                        case TsPlane _:
                        case TsPoint _:
                            return true;

                        default:
                            return false;
                    }

                case TsVector _:
                    return true;

                default:
                    return false;
            }
        }

        public bool CanConvertToNative(Base @object)
        {
            switch (@object)
            {
                case Box _:
                case Plane _:
                case Point _:
                case Vector _:

                default:
                    return false;
            }
        }

    }
}

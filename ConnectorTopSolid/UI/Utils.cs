using Speckle.Core.Kits;
using Speckle.Core.Models;

using System.Collections.Generic;
using System.Text.RegularExpressions;
using TopSolid.Kernel.DB.D3.Documents;
using TopSolid.Kernel.DB.D3.Modeling.Documents;
using TopSolid.Kernel.TX.Units;
using TopSolid.Kernel.DB.Elements;


namespace Speckle.ConnectorTopSolid.UI
{
    public static class Utils
    {

#if TOPSOLID715
    public static string VersionedAppName = VersionedHostApplications.TopSolid715;
    public static string AppName = HostApplications.TopSolid.Name;
    public static string Slug = HostApplications.TopSolid.Slug;
#elif TOPSOLID716
    public static string VersionedAppName = VersionedHostApplications.TopSolid716;
    public static string AppName = HostApplications.TopSolid.Name;
    public static string Slug = HostApplications.TopSolid.Slug;
#endif
        public static string invalidChars = @"<>/\:;""?*|=,‘";

        #region extension methods

        #endregion

        /// <summary>
        /// Retrieves the document's units.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        /// 
        public static string GetUnits(GeometricDocument doc)
        {

            var insUnits = doc.LengthUnit;
            string units = UnitToSpeckle(insUnits);
            return units;

        }

        private static string UnitToSpeckle(Unit units)
        {

            switch (units.Name) // TODO: Check Name conversion
            {
                case "Millimeter":
                    return Units.Millimeters;
                case "Centimeter":
                    return Units.Centimeters;
                case "Meter":
                    return Units.Meters;
                case "Kilometer":
                    return Units.Kilometers;
                case "Inche":
                    return Units.Inches;
                case "Fee":
                    return Units.Feet;
                case "Yard":
                    return Units.Yards;
                case "Mile":
                    return Units.Miles;
                default:
                    throw new System.Exception("The current Unit System is unsupported.");
            }
        }


        /// <summary>
        /// Removes invalid characters for Autocad layer and block names
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveInvalidChars(string str)
        {
            // using this to handle rhino nested layer syntax
            // replace "::" layer delimiter with "$" (acad standard)
            string cleanDelimiter = str.Replace("::", "$");

            // remove all other invalid chars
            return Regex.Replace(cleanDelimiter, $"[{invalidChars}]", string.Empty);
        }

        public static string RemoveInvalidDynamicPropChars(string str)
        {
            // remove ./
            return Regex.Replace(str, @"[./]", "-");
        }

        /// <summary>
        /// Gets the handles of all visible document objects that can be converted to Speckle
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static List<string> ConvertibleObjects(this ModelingDocument doc, ISpeckleConverter converter)
        {
            var objs = new List<string>();
            IEnumerable<Element> elements = doc.Elements.GetAll();

            foreach (Element element in elements)
                {
                    if (element is TopSolid.Kernel.G.IGeometry)
                {
                   System.Console.WriteLine(element.Id.ToString());
                }
                    if (converter.CanConvertToSpeckle(element))
                        objs.Add(element.Id.ToString());
                }
     
            return objs;
        }

    }

}

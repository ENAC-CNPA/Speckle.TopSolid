using Speckle.Core.Kits;
using Speckle.Core.Models;

using System.Drawing;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TsUnits = TopSolid.Kernel.TX.Units.UnitFormat;
using TopSolid.Kernel.DB.D3.Documents;
using TopSolid.Kernel.DB.D3.Modeling.Documents;
using TopSolid.Kernel.UI;
using TopSolid.Kernel.DB.OptionSets;
using TopSolid.Kernel.TX.Units;
using TopSolid.Kernel.GR.Transforms;
using TsApp = TopSolid.Kernel.UI.Application;



namespace EPFL.SpeckleTopSolid.UIXXXX
{
    public partial class UtilsXXXX
    {

        public static string AppName = "TopSolid";
#if TOPSOLID715
        public static string TopSolidAppName = "TopSolid715"; // TODO: Update Speckle.Core.Kits
#else
        public static string TopSolidAppName = "TopSolid716"; // TODO: Update Speckle.Core.Kits
#endif

        #region units
        private string _modelUnits;
        public string ModelUnits
        {
            get
            {

                GeometricDocument Doc = TsApp.CurrentDocument as ModelingDocument;

                if (string.IsNullOrEmpty(_modelUnits))
                    _modelUnits = UnitToSpeckle(Doc.LengthUnit);
                return _modelUnits;
            }
        }
        private void SetUnits(Base geom)
        {
            geom["units"] = ModelUnits;
        }

        private double ScaleToNative(double value, string units)
        {
            var f = Units.GetConversionFactor(units, ModelUnits);
            return value * f;
        }

        private string UnitToSpeckle(Unit units)
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
        #endregion
    }
}



namespace EPFL.SpeckleTopSolid.UI
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

    }

}

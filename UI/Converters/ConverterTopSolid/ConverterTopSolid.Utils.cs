using Speckle.Core.Kits;
using Speckle.Core.Models;
using TopSolid.Kernel.DB.D3.Documents;
using TopSolid.Kernel.DB.D3.Modeling.Documents;
using TopSolid.Kernel.TX.Units;
using TopSolid.Kernel.UI;

namespace Objects.Converter.TopSolid
{
    public partial class ConverterTopSolid
    {
        #region units
        private string _modelUnits;
        public string ModelUnits
        {
            get
            {

                GeometricDocument Doc = Application.CurrentDocument as ModelingDocument;

                if (string.IsNullOrEmpty(_modelUnits))
                    _modelUnits = UnitToSpeckle(Doc.LengthUnit);
                return _modelUnits;
            }
        }
        private void SetUnits(Base geom)
        {
            geom.units = ModelUnits;
        }

        private double ScaleToNative(double value, string units)
        {
            var f = Units.GetConversionFactor(units, ModelUnits);
            return value * f;
        }

        private string UnitToSpeckle(Unit units)
        {

            switch (units.Name) // TODO: Check Name conversion + Add All French Names too
            {
                case "Millimeter":
                    return Units.Millimeters;
                case "Millimètre":
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

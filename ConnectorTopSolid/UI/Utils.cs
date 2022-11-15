using Speckle.Core.Kits;
using Speckle.Core.Models;

using System.Collections.Generic;
using System.Text.RegularExpressions;
using TopSolid.Kernel.DB.D3.Documents;
using TopSolid.Kernel.DB.D3.Modeling.Documents;
using TopSolid.Kernel.TX.Units;
using TopSolid.Kernel.DB.Elements;
using TopSolid.Kernel.DB.Parameters;
using TopSolid.Kernel.DB.Entities;
using TopSolid.Cad.Design.DB;
using System.Linq;
using System;

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

            //SimpleUnit insUnits = doc.LengthUnit.BaseUnit;
            //string units = UnitToSpeckle(insUnits.Symbol);
            return "m";

        }

        private static string UnitToSpeckle(string unit)
        {

            switch (unit) // TODO: Check Name conversion
            {
                case "mm": // "Millimeter":
                    return Units.Millimeters;
                case "cm":
                    return Units.Centimeters;
                case "m":
                    return Units.Meters;
                case "km":
                    return Units.Kilometers;
                case "in":
                    return Units.Inches;
                case "ft":
                    return Units.Feet;
                case "yd":
                    return Units.Yards;
                case "mi":
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

        public static List<KeyValuePair<string, string>> getParameters(ModelingDocument doc)
        {
            List<KeyValuePair<string, string>> speckleParameters = new List<KeyValuePair<string, string>>();
            List<string> checkTypes = new List<string>();

            IEnumerable<ParameterEntity> paramElements = doc.ParametersFolderEntity.DeepParameters;
            foreach (ParameterEntity param in paramElements)
            {
                //TextParameterEntity name = doc.ParametersFolderEntity.SearchDeepEntity("") as TextParameterEntity;

                if (param is TextParameterEntity textParam)
                {
                    KeyValuePair<string, string> sp = new KeyValuePair<string, string>(textParam.GetFriendlyName(), textParam.Value);
                    speckleParameters.Add(sp);
                }
                else if (param is DateTimeParameterEntity dateParam)
                {
                    KeyValuePair<string, string> sp = new KeyValuePair<string, string>(dateParam.GetFriendlyName(), dateParam.Value.ToString());
                    speckleParameters.Add(sp);
                }
                else
                {

                    checkTypes.Add(param.GetType().ToString());
                    KeyValuePair<string, string> sp = new KeyValuePair<string, string>(param.GetFriendlyName(), "");
                    speckleParameters.Add(sp);
                }
            }

            return speckleParameters;

        }


        public static List<KeyValuePair<string, string>> getParameters(Element element)
        {
            List<KeyValuePair<string, string>> speckleParameters = new List<KeyValuePair<string, string>>();
            List<string> checkTypes = new List<string>();
            IEnumerable<ParameterEntity> paramElements = null;
            PartEntity ownerDoc = element.Owner as PartEntity;
            if (ownerDoc is null)
            {
                AssemblyEntity ownerAss = element.Owner as AssemblyEntity;
                paramElements = ownerAss.DefinitionDocument.ParametersFolderEntity.DeepParameters;
            }
            else
            {
                paramElements = ownerDoc.DefinitionDocument.ParametersFolderEntity.DeepParameters;
            }

            if (paramElements != null)
            {
                foreach (ParameterEntity param in paramElements)
                {
                    //TextParameterEntity name = doc.ParametersFolderEntity.SearchDeepEntity("") as TextParameterEntity;

                    if (param is TextParameterEntity textParam)
                    {
                        KeyValuePair<string, string> sp = new KeyValuePair<string, string>(textParam.GetFriendlyName(), textParam.Value);
                        speckleParameters.Add(sp);
                    }
                    else if (param is DateTimeParameterEntity dateParam)
                    {
                        KeyValuePair<string, string> sp = new KeyValuePair<string, string>(dateParam.GetFriendlyName(), dateParam.Value.ToString());
                        speckleParameters.Add(sp);
                    }
                    else
                    {

                        checkTypes.Add(param.GetType().ToString());
                        KeyValuePair<string, string> sp = new KeyValuePair<string, string>(param.GetFriendlyName(), "");
                        speckleParameters.Add(sp);
                    }
                }
            }

            return speckleParameters;

        }


        public static bool GetOrMakeLayer(string layerName, ModelingDocument doc, out string cleanName)
        {
            cleanName = RemoveInvalidChars(layerName);
            try
            {

                // TODO Create Layer TopSolid

                //LayerTable layerTable = tr.GetObject(Doc.Database.LayerTableId, OpenMode.ForRead) as LayerTable;
                //if (layerTable.Has(cleanName))
                //{
                //    return true;
                //}
                //else
                //{
                //    layerTable.UpgradeOpen();
                //    var _layer = new LayerTableRecord();

                //    // Assign the layer properties
                //    _layer.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByColor, 7); // white
                //    _layer.Name = cleanName;

                //    // Append the new layer to the layer table and the transaction
                //    layerTable.Add(_layer);
                //    tr.AddNewlyCreatedDBObject(_layer, true);
                //}
            }
            catch { return false; }
            return true;
        }


        public static void SetStyle(Base styleBase, Element element, Dictionary<string, int> lineTypeDictionary)
        {
            var units = styleBase["units"] as string;
            var color = styleBase["color"] as int?;
            if (color == null) color = styleBase["diffuse"] as int?; // in case this is from a rendermaterial base
            var lineType = styleBase["linetype"] as string;
            var lineWidth = styleBase["lineweight"] as double?;

            // TODO Create Line Type TopSolid => LineType

            if (color != null)
            {
                var systemColor = System.Drawing.Color.FromArgb((int)color);
                //element.Color = Color.FromRgb(systemColor.R, systemColor.G, systemColor.B);
                //element.Transparency = new Transparency(systemColor.A);
            }

            double conversionFactor = (units != null) ? Units.GetConversionFactor(Units.GetUnitsFromString(units), Units.Millimeters) : 1;
            //if (lineWidth != null)
            //    element.LineWeight = GetLineWeight((double)lineWidth * conversionFactor);

            //if (lineType != null)
            //    if (lineTypeDictionary.ContainsKey(lineType))
            //        element.LinetypeId = lineTypeDictionary[lineType];
        }
    }

}

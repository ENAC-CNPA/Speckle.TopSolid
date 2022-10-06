using System;
using System.Collections.Generic;
using System.Linq;

using DesktopUI2.Models;
using Speckle.Newtonsoft.Json;
using TopSolid.Kernel.DB.D3.Documents;
using TopSolid.Kernel.DB.Parameters;
using TopSolid.Kernel.DB.D3.Modeling.Documents;
using TopSolid.Kernel.DB.Elements;
using TopSolid.Kernel.TX.Undo;

namespace Speckle.ConnectorTopSolid.UI.Storage
{
    /// <summary>
    /// Manages the serialisation of speckle stream state
    /// </summary>
    /// <remarks>
    /// Uses a child dictionary for custom data in the Named Object Dictionary (NOD) which is the root level dictionary.
    /// This is because NOD persists after a document is closed (unlike file User Data).
    /// Custom data is stored as XRecord key value entries of type (string, ResultBuffer).
    /// ResultBuffers are TypedValue arrays, with the DxfCode of the input type as an integer.
    /// Used for DesktopUI2
    /// </remarks>
    public static class SpeckleStreamManager
    {
        readonly static string SpeckleExtensionDictionary = "Speckle";
        readonly static string SpeckleStreamStates = "StreamStates";
        readonly static string SpeckleCommit = "Commit";

        /// <summary>
        /// Returns all the speckle stream states present in the current document.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<StreamState> ReadState(ModelingDocument doc)
        {
            var streams = new List<StreamState>();

            if (doc == null)
                return streams;

            string parameterValue = "";
            Element element = doc.Elements[SpeckleStreamStates];
            if (element != null && element is TextParameterEntity parameter)
            {
                parameterValue = parameter.Value;
            }

            streams = JsonConvert.DeserializeObject<List<StreamState>>(parameterValue);

            return streams;
        }

        /// <summary>
        /// Writes the stream states to the current document.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="wrap"></param>
        public static void WriteStreamStateList(GeometricDocument doc, List<StreamState> streamStates)
        {
            if (doc == null)
                return;

            UndoSequence.UndoCurrent();
            UndoSequence.Start("write state", true);

            string value = JsonConvert.SerializeObject(streamStates) as string;

            Element element = doc.Elements[SpeckleStreamStates];
            if (element != null && element is TextParameterEntity parameter)
            {
                parameter.Value = value;
            }
            else
            {
                TextParameterEntity stateParameter = new TextParameterEntity(doc, 0);
                stateParameter.Name = SpeckleStreamStates;
                stateParameter.Create();
                stateParameter.Value = value;
            }

            UndoSequence.End();
        }


        /// <summary>
        /// Returns commit info present in the current document.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static string ReadCommit(ModelingDocument doc)
        {
            string commit = null;

            if (doc == null)
                return null;


            string parameterValue = "";
            Element element = doc.Elements[SpeckleCommit];
            if (element != null && element is TextParameterEntity parameter)
            {
                parameterValue = parameter.Value;
            }

            // TODO : Structure commit like Object
            commit = JsonConvert.DeserializeObject<string>(parameterValue);

            return commit;
        }

        /// <summary>
        /// Writes commit info to the current document.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="wrap"></param>
        public static void WriteCommit(GeometricDocument doc, string commit)
        {

            if (doc == null)
                return;

            UndoSequence.UndoCurrent();
            UndoSequence.Start("write param", true);

            string value = "";
            if (commit != null && commit != "") value = JsonConvert.SerializeObject(commit) as string;

            Element element = doc.Elements[SpeckleCommit];
            if (element != null && element is TextParameterEntity parameter)
            {
                parameter.Value = value;
            }
            else
            {
                TextParameterEntity commitParameter = new TextParameterEntity(doc, 0);
                commitParameter.Name = SpeckleCommit;
                commitParameter.Create();
                commitParameter.Value = value;
            }
            UndoSequence.End();

        }




    }
}

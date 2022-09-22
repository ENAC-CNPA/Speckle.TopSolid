
using DesktopUI2.ViewModels;
using DesktopUI2.Models;
using Speckle.ConnectorTopSolid.UI;


namespace Speckle.ConnectorTopSolid.UI.Entry
{
    public class OneClickCommand
    {
        public static ConnectorBindingsTopSolid Bindings { get; set; }
        public static StreamState FileStream { get; set; }

        /// <summary>
        /// Command to send selection to the document stream, or everything if nothing is selected
        /// </summary>
        public static void SendCommand()
        {
            // initialize dui
            SpeckleTopSolidCommand.CreateOrFocusSpeckle(false);

            // send
            var oneClick = new OneClickViewModel(Bindings, FileStream);
            oneClick.Send();
            FileStream = oneClick.FileStream;
        }
    }
}


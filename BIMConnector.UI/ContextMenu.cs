using System.Reflection;
using TopSolid.Kernel.WX.Documents;
using TK = TopSolid.Kernel;

namespace BIMConnector.UI
{
    /// <summary>
    /// Implements the context menu management for this AddIn.
    /// </summary>
    public static class ContextMenu
    {
        /// <summary>
        /// Adds the context menu management for this AddIn.
        /// </summary>
        public static void AddMenu()
        {
            //Browse all the available document types...
            foreach (DocumentWindowFactory factory in DocumentWindowFactoryStore.Factories)
            {
                factory.AddMenuContext(typeof(ContextMenu), "xml");
            }

        }
    }
}

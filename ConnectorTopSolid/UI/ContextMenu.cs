using System;
using System.Reflection;
using System.IO;
using System.Resources;
using TopSolid.Kernel.WX.Documents;
using TK = TopSolid.Kernel;

namespace EPFL.SpeckleTopSolid.UI
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
            // Add the menu when there is no document open in TopSolid
            TK.WX.Application.Window.AddMenuContext(typeof(ContextMenu), "xml");

            //Browse all the available document types...
            foreach (DocumentWindowFactory factory in DocumentWindowFactoryStore.Factories)
            {
                //... and add the menu
                factory.AddMenuContext(typeof(ContextMenu), "xml");

                // To go further:
                //   It is possible to filter the document types you want to display the menu like in the following sample:
                // //Ignore all the Documents that are not PartDocument
                // if (factory.DocumentFactory.DocumentType != typeof(PartDocument)) continue; 
                // factory.AddMenuContext(typeof(ContextMenu), "xml");
            }

        }
    }
}
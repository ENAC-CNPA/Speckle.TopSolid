using System.Reflection;
using System.Resources;
using TopSolid.Kernel.SX.Resources;

namespace TopSolid.AdsSamples.Cad.Lego.DB
{
    /// <summary>
    /// Manages the resources.
    /// </summary>
    [Obfuscation(Exclude = true)]
    public static class Resources
    {
        // Static fields:

        /// <summary>
        /// Resources manager.
        /// </summary>
        private static TopSolid.Kernel.SX.Resources.ResourceManager manager = null;

        // Properties:

        /// <summary>
        /// Gets the resources manager.
        /// </summary>
        public static TopSolid.Kernel.SX.Resources.ResourceManager Manager
        {
            get
            {
                if (manager == null)
                {
                    manager = new TopSolid.Kernel.SX.Resources.ResourceManager(typeof(Resources));
                }

                return manager;
            }
        }
    }
}

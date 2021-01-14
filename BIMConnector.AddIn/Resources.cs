using TopSolid.Kernel.SX.Resources;

namespace BIMConnector.Addin
{
    /// <summary>
    /// resources of Localization
    /// </summary>
    public static class Resources
    {
        /// <summary>
        /// New instance ...
        /// </summary>
        private static ResourceManager instance = null;

        /// <summary>
        /// New Resource Manager 
        /// </summary>
        /// <returns>new instance of Ressource Manager</returns>
        public static ResourceManager Manager
        {
            get
            {
                //$SpecificSolutionName$
                if (instance == null)
                {
                    instance = new ResourceManager(typeof(Resources));
                }
                return instance;
            }
        }

    }
}

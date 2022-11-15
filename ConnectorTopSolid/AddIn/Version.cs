using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TK = TopSolid.Kernel;

namespace EPFL.SpeckleTopSolid.UI
{
    /// <summary>
    /// Specific version
    /// </summary>
    public static class Version
    {
        /// <summary>
        /// Current version
        /// </summary>
        public static readonly TK.SX.Version Current = new TK.SX.Version(7, 16, 400,01); // 2022-11-14 : First version
        ///<summary>
        /// Current assembly version
        /// </summary>
        public const string CurrentAssemblyVersionString = "7.16.400.001";
    }
}
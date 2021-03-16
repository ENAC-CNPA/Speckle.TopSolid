using System;
using System.Resources;
using TopSolid.Kernel.SX.Resources;
using ResourceManager = TopSolid.Kernel.SX.Resources.ResourceManager;

namespace EPFL.SpeckleTopSolid.AddIn
{
	/// <summary>
	/// Manages the resources.
	/// </summary>
	public static class Resources
	{
		// Static fields:

		/// <summary>
		/// Resources manager.
		/// </summary>
		private static ResourceManager manager = null;

		// Properties:

		/// <summary>
		/// Gets the resources manager.
		/// </summary>
		public static ResourceManager Manager
		{
			get
			{
				if (manager == null)
				{
					manager = new ResourceManager(typeof(Resources));
				}

				return manager;
			}
		}
	}
}

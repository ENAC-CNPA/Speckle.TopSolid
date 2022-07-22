using System;
using System.Reflection;


// This line is not mandatory, but improves loading performances


namespace EPFL.SpeckleTopSolid.UI
{
    public class Resolver
    {
        /// <summary>
        /// Set up an assembly resolver to load RhinoCommon and other Rhino
        /// assemblies from where Rhino is installed
        /// </summary>
        public static void Initialize()
        {
            if (System.IntPtr.Size != 8)
                throw new Exception("Only 64 bit applications can use Speckle");
            //AppDomain.CurrentDomain.AssemblyResolve += ResolveForSpeckleAssemblies;
            //AppDomain.CurrentDomain.AssemblyResolve += ResolveForAvaloniaAssemblies;
        }


        /// <summary>
        /// Whether or not to use the newest installation of Rhino on the system. By default the resolver will only use an
        /// installation with a matching major version.
        /// </summary>
        public static bool UseLatest { get; set; } = false;

        static Assembly ResolveForSpeckleAssemblies(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name).Name;
            string path = System.IO.Path.Combine("C:\\Sources\\Topsolid 7.15\\Debug x64", assemblyName + ".dll");
            //string path = System.IO.Path.Combine(RhinoSystemDirectory, assemblyName + ".dll");
            if (System.IO.File.Exists(path))
                return Assembly.LoadFrom(path);

            return null;
        }

        static Assembly ResolveForAvaloniaAssemblies(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name).Name;
            string path = System.IO.Path.Combine("C:\\Sources\\Topsolid 7.15\\Debug x64", assemblyName + ".dll");
            //string path = System.IO.Path.Combine(RhinoSystemDirectory, assemblyName + ".dll");
            if (System.IO.File.Exists(path))
                return Assembly.LoadFrom(path);

            return null;
        }

    }
}

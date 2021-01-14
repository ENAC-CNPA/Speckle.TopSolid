using System;
using System.Reflection;
using System.IO;


// This line is not mandatory, but improves loading performances
//[assembly: ExtensionApplication(typeof(EPFL.RhinoInsideTopSolid.UI))]


namespace EPFL.RhinoInsideTopSolid.UI
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
        throw new Exception("Only 64 bit applications can use RhinoInside");
      AppDomain.CurrentDomain.AssemblyResolve += ResolveForRhinoAssemblies;
    }

    static string _rhinoSystemDirectory;

    /// <summary>
    /// Directory used by assembly resolver to attempt load core Rhino assemblies. If not manually set,
    /// this will be determined by inspecting the registry
    /// </summary>
    public static string RhinoSystemDirectory
    {
      get
      {
        if(string.IsNullOrWhiteSpace(_rhinoSystemDirectory))
        {
          string baseName = @"SOFTWARE\McNeel\Rhinoceros";
          using (var baseKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(baseName))
          {
            string[] children = baseKey.GetSubKeyNames();
            Array.Sort(children);
            string versionName = "";
            for(int i=children.Length-1; i>=0; i--)
            {
              if(double.TryParse(children[i], out double d))
              {
                versionName = children[i];
                using (var installKey = baseKey.OpenSubKey($"{versionName}\\Install"))
                {
                  string corePath = installKey.GetValue("CoreDllPath") as string;
                  if( System.IO.File.Exists(corePath))
                  {
                    _rhinoSystemDirectory = System.IO.Path.GetDirectoryName(corePath);
                    break;
                  }
                }
              }
            }
          }
        }
        if (_rhinoSystemDirectory == null) _rhinoSystemDirectory = @"C:\Program Files\Rhino WIP\System";

        return _rhinoSystemDirectory;
      }
      set
      {
        _rhinoSystemDirectory = value;
      }
    }

    static Assembly ResolveForRhinoAssemblies(object sender, ResolveEventArgs args)
    {
      var assemblyName = new AssemblyName(args.Name).Name;
      string path = System.IO.Path.Combine(RhinoSystemDirectory, assemblyName + ".dll");
      if (System.IO.File.Exists(path))
            {
                return Assembly.LoadFrom(path);
            }
            else
            {
                Console.WriteLine(path);
            }

      return null;
    }
  }
}



namespace EPFL.RhinoInsideTopSolid.UI33
{
    // This class is instantiated by AutoCAD once and kept alive for the 
    // duration of the session. If you don't do any one time initialization 
    // then you should remove this class.
    public class Resolver2 //: IExtensionApplication
    {
        private Rhino.Runtime.InProcess.RhinoCore m_rhino_core;

        #region Plugin static constructor
        static readonly string SystemDir = (string)Microsoft.Win32.Registry.GetValue
        (
          @"HKEY_LOCAL_MACHINE\SOFTWARE\McNeel\Rhinoceros\7.0\Install", "Path",
          Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Rhino WIP", "System")
        );

        public void Resolver3()
        {
            ResolveEventHandler OnRhinoCommonResolve = null;
            AppDomain.CurrentDomain.AssemblyResolve += OnRhinoCommonResolve = (sender, args) =>
            {
                const string rhinoCommonAssemblyName = "RhinoCommon";
                var assembly_name = new AssemblyName(args.Name).Name;

                if (assembly_name != rhinoCommonAssemblyName)
                    return null;

                AppDomain.CurrentDomain.AssemblyResolve -= OnRhinoCommonResolve;
                return Assembly.LoadFrom(Path.Combine(SystemDir, rhinoCommonAssemblyName + ".dll"));
            };
        }
        #endregion // Plugin static constructor

        #region IExtensionApplication Members

        public void Initialize() //IExtensionApplication.Initialize()
        {
            // Load Rhino
            try
            {
                string SchemeName = "Inside-TopSolid";//$"Inside-{HostApplicationServices.Current.Product}-{HostApplicationServices.Current.releaseMarketVersion}";
                m_rhino_core = new Rhino.Runtime.InProcess.RhinoCore(new[] { $"/scheme={SchemeName}" });
            }
            catch
            {
                // ignored
            }

        }

        void Terminate() //IExtensionApplication.Terminate()
        {
            try
            {
                m_rhino_core?.Dispose();
            }
            catch
            {
                // ignored
            }
        }

        #endregion // IExtensionApplication Members
    }
}

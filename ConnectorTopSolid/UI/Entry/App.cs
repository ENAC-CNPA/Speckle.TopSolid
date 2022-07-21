using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection;
using System.IO;
using System.Windows.Media.Imaging;
using System.Linq;
using Forms = System.Windows.Forms;
using EPFL.SpeckleTopSolid.UI;


namespace EPFL.SpeckleTopSolid.UI.Entry
{
    public class App
    {

        #region Initializing and termination
        public void Initialize()
        {
            try
            {
         
                //Some dlls fail to load due to versions matching (0.10.7 vs 0.10.0)
                //the below should fix it! This affects Avalonia and Material 
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(OnAssemblyResolve);

                // DUI2
                SpeckleTopSolidCommand.InitAvalonia();
                var bindings = new ConnectorBindingsTopSolid();
                bindings.RegisterAppEvents();
                SpeckleTopSolidCommand.Bindings = bindings;
                OneClickCommand.Bindings = bindings;
            }
            catch (System.Exception e)
            {
                Forms.MessageBox.Show($"Add-in initialize context (true = application, false = doc): Error encountered: {e.ToString()}");
            }
        }

        Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly a = null;
            var name = args.Name.Split(',')[0];
            string path = Path.GetDirectoryName(typeof(App).Assembly.Location);

            string assemblyFile = Path.Combine(path, name + ".dll");

            if (File.Exists(assemblyFile))
                a = Assembly.LoadFrom(assemblyFile);

            return a;
        }


        #endregion

    }
}

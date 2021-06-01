using Speckle.DesktopUI;
using System;
using TopSolid.Cad.Design.DB.Documents;
using TopSolid.Kernel.DB.D3.Documents;
using TopSolid.Kernel.DB.D3.Planes;
using TopSolid.Kernel.DB.Documents;
using TopSolid.Kernel.DB.Parameters;
using TopSolid.Kernel.G.D3;
using TopSolid.Kernel.GR.Displays;
using TopSolid.Kernel.TX.Units;
using TopSolid.Kernel.UI.Commands;
using TopSolid.Kernel.WX;

namespace EPFL.SpeckleTopSolid.UI.LaunchCommand
{
    class LaunchSpeckleCommand : MenuCommand
    {
        protected override void Invoke()
        {
            //Show a message box to make sure the component is working
            
            SpeckleCommand();
        }
        // copied from autocad connector
        public static Bootstrapper Bootstrapper { get; set; }



        /// <summary>
        /// Main command to initialize Speckle Connector
        /// </summary>
        public static void SpeckleCommand()

        {
            /* try
             {
                 if (Bootstrapper != null)
                 {
                     Bootstrapper.Application.MainWindow.Show();
                     return;
                 }

                 Bootstrapper = new Bootstrapper()
                 {
                     Bindings = new ConnectorBindingsTopSolid()
                 };

                 Bootstrapper.Setup(System.Windows.Application.Current != null ? System.Windows.Application.Current : new System.Windows.Application());

                 Bootstrapper.Application.Startup += (o, e) =>
                 {
                     var helper = new System.Windows.Interop.WindowInteropHelper(Bootstrapper.Application.MainWindow);
                     helper.Owner = Application.Window.Handle;
                     //helper.Owner = Application.MainWindow.Handle;
                 };
             }
             catch (System.Exception e)
             {

             }
            */


            try
            {
                //copied from Matteo's Guide
                if (Bootstrapper != null)
                {
                    Bootstrapper.ShowRootView();
                    return;
                }

                Bootstrapper = new Bootstrapper()
                {
                    Bindings = new ConnectorBindingsTopSolid()
                };

                if (System.Windows.Application.Current != null)
                    new StyletAppLoader() { Bootstrapper = Bootstrapper };
                else
                    new Speckle.DesktopUI.App(Bootstrapper);

                Bootstrapper.Start(System.Windows.Application.Current);
            }
            catch
            {
                //MessageBox.Show("BOOM");
                Console.WriteLine("Erreur Boostrapper");
            }
            //catch (System.Exception e)
            //{

            //}
        }
    }
}

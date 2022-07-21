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
            //MessageBox.Show("BOOM");
            // SpeckleCommand();
            SpeckleCommand();
        }
        // copied from autocad connector
        //public static Bootstrapper Bootstrapper { get; set; }


        /// <summary>
        /// Main command to initialize Speckle Connector
        /// </summary>
        public static void SpeckleCommand()

        {

            // Speckle.ConnectorTopSolid.Entry.OneClickCommand.SendCommand();

        }


            /// <summary>
            /// Main command to initialize Speckle Connector
            /// </summary>
            public static void SpeckleCommandOld()

        {


            //try
            //{
            //    //copied from Matteo's Guide
            //    if (Bootstrapper != null)
            //    {
            //        Bootstrapper.ShowRootView();
            //        return;
            //    }

            //    Bootstrapper = new Bootstrapper()
            //    {
            //        Bindings = new ConnectorBindingsTopSolid()
            //    };

            //    if (System.Windows.Application.Current != null)
            //        new StyletAppLoader() { Bootstrapper = Bootstrapper };
            //    else
            //        new Speckle.DesktopUI.App(Bootstrapper);

            //    Bootstrapper.Start(System.Windows.Application.Current);
            //}
            //catch (System.Exception e)
            //{

            //}
        }
    }
}

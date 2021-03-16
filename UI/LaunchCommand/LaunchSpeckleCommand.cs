using Speckle.DesktopUI;
using TopSolid.Cad.Design.DB.Documents;
using TopSolid.Kernel.DB.D3.Planes;
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
            MessageBox.Show("BOOM");

            //Command as in other Speckle connectors, for the moement it does nothing
            Bootstrapper BootstrapperTopSolid = new Bootstrapper();

            //Copied from the Revit Connector, replaces Revit by TopSolid
             public class SpeckleTopSolidCommand : IExternalCommand
        {

            public static Bootstrapper Bootstrapper { get; set; }
            public static ConnectorBindingsTopSolid Bindings { get; set; }

            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                OpenOrFocusSpeckle(commandData.Application);
                return Result.Succeeded;
            }

            public static void OpenOrFocusSpeckle(UIApplication app)
            {
                try
                {
                    if (Bootstrapper != null)
                    {
                        Bootstrapper.Application.MainWindow.Show();
                        return;
                    }

                    Bootstrapper = new Bootstrapper()
                    {
                        Bindings = Bindings
                    };

                    Bootstrapper.Setup(Application.Current != null ? Application.Current : new Application());

                    Bootstrapper.Application.Startup += (o, e) =>
                    {
                        var helper = new System.Windows.Interop.WindowInteropHelper(Bootstrapper.Application.MainWindow);
                        helper.Owner = app.MainWindowHandle;
                    };

                }
                catch (Exception e)
                {

                }
            }
        }
    }
}

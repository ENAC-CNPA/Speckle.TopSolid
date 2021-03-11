using System;
using TK = TopSolid.Kernel;
using Speckle;
using TopSolid.Kernel.SX.UI;
using Speckle.DesktopUI;

namespace BIMConnector.UI.Test
{

    public partial class TestCommand : TopSolid.Kernel.UI.Commands.MenuCommand
    {
        // Constructors:

        /// <summary>
        /// Command constructor
        /// </summary>
        public TestCommand()
        {
            Console.WriteLine("Test Start 0");
        }


        // Properties:


        /// <summary>
        /// Return true to Enable the command button
        /// </summary>
        protected override bool CanInvoke
        {
            get
            {
                //Best practice:
                // return the combinaison to the base & your tests

                //In this case, just return the base CanInvoke
                return base.CanInvoke;
            }
        }

        // Methods:



        /// <summary>
        /// Method call when the command button is pressed
        /// </summary>
        protected override void Invoke()
        {
            //Invoke the base command...
            base.Invoke();

            Bootstrapper BootstrapperTOpSolid = new Bootstrapper();

        }
    }
}

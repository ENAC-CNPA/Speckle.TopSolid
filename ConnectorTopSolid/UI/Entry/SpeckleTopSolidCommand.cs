using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;

using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;

using DesktopUI2;
using DesktopUI2.ViewModels;
using DesktopUI2.Views;
using EPFL.SpeckleTopSolid.UI;
using Application = TopSolid.Kernel.UI.Application;


namespace EPFL.SpeckleTopSolid.UI.Entry
{
    class SpeckleTopSolidCommand
    {
        #region Avalonia parent window
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr value);
        const int GWL_HWNDPARENT = -8;
        #endregion

        private static Avalonia.Application AvaloniaApp { get; set; }
        public static Window MainWindow { get; private set; }
        private static CancellationTokenSource Lifetime = null;
        public static ConnectorBindingsTopSolid Bindings { get; set; }

        public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<DesktopUI2.App>()
          .UsePlatformDetect()
          .With(new SkiaOptions { MaxGpuResourceSizeBytes = 8096000 })
          .With(new Win32PlatformOptions { AllowEglInitialization = true, EnableMultitouch = false })
          .LogToTrace()
          .UseReactiveUI();


        public static void SpeckleCommand()
        {
            CreateOrFocusSpeckle();
        }

        public static void InitAvalonia()
        {
            BuildAvaloniaApp().Start(AppMain, null);
        }

        public static void CreateOrFocusSpeckle(bool showWindow = true)
        {

            if (Bindings == null) {
                App newApp = new App();
                newApp.Initialize();
            }

            if (MainWindow == null)
            {
                var viewModel = new MainWindowViewModel(Bindings);
                MainWindow = new MainWindow
                {
                    DataContext = viewModel
                };
            }

            try
            {
                if (showWindow)
                {
                    MainWindow.Show();
                    MainWindow.Activate();

                    //required to gracefully quit avalonia and the skia processes
                    //https://github.com/AvaloniaUI/Avalonia/wiki/Application-lifetimes
                    if (Lifetime == null)
                    {
                        Lifetime = new CancellationTokenSource();
                        Task.Run(() => AvaloniaApp.Run(Lifetime.Token));
                    }

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        var parentHwnd = Application.ActiveDocumentWindow.Handle;
                        var hwnd = MainWindow.PlatformImpl.Handle.Handle;
                        SetWindowLongPtr(hwnd, GWL_HWNDPARENT, parentHwnd);
                    }
                }
            }
            catch { }
        }

        private static void AppMain(Avalonia.Application app, string[] args)
        {
            AvaloniaApp = app;
        }


    }
}

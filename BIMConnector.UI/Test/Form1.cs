using Rhino.Runtime.InProcess;
using System;
using System.Windows.Forms;

namespace Sample_2
{
    public partial class Form1 : Form
    {
        Rhino.Runtime.InProcess.RhinoCore rhinoCore;
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            rhinoCore = new Rhino.Runtime.InProcess.RhinoCore(new string[] {"/NOSPLASH"}, WindowStyle.Hidden, Handle);
            base.OnHandleCreated(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            rhinoCore.Dispose();
            rhinoCore = null;
            base.OnHandleDestroyed(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //Rhino.RhinoDoc.ActiveDoc.Objects.AddSphere(new Rhino.Geometry.Sphere(Rhino.Geometry.Point3d.Origin, 10));
           string pathFolder = @"C:\Users\tamu\Documents\";
           string rhinoModel = System.IO.Path.Combine(pathFolder, "test2.3dm") ;
           string objModel = System.IO.Path.Combine(pathFolder, "test2.obj");

            Rhino.RhinoDoc.Open(pathFolder, out bool alreadyOpen);
            Rhino.RhinoDoc doc = Rhino.RhinoDoc.ActiveDoc;

            AddSphere(doc);

            var read_options = new Rhino.FileIO.FileReadOptions();
            read_options.BatchMode = true;
            read_options.ImportMode = true;
            var obj_options = new Rhino.FileIO.FileObjReadOptions(read_options);
            obj_options.MapYtoZ = false;
            var rc = Rhino.FileIO.FileObj.Read(objModel, doc, obj_options);
            if (!rc)
            {
              Console.WriteLine("Failed to import file");
              return;
            }


          // viewportControl1.Viewport.DisplayMode = Rhino.Display.DisplayModeDescription.FindByName("Arctic");
           viewportControl1.Viewport.DisplayMode = Rhino.Display.DisplayModeDescription.FindByName("Wireframe");
          viewportControl1.Invalidate();

      var plane = Rhino.Geometry.Plane.WorldXY;


      try
      {
        bool v = plane.IsValid;
      }
      catch (System.Exception )
      {
        return;
      }

    }

        public static Rhino.Commands.Result AddSphere(Rhino.RhinoDoc doc)
        {
          Rhino.Geometry.Point3d center = new Rhino.Geometry.Point3d(0, 0, 0);
          const double radius = 5.0;
          Rhino.Geometry.Sphere sphere = new Rhino.Geometry.Sphere(center, radius);
          if (doc.Objects.AddSphere(sphere) != Guid.Empty)
          {
            doc.Views.Redraw();
            return Rhino.Commands.Result.Success;
          }
          return Rhino.Commands.Result.Failure;
        }

    private void btnZoom_Click(object sender, EventArgs e)
    {
      Rhino.RhinoDoc doc = Rhino.RhinoDoc.ActiveDoc;
      // Rhino.Zo
     // Rhino.DocObjects.SelectionMethod.MousePick();

      doc.Views.ActiveView.ActiveViewport.ZoomExtentsSelected(); // ZoomExtentsSelected();
      //AddLayout(doc);

    }

    private void btnRedraw_Click(object sender, EventArgs e)
    {
      Rhino.RhinoDoc doc = Rhino.RhinoDoc.ActiveDoc;
      doc.Views.Redraw();
    }

    /// <summary>
    /// Generate a layout with a single detail view that zooms to a list of objects
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    public static Rhino.Commands.Result AddLayout(Rhino.RhinoDoc doc)
    {
      doc.PageUnitSystem = Rhino.UnitSystem.Millimeters;
      var page_views = doc.Views.GetPageViews();
      int page_number = (page_views == null) ? 1 : page_views.Length + 1;
      var pageview = doc.Views.AddPageView(string.Format("A0_{0}", page_number), 1189, 841);
      if (pageview != null)
      {
        Rhino.Geometry.Point2d top_left = new Rhino.Geometry.Point2d(20, 821);
        Rhino.Geometry.Point2d bottom_right = new Rhino.Geometry.Point2d(1169, 20);
        var detail = pageview.AddDetailView("ModelView", top_left, bottom_right, Rhino.Display.DefinedViewportProjection.Top);
        if (detail != null)
        {
          pageview.SetActiveDetail(detail.Id);
          detail.Viewport.ZoomExtents();
          detail.DetailGeometry.IsProjectionLocked = true;
          detail.DetailGeometry.SetScale(1, doc.ModelUnitSystem, 10, doc.PageUnitSystem);
          // Commit changes tells the document to replace the document's detail object
          // with the modified one that we just adjusted
          detail.CommitChanges();
        }
        pageview.SetPageAsActive();
        doc.Views.ActiveView = pageview;
        doc.Views.Redraw();
        return Rhino.Commands.Result.Success;
      }
      return Rhino.Commands.Result.Failure;
    }


  }
}

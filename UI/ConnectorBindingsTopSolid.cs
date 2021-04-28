using Speckle.Core.Api;
using Speckle.DesktopUI;
using Speckle.DesktopUI.Utils;
using Speckle.Newtonsoft.Json;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TopSolid.Kernel.DB.D3.Documents;
using TopSolid.Kernel.DB.Documents;
using TopSolid.Kernel.DB.Entities;
using TopSolid.Kernel.DB.Parameters;
using TopSolid.Kernel.DB.SmartObjects;
using TopSolid.Kernel.UI.Selections;

namespace EPFL.SpeckleTopSolid.UI.LaunchCommand
{
    public partial class ConnectorBindingsTopSolid : ConnectorBindings
    {
        [Inject]
        private IEventAggregator _events;
        private static string SpeckleKey = "speckle";
        public ConnectorBindingsTopSolid()
        {
            /*
            RhinoDoc.EndOpenDocument += RhinoDoc_EndOpenDocument;

            SelectionTimer = new Timer(2000) { AutoReset = true, Enabled = true };
            SelectionTimer.Elapsed += SelectionTimer_Elapsed;
            SelectionTimer.Start();
             */

        }

        internal void GetFileContextAndNotifyUI()
        {
            var streamStates = GetStreamsInFile();

            var appEvent = new ApplicationEvent()
            {
                Type = ApplicationEvent.EventType.DocumentOpened,
                DynamicInfo = streamStates
            };

            NotifyUi(appEvent);
        }

        /// <summary>
        /// Sends an event to the UI. The event types are pre-defined and inherit from EventBase.
        /// </summary>
        /// <param name="notifyEvent">The event to be published</param>

        //public virtual void NotifyUi(EventBase notifyEvent)
        //{
        //  //TODO: checked why it's null sometimes
        //  if(_events!=null)
        //    _events.PublishOnUIThread(notifyEvent);
        //}

        /// <summary>
        /// Raise a toast notification which is shown in the bottom of the main UI window.
        /// </summary>
        /// <param name="message">The body of the notification</param>

        //public virtual void RaiseNotification(string message)
        //{
        //    var notif = new ShowNotificationEvent() { Notification = message };
        //    NotifyUi(notif);
        //}

        //public virtual bool CanSelectObjects()
        //{
        //    return false;
        //}

        //public virtual bool CanTogglePreview()
        //{
        //    return false;
        //}

        #region abstract methods

        /// <summary>
        /// Gets the current host application name.
        /// </summary>
        /// <returns></returns>
        public override string GetHostAppName() => TopSolid.Kernel.UI.Application.Name;

        /// <summary>
        /// Gets the current opened/focused file's name.
        /// Make sure to check regarding unsaved/temporary files.
        /// </summary>
        /// <returns></returns>
        public override string GetFileName() => TopSolid.Kernel.UI.Application.CurrentDocument.Name.ToString();

        /// <summary>
        /// Gets the current opened/focused file's id. 
        /// Generate one in here if the host app does not provide one.
        /// </summary>
        /// <returns></returns>
        public override string GetDocumentId() => TopSolid.Kernel.UI.Application.CurrentDocument.PdmDocumentId;

        /// <summary>
        /// Gets the current opened/focused file's locations.
        /// Make sure to check regarding unsaved/temporary files.
        /// </summary>
        /// <returns></returns>
        public override string GetDocumentLocation() => TopSolid.Kernel.UI.Application.CurrentDocument.FilePath;

        /// <summary>
        /// Gets the current opened/focused file's view, if applicable.
        /// </summary>
        /// <returns></returns>
        public override string GetActiveViewName() => TopSolid.Kernel.UI.Application.ActiveDocumentWindow.Name;

        public override List<StreamState> GetStreamsInFile()
        {
            return new List<StreamState>();
        }

        GeometricDocument document = TopSolid.Kernel.UI.Application.CurrentDocument as GeometricDocument;



        public override void AddNewStream(StreamState state)
        {
            //Create a text parameter to hold the Json string
            TopSolid.Kernel.TX.Undo.UndoSequence.UndoCurrent();
            TopSolid.Kernel.TX.Undo.UndoSequence.Start("Test", true);
            TextParameterEntity texte = new TextParameterEntity(document, 0);
            texte.Value = (JsonConvert.SerializeObject(state));
            texte.Name = "TestparamSpeckle";
            document.ParametersFolderEntity.AddEntity(texte);
            TopSolid.Kernel.TX.Undo.UndoSequence.End();


            //Doc.Strings.SetString(SpeckleKey, state.Stream.id, JsonConvert.SerializeObject(state));
        }

        public override void PersistAndUpdateStreamInFile(StreamState state)
        {
            //Update value of the text parameter in TS
            TopSolid.Kernel.TX.Undo.UndoSequence.UndoCurrent();
            TopSolid.Kernel.TX.Undo.UndoSequence.Start("Test", true);
            var a = document.ParametersFolderEntity.SearchEntity("TestparamSpeckle") as TextParameterEntity;
            a.Value = (JsonConvert.SerializeObject(state));
            TopSolid.Kernel.TX.Undo.UndoSequence.End();
        }

        public override async Task<StreamState> SendStream(StreamState state)
        {
            if (state.Filter != null)
            {
                state.SelectedObjectIds = GetSelectedObjects();
            }

            try
            {
                //var commitId = await Client.CommitCreate(actualCommit);

                await state.RefreshStream();
                //state.PreviousCommitId = commitId;

                PersistAndUpdateStreamInFile(state);
                //RaiseNotification($"{objCount} objects sent to {state.Stream.name}.");
            }
            catch (Exception e)
            {
                Globals.Notify($"Failed to create commit.\n{e.Message}");
                state.Errors.Add(e);
            }

            return state;
        }

        public override Task<StreamState> ReceiveStream(StreamState state)
        {
            throw new NotImplementedException();
        }

        public override List<string> GetSelectedObjects()
        {
            List<string> Objs = new List<string>();
            Objs.Add(document.AbsoluteOriginPointEntity.Id.ToString());
            return Objs;
            //var objs = Doc?.Objects.GetSelectedObjects(true, false).Select(obj => obj.Id.ToString()).ToList();
            //return objs;

        }

        public override List<string> GetObjectsInView()
        {
            throw new NotImplementedException();
        }

        public override void RemoveStreamFromFile(string streamId)
        {
            throw new NotImplementedException();
        }

        public override void SelectClientObjects(string args)
        {
            throw new NotImplementedException();
        }

        public override List<ISelectionFilter> GetSelectionFilters()
        {
            //Copied from Revit 
            return new List<ISelectionFilter>()
            {
                new ListSelectionFilter {
                Name = "Category", Icon = "Category", Description = "Hello world. This is a something something filter.", Values = new List<string>() { "Boats", "Rafts", "Barges" }
            }
            };

            //copied from Rhino
            /*var layers = Doc.Layers.ToList().Select(layer => layer.Name).ToList();

             return new List<ISelectionFilter>()
             {
                 new ListSelectionFilter { Name = "Layers", Icon = "Filter", Description = "Selects objects based on their layers.", Values = layers }
             };
            */

        }

        /// <summary>
        /// Returns the serialised clients present in the current open host file.
        /// </summary>
        /// <returns></returns>
        //public void List<StreamState> GetStreamsInFile();

        /// <summary>
        /// Adds a new client and persists the info to the host file
        /// </summary>

        //public abstract void AddNewStream(StreamState state);

        /// <summary>
        /// Persists the stream info to the host file; if maintaining a local in memory copy, make sure to update it too.
        /// </summary>

        //public abstract void PersistAndUpdateStreamInFile(StreamState state);

        /// <summary>
        /// Pushes a client's stream
        /// </summary>
        /// <param name="state"></param>
        /// <param name="progress"></param>

        //public abstract Task<StreamState> SendStream(StreamState state);

        /// <summary>
        /// Receives stream data from the server
        /// </summary>
        /// <param name="state"></param>
        /// <param name="progress"></param>
        /// <returns></returns>

        //public abstract Task<StreamState> ReceiveStream(StreamState state);

        /// <summary>
        /// Adds the current selection to the provided client.
        /// </summary>
        //public abstract List<string> GetSelectedObjects();

        ///// <summary>
        ///// Gets a list of objects in the currently active view
        ///// </summary>
        ///// <returns></returns>
        //public abstract List<string> GetObjectsInView();

        ///// <summary>
        ///// Removes a client from the file and updates the host file.
        ///// </summary>
        ///// <param name="args"></param>
        //public abstract void RemoveStreamFromFile(string streamId);

        ///// <summary>
        ///// clients should be able to select/preview/hover one way or another their associated objects
        ///// </summary>
        ///// <param name="args"></param>
        //public abstract void SelectClientObjects(string args);

        /// <summary>
        /// Should return a list of filters that the application supports. 
        /// </summary>
        /// <returns></returns>
        //public abstract List<ISelectionFilter> GetSelectionFilters();

        #endregion
    }
}

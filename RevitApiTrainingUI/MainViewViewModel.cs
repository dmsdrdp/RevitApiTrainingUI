using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Prism.Commands;
using RevitAPITrainingLibrary;
using System;
using System.Collections.Generic;

namespace RevitApiTrainingUI
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand SaveCommand { get; }
        public List<Element> PickedObjects { get; } = new List<Element>();
        public List<WallType> WallTypes { get; } = new List<WallType>();
        public WallType SelectedWallTypes { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)

        {
            _commandData = commandData;
            SaveCommand = new DelegateCommand(OnSaveCommand);
            PickedObjects = SelectionUtils.PickObjects(commandData);
            WallTypes = WallsUtils.GetWallTypes(commandData);
        }

        private void OnSaveCommand()        //изменение типа стен
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (PickedObjects.Count == 0 || SelectedWallTypes == null)
                return;
            using (var ts = new Transaction(doc, "Set system type"))
            {
                try 
                {
                    ts.Start();
                    foreach (var pickedObject in PickedObjects)
                    {
                        if (pickedObject is Wall)
                        {
                            var oWall = pickedObject as Wall;
                            oWall.ChangeTypeId(SelectedWallTypes.Id);
                        }
                    }
                    ts.Commit();
                }
                catch
                {
                    RaiseCloseRequest();
                }
                
            }
            RaiseCloseRequest();
        }
        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
                CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}

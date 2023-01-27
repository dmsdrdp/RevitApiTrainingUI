using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace RevitAPITrainingLibrary
{
    public class SelectionUtils
    {
    

        // выбор элементов
        public static List<Element> PickObjects(ExternalCommandData commandData, string message = "Выберите элемент")
        {
            try
            {
                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                var selectedObjects = uidoc.Selection.PickObjects(ObjectType.Element, message);
                List<Element> elementList = selectedObjects.Select(selectedObject => doc.GetElement(selectedObject)).ToList();
                return elementList;
            }
            catch
            { 
                return null; 
            }
        }
    }
}

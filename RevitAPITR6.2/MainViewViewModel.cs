using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAPITrainingLibrary;
using System.Collections.Generic;

namespace RevitAPITR6._2
{
    public class MainViewViewModel
    {
        private ExternalCommandData commandData;

        public Furniture Furniture { get; }
        public List<FamilySymbol> FamilyTypes { get; } = new List<FamilySymbol>();
       
        public DelegateCommand SaveCommand { get; }

        public FamilySymbol SelectedFamilyType { get; set; }
        //private ExternalCommandData commandData1;

        public MainViewViewModel(ExternalCommandData commandData, DelegateCommand saveCommand = null)
        {
            this.commandData = commandData;
            Furniture = SelectionUtils.GetObject<Furniture>(commandData, "Выберите мебель");
            FamilyTypes = FamilySymbolUtils.GetFamilySymbols(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            SaveCommand = saveCommand;
        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var locationCurve=Furniture.Location as LocationCurve
            var furnitureCurve = locationCurve.Curve;

            var oLevel = (Level)doc.GetElement(Furniture.LevelId);
            FamilyInstanceUtils.CreateFamilyInstance(_commandData, SelectedFamilyType, furnitureCurve.GetEndPoint(0), oLevel);

            RaiseCloseRequest();
        }

        //public MainViewViewModel(ExternalCommandData commandData1)
        //{
        //    this.commandData1 = commandData1;
        //}

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
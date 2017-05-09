using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Rhino;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

namespace Apoorv.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand SelectPointCommand { get; }
        public RhinoDoc Doc { get; set; }

        public MainViewModel()
        {
            Doc = RhinoDoc.ActiveDoc;
            SelectPointCommand = new RelayCommand(OnSelectPoint);
        }

        private void OnSelectPoint()
        {
            RhinoApp.WriteLine("The {0} command will add a line right now.");

            Point3d pt0;
            using (var getPointAction = new GetPoint())
            {
                getPointAction.SetCommandPrompt("Please select the start point");
                if (getPointAction.Get() != GetResult.Point)
                {
                    RhinoApp.WriteLine("No start point was selected.");
                    return;
                }
                pt0 = getPointAction.Point();
            }

            Point3d pt1;
            using (var getPointAction = new GetPoint())
            {
                getPointAction.SetCommandPrompt("Please select the end point");
                getPointAction.SetBasePoint(pt0, true);
                getPointAction.DynamicDraw +=
                    (sender, e) => e.Display.DrawLine(pt0, e.CurrentPoint, System.Drawing.Color.DarkRed);
                if (getPointAction.Get() != GetResult.Point)
                {
                    RhinoApp.WriteLine("No end point was selected.");
                    return;
                }
                pt1 = getPointAction.Point();
            }

            Doc.Objects.AddLine(pt0, pt1);
            Doc.Views.Redraw();
            RhinoApp.WriteLine("The {0} command added one line to the document.");
        }
    }
}

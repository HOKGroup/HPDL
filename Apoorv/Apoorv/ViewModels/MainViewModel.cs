using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Rhino;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Grasshopper.Plugin;

namespace Apoorv.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand SelectPointCommand { get; }
        public RelayCommand OpenGrasshopperCommand { get; }
        public RelayCommand OpenFileDialog { get; }
        public RhinoDoc Doc { get; set; }

        public MainViewModel()
        {
            Doc = RhinoDoc.ActiveDoc;
            SelectPointCommand = new RelayCommand(OnSelectPoint);
            OpenGrasshopperCommand = new RelayCommand(OnOpenGrasshoper);
            OpenFileDialog = new RelayCommand(OnOpenFileDialog);
        }

        /// <summary>
        /// Sets EPW File Path.
        /// </summary>
        private void OnOpenFileDialog()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".epw",
                Filter =
                    "EPW Files (*.epw)|*.epw"
            };
            var result = dialog.ShowDialog();
            if (result == true)
            {
                EpwFilePath = dialog.FileName;
            }
        }

        /// <summary>
        /// Opens Grasshopper and writes a value to it.
        /// </summary>
        private void OnOpenGrasshoper()
        {
            // (Konrad) This bit might have to change later since we are not likely to publish
            // this plug-in with that same folder structure. This is solely caused by the fact
            // that we are debugging this and *bin/ folders are ignored from GitHub.
            var newPath = string.Empty;
            var dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (dir != null)
            {
                newPath = Path.GetFullPath(Path.Combine(dir, @"..\..\..\")) + @"_Samples\sampleFile.gh";
            }
            if (!File.Exists(newPath)) return;

            var gh = RhinoApp.GetPlugInObject("Grasshopper") as GH_RhinoScriptInterface;
            gh?.OpenDocument(dir);
            gh?.HideEditor();
            gh?.AssignDataToParameter("1f808148-f532-4d5a-8a1e-8b0cec03975f", EpwFilePath);
            gh?.RunSolver(true);
            gh?.SaveDocument();
            gh?.CloseDocument();
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// Selected EPW File Path
        /// </summary>
        private string _epwFilePath;
        public string EpwFilePath
        {
            get => _epwFilePath;
            set
            {
                _epwFilePath = value;
                RaisePropertyChanged(() => EpwFilePath);
            }
        }
    }
}

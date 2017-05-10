using System.Diagnostics;
using System.Windows;
using Rhino;
using Rhino.DocObjects;

namespace Apoorv.Views
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainView
    {
        public bool IsEventsEnabled { get; }

        public MainView()
        {
            InitializeComponent();
            IsEventsEnabled = false;
            IsVisibleChanged += OnPanelVisibleChanged;
        }

        /// <summary>
        /// Handles IsVisible change events
        /// </summary>
        private void OnPanelVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DebugWriteMethod();
            EnablePanelEvents((bool)e.NewValue);
        }

        /// <summary>
        /// Enables or disables panel event watchers
        /// </summary>
        private void EnablePanelEvents(bool bEnable)
        {
            if (bEnable != IsEnabled)
            {
                if (bEnable)
                {
                    RhinoDoc.SelectObjects += OnSelectObjects;
                    RhinoDoc.DeselectObjects += OnDeselectObjects;
                    RhinoDoc.DeselectAllObjects += OnDeselectAllObjects;
                }
                else
                {
                    RhinoDoc.SelectObjects -= OnSelectObjects;
                    RhinoDoc.DeselectObjects -= OnDeselectObjects;
                    RhinoDoc.DeselectAllObjects -= OnDeselectAllObjects;
                }
            }
            IsEnabled = bEnable;
        }

        /// <summary>
        /// Called when objects are selected
        /// </summary>
        private void OnSelectObjects(object sender, RhinoObjectSelectionEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when objects are deselected
        /// </summary>
        private void OnDeselectObjects(object sender, RhinoObjectSelectionEventArgs e)
        {
            DebugWriteMethod();
        }

        /// <summary>
        /// Called when all objects are deselected
        /// </summary>
        private void OnDeselectAllObjects(object sender, RhinoDeselectAllObjectsEventArgs e)
        {
            DebugWriteMethod();
        }

        private void DebugWriteMethod()
        {
#if DEBUG
            var className = GetType().Name;
            var stackTrace = new StackTrace();
            var methodName = stackTrace.GetFrame(1).GetMethod().Name;
            RhinoApp.WriteLine("** {0}.{1} called **", className, methodName);
#endif
        }
    }
}

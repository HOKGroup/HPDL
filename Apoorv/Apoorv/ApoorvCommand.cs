using Apoorv.ViewModels;
using Apoorv.Views;

namespace Apoorv
{
    [System.Runtime.InteropServices.Guid("8cdc497d-2357-48a8-9a3f-21947420a776")]
    public class WpfPanelHost : RhinoWindows.Controls.WpfElementHost
    {
        public WpfPanelHost()
            : base(new MainView(), new MainViewModel())
        {
        }

        /// <summary>
        /// Returns the ID of this panel.
        /// </summary>
        public static System.Guid PanelId => typeof(WpfPanelHost).GUID;
    }
}

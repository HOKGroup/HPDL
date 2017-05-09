using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.UI;

namespace Apoorv.Commands
{
    [System.Runtime.InteropServices.Guid("664de49d-11f7-4de8-9c7f-26f768685e21")]
    public class WpfPanel : Command
    {
        public override string EnglishName => "HPDL";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var panelId = WpfPanelHost.PanelId;
            var panelVisible = Panels.IsPanelVisible(panelId);

            var prompt = (panelVisible)
                ? "HPDL panel is visible. New value"
                : "HPDL panel is hidden. New value";

            var go = new GetOption();
            go.SetCommandPrompt(prompt);
            var hideIndex = go.AddOption("Hide");
            var showIndex = go.AddOption("Show");
            var toggleIndex = go.AddOption("Toggle");
            go.Get();

            if (go.CommandResult() != Result.Success) return go.CommandResult();

            var option = go.Option();
            if (null == option) return Result.Failure;

            var index = option.Index;
            if (index == hideIndex)
            {
                if (panelVisible) Panels.ClosePanel(panelId);
            }
            else if (index == showIndex)
            {
                if (!panelVisible) Panels.OpenPanel(panelId);
            }
            else if (index == toggleIndex)
            {
                if (panelVisible) Panels.ClosePanel(panelId);
                else Panels.OpenPanel(panelId);
            }
            return Result.Success;
        }
    }
}

using Shanism.Client.Scenarios;
using Shanism.Client.UI.Containers;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Scenario
{
    /// <summary>
    /// A list of scenarios in or more directories.
    /// </summary>
    public class ScenarioListControl : PagedListPanel
    {
        static readonly string[] ScannedDirectories =
        {
#if DEBUG
            "../../../../../maps",
#else
            "../../maps",
#endif
        };

        readonly ScenarioList model;

        public IEnumerable<ScenarioConfig> Scenarios => model?.Scenarios ?? Enumerable.Empty<ScenarioConfig>();

        public event Action<ScenarioConfig> ScenarioSelected;
        public event Action<ScenarioListControl> FinishLoading;

        bool hasLoaded;
        ScenarioRow expandedRow;

        public ScenarioListControl()
        {
            BackColor = UiColors.ControlBackground;

            try
            {
                model = new ScenarioList(ScannedDirectories);
                SetText("Loading...");
            }
            catch (Exception e)
            {
                SetText(e.Message);
            }

            List.SizeChanged += onSizeChanged;
            onSizeChanged(null, null);

            PageChanged += (_) => onRowExpanded(null, null);
        }

        protected override void OnInitialized(EventArgs e)
        {
        }

        void SetText(string text)
        {
            List.RemoveAll();
            List.Add(new Label
            {
                AutoSize = true,
                Text = text,
            });
        }

        void onSizeChanged(object o, EventArgs e)
        {
            var btnHeight = ScenarioRow.CollapsedHeight + Padding;
            var btnHeightDiff = ScenarioRow.ExpandedHeight - ScenarioRow.CollapsedHeight;
            var maxSz = List.Height - Padding - btnHeightDiff;

            ItemsPerPage = (int)(maxSz / btnHeight);
        }

        public override void Update(int msElapsed)
        {
            if(!hasLoaded && model?.HasLoaded == true)
            {
                finishLoading();
                hasLoaded = true;
            }

            base.Update(msElapsed);
        }

        void finishLoading()
        {
            // re-add all rows
            List.RemoveAll();
            foreach(var sc in Scenarios.OrderBy(sc => sc.Name))
            {
                var row = new ScenarioRow(sc)
                {
                    Size = new System.Numerics.Vector2(Size.X - 2 * Padding, ScenarioRow.CollapsedHeight),
                    ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                };

                row.ScenarioSelected += onScenarioSelected;
                row.FocusGained += onRowExpanded;

                List.Add(row);
            }

            // de-select any rows
            FocusControl.ClearFocus();

            // fire the event
            FinishLoading?.Invoke(this);
        }

        void onScenarioSelected(ScenarioConfig sc)
        {
            ScenarioSelected?.Invoke(sc);
        }

        void onRowExpanded(Control sender, EventArgs c)
        {
            var r = (ScenarioRow)sender;
            if(expandedRow != null && expandedRow != r)
                expandedRow.ClearFocus();
            expandedRow = r;
        }
    }
}

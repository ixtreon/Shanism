using Shanism.Client.Scenarios;
using Shanism.Client.UI;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Scenario
{
    public class ScenarioListControl : PagedFlowPanel
    {
        ScenarioList scenarios;

        public event Action<ScenarioConfig> ScenarioSelected;

        bool needsRefresh;
        ScenarioRow expandedRow;

        public ScenarioListControl()
        {
            scenarios = new ScenarioList();

            startLoading();
            scenarios.LoadList("directories.txt");

            List.SizeChanged += onSizeChanged;
            onSizeChanged(this);

            PageChanged += (_) => onRowExpanded(null);
        }

        void onSizeChanged(Control c)
        {
            var btnHeight = ScenarioRow.CollapsedHeight + Padding;
            var btnHeightDiff = ScenarioRow.ExpandedHeight - ScenarioRow.CollapsedHeight;
            var maxSz = List.Height - Padding - btnHeightDiff;

            ItemsPerPage = (int)(maxSz / btnHeight);
        }

        protected override void OnUpdate(int msElapsed)
        {
            if (needsRefresh && scenarios.HasLoaded)
                finishLoading();

            base.OnUpdate(msElapsed);
        }

        void startLoading()
        {
            needsRefresh = true;

            List.RemoveAll();
            List.Add(new Label { Text = "Loading..." });
        }

        void finishLoading()
        {
            List.RemoveAll();
            foreach (var sc in scenarios.Scenarios.OrderBy(sc => sc.Name))
            {
                var row = new ScenarioRow(sc)
                {
                    Size = new Common.Vector(Size.X - 2 * Padding, ScenarioRow.CollapsedHeight),
                    ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                };

                row.ScenarioSelected += onScenarioSelected;
                row.Expanded += onRowExpanded;
                List.Add(row);
            }

            needsRefresh = false;
        }

        void onScenarioSelected(ScenarioConfig sc)
        {
            ScenarioSelected?.Invoke(sc);
        }

        void onRowExpanded(ScenarioRow r)
        {
            if (expandedRow != null && expandedRow != r)
                expandedRow.IsExpanded = false;
            expandedRow = r;
        }
    }
}

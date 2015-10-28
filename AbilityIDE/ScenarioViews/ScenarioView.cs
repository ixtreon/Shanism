using ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbilityIDE.ScenarioViews
{
    public class ScenarioView : UserControl
    {
        public virtual ScenarioViewType ViewType { get; }

        public event Action ScenarioChanged;

        protected bool _loadingScenario;
        ScenarioFile _scenario;
        public ScenarioFile Scenario
        {
            get { return _scenario; }
            set
            {
                if (_scenario != value)
                {
                    _scenario = value;

                    Task.Run(async () =>
                    {
                        _loadingScenario = true;
                        await LoadScenario();
                        _loadingScenario = false;
                    });
                }
            }
        }

        protected virtual async Task LoadScenario() { }

        public virtual void SaveScenario() { }

        public void MarkAsChanged()
        {
            if (!_loadingScenario)
            {
                Scenario.IsDirty = true;
                ScenarioChanged?.Invoke();
            }
        }
    }
}

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

        bool _loadingScenario;
        ScenarioBase _scenario;
        public ScenarioBase Scenario
        {
            get { return _scenario; }
            set
            {
                if (_scenario != value)
                {
                    _scenario = value;

                    _loadingScenario = true;
                    LoadScenario();
                    _loadingScenario = false;
                }
            }
        }

        protected virtual void LoadScenario() { }

        protected virtual void SaveScenario() { }

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

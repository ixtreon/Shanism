using Shanism.Editor.ViewModels;

namespace Shanism.Editor.Views
{
    internal interface IScenarioControl
    {
        ScenarioViewModel Model { get; }

        void SetModel(ScenarioViewModel model);
    }
}
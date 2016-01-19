using ShanoEditor.ViewModels;

namespace ShanoEditor.Views
{
    internal interface IScenarioControl
    {
        ScenarioViewModel Model { get; }

        void SetModel(ScenarioViewModel model);
    }
}
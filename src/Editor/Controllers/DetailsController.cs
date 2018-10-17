using Shanism.Editor.Actions;
using Shanism.ScenarioLib;

namespace Shanism.Editor.Controllers
{
    /// <summary>
    /// Allows editing the scenario details.
    /// </summary>
    class DetailsController
    {

        readonly ActionList history;
        readonly ScenarioConfig scenario;

        public string Name => scenario.Name;
        public string Author => scenario.Author;
        public string Description => scenario.Description;

        public DetailsController(ActionList history, ScenarioConfig scenario)
        {
            this.scenario = scenario;
            this.history = history; }

        public void SetName(string scenarioName)
        {
            var act = new ScenarioChangeAction<string>(scenario, s => s.Name, scenarioName);
            history.Do(act);
        }

        public void SetAuthor(string authorName)
        {
            var act = new ScenarioChangeAction<string>(scenario, s => s.Author, authorName);
            history.Do(act);
        }

        public void SetDescription(string description)
        {
            var act = new ScenarioChangeAction<string>(scenario, s => s.Description, description);
            history.Do(act);
        }
    }
}

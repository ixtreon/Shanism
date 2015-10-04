using System;
using Engine;

namespace DefaultScenario
{
    public class ScenarioTemplate : Scenario
    {

        public ScenarioTemplate()
        {
            Description = "Nqkav mazen scenarii koito ima Nqkvo Ime";
            Name = "Scenarioto Brat";
        }


        public override void ListFiles()
        {
            files.Add("Abilities/Attack.cs");
            throw new NotImplementedException();
        }
    }
}

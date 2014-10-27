using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;

namespace Engine.Systems
{
    public abstract class CustomScript : ScenarioObject
    {
        public virtual void LoadModels(ModelManager manager) { }

        public virtual void GameStart() { }

        public virtual void OnPlayerJoined(Player p) { }

        public virtual void OnHeroSpawned(Hero hero) { }
        public virtual void OnUnitSpawned(Unit unit) { }

        public virtual void OnUnitDeath(Unit unit) { }
    }
}

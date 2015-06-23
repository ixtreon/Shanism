using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using Engine.Objects.Game;

namespace Engine.Systems
{
    /// <summary>
    /// Represents an abstract base for the creation of custom game scripts. 
    /// </summary>
    public abstract class CustomScript : ScenarioObject
    {
        public virtual void LoadModels(ModelManager manager) { }

        public virtual void GameStart() { }

        public virtual void OnPlayerJoined(Player p) { }

        public virtual void OnHeroSpawned(Hero hero) { }

        public virtual void OnUnitAdded(Unit unit) { }

        public virtual void OnDoodadAdded(Doodad d) { }

        public virtual void OnUnitDeath(Unit unit) { }
    }
}

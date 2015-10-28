using Client.Objects;
using Client.UI;
using IO;
using IO.Common;
using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class ObjectManager : Control
    {
        static Dictionary<Type, Func<IGameObject, ObjectControl>> gameObjectToControlMap = new Dictionary<Type, Func<IGameObject, ObjectControl>>
        {
            { typeof(IHero), (o) => new HeroControl((IHero)o) },
            { typeof(IUnit), (o) => new UnitControl((IUnit)o) },
            { typeof(IDoodad), (o) => new DoodadControl((IDoodad)o) },
        };

        public int MainHeroGuid { get; private set; }

        HeroControl _mainHeroControl;
        public HeroControl MainHeroControl
        {
            get
            {
                if(_mainHeroControl == null)
                    _mainHeroControl = objects.TryGet(MainHeroGuid) as HeroControl;
                return _mainHeroControl;
            }
        }

        public IHero MainHero
        {
            get { return MainHeroControl?.Hero; }
        }

        
        /// <summary>
        /// A dictionary of all objects indexed by their guid. 
        /// </summary>
        Dictionary<int, ObjectControl> objects = new Dictionary<int, ObjectControl>();

        /// <summary>
        /// The event raised whenever a unit was clicked. 
        /// </summary>
        public event Action<UnitControl> UnitClicked;

        public IEnumerable<IGameObject> VisibleObjects
        {
            get
            {
                return objects.Select(kvp => kvp.Value.Object).ToArray();
            }
        }

        public override void Update(int msElapsed)
        {
            // update everyone who is still around
            foreach (var u in objects.Values)
                u.Update(msElapsed);
        }

        public void AddObject(IGameObject o)
        {
            //get the control constructor for this type of game object. 
            var mapping = gameObjectToControlMap
                .Where(kvp => kvp.Key.IsAssignableFrom(o.GetType()))
                .Select(kvp => kvp.Value)
                .FirstOrDefault();

            if(mapping == null)
                throw new Exception("Some type of object we don't recognize yet!");

            //create the UI control
            var gameObject = mapping(o);

            //hook to events
            gameObject.MouseDown += gameObject_MouseDown;

            //add to the dictionary
            objects[o.Guid] = gameObject;

            //add the UI control
            this.Add(gameObject);
        }

        public void RemoveObject(int guid)
        {
            ////never remove the local hero
            //if (LocalHero != null && guid == LocalHero.Unit.Guid)
            //    return;

            try
            {
                this.Remove(objects[guid]);
            }
            catch { }
            objects.Remove(guid);
        }

        public void SetMainHero(int guid)
        {
            MainHeroGuid = guid;
        }

        void gameObject_MouseDown(Control c, Vector pos)
        {
            if (c is UnitControl)
                UnitClicked?.Invoke((UnitControl)c);
        }
    }
}

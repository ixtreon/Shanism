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
    class ObjectGod : Control
    {
        HeroControl _mainHeroControl;

        /// <summary>
        /// Contains a mapping from each subtype of <see cref="IGameObject"/> 
        /// to a function that constructs the respective UI <see cref="Control"/>. 
        /// </summary>
        static readonly Dictionary<Type, Func<IGameObject, ObjectControl>> gameObjectToControlMap = new Dictionary<Type, Func<IGameObject, ObjectControl>>
        {
            { typeof(IHero), (o) => new HeroControl((IHero)o) },
            { typeof(IUnit), (o) => new UnitControl((IUnit)o) },
            { typeof(IDoodad), (o) => new DoodadControl((IDoodad)o) },
        };

        /// <summary>
        /// Gets the GUID of the main hero. 
        /// </summary>
        public int MainHeroGuid { get; private set; }


        /// <summary>
        /// Gets the UI control of the main hero, if the hero is available. 
        /// </summary>
        public HeroControl MainHeroControl
        {
            get
            {
                if(_mainHeroControl?.Hero.Guid != MainHeroGuid)
                    _mainHeroControl = objects.TryGet(MainHeroGuid) as HeroControl;
                return _mainHeroControl;
            }
        }

        /// <summary>
        /// Gets the main hero, if it is available. 
        /// </summary>
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

        public override void Update(int msElapsed)
        {
            // update everyone who is still around
            foreach (var objectControl in objects.Values)
                objectControl.Update(msElapsed);
        }

        /// <summary>
        /// Adds the given game object to the index. 
        /// </summary>
        /// <param name="o"></param>
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

        /// <summary>
        /// Removes a game object by its GUID. 
        /// </summary>
        /// <param name="guid">The GUID of the object to remove. </param>
        public void RemoveObject(int guid)
        {
            try //being lazy..
            {
                Remove(objects[guid]);
            }
            catch { }

            objects.Remove(guid);
        }

        /// <summary>
        /// Sets the game object with the given GUID as the main hero. 
        /// </summary>
        /// <param name="guid">The GUID of the game object. </param>
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

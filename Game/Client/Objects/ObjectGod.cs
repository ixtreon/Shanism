using Client.Input;
using Client.Objects;
using Client.UI;
using Client.UI.CombatText;
using IO;
using IO.Common;
using IO.Objects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class ObjectGod : Control
    {
        public static readonly ObjectGod Default = new ObjectGod();

        const uint NoHeroGuid = 0;


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
            { typeof(IEffect), (o) => new EffectControl((IEffect)o) },
        };


        /// <summary>
        /// A dictionary of all objects indexed by their guid. 
        /// </summary>
        readonly Dictionary<uint, ObjectControl> objects = new Dictionary<uint, ObjectControl>();

        /// <summary>
        /// The event raised whenever a game object clicked. 
        /// </summary>
        public event Action<MouseButtonEvent> ObjectClicked;

        /// <summary>
        /// The event raised whenever the terrain is clicked. 
        /// </summary>
        public event Action<MouseButtonEvent> TerrainClicked;

        #region Main Hero Properties
        /// <summary>
        /// Gets the GUID of the main hero. 
        /// </summary>
        public uint MainHeroGuid { get; private set; } = NoHeroGuid;


        /// <summary>
        /// Gets the UI control of the main hero, if the hero is available. 
        /// </summary>
        public HeroControl MainHeroControl
        {
            get
            {
                if(MainHeroGuid != (_mainHeroControl?.Hero.Guid ?? NoHeroGuid))
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
        #endregion

        #region Cursor Hover Properties


        public Vector CursorGamePosition { get; private set; }

        #endregion

        protected ObjectGod()
        {
            MouseDown += ObjectGod_MouseDown;
        }

        //terrain click
        void ObjectGod_MouseDown(MouseButtonEvent e)
        {
            TerrainClicked?.Invoke(e);
        }

        protected override void OnUpdate(int msElapsed)
        {

        }

        /// <summary>
        /// Adds the given game object to the index. 
        /// </summary>
        /// <param name="o"></param>
        public void AddObject(IGameObject o)
        {
            if (objects.ContainsKey(o.Guid))
            {
                Console.WriteLine("An object with the guid of {0} already exists: {1}".F(o.Guid, o.Name));
                return;
            }

            //get the control constructor for this type of game object. 
            var mapping = gameObjectToControlMap
                .Where(kvp => kvp.Key.IsAssignableFrom(o.GetType()))
                .Select(kvp => kvp.Value)
                .FirstOrDefault();

            if(mapping == null)
                throw new Exception("Some type of object we don't recognize yet!");

            //create the UI control
            var gameObject = mapping(o);
            gameObject.MouseDown += gameObject_MouseDown;

            //add to the dictionary
            objects[o.Guid] = gameObject;

            //add the UI control
            Add(gameObject);
        }

        public IGameObject TryGet(uint guid)
        {
            return objects.TryGet(guid).Object;
        }

        /// <summary>
        /// Removes a game object by its GUID. 
        /// </summary>
        /// <param name="guid">The GUID of the object to remove. </param>
        public void RemoveObject(uint guid)
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
        public void SetMainHero(uint guid)
        {
            MainHeroGuid = guid;
        }

        void gameObject_MouseDown(MouseButtonEvent e)
        {
            ObjectClicked?.Invoke(e);
        }

        //draws objects' shadows to the buffer
        public void DrawShadows(SpriteBatch sb)
        {
            var g = new Graphics(sb, Location, Size);

            foreach(var c in Controls)
            {
                if (c == MainHeroControl || !(c is ObjectControl))
                    continue;

                var oc = (ObjectControl)c;
                var obj = oc.Object;

                oc.Draw(g);
            }
        }
        
    }
}

using Client.Input;
using Client.Objects;
using Client.UI;
using Client.UI.CombatText;
using IO;
using IO.Common;
using IO.Message;
using IO.Message.Server;
using IO.Objects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class ObjectManager : Control, IClientSystem
    {
        const uint NoHeroGuid = 0;


        public static readonly ObjectManager Default = new ObjectManager();



        HeroControl _mainHeroControl;

        /// <summary>
        /// Contains a mapping from each subtype of <see cref="IEntity"/> 
        /// to a function that constructs the respective UI <see cref="Control"/>. 
        /// </summary>
        static readonly Dictionary<Type, Func<IEntity, ObjectControl>> gameObjectToControlMap = new Dictionary<Type, Func<IEntity, ObjectControl>>
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
        public event Action<MouseButtonArgs> ObjectClicked;

        /// <summary>
        /// The event raised whenever the terrain is clicked. 
        /// </summary>
        public event Action<MouseButtonArgs> TerrainClicked;

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
                if (MainHeroGuid != (_mainHeroControl?.Hero.Id ?? NoHeroGuid))
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


        protected ObjectManager()
        {
            MouseDown += ObjectGod_MouseDown;
        }

        //terrain click
        void ObjectGod_MouseDown(MouseButtonArgs e)
        {
            TerrainClicked?.Invoke(e);
        }

        protected override void OnUpdate(int msElapsed)
        {
            Maximize();
        }

        public void HandleMessage(IOMessage ioMsg)
        {
            switch (ioMsg.Type)
            {
                case MessageType.ObjectSeen:
                    AddObject(((ObjectSeenMessage)ioMsg).Object);
                    break;

                case MessageType.ObjectUnseen:
                    RemoveObject(((ObjectUnseenMessage)ioMsg).ObjectId);
                    break;

                case MessageType.PlayerStatusUpdate:
                    SetMainHero(((PlayerStatusMessage)ioMsg).HeroId);
                    break;
            }
        }


        /// <summary>
        /// Adds the given game object to the index. 
        /// </summary>
        /// <param name="o"></param>
        public void AddObject(IEntity o)
        {
            var objControl = objects.TryGet(o.Id);

            if (objControl == null)
            {
                //get the control constructor for this type of game object. 
                var objType = o.GetType();
                var mapping = gameObjectToControlMap
                    .FirstOrDefault(kvp => kvp.Key.IsAssignableFrom(objType)).Value;

                if (mapping == null)
                    throw new Exception($"The object type `{objType.FullName}` cannot be mapped to an `{nameof(ObjectControl)}` type. !");

                //create a new UI control
                objControl = mapping(o);
                objControl.MouseDown += gameObject_MouseDown;

                //add to the dictionary
                objects[o.Id] = objControl;

                //add the UI control
                Add(objControl);
            }
            else
            {
                //TODO: see if cached, uncache it
            }
        }

        public IEntity TryGet(uint guid)
        {
            return objects.TryGet(guid).Object;
        }

        /// <summary>
        /// Removes a game object by its GUID. 
        /// </summary>
        /// <param name="guid">The GUID of the object to remove. </param>
        public void RemoveObject(uint guid)
        {
            var objControl = objects.TryGet(guid);
            if (objControl != null && !(objControl is DoodadControl))
            {
                Remove(objControl);
                objects.Remove(guid);
            }
        }

        /// <summary>
        /// Sets the game object with the given GUID as the main hero. 
        /// </summary>
        /// <param name="guid">The GUID of the game object. </param>
        public void SetMainHero(uint guid)
        {
            MainHeroGuid = guid;
        }

        void gameObject_MouseDown(MouseButtonArgs e)
        {
            ObjectClicked?.Invoke(e);
        }

        //draws objects' shadows to the buffer
        public void DrawShadows(SpriteBatch sb)
        {
            var g = new Graphics(sb, Location, Size);

            foreach (var c in Controls)
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

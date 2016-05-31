using Shanism.Client.Input;
using Shanism.Client.Objects;
using Shanism.Client.UI;
using Shanism.Client.UI.CombatText;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using Shanism.Common.Objects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.Systems;

namespace Shanism.Client.Systems
{
    class ObjectSystem : ClientSystem
    {
        const uint NoHeroGuid = 0;


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


        public readonly Control Root = new Control();

        /// <summary>
        /// A dictionary of all objects indexed by their guid. 
        /// </summary>
        readonly Dictionary<uint, ObjectControl> objects = new Dictionary<uint, ObjectControl>();


        /// <summary>
        /// Gets the UI control of the main hero, if the hero is available. 
        /// </summary>
        public HeroControl MainHeroControl { get; private set; }

        /// <summary>
        /// Gets the GUID of the main hero. 
        /// </summary>
        public uint MainHeroGuid { get; set; } = NoHeroGuid;

        /// <summary>
        /// Gets the main hero, if it is available. 
        /// </summary>
        public IHero MainHero => MainHeroControl?.Hero;

        /// <summary>
        /// The event raised whenever a game object clicked. 
        /// </summary>
        public event Action<MouseButtonArgs> ObjectClicked;

        /// <summary>
        /// The event raised whenever the terrain is clicked. 
        /// </summary>
        public event Action<MouseButtonArgs> TerrainClicked;


        public ObjectSystem()
        {
            Root.MouseDown += (e) => TerrainClicked?.Invoke(e);
        }

        public override void Update(int msElapsed)
        {
            Root.Maximize();

            if (MainHeroControl?.Hero.Id != MainHeroGuid)
                MainHeroControl = objects.TryGet(MainHeroGuid) as HeroControl;

            foreach (var o in objects)
                o.Value.Update(msElapsed);

            //objList.Update(objects);
        }

        public override void HandleMessage(IOMessage ioMsg)
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
                Root.Add(objControl);
            }
            else
            {
                //TODO: see if cached, uncache it
            }
        }

        public IEntity TryGet(uint guid)
        {
            return objects.TryGet(guid)?.Object;
        }

        /// <summary>
        /// Removes a game object by its GUID. 
        /// </summary>
        /// <param name="guid">The GUID of the object to remove. </param>
        public void RemoveObject(uint guid)
        {
            var objControl = objects.TryGet(guid);
            if (objControl != null)
            {
                Root.Remove(objControl);
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

        //draws objects' shadows to a buffer
        public void DrawShadows(SpriteBatch sb)
        {
            var g = new Graphics(sb, Root.Location, Root.Size);

            foreach (var c in Root.Controls)
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

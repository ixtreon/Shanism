using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.Drawing;
using Shanism.Client.Input;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Systems
{
    class ObjectSystem : ClientSystem
    {
        const uint NoHeroGuid = 0;

        readonly GraphicsDevice device;

        readonly AssetList content;

        readonly Dictionary<uint, EntitySprite> unitSpriteMapping = new Dictionary<uint, EntitySprite>();



        uint mainHeroGuid;

        UnitSprite mainSprite;
        EntitySprite hoverSprite;

        SpriteBatch objectBatch;
        Matrix transformMatrix;


        /// <summary>
        /// Gets the GUID of the main hero. 
        /// </summary>
        public uint MainHeroGuid => mainHeroGuid;


        /// <summary>
        /// Gets the main hero, if it is available. 
        /// </summary>
        public IHero MainHero => mainSprite?.Entity as IHero;

        /// <summary>
        /// Gets the sprite of our main hero, if it exists.
        /// </summary>
        public UnitSprite MainHeroSprite => mainSprite;


        public EntitySprite HoverSprite => hoverSprite;

        public IEnumerable<IEntity> Entities => unitSpriteMapping.Select(kvp => kvp.Value.Entity);

        public event Action<uint> MainHeroChanged;


        public ObjectSystem(GraphicsDevice device, AssetList content)
        {
            this.device = device;
            this.content = content;

            objectBatch = new SpriteBatch(device);
        }

        public override void Update(int msElapsed)
        {
            //update matrix
            var proj = Matrix.CreateOrthographic(
                (float)Screen.GameSize.X, (float)Screen.GameSize.Y, -5, 5);
            transformMatrix = Matrix.CreateTranslation(-(float)Screen.InGameCenter.X, -(float)Screen.InGameCenter.Y, 0)
                * Matrix.CreateScale((float)Screen.GameScale.X, (float)Screen.GameScale.Y, 1)
                * Matrix.CreateTranslation(Screen.HalfSize.X, Screen.HalfSize.Y, 0);


            //update all sprites + hover guy
            var mousePos = MouseInfo.InGamePosition;

            hoverSprite = null;
            foreach (var kvp in unitSpriteMapping)
            {
                var s = kvp.Value;
                var e = s.Entity;

                s.Update(msElapsed);

                //update hover sprite
                if (Vector.Abs(mousePos - e.Position) < e.Scale / 2)
                    if (hoverSprite == null || s.DrawDepth < HoverSprite.DrawDepth)
                        hoverSprite = s;
            }

            //re-set mainhero
            if (mainSprite?.Entity.Id != mainHeroGuid)
            {
                EntitySprite sprite;
                if (unitSpriteMapping.TryGetValue(mainHeroGuid, out sprite))
                    mainSprite = sprite as UnitSprite;

            }
        }



        //draws objects to the device
        public void Draw()
        {
            objectBatch.Begin(SpriteSortMode.FrontToBack,
                BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.DepthRead, RasterizerState.CullNone,
                null, transformMatrix);

            //draw sprites at units' in-game positions
            foreach (var kvp in unitSpriteMapping)
                kvp.Value.Draw(objectBatch);

            objectBatch.End();
        }

        public override void HandleMessage(IOMessage ioMsg)
        {
            switch (ioMsg.Type)
            {
                case MessageType.ObjectSeen:
                    AddEntity(((ObjectSeenMessage)ioMsg).Object);
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
        public void AddEntity(IEntity o)
        {
            EntitySprite sprite;
            if (!unitSpriteMapping.TryGetValue(o.Id, out sprite))
            {
                //get the control constructor for this type of game object. 
                switch (o.ObjectType)
                {
                    case ObjectType.Hero:
                    case ObjectType.Unit:
                        sprite = new UnitSprite(content, (IUnit)o);
                        break;

                    case ObjectType.Doodad:
                    case ObjectType.Effect:
                        sprite = new EntitySprite(content, o);
                        break;

                    default:
                        throw new Exception("Missing switch case!");
                }

                unitSpriteMapping[o.Id] = sprite;
            }
        }

        public IEntity TryGet(uint guid)
        {
            EntitySprite sprite;
            if (unitSpriteMapping.TryGetValue(guid, out sprite))
                return sprite.Entity;

            return null;
        }

        /// <summary>
        /// Removes a game object by its GUID. 
        /// </summary>
        /// <param name="guid">The GUID of the object to remove. </param>
        public void RemoveObject(uint guid)
        {
            unitSpriteMapping.Remove(guid);
        }

        /// <summary>
        /// Sets the game object with the given GUID as the main hero. 
        /// </summary>
        /// <param name="guid">The GUID of the game object. </param>
        public void SetMainHero(uint guid)
        {
            mainHeroGuid = guid;
            MainHeroChanged?.Invoke(guid);
        }
    }
}

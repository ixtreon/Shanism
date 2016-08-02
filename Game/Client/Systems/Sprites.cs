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
    class SpriteSystem : ClientSystem
    {
        const uint NoHeroGuid = 0;

        readonly GraphicsDevice device;

        readonly AssetList content;

        readonly Dictionary<uint, EntitySprite> unitSpriteDict = new Dictionary<uint, EntitySprite>();

        readonly HashSet<uint> visibleUnits = new HashSet<uint>();


        uint mainHeroGuid;

        UnitSprite mainSprite;
        EntitySprite hoverSprite;

        SpriteBatch objectBatch;
        Matrix transformMatrix;



        /// <summary>
        /// Gets the player's main hero, if it exists. 
        /// </summary>
        public IHero MainHero => mainSprite?.Entity as IHero;

        /// <summary>
        /// Gets the sprite of our main hero, if it exists.
        /// </summary>
        public UnitSprite MainHeroSprite => mainSprite;

        /// <summary>
        /// Gets the sprite currently under the cursor.
        /// </summary>
        public EntitySprite HoverSprite => hoverSprite;


        public SpriteSystem(GraphicsDevice device, AssetList content)
        {
            this.device = device;
            this.content = content;

            objectBatch = new SpriteBatch(device);
        }

        public override void Update(int msElapsed)
        {
            //update the transformation matrix
            transformMatrix = Matrix.CreateTranslation(-(float)Screen.InGameCenter.X, -(float)Screen.InGameCenter.Y, 0)
                * Matrix.CreateScale((float)Screen.GameScale.X, (float)Screen.GameScale.Y, 1)
                * Matrix.CreateTranslation(Screen.HalfSize.X, Screen.HalfSize.Y, 0);


            //update all sprites + hover guy
            var mousePos = MouseInfo.InGamePosition;

            foreach (var kvp in unitSpriteDict)
                kvp.Value.RemoveFlag = true;

            foreach (var e in Server.VisibleEntities)
            {
                EntitySprite sprite;
                if (!unitSpriteDict.TryGetValue(e.Id, out sprite))
                {
                    unitSpriteDict[e.Id] = sprite = createSprite(e);
                }

                sprite.Update(msElapsed);
                sprite.RemoveFlag = false;
            }

            hoverSprite = null;
            foreach (var e in Server.VisibleEntities)
                if (Vector.Abs(mousePos - e.Position) < e.Scale / 2)
                {
                    hoverSprite = unitSpriteDict[e.Id];
                    break;
                }

            //remove old sprites
            const int CleanupFactor = 20;
            if (unitSpriteDict.Count > CleanupFactor * Server.VisibleEntities.Count)
                foreach (var kvp in unitSpriteDict.Where(kvp => kvp.Value.RemoveFlag).ToList())
                    unitSpriteDict.Remove(kvp.Key);

            //re-set mainhero
            if (mainSprite?.Entity.Id != mainHeroGuid)
            {
                EntitySprite sprite;
                if (unitSpriteDict.TryGetValue(mainHeroGuid, out sprite))
                    mainSprite = sprite as UnitSprite;

            }
        }

        private EntitySprite createSprite(IEntity e)
        {
            EntitySprite sprite;
            switch (e.ObjectType)
            {
                case ObjectType.Hero:
                case ObjectType.Unit:
                    sprite = new UnitSprite(content, (IUnit)e);
                    break;

                case ObjectType.Doodad:
                case ObjectType.Effect:
                    sprite = new EntitySprite(content, e);
                    break;

                default:
                    throw new Exception("Missing switch case!");
            }

            return sprite;
        }



        //draws objects to the device
        public void Draw()
        {
            objectBatch.Begin(SpriteSortMode.FrontToBack,
                BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.DepthRead, RasterizerState.CullNone,
                null, transformMatrix);

            //draw sprites at units' in-game positions
            foreach (var kvp in unitSpriteDict)
                if (!kvp.Value.RemoveFlag)
                    kvp.Value.Draw(objectBatch);

            objectBatch.End();
        }

        public override void HandleMessage(IOMessage ioMsg)
        {
            switch (ioMsg.Type)
            {
                case MessageType.PlayerStatusUpdate:
                    SetMainHero(((PlayerStatusMessage)ioMsg).HeroId);
                    break;
            }
        }


        /// <summary>
        /// Adds the given game object to the index. 
        /// </summary>
        /// <param name="o"></param>
        void AddEntity(IEntity o)
        {
            EntitySprite sprite;
            if (!unitSpriteDict.TryGetValue(o.Id, out sprite))
            {
            }
        }

        public IEntity TryGet(uint guid)
        {
            EntitySprite sprite;
            if (unitSpriteDict.TryGetValue(guid, out sprite))
                return sprite.Entity;

            return null;
        }

        /// <summary>
        /// Removes a game object by its GUID. 
        /// </summary>
        /// <param name="guid">The GUID of the object to remove. </param>
        void Remove(uint guid)
        {
            unitSpriteDict.Remove(guid);
        }

        /// <summary>
        /// Sets the game object with the given GUID as the main hero. 
        /// </summary>
        /// <param name="guid">The GUID of the game object. </param>
        public void SetMainHero(uint guid)
        {
            mainHeroGuid = guid;
        }
    }
}

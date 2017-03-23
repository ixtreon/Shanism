using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Common;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.Sprites;

namespace Shanism.Client.Systems
{
    /// <summary>
    /// Lists and draws objects.
    /// </summary>
    class SpriteSystem : ShanoComponent, IClientSystem
    {
        const uint NoHeroGuid = 0;


        readonly Dictionary<uint, EntitySprite> unitSpriteDict = new Dictionary<uint, EntitySprite>();

        readonly HashSet<uint> visibleUnits = new HashSet<uint>();

        readonly IReceptor server;


        uint mainHeroGuid;

        UnitSprite mainSprite;
        EntitySprite hoverSprite;

        Canvas canvas;
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


        public SpriteSystem(IShanoComponent game, IReceptor server)
            : base(game)
        {
            this.server = server;
            canvas = new Canvas(game.Screen);
        }

        public void Update(int msElapsed)
        {
            //update the transformation matrix
            transformMatrix = Matrix.CreateTranslation(-(float)Screen.GameCenter.X, -(float)Screen.GameCenter.Y, 0)
                * Matrix.CreateScale((float)Screen.GameScale.X * Screen.RenderScale, (float)Screen.GameScale.Y * Screen.RenderScale, 1)
                * Matrix.CreateTranslation(Screen.HalfSize.X * Screen.RenderScale, Screen.HalfSize.Y * Screen.RenderScale, 0);


            //update all sprites + hover guy
            var mousePos = Mouse.InGamePosition;

            //set the `remove` flag
            foreach (var kvp in unitSpriteDict)
                kvp.Value.RemoveFlag = true;

            //prepare hover sprite
            double hoverMinDist = double.MaxValue;
            hoverSprite = null;

            //go thru the freshly visible entities
            foreach (var e in server.VisibleEntities)
            {
                //get or create the sprite
                EntitySprite sprite;
                if (!unitSpriteDict.TryGetValue(e.Id, out sprite))
                    unitSpriteDict[e.Id] = sprite = createSprite(e);

                //update the sprite
                sprite.Update(msElapsed);
                sprite.RemoveFlag = false;

                //update the hover sprite
                var d = mousePos.DistanceTo(e.Position);
                if (d < e.Scale / 2 && d < hoverMinDist)
                {
                    hoverSprite = sprite;
                    hoverMinDist = d;
                }
            }

            //remove old sprites when too many of 'em
            const int CleanupFactor = 20;
            if (unitSpriteDict.Count > CleanupFactor * server.VisibleEntities.Count)
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
                    sprite = new UnitSprite(this, (IUnit)e);
                    break;

                case ObjectType.Doodad:
                case ObjectType.Effect:
                    sprite = new EntitySprite(this, e);
                    break;

                default:
                    throw new Exception("Missing switch case!");
            }

            return sprite;
        }



        //draws objects to the device
        public void Draw()
        {
            canvas.Begin(SpriteSortMode.FrontToBack,
                BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.DepthRead, RasterizerState.CullNone,
                null, transformMatrix);

            //draw sprites at units' in-game positions
            foreach (var kvp in unitSpriteDict)
                if (!kvp.Value.RemoveFlag)
                    kvp.Value.Draw(canvas);

            canvas.End();
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

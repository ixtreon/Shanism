using Shanism.Client.Assets;
using Shanism.Client.IO;
using Shanism.Client.Sprites;
using Shanism.Common;
using Shanism.Common.Entities;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Shanism.Client.Systems
{
    public class EntitySystem
    {

        readonly List<EntitySprite> sprites = new List<EntitySprite>();
        readonly Dictionary<uint, EntitySprite> spriteMap = new Dictionary<uint, EntitySprite>();

        uint? mainHeroGuid;

        public EntitySprite HoverSprite { get; private set; }

        public UnitSprite HeroSprite { get; private set; }

        public IHero Hero { get; private set; }

        public IReadOnlyList<EntitySprite> Sprites => sprites;

        public IEngineReceptor Receptor { get; }

        readonly InGameTransform screen;
        readonly ContentList content;
        readonly MouseSystem mouse;

        public EntitySystem(IEngineReceptor receptor, InGameTransform screen, ContentList content, MouseSystem mouse)
        {
            this.mouse = mouse;
            this.screen = screen;
            this.content = content;

            Receptor = receptor;
        }

        public void SetHeroId(uint? guid)
        {
            mainHeroGuid = guid;
        }

        public void Update(int msElapsed)
        {
            var mousePos = mouse.InGamePosition;

            // add new
            foreach (var e in Receptor.VisibleEntities)
            {
                if (!spriteMap.TryGetValue(e.Id, out var sprite))
                    sprite = Add(e);

                sprite.Age = 0;
            }

            // update current + remove stale + find hover
            var distToHover = 1f;
            EntitySprite hover = null;
            for (int i = sprites.Count - 1; i >= 0; i--)
            {
                var s = sprites[i];

                //remove stales
                if (shouldRemove(s))
                {
                    sprites.RemoveAtFast(i);
                    spriteMap.Remove(s.Entity.Id);
                    continue;
                }

                //update entity + age
                if (s is UnitSprite u)
                {
                    UpdateUnit(u, msElapsed);
                }
                else
                {
                    UpdateEntity(s, msElapsed);
                }

                //update hover
                var r = s.Entity.Scale / 2;
                var dist = ((s.Entity.Position - mousePos) / r).LengthSquared();
                if (dist < distToHover)
                {
                    hover = s;
                    distToHover = dist;
                }
            }

            HoverSprite = hover;

            refreshMainHero();
        }

        double DepthRange => screen.Size.Y;
        double MinDepth => screen.Center.Y - DepthRange / 2;

        void UpdateDynamic(DynamicSprite sprite, int msElapsed)
        {
            var anim = sprite.Animation;
            // if anim is static, there's nothing to do
            if (anim == null || !anim.IsDynamic)
                return;

            sprite.CurrentFrameElapsed += msElapsed;

            // if not time to change the anim yet, return
            if (sprite.CurrentFrameElapsed < anim.Period)
                return;

            // advance to next frame, replay, or stop
            sprite.CurrentFrame++;
            sprite.CurrentFrameElapsed -= anim.Period;

            if (sprite.CurrentFrame < anim.Frames.Count)
            {
                sprite.SetFrame(sprite.CurrentFrame);
            }
            else if (sprite.LoopAnimation)
            {
                sprite.Restart();
            }
        }

        void UpdateUnit(UnitSprite sprite, int msElapsed)
        {
            UpdateEntity(sprite, msElapsed);

            //update unit orientation
            if (sprite.Unit.MovementState.IsMoving)
            {
                sprite.SetAnimation("move", true);
                sprite.SetOrientation(sprite.Unit.MovementState.MoveDirection);
            }
            else if (sprite.Unit.IsCasting())
            {
                sprite.SetOrientation(mouse.UiPosition.Angle());
                sprite.SetAnimation("attack", false);
            }
            else
                sprite.SetAnimation(string.Empty, true);

            //tint black if dead
            if (sprite.Unit.IsDead)
            {
                sprite.DrawDepth = 1e-5f;
                sprite.Tint = Color.Black;
            }
        }

        void UpdateEntity(EntitySprite sprite, int msElapsed)
        {
            refreshAnimation(sprite);

            UpdateDynamic(sprite, msElapsed);

            if (!sprite.IsPlaying)
                sprite.SetAnimation(content.Animations.Default);

            // bounds 
            // -> if anim or position changed
            sprite.InGameBounds = sprite.Animation.GetTextureBounds(sprite.Entity.Position, sprite.Entity.Scale);

            // z-order
            //if(Entity.Name)
            sprite.DrawDepth = (float)((sprite.Entity.Position.Y - MinDepth) / DepthRange);

            // tint
            sprite.Tint = sprite.Entity.CurrentTint;

            sprite.Age += msElapsed;
        }




        void refreshAnimation(EntitySprite sprite)
        {
            if (content.Animations.TryGetChangedModel(sprite.Animation.Name, sprite.Entity.Model, sprite.AnimationName, out var anim))
            {
                sprite.SetAnimation(anim);
            }
        }

        public bool TryGetValue(uint guid, out IEntity entity)
        {
            if (spriteMap.TryGetValue(guid, out var sprite))
            {
                entity = sprite.Entity;
                return true;
            }

            entity = null;
            return false;
        }

        void refreshMainHero()
        {
            if (HeroSprite?.Entity.Id == mainHeroGuid)
                return;

            // get hero sprite + entity
            EntitySprite sprite = null;
            if (mainHeroGuid != null)
                spriteMap.TryGetValue(mainHeroGuid.Value, out sprite);

            HeroSprite = sprite as UnitSprite;
            Hero = sprite?.Entity as IHero;
        }

        bool shouldRemove(EntitySprite s)
        {
            switch (s.Entity.ObjectType)
            {
                case ObjectType.Hero:
                case ObjectType.Unit:
                    return s.Age > 0;

                case ObjectType.Effect:
                    return s.Age > 0;

                case ObjectType.Doodad:
                    return false;

                default:
                    throw new NotImplementedException();
            }

        }

        EntitySprite Add(IEntity e)
        {
            // create
            var sprite = Create(e);
            sprite.SetAnimation(content.Animations.Default);
            sprite.SetOrientation(sprite.Entity.Orientation);
            refreshAnimation(sprite);

            // add
            sprites.Add(sprite);
            spriteMap[sprite.Entity.Id] = sprite;

            return sprite;
        }

        EntitySprite Create(IEntity e)
        {
            switch (e.ObjectType)
            {
                case ObjectType.Hero:
                case ObjectType.Unit:
                    return new UnitSprite(content, (IUnit)e);

                case ObjectType.Doodad:
                case ObjectType.Effect:
                    return new EntitySprite(content, e);
            }
            throw new Exception("Missing switch case!");
        }
    }
}

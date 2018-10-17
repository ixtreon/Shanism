using Shanism.Client.UI.Containers;
using Shanism.Common;
using Shanism.Common.Entities;
using System;
using System.Numerics;

namespace Shanism.Client.UI.Game
{
    /// <summary>
    /// Displays information about the hover unit such as its name, level, life and mana. 
    /// </summary>
    class UnitInfoPanel : ListPanel
    {
        public const float DefaultWidth = 0.5f;

        const float hpBarHeight = 0.04f;
        const float manaBarHeight = 0.03f;

        public IUnit Target { get; set; }

        readonly Label nameLabel;
        readonly Label lvlLabel;
        readonly HealthBar healthBar;
        readonly ManaBar manaBar;


        public float HealthBarHeight
        {
            get => healthBar.Height;
            set => healthBar.Height = value;
        }

        public float ContentWidth
        {
            get => nameLabel.Width;
            set => nameLabel.Width
                 = lvlLabel.Width
                 = healthBar.Width
                 = manaBar.Width
                 = value;
        }

        public UnitInfoPanel()
            : base(Direction.TopDown, sizeMode: ListSizeMode.ResizeBoth)
        {
            BackColor = UiColors.ControlBackground;
            Direction = Direction.TopDown;

            Size = new Vector2(DefaultWidth);

            var nameFont = Content.Fonts.NormalFont;
            var lvlFont = Content.Fonts.SmallFont;

            Add(nameLabel = new Label
            {
                Size = new Vector2(ClientBounds.Width, nameFont.Height),

                Font = nameFont,
                TextColor = UiColors.Text,
                TextAlign = AnchorPoint.Center,
                Text = "Dummy Unit",
            });

            Add(lvlLabel = new Label
            {
                Size = new Vector2(ClientBounds.Width, lvlFont.Height),

                Font = lvlFont,
                TextColor = UiColors.Text,
                TextAlign = AnchorPoint.Center,
                Text = "Level 42",
            });

            Add(healthBar = new HealthBar
            {
                Size = new Vector2(ClientBounds.Width, hpBarHeight),
                ShowText = false,

                Padding = DefaultPadding / 2,
                ForeColor = Color.DarkRed,
            });

            Add(manaBar = new ManaBar
            {
                Size = new Vector2(ClientBounds.Width, manaBarHeight),
                ShowText = false,

                Padding = DefaultPadding / 2,
                ForeColor = Color.DarkBlue,
            });
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        public override void Update(int msElapsed)
        {
            var shown = (Target != null);
            if(shown)
            {
                healthBar.IsVisible = manaBar.IsVisible = !Target.IsDead;

                healthBar.Target = Target;
                manaBar.Target = Target;

                nameLabel.Text = Target.Name;
                lvlLabel.Text = $"Level {Target.Level}";

                if(Target is IHero h)
                    lvlLabel.ToolTip = $"{h.Experience}/{h.ExperienceNeeded} XP";
                else
                    lvlLabel.ToolTip = string.Empty;

            }

            IsVisible = shown;
        }
    }
}

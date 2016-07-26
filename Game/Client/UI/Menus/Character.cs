using Shanism.Client.Input;
using Shanism.Client.UI.Common;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.UI.Menus.Character;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Client.UI.Menus
{
    class CharacterMenu : Window
    {
        public IHero Target { get; set; }

        readonly Label charName;
        readonly XpBar charXpBar;

        readonly StatBox vitalityBox;
        readonly StatBox strengthBox;
        readonly StatBox agilityBox;
        readonly StatBox intellectBox;

        public CharacterMenu()
             : base(AnchorMode.Left)
        {
            TitleText = "Character";
            ToggleAction = ClientAction.ToggleCharacterMenu;
            VisibleChanged += onVisibleChanged;
            CanFocus = false;

            var charNameFont = Content.Fonts.FancyFont;
            var charNamePadding = Padding;
            charName = new Label
            {
                Location = new Vector(charNamePadding, TitleHeight + 2 * Padding),
                Size = new Vector(Size.X - 2 * charNamePadding, charNameFont.HeightUi + 2 * Padding),
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,


                AutoSize = false,
                TextXAlign = 0.5f,

                Font = charNameFont,
                //BackColor = Microsoft.Xna.Framework.Color.MistyRose,
            };

            charXpBar = new XpBar
            {
                Location = new Vector(Padding, charName.Bottom),
                Size = new Vector(Size.X - 2 * Padding, 0.05),
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,
            };

            vitalityBox = new StatBox
            {
                Location = new Vector(Padding, charXpBar.Bottom + 2 * Padding),
                Size = new Vector(Size.X - 2 * Padding, 0.12),
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,

                StatText = "Vitality",
                StatTooltip = "Each point increases your life by X and life regen by Y. ",

            };
            vitalityBox.AddLabel("Life", "Your current and total life. ");
            vitalityBox.AddLabel("Life Regen", "The amount of life regained or lost each second. ");
            vitalityBox.Resize();

            strengthBox = new StatBox
            {
                Location = new Vector(Padding, vitalityBox.Bottom + Padding),
                Size = vitalityBox.Size,
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,

                StatText = "Strength",
                StatTooltip = "Each point increases your damage by X and crit by Y. ",
            };
            strengthBox.AddLabel("Damage", "The damage dealt with each attack. ");
            strengthBox.AddLabel("Crit", "The chance to deal critical damage with each attack. ");
            strengthBox.Resize();

            agilityBox = new StatBox
            {
                Location = new Vector(Padding, strengthBox.Bottom + Padding),
                Size = vitalityBox.Size,
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,

                StatText = "Agility",
                StatTooltip = "Each point increases your dodge by X and attack speed by Y. ",
            };
            agilityBox.AddLabel("Dodge", "Your total chance to dodge attacks. ");
            agilityBox.AddLabel("Attack Speed", "The amount of attacks that can be done per second. ");
            agilityBox.Resize();

            intellectBox = new StatBox
            {
                Location = new Vector(Padding, agilityBox.Bottom + Padding),
                Size = vitalityBox.Size,
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,

                StatText = "Intellect",
                StatTooltip = "Each point increases your mana by X, mana regen by Y, and magic damage by Z. ",
            };
            intellectBox.AddLabel("Mana", "Your total mana. ");
            intellectBox.AddLabel("Mana Regen", "The amount of mana regained or lost each second. ");
            intellectBox.AddLabel("Magic Damage", "The bonus magic damage dealt with magical attacks. ");
            intellectBox.Resize();


            Add(charName);
            Add(charXpBar);

            Add(vitalityBox);
            Add(strengthBox);
            Add(agilityBox);
            Add(intellectBox);

        }

        void onVisibleChanged(Control obj)
        {
            IsVisible &= (Target != null);
        }

        protected override void OnUpdate(int msElapsed)
        {
            if (Target == null)
                return;

            charName.Text = Target.Name;
            charXpBar.Target = Target;

            vitalityBox.SetStatValue(Target.BaseVitality, Target.Vitality);
            vitalityBox.SetLabelValue(0,
                $"{Target.Life:0} / {Target.MaxLife:0}",
                $"{Target.Life:0.00} / {Target.MaxLife:0.00}");
            vitalityBox.SetLabelValue(1, $"{Target.LifeRegen:+0.0;-0.0;0}", $"{Target.LifeRegen:+0.0;-0.0;0} / sec");

            strengthBox.SetStatValue(Target.BaseStrength, Target.Strength);
            strengthBox.SetLabelValue(0, $"{Target.MinDamage} - {Target.MaxDamage}", "");

            agilityBox.SetStatValue(Target.BaseAgility, Target.Agility);
            agilityBox.SetLabelValue(0, 0, 0, "%");
            agilityBox.SetLabelValue(1, 
                $"{Target.AttackCooldown / 1000.0:0.0}", 
                $"{Target.AttackCooldown / 1000.0:0.0} sec / attack.\n{1000.0 / Target.AttackCooldown:0.0} attacks / sec");
            //intellectBox.SetLabelValue(0, 0, Target.MaxMana);

            intellectBox.SetStatValue(Target.BaseIntellect, Target.Intellect);
            intellectBox.SetLabelValue(0,
                $"{Target.Mana:0} / {Target.MaxMana:0}",
                $"{Target.Mana:0.00} / {Target.MaxMana:0.00}");
            intellectBox.SetLabelValue(1, $"{Target.ManaRegen:+0.0;-0.0;0}", $"{Target.ManaRegen:+0.0;-0.0;0} / sec");
            intellectBox.SetLabelValue(2, 0, Target.MagicDamage);

            //lblCharName.Text = Target?.Owner.Name ?? string.Empty;
            //lblCharName.Left = (Size.X - lblCharName.Size.X) / 2;
        }
    }
}

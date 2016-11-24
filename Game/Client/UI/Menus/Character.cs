using Shanism.Client.Input;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.UI.Menus.Character;
using Shanism.Common.Interfaces.Entities;
using Shanism.Client.UI.Game;

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
            agilityBox.AddLabel("Movement Speed", "The amount of tiles you can cross per second. ");
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

            var maxLife = Target.Stats[UnitStat.MaxLife];
            var lifeRegen = Target.Stats[UnitStat.LifeRegen];
            vitalityBox.SetStatValue(Target.BaseAttributes[HeroAttribute.Vitality], Target.Attributes[HeroAttribute.Vitality]);
            vitalityBox.SetLabelValue(0,
                $"{Target.Life:0} / {maxLife:0}",
                $"{Target.Life:0.00} / {maxLife:0.00}");
            vitalityBox.SetLabelValue(1, 
                $"{lifeRegen:+0.0;-0.0;0}", 
                $"{lifeRegen:+0.0;-0.0;0} / sec");

            var minDmg = Target.Stats[UnitStat.MinDamage];
            var maxDmg = Target.Stats[UnitStat.MaxDamage];
            strengthBox.SetStatValue(Target.BaseAttributes[HeroAttribute.Strength], Target.Attributes[HeroAttribute.Strength]);
            strengthBox.SetLabelValue(0, 
                $"{minDmg} - {maxDmg}", 
                "");

            var attacksPerSec = Target.Stats[UnitStat.AttacksPerSecond];
            var dodge = 0f;
            var moveSpeed = Target.Stats[UnitStat.MoveSpeed];
            agilityBox.SetStatValue(Target.BaseAttributes[HeroAttribute.Agility], Target.Attributes[HeroAttribute.Agility]);
            agilityBox.SetLabelValue(0, 
                $"{dodge:0}%", 
                $"{dodge:0.00}%");
            agilityBox.SetLabelValue(1,
                $"{attacksPerSec:0.0}",
                $"{attacksPerSec:0.0} attacks / sec\n{1f / attacksPerSec:0.0} sec / attack.");
            agilityBox.SetLabelValue(2,
                $"{moveSpeed:0.0}",
                $"{moveSpeed:0.0#}");

            var maxMana = Target.Stats[UnitStat.MaxMana];
            var manaRegen = Target.Stats[UnitStat.ManaRegen];
            var magicDmg = Target.Stats[UnitStat.MagicDamage];
            intellectBox.SetStatValue(Target.BaseAttributes[HeroAttribute.Intellect], Target.Attributes[HeroAttribute.Intellect]);
            intellectBox.SetLabelValue(0,
                $"{Target.Mana:0} / {maxMana:0}",
                $"{Target.Mana:0.00} / {maxMana:0.00}");
            intellectBox.SetLabelValue(1, 
                $"{manaRegen:+0.0;-0.0;0}", 
                $"{manaRegen:+0.0;-0.0;0} / sec");
            intellectBox.SetLabelValue(2, 
                $"{magicDmg}",
                "");
        }
    }
}

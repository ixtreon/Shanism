using Shanism.Client.UI.Containers;
using Shanism.Client.UI.Game;
using Shanism.Client.UI.Menus.Character;
using Shanism.Common;
using Shanism.Common.Entities;
using System;
using System.Numerics;

namespace Shanism.Client.UI.Menus
{
    class CharacterDetails : Window
    {
        public IHero Target { get; set; }

        readonly Label charName;
        readonly XpBar charXpBar;

        readonly StatBox vitalityBox;
        readonly StatBox strengthBox;
        readonly StatBox agilityBox;
        readonly StatBox intellectBox;

        public CharacterDetails()
             : base(AnchorMode.Left)
        {
            TitleText = "Character";
            ToggleAction = ClientAction.ToggleCharacterMenu;
            Shown += onShown;

            var charNameFont = Content.Fonts.FancyFont;
            var charNamePadding = Padding;
            charName = new Label
            {
                Location = new Vector2(charNamePadding, DefaultTitleHeight + 2 * Padding),
                Size = new Vector2(Size.X - 2 * charNamePadding, charNameFont.Height + 2 * Padding),
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,


                TextAlign = AnchorPoint.Center,

                Font = charNameFont,
                //BackColor = Microsoft.Xna.Framework.Color.MistyRose,
            };

            charXpBar = new XpBar
            {
                Location = new Vector2(Padding, charName.Bottom),
                Size = new Vector2(Size.X - 2 * Padding, 0.05f),
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,
            };

            vitalityBox = new StatBox
            {
                Location = new Vector2(Padding, charXpBar.Bottom + 2 * Padding),
                Size = new Vector2(Size.X - 2 * Padding, 0.12f),
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,

                MainText = "Vitality",
                MainTooltip = "Each point increases your life by X and life regen by Y. ",

            };
            vitalityBox.AddLabel("Life", "Your current and total life. ");
            vitalityBox.AddLabel("Life Regen", "The amount of life regained or lost each second. ");
            vitalityBox.Resize();

            strengthBox = new StatBox
            {
                Location = new Vector2(Padding, vitalityBox.Bottom + Padding),
                Size = vitalityBox.Size,
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,

                MainText = "Strength",
                MainTooltip = "Each point increases your damage by X and crit by Y. ",
            };
            strengthBox.AddLabel("Damage", "The damage dealt with each attack. ");
            strengthBox.AddLabel("Crit", "The chance to deal critical damage with each attack. ");
            strengthBox.Resize();

            agilityBox = new StatBox
            {
                Location = new Vector2(Padding, strengthBox.Bottom + Padding),
                Size = vitalityBox.Size,
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,

                MainText = "Agility",
                MainTooltip = "Each point increases your dodge by X and attack speed by Y. ",
            };
            agilityBox.AddLabel("Dodge", "Your total chance to dodge attacks. ");
            agilityBox.AddLabel("Attack Speed", "The amount of attacks that you can do per second. ");
            agilityBox.AddLabel("Movement Speed", "The amount of tiles you can cross per second. ");
            agilityBox.Resize();

            intellectBox = new StatBox
            {
                Location = new Vector2(Padding, agilityBox.Bottom + Padding),
                Size = vitalityBox.Size,
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,

                MainText = "Intellect",
                MainTooltip = "Each point increases your mana by X, mana regen by Y, and magic damage by Z. ",
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

        void onShown(object o, EventArgs e)
        {
            if (Target == null)
                Hide();
        }

        public override void Update(int msElapsed)
        {
            if (Target == null)
                return;

            charName.Text = Target.Name;
            charXpBar.Target = Target;

            var maxLife = Target.Stats[UnitField.MaxLife];
            var lifeRegen = Target.Stats[UnitField.LifeRegen];
            vitalityBox.SetMainValue(Target.BaseAttributes[HeroAttribute.Vitality], Target.Attributes[HeroAttribute.Vitality]);
            vitalityBox.SetChildValue(0,
                $"{Target.Life:0} / {maxLife:0}",
                $"{Target.Life:0.00} / {maxLife:0.00}");
            vitalityBox.SetChildValue(1,
                $"{lifeRegen:+0.0;-0.0;0}",
                $"{lifeRegen:+0.0;-0.0;0} / sec");

            var minDmg = Target.Stats[UnitField.MinDamage];
            var maxDmg = Target.Stats[UnitField.MaxDamage];
            strengthBox.SetMainValue(Target.BaseAttributes[HeroAttribute.Strength], Target.Attributes[HeroAttribute.Strength]);
            strengthBox.SetChildValue(0,
                $"{minDmg} - {maxDmg}",
                "");

            var attacksPerSec = Target.Stats[UnitField.AttacksPerSecond];
            var dodge = 0f;
            var moveSpeed = Target.Stats[UnitField.MoveSpeed];
            agilityBox.SetMainValue(Target.BaseAttributes[HeroAttribute.Agility], Target.Attributes[HeroAttribute.Agility]);
            agilityBox.SetChildValue(0,
                $"{dodge:0}%",
                $"{dodge:0.00}%");
            agilityBox.SetChildValue(1,
                $"{attacksPerSec:0.0}",
                $"{attacksPerSec:0.0} attacks / sec\n{1f / attacksPerSec:0.0} sec / attack.");
            agilityBox.SetChildValue(2,
                $"{moveSpeed:0.0}",
                $"{moveSpeed:0.0#}");

            var maxMana = Target.Stats[UnitField.MaxMana];
            var manaRegen = Target.Stats[UnitField.ManaRegen];
            var magicDmg = Target.Stats[UnitField.MagicDamage];
            intellectBox.SetMainValue(Target.BaseAttributes[HeroAttribute.Intellect], Target.Attributes[HeroAttribute.Intellect]);
            intellectBox.SetChildValue(0,
                $"{Target.Mana:0} / {maxMana:0}",
                $"{Target.Mana:0.00} / {maxMana:0.00}");
            intellectBox.SetChildValue(1,
                $"{manaRegen:+0.0;-0.0;0}",
                $"{manaRegen:+0.0;-0.0;0} / sec");
            intellectBox.SetChildValue(2,
                $"{magicDmg}",
                "");
        }
    }
}

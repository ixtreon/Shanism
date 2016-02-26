using Client.Input;
using Client.UI.Common;
using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using IO.Objects;
using Client.UI.Menus.Character;

namespace Client.UI.Menus
{
    class CharacterMenu : Window
    {
        public IHero Target { get; set; }

        readonly Label charName;
        readonly Control levelBox;

        readonly StatBox vitalityBox;
        readonly StatBox strengthBox;
        readonly StatBox agilityBox;
        readonly StatBox intellectBox;

        public CharacterMenu()
             : base(AnchorMode.Left)
        {
            TitleText = "Character";
            ToggleAction = GameAction.ToggleCharacterMenu;
            VisibleChanged += onVisibleChanged;


            vitalityBox = new StatBox
            {
                AbsolutePosition = new Vector(Padding, Padding),

                StatText = "Vitality",
                StatTooltip = "Each point increases your life by X and life regen by Y. ",
            };
            vitalityBox.AddLabel("Life", "");
            vitalityBox.AddLabel("Life Regen", "");

            strengthBox = new StatBox
            {
                AbsolutePosition = new Vector(Padding, vitalityBox.Bottom + Padding),

                StatText = "Strength",
                StatTooltip = "Each point increases your damage by X and crit by Y. ",
            };
            strengthBox.AddLabel("Damage", "");
            strengthBox.AddLabel("Crit", "");

            agilityBox = new StatBox
            {
                AbsolutePosition = new Vector(Padding, strengthBox.Bottom + Padding),

                StatText = "Agility",
                StatTooltip = "Each point increases your dodge by X and attack speed by Y. ",
            };
            agilityBox.AddLabel("Dodge", "");
            agilityBox.AddLabel("Attack Speed", "");
             
            intellectBox = new StatBox
            {
                AbsolutePosition = new Vector(Padding, agilityBox.Bottom + Padding),

                StatText = "Intellect",
                StatTooltip = "Each point increases your mana by X, mana regen by Y, and magic damage by Z. ",
            };
            intellectBox.AddLabel("Mana", "");
            intellectBox.AddLabel("Mana Regen", "");
            intellectBox.AddLabel("Magic Damage", "");


            charName = new Label
            {
                Location = new Vector(Padding, TitleHeight + Padding),
            };



            Add(vitalityBox);
            Add(strengthBox);
            Add(agilityBox);
            Add(intellectBox);

            Add(charName);
        }

        void onVisibleChanged(Control obj)
        {
            IsVisible &= (Target != null);
        }

        protected override void OnUpdate(int msElapsed)
        {
            if (Target == null)
                return;

            vitalityBox.SetStatValue(Target.BaseVitality, Target.Vitality);
            vitalityBox.SetLabelValue(0, 0, Target.MaxLife);

            //lblCharName.Text = Target?.Owner.Name ?? string.Empty;
            //lblCharName.Left = (Size.X - lblCharName.Size.X) / 2;
        }
    }
}

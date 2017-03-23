using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shanism.Client.Input;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{

    public abstract class TitleScreen : UiScreen
    {
        static readonly Vector TitleLocation = new Vector(0, 0.2);


        readonly Label title;

        readonly Label subTitle;
        

        public event Action<IShanoEngine> GameStarted;

        public double ContentStartY => subTitle.Bottom + Control.LargePadding * 5;


        public string TitleText
        {
            get { return title.Text; }
            set { title.Text = value; }
        }

        public string SubTitleText
        {
            get { return subTitle.Text; }
            set { subTitle.Text = value; }
        }


        public TitleScreen(IShanoComponent game)
            : base(game)
        {
            var titleFont = Content.Fonts.ShanoFont;
            var subTitleFont = Content.Fonts.FancyFont;
            
            Root.Add(title = new Label
            {
                Font = titleFont,
                Text = "Shanism",

                ParentAnchor = AnchorMode.Left | AnchorMode.Right,

                Width = Screen.UiSize.X,
                LineHeight = 1,

                Location = TitleLocation,
                TextXAlign = 0.5f,

                AutoSize = false,
            });
            Root.Add(subTitle = new Label
            {
                Font = subTitleFont,
                Text = string.Empty,

                ParentAnchor = AnchorMode.Left | AnchorMode.Right,

                Width = Screen.UiSize.X,
                LineHeight = 1,

                Location = new Vector(0, title.Bottom + Control.LargePadding),
                TextXAlign = 0.5f,

                AutoSize = false,
            });
        }

        public void StartGame(IShanoEngine game)
        { 
            GameStarted?.Invoke(game);
        }
    }
}

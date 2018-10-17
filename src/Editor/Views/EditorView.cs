using Ix.Math;
using Shanism.Client;
using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Client.UI.Tooltips;
using Shanism.Client.Views;
using Shanism.Common;
using Shanism.Editor.Controls;
using Shanism.Editor.Models.Brushes;
using Shanism.Editor.UI;
using Shanism.Editor.Views.InGame;
using Shanism.ScenarioLib;
using System.Numerics;
using System.Threading.Tasks;

namespace Shanism.Editor.Views
{
    sealed class EditorView : GameViewBase
    {

        ListPanel menuBar;

        //EditorEngine engine => game.Engine;
        CameraSystem camera;

        readonly Controllers.EditorGameState game;


        ScenarioConfig scenario => game.Scenario;

        BrushCollection Brushes { get; set; }

        public EditorView(Controllers.EditorGameState game) : base(game)
        {
            this.game = game;
        }


        protected override void OnInit()
        {
            base.OnInit();

            var brushArgs = new BrushArgs(Client, this);

            camera = new CameraSystem(Client, scenario, this);
            Brushes = new BrushCollection
            {
                { EditorBrushType.Objects, new ObjectBrush(brushArgs) },
                { EditorBrushType.Terrain, new TerrainBrush(brushArgs, game.TerrainEditor) },
                { EditorBrushType.Selection, new SelectionBrush(brushArgs, game.TerrainEditor) }
            };

            ActionActivated += async(o, e) => await onRootAction(o, e);

            Brushes.SetBrush(EditorBrushType.Terrain);
        }

        protected override void OnReload()
        {
            // UI
            RemoveAll();

            AddTooltip<TextTooltip>();

            // add the menu bar
            Add(menuBar = new ListPanel
            {
                Direction = Direction.LeftToRight,
                SizeMode = ListSizeMode.Static,

                ParentAnchor = AnchorMode.Top | AnchorMode.Horizontal,
                Size = new Vector2(Screen.UiSize.X, 0.12f),

                BackColor = UiColors.ControlBackground,
                Location = Vector2.Zero,
            });

            Control Separator() => new Control
            {
                Size = new Vector2(0.02f, 0.1f),
                //BackColor = Color.Black,
            };


            // content panels
            var content = new PanelContainer(game);

            var fullSizeBounds = new RectangleF(0, menuBar.Bottom, Width, Height - menuBar.Bottom);
            foreach (var p in content.FullSizePanels())
            {
                p.Bounds = fullSizeBounds;
                p.ParentAnchor = AnchorMode.All;
                p.IsVisible = false;

                Add(p);
            }

            var sideBounds = new RectangleF(0, menuBar.Bottom, 0.5f, Height - menuBar.Bottom);
            foreach (var p in content.SidePanels())
            {
                p.Bounds = sideBounds;
                p.ParentAnchor = AnchorMode.Left | AnchorMode.Vertical;
                p.IsVisible = false;

                Add(p);
            }

            void ActivateMenu(Control target)
            {
                foreach (var c in content.AllPanels())
                    c.IsVisible = (c == target) && !c.IsVisible;
            }


            // create the top action-bar
            void AddMenuButton(string tooltip, string icon, Color tint, Control menuToToggle)
            {
                const float MapButtonWidth = 0.1f;
                var btn = new Button
                {
                    Size = new Vector2(MapButtonWidth),
                    BackColor = UiColors.ControlBackground,
                    ToolTip = tooltip,

                    Padding = DefaultPadding,

                    SpriteSizeMode = TextureSizeMode.FitZoom,
                    IconName = icon,
                    Tint = tint,
                };

                btn.MouseClick += (o, e) => ActivateMenu(menuToToggle);

                menuBar.Add(btn);
            }

            menuBar.Add(Separator());

            // map edit
            AddMenuButton("Terrain", "peaks", Color.SandyBrown, content.Terrain);
            AddMenuButton("Objects", "gargoyle", Color.OrangeRed, content.Objects);
            AddMenuButton("Props", "stump-regrowth", Color.LightGoldenrodYellow, content.Props);

            menuBar.Add(Separator());

            // content edit
            AddMenuButton("Details", "tied-scroll", Color.DarkBlue, content.Scenario);
            AddMenuButton("Textures", "rolled-cloth", Color.MediumPurple, content.Textures);
            AddMenuButton("Animations", "throwing-ball", Color.LightSkyBlue, content.Animations);

            menuBar.Add(Separator());

            // history
            var historyButtons = new HistoryBar(game.History, Content.Icons);
            menuBar.AddRange(historyButtons.All());
        }


        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);

            camera.Update(msElapsed);
            game.Update(msElapsed);
        }

        public override void Draw(Canvas c)
        {
            base.Draw(c);

            Brushes.CurrentBrush.Draw(c);
        }


        async Task onRootAction(Control _, ClientActionArgs act)
        {
            if (act.Action == ClientAction.HideMenus)
            {
                await promptForQuit();
            }
        }

        async Task promptForQuit()
        {
            var hasUnsavedChanges = true;   // TODO

            // confirm, ask for save
            bool shouldQuit, shouldSave;
            if (hasUnsavedChanges)
            {
                var result = await ShowMessagePrompt(
                    "Quit", "Save changes to the scenario?",
                    MessageBoxButtons.Yes | MessageBoxButtons.No | MessageBoxButtons.Cancel,
                    true);
                shouldQuit = result != MessageBoxButtons.Cancel;
                shouldSave = result == MessageBoxButtons.Yes;
            }
            else
            {
                var result = await ShowMessagePrompt(
                    "Quit", "Quit to scenario selection?",
                    MessageBoxButtons.Yes | MessageBoxButtons.No,
                    true);
                shouldQuit = result == MessageBoxButtons.Yes;
                shouldSave = false;
            }

            if (shouldSave)
                scenario.SaveToDisk();

            if (shouldQuit)
                ViewStack.ResetToMain();
        }

    }
}

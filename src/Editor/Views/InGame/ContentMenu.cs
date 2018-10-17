using Shanism.Client.UI;
using Shanism.Editor.Controllers;
using Shanism.Editor.UI.InGame;
using System.Collections.Generic;

namespace Shanism.Editor.Views.InGame
{
    class PanelContainer
    {

        // map
        public TerrainPanel Terrain { get; }
        public ObjectsPanel Objects { get; }
        public PropsPanel Props { get; }


        // content
        public DetailsPanel Scenario { get; }
        public TexturePanel Textures { get; }
        public AnimationPanel Animations { get; }

        public PanelContainer(EditorGameState game)
        {
            Terrain = new TerrainPanel(game.TerrainEditor);
            Objects = new ObjectsPanel();
            Props = new PropsPanel();

            Scenario = new DetailsPanel(game.ContentEditor.Details);
            Textures = new TexturePanel(game.ContentEditor.Textures);
            Animations = new AnimationPanel(game.ContentEditor.Animations);
        }

        public IEnumerable<Control> AllPanels()
        {
            yield return Scenario;
            yield return Textures;
            yield return Animations;

            yield return Terrain;
            yield return Objects;
            yield return Props;
        }

        public IEnumerable<Control> FullSizePanels()
        {
            yield return Scenario;
            yield return Textures;
            yield return Animations;
        }

        public IEnumerable<Control> SidePanels()
        {
            yield return Terrain;
            yield return Objects;
            yield return Props;
        }
    }
}

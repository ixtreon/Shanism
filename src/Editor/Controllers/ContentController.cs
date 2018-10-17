using Shanism.Client;
using Shanism.Client.Assets;
using Shanism.Editor.Actions;
using Shanism.Editor.Controllers.Content;
using Shanism.ScenarioLib;

namespace Shanism.Editor.Controllers
{
    class ContentController
    {
        public TexturesController Textures { get; }
        public AnimationController Animations { get; }
        public DetailsController Details { get; }

        public ContentController(ActionList history, ScenarioConfig scenario, ContentList content)
        {
            Details = new DetailsController(history, scenario);
            Textures = new TexturesController(history, content);
            Animations = new AnimationController(history, content);
        }


    }
}

using Shanism.Client;
using Shanism.Client.Assets;
using Shanism.Editor.Actions;
using Shanism.Editor.Models.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Editor.Controllers.Content
{
    class AnimationController
    {

        readonly ActionList history;

        public PathTree<ShanoAnimation> Tree { get; }

        public AnimationController(ActionList history, ContentList content)
        {
            Tree = new PathTree<ShanoAnimation>();
            foreach(var anim in content.Animations)
                Tree.Add(anim.Name, anim);

            this.history = history;
            history.ActionDone += onHistoryAction;
            history.ActionUndone += onHistoryAction;
        }

        void onHistoryAction(ActionBase act)
        {

        }
    }
}

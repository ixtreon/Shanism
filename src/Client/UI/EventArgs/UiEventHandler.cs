using System;

namespace Shanism.Client.UI
{

    public delegate void UiEventHandler<T>(Control sender, T args)
        where T : EventArgs;

    public delegate bool CancellableUiEventHandler<T>(Control sender, T args)
        where T : EventArgs;
}

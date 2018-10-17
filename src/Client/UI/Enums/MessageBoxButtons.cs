using System;

namespace Shanism.Client.UI
{
    [Flags]
    public enum MessageBoxButtons
    {
        Yes = 1 << 0,
        No = 1 << 1,
        Ok = 1 << 2,
        Cancel = 1 << 3,
    }
}

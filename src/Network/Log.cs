using Ix.Logging;

namespace Shanism.Network
{
    static class NetLog
    {
        public static Log Default { get; private set; }

        public static void Init(string name = "network")
        {
            Default = new Log(name);
        }
    }
}

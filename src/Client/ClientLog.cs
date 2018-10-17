using Ix.Logging;

namespace Shanism.Client
{
    public static class ClientLog
    {
        public static Log Instance { get; private set; }

        public static void Init()
        {
            Instance = new Log("client.txt");
        }
    }
}

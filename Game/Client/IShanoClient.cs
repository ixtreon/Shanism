using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// A local game client. 
    /// </summary>
    public interface IShanoClient : IClient
    {
        void SetServer(IReceptor server);

        void Run();

        event Action GameLoaded;
    }
}

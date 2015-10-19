using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public interface IShanoClient : IClient
    {
        void SetReceptor(IReceptor server);

        void Run();

        event Action GameLoaded;
    }
}

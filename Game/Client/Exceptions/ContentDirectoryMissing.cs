using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Exceptions
{
    class ContentDirectoryMissingException : Exception
    {
        public override string Message => "Unable to find the default content directory. Please reinstall the game!";
    }
}

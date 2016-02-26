using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoEditor.ViewModels
{
    public interface IViewModel
    {
        Task Save();

        Task Reload();
    }
}

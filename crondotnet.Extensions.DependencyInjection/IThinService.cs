using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crondotnet
{
    public interface IThinService
    {
        Task Run(CancellationToken cancellationToken);
    }
}

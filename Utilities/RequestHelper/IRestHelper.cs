using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.RequestHelper
{
    public interface IRestHelper
    {
        T PostRequest<T>(string action, object data);
        T GetRequest<T>(string action);

        string PostRequest(string action, object data);
        string GetRequest(string action);
    }
}

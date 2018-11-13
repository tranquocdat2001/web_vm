using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Serialization
{
    public class TextSerializerForKeyCached : ITextSerializer
    {
        public T Deserialize<T>(TextReader reader)
        {
            throw new NotImplementedException();
        }

        public void Serialize<T>(TextWriter writer, T objectGraph)
        {
            throw new NotImplementedException();
        }
    }
}

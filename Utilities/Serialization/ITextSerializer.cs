using System.IO;

namespace Utilities
{
    public interface ITextSerializer
    {
        void Serialize<T>(TextWriter writer, T objectGraph);
        T Deserialize<T>(TextReader reader);
    }
}

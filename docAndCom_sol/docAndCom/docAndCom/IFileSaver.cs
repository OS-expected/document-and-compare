using System.IO;
using System.Threading.Tasks;

namespace docAndCom
{
    public interface IFileSaver
    {
        void SaveAsPdf(string filename, string contentType, MemoryStream stream);
    }
}

using System.IO;
using System.Threading.Tasks;

namespace docAndCom
{
    public interface IFileSaver
    {
        string SaveAsPdf(string filename, string contentType, MemoryStream stream);
    }
}

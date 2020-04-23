using docAndCom.Models;
using docAndCom.ViewModels;
using System.Collections.Generic;

namespace docAndCom
{
    public interface IFileSaver
    {
        string GeneratePdfFile(List<Photo> photos, string tag, string fileName, string mode);
    }
}

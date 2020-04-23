using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace docAndCom.ViewModels
{
    public class PdfViewModel
    {
        public FileStream Stream { get; set; }

        public string Path { get; set; }
    }
}

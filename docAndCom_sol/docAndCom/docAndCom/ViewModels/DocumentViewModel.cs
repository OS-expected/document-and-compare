using System;

namespace docAndCom.ViewModels
{
    public class DocumentViewModel
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string Path { get; set; }

        public string GeneratedOn { get; set; }

        public bool IsExisting { get; set; }
    }
}

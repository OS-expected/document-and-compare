using System;

namespace docAndCom.Models
{
    public class Document
    {
        public int Id { get; set; }

        public string Path { get; set; }

        public DateTime GeneratedOn { get; set; }
    }
}

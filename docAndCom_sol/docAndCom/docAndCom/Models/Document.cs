using SQLite;
using System;

namespace docAndCom.Models
{
    public class Document
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string FileName { get; set; }

        public string Path { get; set; }

        public DateTime GeneratedOn { get; set; }
    }
}

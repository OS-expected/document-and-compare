﻿using SQLite;
using System;

namespace docAndCom.Models
{
    class Photo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Path { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}

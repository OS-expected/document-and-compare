using SQLite;

namespace docAndCom.Models
{
    public class Preference
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}

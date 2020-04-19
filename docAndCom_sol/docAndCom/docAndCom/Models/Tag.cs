using SQLite;

namespace docAndCom
{
    class Tag
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}

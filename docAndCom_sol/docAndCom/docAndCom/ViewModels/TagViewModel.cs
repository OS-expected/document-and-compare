using docAndCom.Models;

namespace docAndCom.ViewModels
{
    class TagViewModel : Tag
    {
        public int PhotoCount { get; set; }

        public bool IsEmpty => PhotoCount > 0 ? false : true;
    }
}

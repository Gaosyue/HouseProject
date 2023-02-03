using System.Collections.Generic;

namespace House.Utils
{
    public class Menu
    {

        public int Id { get; set; }

        public int Pid { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string Icon { get; set; }

        public List<Menu> Children { get; set; }
    }
}
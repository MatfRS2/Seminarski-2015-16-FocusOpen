using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FocusOPEN.Website.Controls
{
    public class LookupItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public LookupItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

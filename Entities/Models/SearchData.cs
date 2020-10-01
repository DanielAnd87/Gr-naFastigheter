using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class SearchData
    {
        public string Searchterm { get; set; } = "";
        public string OrderBy { get; set; } = "";
        public bool TitleCheck { get; set; }
        public bool AdressCheck { get; set; }
        public bool TypeCheck { get; set; }
        public int CurrentPage { get; set; }

    }
}

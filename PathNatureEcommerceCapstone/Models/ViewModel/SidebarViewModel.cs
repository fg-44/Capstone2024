using PathNatureEcommerceCapstone.Models.DatabaseCapstone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PathNatureEcommerceCapstone.Models.ViewModel
{
    public class SidebarViewModel
    {
        public List<string> CategoryNames { get; set; }
        public int CategoryID { get; set; }
        public IEnumerable<Categories> ListOfCategories { get; set; }
    }
}

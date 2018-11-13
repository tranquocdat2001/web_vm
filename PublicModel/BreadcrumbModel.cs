using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicModel
{
    public class BreadcrumbModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public List<BreadcrumbModel> ListSub { get; set; }
        public int Priority { get; set; }
    }
}

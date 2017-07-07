using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinChallenge.PageNavigation
{

    public class MasterDetailPageDemoMenuItem
    {
        public MasterDetailPageDemoMenuItem()
        {
            TargetType = typeof(MasterDetailPageDemoDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
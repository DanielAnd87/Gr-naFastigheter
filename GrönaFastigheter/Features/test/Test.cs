using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrönaFastigheter.Features.test
{
    public class Test
    {


        public List<string> OrderAlternatives { get; set; } = new List<string> {
        "Hyra",
        "Säljpris",
        "Titel",
        "Adress"
       };
        private IEnumerable<RealEstate> realEstates;


        public void TestMeth()
        {
            //realEstates.OrderBy(x =>
            //{
            //    if (true)
            //    {
            //    string length = x.Title;
            //    return length;

            //    }else
            //    {
            //        return x.RentingPrice;
            //    }
            //});
        }
    }
}

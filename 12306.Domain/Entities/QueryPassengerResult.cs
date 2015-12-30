using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12306.Domain.Entities
{
    public class QueryPassengerResult
    {
        public object dj_passengers { get; set; }

        public string exMsg { get; set; }

        public bool isExist { get; set; }

        public ICollection<int> other_isOpenClick { get; set; }

        public ICollection<int> two_isOpenClick { get; set; }

        public ICollection<Passengers> normal_passengers { get; set; } 
    }
}

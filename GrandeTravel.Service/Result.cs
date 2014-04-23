using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Service
{
    public class Result<T> where T : class
    {
        public T Data { get; set; }
        public ResultEnum Status { get; set; }
    }
}

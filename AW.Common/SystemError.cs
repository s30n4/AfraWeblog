using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Common
{
    public class SystemError
    {
        public bool Success { get; set; }

        public int Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}

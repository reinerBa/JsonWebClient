using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JsonWebClient.Models
{
    public class ExampleObjects
    {
    }
    public class Abc { public int a { get; set; }
        public int b { get; set; }
        public int c { get; set; }
    }

    public class CBA { public int a { get; set; }
        public int b { get; set; }
        public int c { get; set; }
        public override string ToString()
        {
            return $"a:{a},b:{b},c:{c}";
        }
    }

}
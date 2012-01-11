using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hashzone.Infrastructure
{
    public class HashFunction
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }

        public HashFunction()
        {
        }

        public HashFunction(string displayName, string name)
        {
            DisplayName = displayName;
            Name = name;
        }
    }
}

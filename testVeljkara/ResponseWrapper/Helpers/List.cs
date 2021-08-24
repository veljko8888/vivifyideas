using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResponseWrapper.Helpers
{
    public class List : List<KeyValuePair<string, string>>
    {
        public List()
        {
        }

        public List(IEnumerable<KeyValuePair<string, string>> collection)
            : base(collection)
        {
        }

        public List(int capacity)
            : base(capacity)
        {

        }
    }
}

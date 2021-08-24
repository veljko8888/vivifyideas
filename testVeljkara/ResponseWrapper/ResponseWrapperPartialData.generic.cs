using System.Collections.Generic;

namespace ResponseWrapper
{
    public class ResponseWrapperPartialData<TResult> : ResponseWrapper<TResult>
    {
        public int Count { get; set; }
        public new List<TResult> Data { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testVeljkara.Helpers
{
    public class Result<T>
    {
        public T ResultObject { get; set; }
        public ResultStatus Status { get; set; }
        public string? Message { get; set; }
        public string? ActionName { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}

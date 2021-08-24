using System.Collections;
using System.Collections.Generic;

namespace ResponseWrapper
{
    public class ResponseWrapper<TResult> : ResponseWrapperBase
    {
        public ResponseWrapper()
            : base()
        {
        }

        public ResponseWrapper(string key, string error)
            : base(key, error)
        {
        }

        public ResponseWrapper(string error)
            : base(error)
        {
        }

        public ResponseWrapper(IEnumerable<string> errors)
            : base(errors)
        {
        }

        public ResponseWrapper(IEnumerable<KeyValuePair<string, string>> errors)
            : base(errors)
        {
        }

        public ResponseWrapper(TResult data)
            : base()
        {
            Data = data;
        }

        public TResult Data { get; set; }

        public bool HasData
        {
            get
            {
                if (Data != null)
                {
                    if (typeof(TResult).GetInterface(typeof(IEnumerable).Name) == null)
                    {
                        return true;
                    }

                    foreach (var i in Data as IEnumerable)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public static ResponseWrapper<TResult> Success()
        {
            return new ResponseWrapper<TResult>();
        }

        public static ResponseWrapper<TResult> Success(TResult data)
        {
            return new ResponseWrapper<TResult>(data);
        }

        public static ResponseWrapper<TResult> Error(string key, string error)
        {
            return new ResponseWrapper<TResult>(key, error);
        }

        public static ResponseWrapper<TResult> Error(string error)
        {
            return new ResponseWrapper<TResult>(error);
        }

        public static ResponseWrapper<TResult> Error(IEnumerable<string> errors)
        {
            return new ResponseWrapper<TResult>(errors);
        }

        public static ResponseWrapper<TResult> Error(IEnumerable<KeyValuePair<string, string>> errors)
        {
            return new ResponseWrapper<TResult>(errors);
        }

        public static ResponseWrapper<TResult> PermissionDenied()
        {
            return new ResponseWrapper<TResult>("Permission denied!");
        }

        public static ResponseWrapper<TResult> DisablePropertySave()
        {
            return new ResponseWrapper<TResult>("Please create Property Commissions first");
        }

        public static ResponseWrapper<TResult> InvalidDatesForCommission()
        {
            return new ResponseWrapper<TResult>("Please enter valid dates for Commission");
        }

        public static ResponseWrapper<TResult> RecordDeleted()
        {
            return new ResponseWrapper<TResult>("Record does not exist!");
        }

        public static ResponseWrapper<TResult> InvalidOperation()
        {
            return new ResponseWrapper<TResult>("Invalid operation!");
        }

        public static ResponseWrapper<TResult> WrongData()
        {
            return new ResponseWrapper<TResult>("Data not available!");
        }
        
       
    }
}
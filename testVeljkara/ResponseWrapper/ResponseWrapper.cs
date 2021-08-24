using System.Collections.Generic;

namespace ResponseWrapper
{
    public class ResponseWrapper : ResponseWrapperBase
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

        public static ResponseWrapper Success()
        {
            return new ResponseWrapper();
        }

        public static ResponseWrapper Error(string key, string error)
        {
            return new ResponseWrapper(key, error);
        }

        public static ResponseWrapper Error(string error)
        {
            return new ResponseWrapper(error);
        }

        public static ResponseWrapper Error(IEnumerable<string> errors)
        {
            return new ResponseWrapper(errors);
        }

        public static ResponseWrapper Error(IEnumerable<KeyValuePair<string, string>> errors)
        {
            return new ResponseWrapper(errors);
        }

        public static ResponseWrapper PermissionDenied()
        {
            return new ResponseWrapper("Permission denied!");
        }

        public static ResponseWrapper InvalidOperation()
        {
            return new ResponseWrapper("Invalid operation!");
        }

        public static ResponseWrapper WrongData()
        {
            return new ResponseWrapper("Data not available!");
        }
    }
}

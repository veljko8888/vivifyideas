using ResponseWrapper.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace ResponseWrapper
{
    public abstract class ResponseWrapperBase
    {
        protected ResponseWrapperBase()
        {
            Errors = new List();
        }

        protected ResponseWrapperBase(string key, string error)
            : this()
        {
            AddError(key, error);
        }

        protected ResponseWrapperBase(string error)
            : this(string.Empty, error)
        {
        }

        protected ResponseWrapperBase(IEnumerable<string> errors)
            : this()
        {
            AddErrors(errors);
        }

        protected ResponseWrapperBase(IEnumerable<KeyValuePair<string, string>> errors)
            : this()
        {
            AddErrors(errors);
        }

        public List Errors { get; set; }

        public List<string> ErrorMessages => Errors.Select(e => e.Value).ToList();

        public bool HasError { get { return Errors.Any(); } }

        public bool IsSuccess { get { return !HasError; } }

        public void AddError(string key, string error)
        {
            Errors.Add(new KeyValuePair<string, string>(key, error));
        }

        public void AddError(string error)
        {
            Errors.Add(new KeyValuePair<string, string>(string.Empty, error));
        }

        public void AddErrors(IEnumerable<string> errors)
        {
            Errors.AddRange(errors.Select(i => new KeyValuePair<string, string>(string.Empty, i)));
        }

        public void AddErrors(IEnumerable<KeyValuePair<string, string>> errors)
        {
            Errors.AddRange(errors);
        }

        public void AddErrors(string prefixKey, IEnumerable<KeyValuePair<string, string>> errors)
        {
            if (string.IsNullOrEmpty(prefixKey))
            {
                prefixKey += ".";
            }
            else
            {
                prefixKey = string.Empty;
            }

            if(errors != null)
                Errors.AddRange(errors.Select(i => new KeyValuePair<string, string>($"{prefixKey}{i.Key}", i.Value)));
        }
    }
}

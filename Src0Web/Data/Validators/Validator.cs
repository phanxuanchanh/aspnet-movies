using System.Collections.Generic;

namespace Data.Validators
{
    public class Validator
    {
        public List<string> _errors;

        public Validator() { _errors = new List<string>(); }

        public bool IsValid() { return _errors.Count == 0; }

        public List<string> GetErrors() { return _errors; }

        public void AddError(string error) { _errors.Add(error); }
    }
}
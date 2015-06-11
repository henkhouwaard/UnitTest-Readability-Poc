using System;

using UnitTestReadabilityPoc.Interfaces;

namespace UnitTestReadabilityPoc.SystemsUnderTest
{
    public class Controller
    {
        private readonly IRepository _repository;

        public const string SUFFIX = "controllersuffix";

        public Controller(IRepository repository)
        {
            _repository = repository;
        }

        public void SaveString(string stringToSave)
        {
            _repository.SaveString(stringToSave);
        }

        public string GetStringById(int id)
        {
            string stringFromRepo = _repository.GetStringById(id);
            return String.Concat(stringFromRepo, SUFFIX);
        }
    }
}
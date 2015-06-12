using System;

using UnitTestReadabilityPoc.Interfaces;

namespace UnitTestReadabilityPoc.SystemsUnderTest
{
    public class Controller
    {
        private readonly IRepository _repository;
        private readonly IDateProvider _dateProvider;

        public const string SUFFIX = "controllersuffix";

        public Controller(IRepository repository, IDateProvider dateProvider)
        {
            _repository = repository;
            _dateProvider = dateProvider;
        }

        public void SaveString(string stringToSave)
        {
            _repository.SaveString(stringToSave);
        }

        public string GetStringById(int id)
        {
            string stringFromRepo = _repository.GetStringById(id);
            if(stringFromRepo == null)
            {
                throw new Exception();
            }
            return String.Concat(stringFromRepo, SUFFIX);
        }
    }
}
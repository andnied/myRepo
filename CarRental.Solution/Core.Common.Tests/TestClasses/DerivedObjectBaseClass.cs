using Core.Common.Core;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Tests.TestClasses
{
    public class DerivedObjectBaseClass : ObjectBase
    {
        private string _testProperty;

        public string TestProperty
        {
            get { return _testProperty; }
            set
            {
                _testProperty = value;
                OnPropertyChanged(() => TestProperty);
            }
        }

        protected override IValidator GetValidator()
        {
            return new DerivedObjectBaseClassValidator();
        }

        class DerivedObjectBaseClassValidator : AbstractValidator<DerivedObjectBaseClass>
        {
            public DerivedObjectBaseClassValidator()
            {
                RuleFor(o => o.TestProperty).NotEmpty();
            }
        }
    }
}

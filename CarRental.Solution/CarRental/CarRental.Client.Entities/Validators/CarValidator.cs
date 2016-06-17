using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Entities.Validators
{
    class CarValidator : AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(obj => obj.Description).NotEmpty();
            RuleFor(obj => obj.Color).NotEmpty();
            RuleFor(obj => obj.RentalPrice).GreaterThan(0);
            RuleFor(obj => obj.Year).GreaterThan(2000).LessThanOrEqualTo(DateTime.Now.Year + 1);
        }
    }
}

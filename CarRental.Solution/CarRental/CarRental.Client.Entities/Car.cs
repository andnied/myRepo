using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using CarRental.Client.Entities.Validators;

namespace CarRental.Client.Entities
{
    public class Car : ObjectBase
    {
        #region Fields

        private int _carId;
        private string _description;
        private string _color;
        private int _year;
        private decimal _rentalPrice;
        private bool _currentlyRented;

        #endregion

        #region Props

        public int CarId
        {
            get { return _carId; }
            set
            {
                _carId = value;
                OnPropertyChanged(() => CarId);
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(() => Description);
            }
        }

        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged(() => Color);
            }
        }

        public int Year
        {
            get { return _year; }
            set
            {
                _year = value;
                OnPropertyChanged(() => Year);
            }
        }

        public decimal RentalPrice
        {
            get { return _rentalPrice; }
            set
            {
                _rentalPrice = value;
                OnPropertyChanged(() => RentalPrice);
            }
        }

        public bool CurrentlyRented
        {
            get { return _currentlyRented; }
            set
            {
                _currentlyRented = value;
                OnPropertyChanged(() => CurrentlyRented);
            }
        }

        #endregion

        protected override IValidator GetValidator()
        {
            return new CarValidator();
        }
    }
}

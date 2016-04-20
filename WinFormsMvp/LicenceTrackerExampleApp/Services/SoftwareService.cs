﻿using LicenceTracker.Db;
using LicenceTracker.Entities;
using System;
using System.Linq;

namespace LicenceTracker.Services
{
    public class SoftwareService : ISoftwareService, IDisposable
    {
        private readonly LicenceTrackerContext licenceTrackerContext;
        private bool _disposed = false;

        public SoftwareService()
        {
            licenceTrackerContext = new LicenceTrackerContext();
        }

        public Software AddNewProduct(Software product)
        {
            var newProduct = licenceTrackerContext.SoftwareProducts.Add(product);
            licenceTrackerContext.SaveChanges();
            return newProduct;
        }

        public IQueryable<SoftwareType> GetSoftwareTypes()
        {
            return licenceTrackerContext.SoftwareTypes;
        }


        public SoftwareType AddSoftwareType(SoftwareType softwareType)
        {
            var newType = licenceTrackerContext.SoftwareTypes.Add(softwareType);
            licenceTrackerContext.SaveChanges();
            return newType;
        }

        public Person AddNewPerson(Person person)
        {
            var newType = licenceTrackerContext.People.Add(person);
            licenceTrackerContext.SaveChanges();
            return newType;
        }

        public void Dispose()
        {
            CleanUp(true);
            GC.SuppressFinalize(this);
        }
        private void CleanUp(bool disposing)
        {
            // Be sure we have not already been disposed!
            if (!_disposed)
            {
                if (disposing)
                {
                    licenceTrackerContext.Dispose();
                }
            }
            _disposed = true;
        }
    }
}

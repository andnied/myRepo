using System;
using System.Collections.Generic;
using System.Linq;
using ComplaintTool.Models;

namespace ComplaintTool.Common.Config
{
    public class ComplaintDictionaires
    {
        private ComplaintDictionaires() { }

        private IReadOnlyDictionary<string, Mapping> _mappings;
        private IReadOnlyDictionary<string, MappingValue> _mappingValues;
        private IReadOnlyDictionary<short, CurrencyCode> _currencyCodes;

        public IReadOnlyDictionary<string, Mapping> Mappings
        {
            get { return _mappings ?? (_mappings = GetMappings()); }
        }

        public IReadOnlyDictionary<string, MappingValue> MappingValues
        {
            get { return _mappingValues ?? (_mappingValues = GetMappingValues()); }
        }

        public IReadOnlyDictionary<short, CurrencyCode> CurrencyCodes
        {
            get { return _currencyCodes ?? (_currencyCodes = GetCurrencyCode()); }
        }

        #region LoadDictionaries

        private IReadOnlyDictionary<string, Mapping> GetMappings()
        {
            using(var entities=ComplaintConfig.Instance.CreateEntities())
            {
                return entities.Mappings.OrderBy(x => x.Key).ToDictionary(x=>(x.Key??string.Empty)+(x.Value??string.Empty));
            }
        }

        private IReadOnlyDictionary<string, MappingValue> GetMappingValues()
        {
            using (var entities = ComplaintConfig.Instance.CreateEntities())
            {
                return entities.MappingValues.OrderBy(x => x.Field).ToDictionary(x => (x.Field ?? string.Empty) + (x.BaseValue ?? string.Empty));
            }
        }

        public IReadOnlyDictionary<short, CurrencyCode> GetCurrencyCode()
        {
            using (var entities = ComplaintConfig.Instance.CreateEntities())
            {
                return entities.View_CurrencyCode
                    .ToList()
                    .Select(x => new CurrencyCode
                    {
                        Alphabetical = x.Alphabetical,
                        Numeric = short.Parse(x.Numeric),
                        DecimalPrecision = x.DecimalPrecision,
                        Name = x.Name
                    })
                    .ToDictionary(x => x.Numeric);
            }
        }

        #endregion

        #region Singleton

        private static volatile ComplaintDictionaires _instance;
        private static readonly object SyncRoot = new object();

        public static ComplaintDictionaires Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ComplaintDictionaires();
                }

                return _instance;
            }
        }

        public static void Reset()
        {
            lock (SyncRoot)
            {
                _instance = null;
            }
        }

        #endregion

        #region Static Method

        public static string GetMapping(string mappingKey,string Value)
        {
            string key = (mappingKey ?? string.Empty) + (Value ?? string.Empty);

            if (!Instance.Mappings.ContainsKey(key))
                return String.Empty;

            return Instance.Mappings[key].ReturnValue;
        }

        public static string GetMappingValue(string objectName,string baseValue)
        {
            string key = (objectName ?? string.Empty) + (baseValue ?? string.Empty);

            if (!Instance.MappingValues.ContainsKey(key))
                return " ";

            return Instance.MappingValues[key].MappingValue1;
        }
        #endregion
    }

}

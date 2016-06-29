using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using ComplaintTool.Common.Utils;
using  ComplaintTool.Models;

namespace ComplaintTool.Common.Config
{
    public class ComplaintConfig
    {
        #region Fields

        public const string ProviderName = "System.Data.SqlClient";
        public const string ConfigFileName = "ComplaintTool.config";
        public const string ConfigRegistryName = "ComplaintConfig";
        public const bool MultipleActiveResultSets = true;

        public static bool StoreConfigInFile
        {
            get
            {
                return File.Exists(FileName);
            }
        }

        public static string FileName
        {
            get 
            { 
                return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ConfigFileName); 
            }
        }

        private IReadOnlyDictionary<string, Parameter> _parameters;
        public IReadOnlyDictionary<string, Parameter> Parameters
        {
            get { return _parameters ?? (_parameters = GetParameters()); }
        }

        private IReadOnlyDictionary<int, NotificationDefinition> _notifications;
        public IReadOnlyDictionary<int, NotificationDefinition> Notifications
        {
            get { return _notifications ?? (_notifications = GetNotifications()); }
        }

        private IReadOnlyDictionary<int, FilesEndPointDefinition> _endPointDefinitions;
        public IReadOnlyDictionary<int, FilesEndPointDefinition> EndPointDefinitions
        {
            get { return _endPointDefinitions ?? (_endPointDefinitions = GetFilesEndPointDefinitions()); }
        }

        private IReadOnlyDictionary<int, StageDefinition> _stageDefinitions;
        public IReadOnlyDictionary<int, StageDefinition> StageDefinitions
        {
            get { return _stageDefinitions ?? (_stageDefinitions = GetStageDefinitions()); }
        }

        private IReadOnlyDictionary<int, StageMappingIncoming> _stageMappingIncoming;
        public IReadOnlyDictionary<int, StageMappingIncoming> StageMappingIncoming
        {
            get { return _stageMappingIncoming ?? (_stageMappingIncoming = GetStageMappingIncoming()); }
        }

        private IReadOnlyDictionary<int, StageMappingOutgoing> _stageMappingOutgoing;
        public IReadOnlyDictionary<int, StageMappingOutgoing> StageMappingOutgoing
        {
            get { return _stageMappingOutgoing ?? (_stageMappingOutgoing = GetStageMappingOutgoing()); }
        }

        private IReadOnlyDictionary<string, OrganizationBrand> _organizationBrand;
        public IReadOnlyDictionary<string, OrganizationBrand> OrganizationBrand
        {
            get { return _organizationBrand ?? (_organizationBrand = GetOrganizationBrands()); }
        }

        public IConfig Conf { get; private set; }

        public bool? IsEcho { get; set; }

        #endregion

        #region Load ConnectionString from Config

        public string GetEntityConnectionString()
        {
            var entityBuilder = GetEntityConnectionStringBuilder();
            entityBuilder.Metadata = @"res://*/ComplaintModel.csdl|
                            res://*/ComplaintModel.ssdl|
                            res://*/ComplaintModel.msl";
            return entityBuilder.ToString();
        }

//        internal string GetInternalEntityConnectionString()
//        {
//            var entityBuilder = GetEntityConnectionStringBuilder();
//            entityBuilder.Metadata = @"res://*/ComplaintModel.csdl|
//                            res://*/ComplaintModel.ssdl|
//                            res://*/ComplaintModel.msl";
//            return entityBuilder.ToString();
//        }

        public string GetConnectionString()
        {
            if (Conf == null)
                throw new InvalidOperationException("Config not found!");

            var sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = Conf.ServerName,
                InitialCatalog = Conf.DatabaseName,
                ApplicationName = "ComplaintTool.Shell"
            };

            if (Conf.IntegratedSecurity)
                sqlBuilder.IntegratedSecurity = Conf.IntegratedSecurity;
            else
            {
                sqlBuilder.IntegratedSecurity = false;
                sqlBuilder.UserID = Conf.UserID;
                sqlBuilder.Password = Encryption.Decrypt(Conf.Password);
            }
            sqlBuilder.ConnectTimeout = Conf.ConnectionTimeout;
            sqlBuilder.MultipleActiveResultSets = MultipleActiveResultSets;
            return sqlBuilder.ToString();
        }

        private EntityConnectionStringBuilder GetEntityConnectionStringBuilder()
        {
            return new EntityConnectionStringBuilder
            {
                Provider = ProviderName,
                ProviderConnectionString = GetConnectionString()
            };
        } 

        #endregion

        #region Load Configuration From DB

        private IReadOnlyDictionary<string, Parameter> GetParameters()
        {
            try
            {
                using (var entities = CreateEntities())
                {
                    return entities.Parameters.ToDictionary(x => x.ParameterName);
                }
            }
            catch (Exception ex)
            {
                throw new ComplaintConfigLoadingException("Parameters", ex);
            }
        }

        private IReadOnlyDictionary<int, NotificationDefinition> GetNotifications()
        {
            try
            {
                using (var entities = CreateEntities())
                {
                    return entities.NotificationDefinitions
                        .OrderBy(x => x.MessageEventNamber)
                        .ToDictionary(x => x.MessageEventNamber);
                }
            }
            catch (Exception ex)
            {
                throw new ComplaintConfigLoadingException("NotificationDefinitions", ex);
            }            
        }

        private IReadOnlyDictionary<int, FilesEndPointDefinition> GetFilesEndPointDefinitions()
        {
            try
            {
                using (var entities = CreateEntities())
                {
                    return entities.FilesEndPointDefinitions
                        .ToDictionary(x => x.DefinitionId);
                }
            }
            catch (Exception ex)
            {
                throw new ComplaintConfigLoadingException("FilesEndPointDefinitions", ex);
            }
        }

        private IReadOnlyDictionary<int, StageDefinition> GetStageDefinitions()
        {
            try
            {
                using (var entities = CreateEntities())
                {
                    return entities.StageDefinitions
                        .ToDictionary(x => x.StageId);
                }
            }
            catch (Exception ex)
            {
                throw new ComplaintConfigLoadingException("StageDefinitions", ex);
            }
        }

        private IReadOnlyDictionary<int, StageMappingIncoming> GetStageMappingIncoming()
        {
            try
            {
                using (var entities = CreateEntities())
                {
                    return entities.StageMappingIncomings
                        .ToDictionary(x => x.StageMappingId);
                }
            }
            catch (Exception ex)
            {
                throw new ComplaintConfigLoadingException("StageMappingIncoming", ex);
            }
        }

        private IReadOnlyDictionary<int, StageMappingOutgoing> GetStageMappingOutgoing()
        {
            try
            {
                using (var entities = CreateEntities())
                {
                    return entities.StageMappingOutgoings
                        .ToDictionary(x => x.StageMappingId);
                }
            }
            catch (Exception ex)
            {
                throw new ComplaintConfigLoadingException("StageMappingOutgoing", ex);
            }
        }

        private IReadOnlyDictionary<string, OrganizationBrand> GetOrganizationBrands()
        {
            try
            {
                using (var entities = CreateEntities())
                {
                    return entities.OrganizationBrands.ToDictionary(x => x.BrandOffset);
                }
            }
            catch (Exception ex)
            {
                throw new ComplaintConfigLoadingException("OrganizationBrands", ex);
            }
        }

        public ComplaintEntities CreateEntities()
        {
            return Conf != null ? new ComplaintEntities(GetEntityConnectionString()) : new ComplaintEntities();
        }

        #endregion

        #region Singleton

        private static volatile ComplaintConfig _instance;
        private static readonly object SyncRoot = new object();

        private ComplaintConfig()
        {
            GetConfig();
        }

        public static ComplaintConfig Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ComplaintConfig();
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

        #region Static Helpers

        public static string GetParameter(string paramKey)
        {
            return Instance.Parameters[paramKey].ParameterValue;
        }

        public static void SetConfig(IConfig conf, bool deleteIfExists = true)
        {
            if (conf is XmlConfig)
            {
                string fileName = FileName;
                if (deleteIfExists && File.Exists(fileName)) File.Delete(fileName);
                XmlUtil.SerializeToFile(conf, fileName);               
            }
            else if (conf is RegistryConfig)
            {
                RegistryUtil.SetObject(conf, ConfigRegistryName);
            }
            Instance.Conf = conf;
        }

        public static string ReadConfigAsText()
        {
            string fileName = FileName;
            if (File.Exists(fileName))
                return File.ReadAllText(fileName);
            else
            {
                var conf = RegistryUtil.GetObject<RegistryConfig>(ConfigRegistryName);
                return conf.ToString();
            }
        }

        private void GetConfig()
        {
            try
            {
                if (StoreConfigInFile)
                    Conf = XmlUtil.DeserializeFromFile<XmlConfig>(FileName, false);
                else
                    Conf = RegistryUtil.GetObject<RegistryConfig>(ConfigRegistryName);
            }
            catch (Exception ex)
            {
                throw new ComplaintConfigLoadingException("Xml", ex);
            }
        }

        #endregion
    }
}

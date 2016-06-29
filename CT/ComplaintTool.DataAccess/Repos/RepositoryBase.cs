using ComplaintTool.Models;
using eService.MCParser.Model;
using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using Organization = ComplaintTool.Common.Enum.Organization;

namespace ComplaintTool.DataAccess.Repos
{
    public abstract class RepositoryBase : IRepositoryBase
    {
        private readonly DbContext _context;

        protected RepositoryBase(DbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;
        }

        public void Update<T>(T entity) where T : class
        {
            GetDbSet<T>().Attach(entity);
            SetEntityState(entity, EntityState.Modified);
        }

        protected void SetEntityState(object entity, EntityState entityState)
        {
            _context.Entry(entity).State = entityState;
        }

        protected DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        protected TContext GetContext<TContext>() where TContext : DbContext
        {
            return (TContext)_context;
        }

        protected virtual int Commit()
        {
            if (_context.Database.CurrentTransaction == null)
                return _context.SaveChanges();
            return -1;
        }

        protected virtual string GetCurrentUser()
        {
            var identity = WindowsIdentity.GetCurrent();
            return identity != null ? identity.Name : "ComplaintServices";
        }

        protected virtual DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow;
        }

        #region Functions

        public decimal? CalculateExchangeSettlement(decimal currencyValue1, string currencyCode1, string currencyCode2,
            Organization organization)
        {
            object[] parameters =
                    {
                        new SqlParameter("currencyValue1", currencyValue1),
                        new SqlParameter("currencyCode1", currencyCode1),
                        new SqlParameter("currencyCode2", currencyCode2),
                        new SqlParameter("organizationId", organization.ToString())
                    };

            return _context.Database.SqlQuery<decimal?>(
                    "SELECT [dbo].ExchangeSettlementCalculator(@currencyValue1, @currencyCode1, @currencyCode2, @organizationId) ", parameters)
                    .FirstOrDefault();
        }

        #endregion

        #region Stored Procedures

        protected virtual DbCommand CreateStoredProcedure(string spName)
        {
            // workaround dla wspóldzielenia transakcji pomiedzy entity i procedurami składowanymi
            var command = _context.Database.Connection.CreateCommand();
            if (_context.Database.CurrentTransaction != null)
                command.Transaction = _context.Database.CurrentTransaction.UnderlyingTransaction;
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }

        public void AddIncomingFile(Guid fileStreamId, byte[] fileStream, string fileName)
        {
            AddStreamFile("[File].[AddIncomingFile]", fileStreamId, fileStream, fileName);
        }

        public void AddOutgoingFile(Guid fileStreamId, byte[] fileStream, string fileName)
        {
            AddStreamFile("[File].[AddOutgoingFile]", fileStreamId, fileStream, fileName);
        }

        public Guid AddDocument(byte[] fileStream, string fileName)
        {
            var fileStreamId = Guid.NewGuid();
            AddDocument(fileStreamId, fileStream, fileName);
            return fileStreamId;
        }

        public void AddDocument(Guid fileStreamId, byte[] fileStream, string fileName)
        {
            AddStreamFile("[File].[AddDocument]", fileStreamId, fileStream, fileName);
        }

        public object GetOrgIncomingFile(int fileId)
        {
            return GetOrgIncomingFile("[File].[GetOrgIncomingFile]", fileId);
        }

        public string InsertOrgIncomingTranVISA(IncomingRecordVisaMid record, int processingMode)
        {
            #region Parameters

            return InsertOrgIncomingTranVISA("[File].[InsertOrgIncomingTranVISA]",
                            record.BIN,
                            record.ARN,
                            record.PAN,
                            record.PANExtention,
                            record.AuthorisationCode,
                            record.IncomingDate,
                            record.SettlementDate,
                            record.TransactionCode,
                            record.TransactionDate,
                            record.OriginalTransactionAmount,
                            record.OriginalTransactionCurrencyCode,
                            record.BookingAmountSign,
                            record.BookingAmount,
                            record.BookingAmountExponent,
                            record.BookingCurrencyCode,
                            record.StageAmountSign,
                            record.StageAmount,
                            record.StageAmountExponent,
                            record.StageCurrencyCode,
                            record.MerchantName,
                            record.MerchantCity,
                            record.MerchantCountryCode,
                            record.MCC,
                            record.UsageCode,
                            record.RrId,
                            record.CentralProcessingDate,
                            record.TransactionId,
                            record.KkoCbReference,
                            record.TID,
                            record.MOTOECI,
                            record.DestinationBIN,
                            record.SourceBIN,
                            record.ReasonCode,
                            record.MemberMessageText,
                            record.MessageText,
                            record.PartialFlag,
                            record.ReturnReasonCode1,
                            record.ReturnReasonCode2,
                            record.ReturnReasonCode3,
                            record.ReturnReasonCode4,
                            record.ReturnReasonCode5,
                            record.Brand,
                            record.Narritive,
                            record.TransactionAmountSign,
                            record.TransactionAmount,
                            record.TransactionAmountExponent,
                            record.TransactionCurrencyCode,
                            record.Stage,
                            record.StageDate,
                            record.CVVFlag,
                            record.CVCFlag,
                            record.ECommerce,
                            record.MPILogFlag,
                            record.CardAcceptorIDCode,
                            record.MessageType,
                            record.TransactionDateTimeLocal,
                            record.GiccMCC,
                            record.GiccDomesticMCC,
                            record.POSEntryMode,
                            record.DocumentationIndicator,
                            record.PosTerminalType,
                            record.PosCardDataInputMode,
                            record.PosCardPresent,
                            record.PosCardholderPresent,
                            record.PosCardholderAuthMethod,
                            record.ExpiryDate,
                            record.GiccRevDate,
                            record.SettleAmountImpact,
                            record.TranAmountReq,
                            record.PostID,
                            record.PrevPostTranId,
                            record.MultipleClearingSeqNr,
                            record.MultipleClearingSeqCnt,
                            record.AuthorizationSourceCode,
                            record.AVSResponseCode,
                            record.MarketSpecAuth,
                            record.AuthorizationResponseCode,
                            record.PosCardholderAuthEntity,
                            record.PosCardDataOutputAbility,
                            record.SystemTraceAuditNr,
                            record.DatetimeTranLocal,
                            record.MerchantType,
                            record.CardSeqNr,
                            record.PosConditionCode,
                            record.RetrievalReferenceNr,
                            record.AuthIDRsp,
                            record.RspCodeRsp,
                            record.ServiceRestrictionCode,
                            record.TerminalID,
                            record.CardAcceptorNameLoc,
                            record.PosCardDataInputAbility,
                            record.PosCardholderAuthAbility,
                            record.PosCardCaptureAbility,
                            record.PosOperatingEnvironment,
                            record.PosTerminalOutputAbility,
                            record.PosPinCaptureAbility,
                            record.PosTerminalOperator,
                            record.UcafData,
                            record.TranAmountRsp,
                            record.TranType,
                            record.BusinessDate,
                            record.FileId,
                            record.SourceCountryCode,
                            record.PrevMessageType,
                            record.PrevUcafData,
                            record.StructuredDataReq,
                            record.DatetimeRsp,
                            processingMode,
                            record.ParticipantId).ToString();

            #endregion
        }

        protected string InsertOrgIncomingTranMASTERCARD(BlkModel record, int processingMode)
        {
            object result;

            using (var cmd = CreateStoredProcedure("[File].[InsertOrgIncomingTranMASTERCARD]"))
            {
                foreach (var prop in record.GetType().GetProperties())
                {
                    var definition = (McModelAttribute)prop.GetCustomAttribute(typeof(McModelAttribute));
                    if (definition != null)
                        cmd.Parameters.Add(GenerateParameter("@" + definition.Name, definition.Type, ParameterDirection.Input, prop.GetValue(record), definition.Size));
                }

                cmd.Parameters.Add(GenerateParameter("@ProcessingMode", SqlDbType.Int, ParameterDirection.Input, processingMode));
                cmd.CommandTimeout = 180;
                result = cmd.ExecuteScalar();
            }

            return result.ToString();
        }

        private void AddStreamFile(string procName, Guid fileStreamId, byte[] fileStream, string fileName)
        {
            using (var command = CreateStoredProcedure(procName))
            {
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@file_stream_id",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    Value = fileStreamId
                });
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@file_stream",
                    SqlDbType = SqlDbType.VarBinary,
                    Direction = ParameterDirection.Input,
                    Value = fileStream
                });
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@file_name",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Value = fileName
                });
                command.ExecuteNonQuery();
            }
        }

        public void AddDocumentToComplaint(string caseId, long? stageId, Guid streamId, string fileName, string sourceFileName, string sourceIncoming, string description, bool? incoming, bool? manual, DateTimeOffset? insertDate, string insertUser)
        {
            using (var command = CreateStoredProcedure("[File].[AddDocumentToComplaint]"))
            {
                #region PARAMETRY KOMENDY
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@CaseId",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    Value = caseId
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@StageId",
                    SqlDbType = SqlDbType.BigInt,
                    Direction = ParameterDirection.Input,
                    Value = stageId ?? (object)DBNull.Value
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@stream_id",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    Value = streamId
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FileName",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    Value = fileName
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@SourceFileName",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    Value = sourceFileName
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@SourceIncoming",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    Value = sourceIncoming
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Description",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    Value = description ?? (object)DBNull.Value
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Incoming",
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    Value = incoming
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Manual",
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    Value = manual
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@InsertDate",
                    SqlDbType = SqlDbType.DateTimeOffset,
                    Direction = ParameterDirection.Input,
                    Value = insertDate.HasValue ? insertDate : (object)DBNull.Value
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    Value = insertUser ?? (object)DBNull.Value
                });
                #endregion

                command.ExecuteNonQuery();
            }
        }

        protected void GetDocument(long docId, out string fileName, out byte[] fileStream)
        {
            using (var cmd = CreateStoredProcedure("[Case].[GetDocument]"))
            {
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@DocumentId",
                    SqlDbType = SqlDbType.BigInt,
                    Direction = ParameterDirection.Input,
                    Value = docId
                });
                var fileNameParam = new SqlParameter
                {
                    ParameterName = "@file_name",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 255,
                    Direction = ParameterDirection.Output
                };
                var fileStreamParam = new SqlParameter
                {
                    ParameterName = "@file_stream",
                    SqlDbType = SqlDbType.VarBinary,
                    Size = -1,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(fileNameParam);
                cmd.Parameters.Add(fileStreamParam);
                cmd.ExecuteNonQuery();

                fileName = fileNameParam.Value.ToString();
                fileStream = (byte[])fileStreamParam.Value;
            }
        }

        private object GetOrgIncomingFile(string procName, int fileId)
        {
            object result;

            using (var command = CreateStoredProcedure(procName))
            {
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@FileId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = fileId
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@file_stream",
                    SqlDbType = SqlDbType.VarBinary,
                    Size = (int)(Math.Pow(2, 31) - 1),
                    Direction = ParameterDirection.Output
                });
                
                command.ExecuteNonQuery();
                result = command.Parameters["@file_stream"].Value == DBNull.Value ? null : command.Parameters["@file_stream"].Value;
            }

            return result;
        }

        private object InsertOrgIncomingTranVISA(string procName, string bIN, string aRN, string pAN, string pANExtention, string aCode, string incomingDate, string settlementDate, string transactionCode, string transactionDate, string originalTransactionAmount, string originalTransactionCurrencyCode, string bookingAmountSign, string bookingAmount, string bookingAmountExponent, string bookingCurrencyCode, string stageAmountSign, string stageAmount, string stageAmountExponent, string stageCurrencyCode, string merchantName, string merchantCity, string merchantCountryCode, string mCC, string usageCode, string rId, string centralProcessingDate, string transactionId, string kKOCbReference, string tID, string mOTOECI, string destinationBin, string sourceBin, string reasonCode, string memberMessageText, string messageText, string partialFlag, string returnReasonCode1, string returnReasonCode2, string returnReasonCode3, string returnReasonCode4, string returnReasonCode5, string brand, string narritive, string transactionAmountSign, string transactionAmount, string transactionAmountExponent, string transactionCurrencyCode, string stage, string stageDate, string cVVFlag, string cVCFlag, string eCommerce, string mPILogFlag, string cardAcceptorIdCode, string messageType, string transactionDateTimeLocal, string giccMCC, string giccDomesticMCC, string pOSEntryMode, string documentationIndicator, string posTerminalType, string posCardDataImputMode, string posCardPresent, string posCardholderPresent, string posCardholderAuthMethod, string expiryDate, string giccRevDate, string settleAmountImpact, string tranAmountReq, string postTranId, string prevPostTranId, string multiClearingSeqNr, string multiClearingSeqCnt, string authSourceCode, string aVSRspCode, string marketSpecAuth, string authRspCode, string posCardholderAuthEntity, string posCardDataOutputAbility, string systemTraceAuditNr, string datetimeTranLocal, string merchantType, string cardSeqNr, string posConditionCode, string retrievalReferenceNr, string authIDRsp, string rspCodeRsp, string serviceRestrictionCode, string terminalID, string cardAcceptorNameLoc, string posCardDataInputAbility, string posCardholderAuthAbility, string posCardCaptureAbility, string posOperatingEnvironment, string posTerminalOutputAbility, string posPinCaptureAbility, string posTerminalOperator, string ucafData, string tranAmountRsp, string tranType, Nullable<System.DateTime> businessDate, Nullable<int> fileId, string sourceCountryCode, string prevMessageType, string prevUcafData, string structuredDataReq, Nullable<System.DateTime> dateTimeRsp, Nullable<int> processingMode, string participantId)
        {
            object result;

            using (var command = CreateStoredProcedure(procName))
            {
                #region PARAMS

                command.Parameters.Add(GenerateParameter("@BIN", SqlDbType.NVarChar, ParameterDirection.Input, bIN, 6));
                command.Parameters.Add(GenerateParameter("@ARN", SqlDbType.NVarChar, ParameterDirection.Input, aRN, 23));
                command.Parameters.Add(GenerateParameter("@PAN", SqlDbType.NVarChar, ParameterDirection.Input, pAN, 16));
                command.Parameters.Add(GenerateParameter("@PANExtention", SqlDbType.NVarChar, ParameterDirection.Input, pANExtention, 3));
                command.Parameters.Add(GenerateParameter("@ACode", SqlDbType.NVarChar, ParameterDirection.Input, aCode, 6));
                command.Parameters.Add(GenerateParameter("@IncomingDate", SqlDbType.NVarChar, ParameterDirection.Input, incomingDate, 23));
                command.Parameters.Add(GenerateParameter("@SettlementDate", SqlDbType.NVarChar, ParameterDirection.Input, settlementDate, 23));
                command.Parameters.Add(GenerateParameter("@TransactionCode", SqlDbType.NVarChar, ParameterDirection.Input, transactionCode, 2));
                command.Parameters.Add(GenerateParameter("@TransactionDate", SqlDbType.NVarChar, ParameterDirection.Input, transactionDate, 4));
                command.Parameters.Add(GenerateParameter("@OriginalTransactionAmount", SqlDbType.NVarChar, ParameterDirection.Input, originalTransactionAmount, 12));
                command.Parameters.Add(GenerateParameter("@OriginalTransactionCurrencyCode", SqlDbType.NVarChar, ParameterDirection.Input, originalTransactionCurrencyCode, 3));
                command.Parameters.Add(GenerateParameter("@BookingAmountSign", SqlDbType.NVarChar, ParameterDirection.Input, bookingAmountSign, 1));
                command.Parameters.Add(GenerateParameter("@BookingAmount", SqlDbType.NVarChar, ParameterDirection.Input, bookingAmount, 12));
                command.Parameters.Add(GenerateParameter("@BookingAmountExponent", SqlDbType.NVarChar, ParameterDirection.Input, bookingAmountExponent, 1));
                command.Parameters.Add(GenerateParameter("@BookingCurrencyCode", SqlDbType.NVarChar, ParameterDirection.Input, bookingCurrencyCode, 3));
                command.Parameters.Add(GenerateParameter("@StageAmountSign", SqlDbType.NVarChar, ParameterDirection.Input, stageAmountSign, 1));
                command.Parameters.Add(GenerateParameter("@StageAmount", SqlDbType.NVarChar, ParameterDirection.Input, stageAmount, 12));
                command.Parameters.Add(GenerateParameter("@StageAmountExponent", SqlDbType.NVarChar, ParameterDirection.Input, stageAmountExponent, 1));
                command.Parameters.Add(GenerateParameter("@StageCurrencyCode", SqlDbType.NVarChar, ParameterDirection.Input, stageCurrencyCode, 3));
                command.Parameters.Add(GenerateParameter("@MerchantName", SqlDbType.NVarChar, ParameterDirection.Input, merchantName, 25));
                command.Parameters.Add(GenerateParameter("@MerchantCity", SqlDbType.NVarChar, ParameterDirection.Input, merchantCity, 13));
                command.Parameters.Add(GenerateParameter("@MerchantCountryCode", SqlDbType.NVarChar, ParameterDirection.Input, merchantCountryCode, 3));
                command.Parameters.Add(GenerateParameter("@MCC", SqlDbType.NVarChar, ParameterDirection.Input, mCC, 4));
                command.Parameters.Add(GenerateParameter("@UsageCode", SqlDbType.NVarChar, ParameterDirection.Input, usageCode, 1));
                command.Parameters.Add(GenerateParameter("@RId", SqlDbType.NVarChar, ParameterDirection.Input, rId, 12));
                command.Parameters.Add(GenerateParameter("@CentralProcessingDate", SqlDbType.NVarChar, ParameterDirection.Input, centralProcessingDate, 4));
                command.Parameters.Add(GenerateParameter("@TransactionId", SqlDbType.NVarChar, ParameterDirection.Input, transactionId, 15));
                command.Parameters.Add(GenerateParameter("@KKOCbReference", SqlDbType.NVarChar, ParameterDirection.Input, kKOCbReference, 10));
                command.Parameters.Add(GenerateParameter("@TID", SqlDbType.NVarChar, ParameterDirection.Input, tID, 8));
                command.Parameters.Add(GenerateParameter("@MOTOECI", SqlDbType.NVarChar, ParameterDirection.Input, mOTOECI, 1));
                command.Parameters.Add(GenerateParameter("@DestinationBin", SqlDbType.NVarChar, ParameterDirection.Input, destinationBin, 6));
                command.Parameters.Add(GenerateParameter("@SourceBin", SqlDbType.NVarChar, ParameterDirection.Input, sourceBin, 6));
                command.Parameters.Add(GenerateParameter("@ReasonCode", SqlDbType.NVarChar, ParameterDirection.Input, reasonCode, 4));
                command.Parameters.Add(GenerateParameter("@MemberMessageText", SqlDbType.NVarChar, ParameterDirection.Input, memberMessageText, 50));
                command.Parameters.Add(GenerateParameter("@MessageText", SqlDbType.NVarChar, ParameterDirection.Input, messageText, 70));
                command.Parameters.Add(GenerateParameter("@PartialFlag", SqlDbType.NVarChar, ParameterDirection.Input, partialFlag, 1));
                command.Parameters.Add(GenerateParameter("@ReturnReasonCode1", SqlDbType.NVarChar, ParameterDirection.Input, returnReasonCode1, 3));
                command.Parameters.Add(GenerateParameter("@ReturnReasonCode2", SqlDbType.NVarChar, ParameterDirection.Input, returnReasonCode2, 3));
                command.Parameters.Add(GenerateParameter("@ReturnReasonCode3", SqlDbType.NVarChar, ParameterDirection.Input, returnReasonCode3, 3));
                command.Parameters.Add(GenerateParameter("@ReturnReasonCode4", SqlDbType.NVarChar, ParameterDirection.Input, returnReasonCode4, 3));
                command.Parameters.Add(GenerateParameter("@ReturnReasonCode5", SqlDbType.NVarChar, ParameterDirection.Input, returnReasonCode5, 3));
                command.Parameters.Add(GenerateParameter("@Brand", SqlDbType.NVarChar, ParameterDirection.Input, brand, 25));
                command.Parameters.Add(GenerateParameter("@Narritive", SqlDbType.NVarChar, ParameterDirection.Input, narritive, 30));
                command.Parameters.Add(GenerateParameter("@TransactionAmountSign", SqlDbType.NVarChar, ParameterDirection.Input, transactionAmountSign, 1));
                command.Parameters.Add(GenerateParameter("@TransactionAmount", SqlDbType.NVarChar, ParameterDirection.Input, transactionAmount, 12));
                command.Parameters.Add(GenerateParameter("@TransactionAmountExponent", SqlDbType.NVarChar, ParameterDirection.Input, transactionAmountExponent, 1));
                command.Parameters.Add(GenerateParameter("@TransactionCurrencyCode", SqlDbType.NVarChar, ParameterDirection.Input, transactionCurrencyCode, 3));
                command.Parameters.Add(GenerateParameter("@Stage", SqlDbType.NVarChar, ParameterDirection.Input, stage, 4));
                command.Parameters.Add(GenerateParameter("@StageDate", SqlDbType.NVarChar, ParameterDirection.Input, stageDate, 10));
                command.Parameters.Add(GenerateParameter("@CVVFlag", SqlDbType.NVarChar, ParameterDirection.Input, cVVFlag, 1));
                command.Parameters.Add(GenerateParameter("@CVCFlag", SqlDbType.NVarChar, ParameterDirection.Input, cVCFlag, 1));
                command.Parameters.Add(GenerateParameter("@ECommerce", SqlDbType.NVarChar, ParameterDirection.Input, eCommerce, 1));
                command.Parameters.Add(GenerateParameter("@MPILogFlag", SqlDbType.NVarChar, ParameterDirection.Input, mPILogFlag, 1));
                command.Parameters.Add(GenerateParameter("@CardAcceptorIdCode", SqlDbType.NVarChar, ParameterDirection.Input, cardAcceptorIdCode, 15));
                command.Parameters.Add(GenerateParameter("@MessageType", SqlDbType.NVarChar, ParameterDirection.Input, messageType, 4));
                command.Parameters.Add(GenerateParameter("@TransactionDateTimeLocal", SqlDbType.NVarChar, ParameterDirection.Input, transactionDateTimeLocal, 23));
                command.Parameters.Add(GenerateParameter("@GiccMCC", SqlDbType.NVarChar, ParameterDirection.Input, giccMCC, 4));
                command.Parameters.Add(GenerateParameter("@GiccDomesticMCC", SqlDbType.NVarChar, ParameterDirection.Input, giccDomesticMCC, 4));
                command.Parameters.Add(GenerateParameter("@POSEntryMode", SqlDbType.NVarChar, ParameterDirection.Input, pOSEntryMode, 3));
                command.Parameters.Add(GenerateParameter("@DocumentationIndicator", SqlDbType.NVarChar, ParameterDirection.Input, documentationIndicator, 1));
                command.Parameters.Add(GenerateParameter("@PosTerminalType", SqlDbType.NVarChar, ParameterDirection.Input, posTerminalType, 2));
                command.Parameters.Add(GenerateParameter("@PosCardDataImputMode", SqlDbType.NVarChar, ParameterDirection.Input, posCardDataImputMode, 1));
                command.Parameters.Add(GenerateParameter("@PosCardPresent", SqlDbType.NVarChar, ParameterDirection.Input, posCardPresent, 1));
                command.Parameters.Add(GenerateParameter("@PosCardholderPresent", SqlDbType.NVarChar, ParameterDirection.Input, posCardholderPresent, 1));
                command.Parameters.Add(GenerateParameter("@PosCardholderAuthMethod", SqlDbType.NVarChar, ParameterDirection.Input, posCardholderAuthMethod, 1));
                command.Parameters.Add(GenerateParameter("@ExpiryDate", SqlDbType.NVarChar, ParameterDirection.Input, expiryDate, 4));
                command.Parameters.Add(GenerateParameter("@GiccRevDate", SqlDbType.NVarChar, ParameterDirection.Input, giccRevDate, 23));
                command.Parameters.Add(GenerateParameter("@SettleAmountImpact", SqlDbType.NVarChar, ParameterDirection.Input, settleAmountImpact, 16));
                command.Parameters.Add(GenerateParameter("@TranAmountReq", SqlDbType.NVarChar, ParameterDirection.Input, tranAmountReq, 16));
                command.Parameters.Add(GenerateParameter("@PostTranId", SqlDbType.NVarChar, ParameterDirection.Input, postTranId, 22));
                command.Parameters.Add(GenerateParameter("@PrevPostTranId", SqlDbType.NVarChar, ParameterDirection.Input, prevPostTranId, 22));
                command.Parameters.Add(GenerateParameter("@MultiClearingSeqNr", SqlDbType.NVarChar, ParameterDirection.Input, multiClearingSeqNr, 2));
                command.Parameters.Add(GenerateParameter("@MultiClearingSeqCnt", SqlDbType.NVarChar, ParameterDirection.Input, multiClearingSeqCnt, 2));
                command.Parameters.Add(GenerateParameter("@AuthSourceCode", SqlDbType.NVarChar, ParameterDirection.Input, authSourceCode, 1));
                command.Parameters.Add(GenerateParameter("@AVSRspCode", SqlDbType.NVarChar, ParameterDirection.Input, aVSRspCode, 1));
                command.Parameters.Add(GenerateParameter("@MarketSpecAuth", SqlDbType.NVarChar, ParameterDirection.Input, marketSpecAuth, 1));
                command.Parameters.Add(GenerateParameter("@AuthRspCode", SqlDbType.NVarChar, ParameterDirection.Input, authRspCode, 2));
                command.Parameters.Add(GenerateParameter("@PosCardholderAuthEntity", SqlDbType.NVarChar, ParameterDirection.Input, posCardholderAuthEntity, 1));
                command.Parameters.Add(GenerateParameter("@PosCardDataOutputAbility", SqlDbType.NVarChar, ParameterDirection.Input, posCardDataOutputAbility, 1));
                command.Parameters.Add(GenerateParameter("@SystemTraceAuditNr", SqlDbType.NVarChar, ParameterDirection.Input, systemTraceAuditNr, 6));
                command.Parameters.Add(GenerateParameter("@DatetimeTranLocal", SqlDbType.NVarChar, ParameterDirection.Input, datetimeTranLocal, 23));
                command.Parameters.Add(GenerateParameter("@MerchantType", SqlDbType.NVarChar, ParameterDirection.Input, merchantType, 4));
                command.Parameters.Add(GenerateParameter("@CardSeqNr", SqlDbType.NVarChar, ParameterDirection.Input, cardSeqNr, 3));
                command.Parameters.Add(GenerateParameter("@PosConditionCode", SqlDbType.NVarChar, ParameterDirection.Input, posConditionCode, 2));
                command.Parameters.Add(GenerateParameter("@RetrievalReferenceNr", SqlDbType.NVarChar, ParameterDirection.Input, retrievalReferenceNr, 12));
                command.Parameters.Add(GenerateParameter("@AuthIDRsp", SqlDbType.NVarChar, ParameterDirection.Input, authIDRsp, 10));
                command.Parameters.Add(GenerateParameter("@RspCodeRsp", SqlDbType.NVarChar, ParameterDirection.Input, rspCodeRsp, 6));
                command.Parameters.Add(GenerateParameter("@ServiceRestrictionCode", SqlDbType.NVarChar, ParameterDirection.Input, serviceRestrictionCode, 3));
                command.Parameters.Add(GenerateParameter("@TerminalID", SqlDbType.NVarChar, ParameterDirection.Input, terminalID, 8));
                command.Parameters.Add(GenerateParameter("@CardAcceptorNameLoc", SqlDbType.NVarChar, ParameterDirection.Input, cardAcceptorNameLoc, 40));
                command.Parameters.Add(GenerateParameter("@PosCardDataInputAbility", SqlDbType.NVarChar, ParameterDirection.Input, posCardDataInputAbility, 1));
                command.Parameters.Add(GenerateParameter("@PosCardholderAuthAbility", SqlDbType.NVarChar, ParameterDirection.Input, posCardholderAuthAbility, 1));
                command.Parameters.Add(GenerateParameter("@PosCardCaptureAbility", SqlDbType.NVarChar, ParameterDirection.Input, posCardCaptureAbility, 1));
                command.Parameters.Add(GenerateParameter("@PosOperatingEnvironment", SqlDbType.NVarChar, ParameterDirection.Input, posOperatingEnvironment, 1));
                command.Parameters.Add(GenerateParameter("@PosTerminalOutputAbility", SqlDbType.NVarChar, ParameterDirection.Input, posTerminalOutputAbility, 1));
                command.Parameters.Add(GenerateParameter("@PosPinCaptureAbility", SqlDbType.NVarChar, ParameterDirection.Input, posPinCaptureAbility, 1));
                command.Parameters.Add(GenerateParameter("@PosTerminalOperator", SqlDbType.NVarChar, ParameterDirection.Input, posTerminalOperator, 1));
                command.Parameters.Add(GenerateParameter("@UcafData", SqlDbType.NVarChar, ParameterDirection.Input, ucafData, 33));
                command.Parameters.Add(GenerateParameter("@TranAmountRsp", SqlDbType.NVarChar, ParameterDirection.Input, tranAmountRsp, 16));
                command.Parameters.Add(GenerateParameter("@TranType", SqlDbType.NVarChar, ParameterDirection.Input, tranType, 2));
                command.Parameters.Add(GenerateParameter("@BusinessDate", SqlDbType.Date, ParameterDirection.Input, businessDate, 0));
                command.Parameters.Add(GenerateParameter("@FileId", SqlDbType.Int, ParameterDirection.Input, fileId, 0));
                command.Parameters.Add(GenerateParameter("@SourceCountryCode", SqlDbType.NVarChar, ParameterDirection.Input, sourceCountryCode, 3));
                command.Parameters.Add(GenerateParameter("@PrevMessageType", SqlDbType.NVarChar, ParameterDirection.Input, prevMessageType, 4));
                command.Parameters.Add(GenerateParameter("@PrevUcafData", SqlDbType.NVarChar, ParameterDirection.Input, prevUcafData, 33));
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@StructuredDataReq",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    Size = (int)(Math.Pow(2, 31) - 1),
                    Value = (object)structuredDataReq ?? DBNull.Value
                });
                command.Parameters.Add(GenerateParameter("@DateTimeRsp", SqlDbType.DateTime2, ParameterDirection.Input, dateTimeRsp, 7));
                command.Parameters.Add(GenerateParameter("@ProcessingMode", SqlDbType.Int, ParameterDirection.Input, processingMode, 0));
                command.Parameters.Add(GenerateParameter("@ParticipantId", SqlDbType.NVarChar, ParameterDirection.Input, participantId, 3));

                #endregion

                command.CommandTimeout = 180;
                result = command.ExecuteScalar();
            }

            return result;
        }

        private static SqlParameter GenerateParameter(string paramName, SqlDbType type, ParameterDirection direction, object value, int size = 0)
        {
            if (size == 0)
                return new SqlParameter()
                {
                    ParameterName = paramName,
                    SqlDbType = type,
                    Direction = direction,
                    Value = value ?? DBNull.Value
                };
            return new SqlParameter()
            {
                ParameterName = paramName,
                SqlDbType = type,
                Size = size,
                Direction = direction,
                Value = value ?? DBNull.Value
            };
        }

        public object GetPanByCaseId(string caseId, out string pan, out string panExtention)
        {
            object result;
            var caseIdParam = GenerateParameter("@CaseId", SqlDbType.NVarChar, ParameterDirection.Input, caseId, 20);
            var panParam = GenerateParameter("@PAN", SqlDbType.NVarChar, ParameterDirection.Output, null, 16);
            var panExtentionParam = GenerateParameter("@PANExtention", SqlDbType.NVarChar, ParameterDirection.Output, null, 3);

            using (var command = CreateStoredProcedure("[Case].GetPANByCaseId"))
            {
                command.Parameters.Add(caseIdParam);
                command.Parameters.Add(panParam);
                command.Parameters.Add(panExtentionParam);
                command.CommandTimeout = 180;
                result = command.ExecuteScalar();
                pan = panParam.Value.ToString();
                panExtention = panExtentionParam.Value.ToString();
            }
            return result;
        }

        public long AddCaseFilingIncomingFile(CaseFilingIncomingFile incomingFile)
        {
            var caseIdParam = GenerateParameter("@CaseId", SqlDbType.NVarChar, ParameterDirection.Input, incomingFile.CaseId, 20);
            var stageIdParam = GenerateParameter("@StageId", SqlDbType.BigInt, ParameterDirection.Input, incomingFile.StageId);
            var streamIdParam = GenerateParameter("@stream_id", SqlDbType.UniqueIdentifier, ParameterDirection.Input, incomingFile.stream_id);
            var fileTypeParam = GenerateParameter("@FileType", SqlDbType.NVarChar, ParameterDirection.Input, incomingFile.FileType, 4);
            var fileNameParam = GenerateParameter("@FileName", SqlDbType.NVarChar, ParameterDirection.Input, incomingFile.FileName, 256);
            var fileContentParam = GenerateParameter("@FileContent", SqlDbType.NVarChar, ParameterDirection.Input, incomingFile.FileContent);
            var processKeyParam = GenerateParameter("@ProcessKey", SqlDbType.UniqueIdentifier, ParameterDirection.Input, incomingFile.ProcessKey);
            var insertDateParam = GenerateParameter("@InsertDate", SqlDbType.DateTimeOffset, ParameterDirection.Input, incomingFile.InsertDate);
            var insertUserParam = GenerateParameter("@InsertUser", SqlDbType.NVarChar, ParameterDirection.Input, incomingFile.InsertUser, 128);
            var incomingIdParam = GenerateParameter("@IncomingId", SqlDbType.BigInt, ParameterDirection.Output, null);

            using (var command = CreateStoredProcedure("[File].[AddNewCaseFilingIncomingFile]"))
            {
                command.Parameters.Add(caseIdParam);
                command.Parameters.Add(stageIdParam);
                command.Parameters.Add(streamIdParam);
                command.Parameters.Add(fileTypeParam);
                command.Parameters.Add(fileNameParam);
                command.Parameters.Add(fileContentParam);
                command.Parameters.Add(processKeyParam);
                command.Parameters.Add(insertDateParam);
                command.Parameters.Add(insertUserParam);
                command.Parameters.Add(incomingIdParam);
                command.CommandTimeout = 180;
                command.ExecuteScalar();
                return long.Parse(incomingIdParam.Value.ToString());
            }
        }

        public long AddCaseFilingOutgoingFile(CaseFilingOutgoingFile outgoingFile)
        {
            var caseIdParam = GenerateParameter("@CaseId", SqlDbType.NVarChar, ParameterDirection.Input, outgoingFile.CaseId, 20);
            var stageIdParam = GenerateParameter("@StageId", SqlDbType.BigInt, ParameterDirection.Input, outgoingFile.StageId);
            var streamIdParam = GenerateParameter("@stream_id", SqlDbType.UniqueIdentifier, ParameterDirection.Input, outgoingFile.stream_id);
            var fileTypeParam = GenerateParameter("@FileType", SqlDbType.NVarChar, ParameterDirection.Input, outgoingFile.FileType, 4);
            var fileNameParam = GenerateParameter("@FileName", SqlDbType.NVarChar, ParameterDirection.Input, outgoingFile.FileName, 256);
            var fileContentParam = GenerateParameter("@FileContent", SqlDbType.NVarChar, ParameterDirection.Input, outgoingFile.FileContent);
            var processKeyParam = GenerateParameter("@ProcessKey", SqlDbType.UniqueIdentifier, ParameterDirection.Input, outgoingFile.ProcessKey);
            var insertDateParam = GenerateParameter("@InsertDate", SqlDbType.DateTimeOffset, ParameterDirection.Input, outgoingFile.InsertDate);
            var insertUserParam = GenerateParameter("@InsertUser", SqlDbType.NVarChar, ParameterDirection.Input, outgoingFile.InsertUser, 128);
            var outgoingIdParam = GenerateParameter("@OutgoingId", SqlDbType.BigInt, ParameterDirection.Output, null);

            using (var command = CreateStoredProcedure("[File].[AddNewCaseFilingOutgoingFile]"))
            {
                command.Parameters.Add(caseIdParam);
                command.Parameters.Add(stageIdParam);
                command.Parameters.Add(streamIdParam);
                command.Parameters.Add(fileTypeParam);
                command.Parameters.Add(fileNameParam);
                command.Parameters.Add(fileContentParam);
                command.Parameters.Add(processKeyParam);
                command.Parameters.Add(insertDateParam);
                command.Parameters.Add(insertUserParam);
                command.Parameters.Add(outgoingIdParam);
                command.CommandTimeout = 180;
                command.ExecuteScalar();
                return long.Parse(outgoingIdParam.Value.ToString());
            }
        }

        public CaseFilingIncomingFile GetCaseFilingIncomingFile(long incomingId)
        {
            var incomingIdParam = GenerateParameter("@IncomingId", SqlDbType.BigInt, ParameterDirection.Input, incomingId);

            using (var command = CreateStoredProcedure("[File].[GetCaseFilingIncomingFile]"))
            {
                command.Parameters.Add(incomingIdParam);
                command.CommandTimeout = 180;
                var reader = command.ExecuteReader();
                return CreateCaseFilingIncomingFileData(reader);
            }
        }

        private static CaseFilingIncomingFile CreateCaseFilingIncomingFileData(IDataReader reader)
        {
            CaseFilingIncomingFile caseFilingIncoming = null;

            while (reader.Read())
            {
                caseFilingIncoming = new CaseFilingIncomingFile
                {
                    CaseId = reader[0].ToString(),
                    StageId = long.Parse(reader[1].ToString()),
                    stream_id = Guid.Parse(reader[2].ToString()),
                    FileType = reader[3].ToString(),
                    FileName = reader[4].ToString(),
                    FileContent = reader[5].ToString(),
                    ProcessKey = Guid.Parse(reader[6].ToString()),
                    InsertDate = DateTimeOffset.Parse(reader[7].ToString()),
                    InsertUser = reader[8].ToString()
                };
            }
            reader.Close();
            return caseFilingIncoming;
        }

        public CaseFilingOutgoingFile GetCaseFilingOutgoingFile(long outgoingId)
        {
            var outgoingIdParam = GenerateParameter("@OutgoingId", SqlDbType.BigInt, ParameterDirection.Input, outgoingId);

            using (var command = CreateStoredProcedure("[File].[GetCaseFilingOutgoingFile]"))
            {
                command.Parameters.Add(outgoingIdParam);
                command.CommandTimeout = 180;
                var reader = command.ExecuteReader();
                return CreateCaseFilingOutgoingFileData(reader);
            }
        }

        private static CaseFilingOutgoingFile CreateCaseFilingOutgoingFileData(IDataReader reader)
        {
            CaseFilingOutgoingFile caseFilingOutgoing = null;

            while (reader.Read())
            {
                caseFilingOutgoing = new CaseFilingOutgoingFile
                {
                    CaseId = reader[0].ToString(),
                    StageId = long.Parse(reader[1].ToString()),
                    stream_id = Guid.Parse(reader[2].ToString()),
                    FileType = reader[3].ToString(),
                    FileName = reader[4].ToString(),
                    FileContent = reader[5].ToString(),
                    ProcessKey = Guid.Parse(reader[6].ToString()),
                    InsertDate = DateTimeOffset.Parse(reader[7].ToString()),
                    InsertUser = reader[8].ToString()
                };
            }
            reader.Close();
            return caseFilingOutgoing;
        }

        public long AddAudit(Audit audit)
        {
            var caseIdParam = GenerateParameter("@CaseId", SqlDbType.NVarChar, ParameterDirection.Input, audit.CaseId, 20);
            var stageIdParam = GenerateParameter("@StageId", SqlDbType.BigInt, ParameterDirection.Input, audit.StageId);
            var streamIdParam = GenerateParameter("@stream_id", SqlDbType.UniqueIdentifier, ParameterDirection.Input, audit.stream_id);
            var incomingDateParam = GenerateParameter("@IncomingDate", SqlDbType.Date, ParameterDirection.Input, audit.IncomingDate);
            var processKeyParam = GenerateParameter("@ProcessKey", SqlDbType.UniqueIdentifier, ParameterDirection.Input, audit.ProcessKey);
            var descriptionParam = GenerateParameter("@Description", SqlDbType.NVarChar, ParameterDirection.Input, audit.Description);
            var statusParam = GenerateParameter("@Status", SqlDbType.Int, ParameterDirection.Input, audit.Status);
            var errorCodeParam = GenerateParameter("@ErrorCode", SqlDbType.NVarChar, ParameterDirection.Input, audit.ErrorCode);
            var errorDescriptionParam = GenerateParameter("@ErrorDescription", SqlDbType.NVarChar, ParameterDirection.Input, audit.ErrorDescription);
            var insertDateParam = GenerateParameter("@InsertDate", SqlDbType.DateTimeOffset, ParameterDirection.Input, audit.InsertDate);
            var insertUserParam = GenerateParameter("@InsertUser", SqlDbType.NVarChar, ParameterDirection.Input, audit.InsertUser, 128);
            var auditIdParam = GenerateParameter("@AuditId", SqlDbType.BigInt, ParameterDirection.Output, null);

            using (var command = CreateStoredProcedure("[File].[AddNewAudit]"))
            {
                command.Parameters.Add(caseIdParam);
                command.Parameters.Add(stageIdParam);
                command.Parameters.Add(streamIdParam);
                command.Parameters.Add(incomingDateParam);
                command.Parameters.Add(processKeyParam);
                command.Parameters.Add(descriptionParam);
                command.Parameters.Add(statusParam);
                command.Parameters.Add(errorCodeParam);
                command.Parameters.Add(errorDescriptionParam);
                command.Parameters.Add(insertDateParam);
                command.Parameters.Add(insertUserParam);
                command.Parameters.Add(auditIdParam);
                command.CommandTimeout = 180;
                command.ExecuteScalar();
                return long.Parse(auditIdParam.Value.ToString());
            }

        }

        public Audit GetAudit(long auditId)
        {
            var auditIdParam = GenerateParameter("@AuditId", SqlDbType.BigInt, ParameterDirection.Input, auditId);

            using (var command = CreateStoredProcedure("[File].[GetAudit]"))
            {
                command.Parameters.Add(auditIdParam);
                command.CommandTimeout = 180;
                var reader = command.ExecuteReader();
                return CreateAuditData(reader);
            }
        }

        private static Audit CreateAuditData(IDataReader reader)
        {
            Audit audit = null;
            while (reader.Read())
            {
                audit = new Audit
                {
                    AuditId = long.Parse(reader[0].ToString()),
                    CaseId = reader[1].ToString(),
                    StageId = long.Parse(reader[2].ToString()),
                    stream_id = Guid.Parse(reader[3].ToString()),
                    IncomingDate = DateTime.Parse(reader[4].ToString()),
                    ProcessKey = Guid.Parse(reader[5].ToString()),
                    Description = reader[6].ToString(),
                    Status = int.Parse(reader[7].ToString()),
                    ErrorCode = reader[8].ToString(),
                    ErrorDescription = reader[9].ToString(),
                    InsertDate = DateTimeOffset.Parse(reader[10].ToString()),
                    InsertUser = reader[11].ToString()
                };
            }
            return audit;
        }

        public long AddFeeCollectionExtract(FeeCollectionExtract feeCollectionExtract)
        {
            var processKeyParam = GenerateParameter("@ProcessKey", SqlDbType.UniqueIdentifier, ParameterDirection.Input, feeCollectionExtract.ProcessKey);
            var caseIdParam = GenerateParameter("@CaseId", SqlDbType.NVarChar, ParameterDirection.Input, feeCollectionExtract.CaseId, 20);
            var feeCollectionIdParam = GenerateParameter("@FeeCollectionId", SqlDbType.BigInt, ParameterDirection.Input, feeCollectionExtract.FeeCollectionId);
            var feeCollPostilionFileIdParam = GenerateParameter("@FeeCollectionPostilionFileId", SqlDbType.BigInt, ParameterDirection.Input, feeCollectionExtract.FeeCollectionPostilionFileId);
            var posExtrClearStringParam = GenerateParameter("@PostilionExtractClearString", SqlDbType.NVarChar, ParameterDirection.Input, feeCollectionExtract.PostilionExtractClearString);
            var posExtractBase64StringParam = GenerateParameter("@PostilionExtractWithBase64String", SqlDbType.NVarChar, ParameterDirection.Input, feeCollectionExtract.PostilionExtractWithBase64String);
            var posStatusParam = GenerateParameter("@PostilionStatus", SqlDbType.NVarChar, ParameterDirection.Input, feeCollectionExtract.PostilionStatus, 4);
            var posStatusMsgParam = GenerateParameter("@PostilionStatusMessage", SqlDbType.NVarChar, ParameterDirection.Input, feeCollectionExtract.PostilionStatusMessage);
            var errorFlagParam = GenerateParameter("@ErrorFlag", SqlDbType.Bit, ParameterDirection.Input, feeCollectionExtract.ErrorFlag);
            var statusParam = GenerateParameter("@Status", SqlDbType.Int, ParameterDirection.Input, feeCollectionExtract.Status);
            var insertDateParam = GenerateParameter("@InsertDate", SqlDbType.DateTimeOffset, ParameterDirection.Input, feeCollectionExtract.InsertDate);
            var insertUserParam = GenerateParameter("@InsertUser", SqlDbType.NVarChar, ParameterDirection.Input, feeCollectionExtract.InsertUser, 128);
            var feeCollExtractIdParam = GenerateParameter("@FeeCollectionExtractId", SqlDbType.BigInt, ParameterDirection.Output, null);

            using (var command = CreateStoredProcedure("[Process].[AddNewFeeCollectionExtract]"))
            {
                command.Parameters.Add(processKeyParam);
                command.Parameters.Add(caseIdParam);
                command.Parameters.Add(feeCollectionIdParam);
                command.Parameters.Add(feeCollPostilionFileIdParam);
                command.Parameters.Add(posExtrClearStringParam);
                command.Parameters.Add(posExtractBase64StringParam);
                command.Parameters.Add(posStatusParam);
                command.Parameters.Add(posStatusMsgParam);
                command.Parameters.Add(errorFlagParam);
                command.Parameters.Add(statusParam);
                command.Parameters.Add(insertDateParam);
                command.Parameters.Add(insertUserParam);
                command.CommandTimeout = 180;
                command.ExecuteScalar();
                return long.Parse(feeCollExtractIdParam.Value.ToString());
            }
        }

        public FeeCollectionExtract GetFeeCollectionExtract(long feeCollectionExtractId)
        {
            var feeCollExtractIdParam = GenerateParameter("@FeeCollectionExtractId", SqlDbType.BigInt, ParameterDirection.Input, feeCollectionExtractId);

            using (var command = CreateStoredProcedure("[Process].[GetFeeCollectionExtract]"))
            {
                command.Parameters.Add(feeCollExtractIdParam);
                command.CommandTimeout = 180;
                var reader = command.ExecuteReader();
                return CreateFeeCollectionExtractData(reader);
            }
        }

        private static FeeCollectionExtract CreateFeeCollectionExtractData(IDataReader reader)
        {
            FeeCollectionExtract feeCollectionExtract = null;
            while (reader.Read())
            {
                feeCollectionExtract = new FeeCollectionExtract
                {
                    FeeCollectionExtractId = long.Parse(reader[0].ToString()),
                    ProcessKey = Guid.Parse(reader[1].ToString()),
                    CaseId = reader[2].ToString(),
                    FeeCollectionId = long.Parse(reader[3].ToString()),
                    FeeCollectionPostilionFileId = long.Parse(reader[4].ToString()),
                    PostilionExtractClearString = reader[5].ToString(),
                    PostilionExtractWithBase64String = reader[6].ToString(),
                    PostilionStatus = reader[7].ToString(),
                    PostilionStatusMessage = reader[8].ToString(),
                    ErrorFlag = reader.GetBoolean(9),
                    Status = int.Parse(reader[10].ToString()),
                    InsertDate = DateTimeOffset.Parse(reader[11].ToString()),
                    InsertUser = reader[12].ToString()
                };
            }
            return feeCollectionExtract;
        }

        public long AddRepresentmentExtract(RepresentmentExtract representmentExtract)
        {
            var processKeyParam = GenerateParameter("@ProcessKey", SqlDbType.UniqueIdentifier, ParameterDirection.Input, representmentExtract.ProcessKey);
            var caseIdParam = GenerateParameter("@CaseId", SqlDbType.NVarChar, ParameterDirection.Input, representmentExtract.CaseId, 20);
            var representmentIdParam = GenerateParameter("@RepresentmentId", SqlDbType.BigInt, ParameterDirection.Input, representmentExtract.RepresentmentId);
            var representmentPostilionFileIdParam = GenerateParameter("@RepresentmentPostilionFileId", SqlDbType.BigInt, ParameterDirection.Input, representmentExtract.RepresentmentPostilionFileId);
            var posExtrClearStringParam = GenerateParameter("@PostilionExtractClearString", SqlDbType.NVarChar, ParameterDirection.Input, representmentExtract.PostilionExtractClearString);
            var posExtractBase64StringParam = GenerateParameter("@PostilionExtractWithBase64String", SqlDbType.NVarChar, ParameterDirection.Input, representmentExtract.PostilionExtractWithBase64String);
            var posStatusParam = GenerateParameter("@PostilionStatus", SqlDbType.NVarChar, ParameterDirection.Input, representmentExtract.PostilionStatus, 4);
            var posStatusMsgParam = GenerateParameter("@PostilionStatusMessage", SqlDbType.NVarChar, ParameterDirection.Input, representmentExtract.PostilionStatusMessage);
            var errorFlagParam = GenerateParameter("@ErrorFlag", SqlDbType.Bit, ParameterDirection.Input, representmentExtract.ErrorFlag);
            var statusParam = GenerateParameter("@Status", SqlDbType.Int, ParameterDirection.Input, representmentExtract.Status);
            var insertDateParam = GenerateParameter("@InsertDate", SqlDbType.DateTimeOffset, ParameterDirection.Input, representmentExtract.InsertDate);
            var insertUserParam = GenerateParameter("@InsertUser", SqlDbType.NVarChar, ParameterDirection.Input, representmentExtract.InsertUser, 128);
            var represenmtentExtractIdParam = GenerateParameter("@RepresentmentExtractId", SqlDbType.BigInt, ParameterDirection.Output, null);

            using (var command = CreateStoredProcedure("[Process].[AddNewRepresentmentExtract]"))
            {
                command.Parameters.Add(processKeyParam);
                command.Parameters.Add(caseIdParam);
                command.Parameters.Add(representmentIdParam);
                command.Parameters.Add(representmentPostilionFileIdParam);
                command.Parameters.Add(posExtrClearStringParam);
                command.Parameters.Add(posExtractBase64StringParam);
                command.Parameters.Add(posStatusParam);
                command.Parameters.Add(posStatusMsgParam);
                command.Parameters.Add(errorFlagParam);
                command.Parameters.Add(statusParam);
                command.Parameters.Add(insertDateParam);
                command.Parameters.Add(insertUserParam);
                command.CommandTimeout = 180;
                command.ExecuteScalar();
                return long.Parse(represenmtentExtractIdParam.Value.ToString());
            }
        }

        public RepresentmentExtract GetRepresentmentExtract(long representmentExtractId)
        {
            var feeCollExtractIdParam = GenerateParameter("@RepresentmentExtractId", SqlDbType.BigInt, ParameterDirection.Input, representmentExtractId);

            using (var command = CreateStoredProcedure("[Process].[GetFeeCollectionExtract]"))
            {
                command.Parameters.Add(feeCollExtractIdParam);
                command.CommandTimeout = 180;
                var reader = command.ExecuteReader();
                return CreateRepresentmentExtractData(reader);
            }
        }

        private static RepresentmentExtract CreateRepresentmentExtractData(IDataReader reader)
        {
            RepresentmentExtract representmentExtract = null;
            while (reader.Read())
            {
                representmentExtract = new RepresentmentExtract
                {
                    RepresentmentExtractId = long.Parse(reader[0].ToString()),
                    ProcessKey = Guid.Parse(reader[1].ToString()),
                    CaseId = reader[2].ToString(),
                    RepresentmentId = long.Parse(reader[3].ToString()),
                    RepresentmentPostilionFileId = long.Parse(reader[4].ToString()),
                    PostilionExtractClearString = reader[5].ToString(),
                    PostilionExtractWithBase64String = reader[6].ToString(),
                    PostilionStatus = reader[7].ToString(),
                    PostilionStatusMessage = reader[8].ToString(),
                    ErrorFlag = reader.GetBoolean(9),
                    Status = int.Parse(reader[10].ToString()),
                    InsertDate = DateTimeOffset.Parse(reader[11].ToString()),
                    InsertUser = reader[12].ToString()
                };
            }
            return representmentExtract;
        }

        #endregion
    }
}

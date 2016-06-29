using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using ComplaintTool.Common;
using ComplaintTool.Common.Config;
using ComplaintTool.Models;
using ComplaintTool.Common.CTLogger;
using System.Data.Entity;

namespace ComplaintTool.DataAccess.Repos
{
    public class PostilionRepo : RepositoryBase
    {
        private static readonly ILogger _logger = LogManager.GetLogger();
        //private readonly string _connectionString = ComplaintConfig.Instance.GetConnectionString();
        private const string Query = "[Case].[GetPostilionData]";
        private const int ColumnCount = 58;
        private readonly bool _checkDepthAndFieldCount;
        private readonly bool _throwOnRead;
        private readonly ComplaintEntities _dbContext;

        public PostilionRepo(DbContext dbContext)
            : base(dbContext)
        {
            _checkDepthAndFieldCount = true;
            _throwOnRead = true;
        }

        public PostilionRepo(ComplaintEntities dbContext)
            : this(dbContext, true, true)
        {

        }

        public PostilionRepo(ComplaintEntities dbContext, bool checkDepthAndFieldCount = true, bool throwOnRead = true)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _checkDepthAndFieldCount = checkDepthAndFieldCount;
            _throwOnRead = throwOnRead;
        }

        #region GetPostilionDataVisaIncoming

        public PostilionData GetPostilionData(string arn, string panFirst6, string panLast4, string connectionString = null)
        {
            var stopwatch = Stopwatch.StartNew();
            PostilionData postilionData = null;
            _logger.LogComplaintEvent(157, arn);
            
            using (var connection = new SqlConnection(connectionString ?? _dbContext.Database.Connection.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand
                {
                    CommandText = Query,
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("@arn", SqlDbType.VarChar).Value = arn;

                    if (!string.IsNullOrWhiteSpace(panFirst6) && !string.IsNullOrWhiteSpace(panLast4))
                    {
                        command.Parameters.AddWithValue("@panfirst6", SqlDbType.VarChar).Value = panFirst6;
                        command.Parameters.AddWithValue("@panlast4", SqlDbType.VarChar).Value = panLast4;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                CheckDepthAndFieldCount(reader, arn);
                                postilionData = CreatePostilionData(reader);

                                if (postilionData != null)
                                {
                                    var samePan = CheckPanMask(postilionData.PAN, panFirst6, panLast4);

                                    if (!samePan)
                                    {
                                        reader.Dispose();
                                        postilionData = GetResultsByArnFromPostilion(command, arn);
                                    }
                                }
                            }
                            else
                            {
                                reader.Dispose();
                                postilionData = GetResultsByArnFromPostilion(command, arn);
                            }
                        }                        
                    }
                    else
                        postilionData = GetResultsByArnFromPostilion(command, arn);
                }
            }

            stopwatch.Stop();
            _logger.LogComplaintEvent(158, arn, stopwatch.ElapsedMilliseconds / 1000, postilionData != null ? postilionData.ToString() : "null");

            return postilionData;
        }

        private PostilionData GetResultsByArnFromPostilion(SqlCommand command, string arn)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@arn", SqlDbType.VarChar).Value = arn;
            command.Parameters.AddWithValue("@panfirst6", SqlDbType.VarChar).Value = DBNull.Value;
            command.Parameters.AddWithValue("@panlast4", SqlDbType.VarChar).Value = DBNull.Value;

            var reader = command.ExecuteReader();

            CheckDepthAndFieldCount(reader, arn);

            return CreatePostilionData(reader);
        }

        private bool CheckPanMask(string maskedPan, string panFirst6, string panLast4)
        {
            var splittedMask = maskedPan.Split(new[] { "******" }, StringSplitOptions.None);
            return panFirst6.Equals(splittedMask[0]) && panLast4.Equals(splittedMask[1]);
        }

        private PostilionData CreatePostilionData(SqlDataReader reader)
        {
            PostilionData postilionData = null;
            try
            {
                #region while read
                while (reader.Read())
                {
                    postilionData = new PostilionData
                    {
                        PostID = long.Parse(reader[0].ToString()),
                        PostTranCustID = long.Parse(reader[1].ToString()),
                        TranAmountRsp = decimal.Parse(reader[2].ToString()),
                        TranCurrencyCode = reader[3].ToString(),
                        DatetimeTranLocal = DateTime.Parse(reader[4].ToString()),
                        StructuredDataReq = reader[5].ToString(),
                        CardVerificationResult = reader[6].ToString(),
                        PosTerminalType = reader[7].ToString(),
                        AlphaCode = reader[8].ToString(),
                        CurrencyCode = reader[9].ToString(),
                        NrDecimals = int.Parse(reader[10].ToString()),
                        Name = reader[11].ToString(),
                        TranType = reader[12].ToString(),
                        ExtendedTranType = reader[13].ToString(),
                        PrimaryFileReference = reader[14].ToString(),
                        MessageType = reader[15].ToString(),
                        CardAcceptorIDCode = reader[16].ToString(),
                        PanReference = reader[17].ToString(),
                        AuthIDRsp = reader[18].ToString(),
                        UcafData = reader[19].ToString(),
                        DatetimeRsp = DateTime.Parse(reader[20].ToString()),
                        TerminalID = reader[21].ToString(),
                        PosCardPresent = reader[22].ToString(),
                        PosCardholderPresent = reader[23].ToString(),
                        PosCardDataInputMode = reader[24].ToString(),
                        ExpiryDate = reader[25].ToString(),
                        PosCardholderAuthEntity = reader[26].ToString(),
                        PosCardDataOutputAbility = reader[27].ToString(),
                        PAN = reader[28].ToString(),
                        PANEncrypted = reader[29].ToString(),
                        MerchantType = reader[30].ToString(),
                        CardAcceptorNameLoc = reader[31].ToString(),
                        POSEntryMode = reader[32].ToString(),
                        POSCardholderAuthMethod = reader[33].ToString(),
                        SettleAmountImpact = decimal.Parse(reader[34].ToString()),
                        TranAmountReq = decimal.Parse(reader[35].ToString()),
                        PrevPostTranId = int.Parse(reader[36].ToString()),
                        PosConditionCode = reader[37].ToString(),
                        RetrievalReferenceNr = reader[38].ToString(),
                        RspCodeRsp = reader[39].ToString(),
                        TranNr = long.Parse(reader[40].ToString()),
                        CardSeqNr = reader[41].ToString(),
                        ServiceRestrictionCode = reader[42].ToString(),
                        PosCardDataInputAbility = reader[43].ToString(),
                        PosCardholderAuthAbility = reader[44].ToString(),
                        PosCardCaptureAbility = reader[45].ToString(),
                        PosOperatingEnvironment = reader[46].ToString(),
                        PosTerminalOutputAbility = reader[47].ToString(),
                        PosPinCaptureAbility = reader[48].ToString(),
                        PosTerminalOperator = reader[49].ToString(),
                        SystemTraceAuditNr = reader[50].ToString(),
                        PrevMessageType = reader[51].ToString(),
                        PrevUcafData = reader[52].ToString(),
                        PrevPosEntryMode = reader[53].ToString(),
                        PrevPosCardPresent = reader[54].ToString(),
                        PtPosCardInputMode = reader[55].ToString(),
                        PrevPtPosCardInputMode = reader[56].ToString(),
                        ParticipantId = reader[57].ToString()
                    };
                } 
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogComplaintException(ex);
                if (_throwOnRead)
                    throw new ComplaintException("Error on reading postilion data", ex);
                postilionData = null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return postilionData;
        }

        private void CheckDepthAndFieldCount(IDataReader reader, string arn)
        {
            if (_checkDepthAndFieldCount)
            {
                if (reader.Depth != 0)
                    throw new Exception(
                        string.Format(
                            "The procedure [Case].[GetPostilionData] requested more results than one for the ARN: {0} , Depth: {1}",
                            arn, reader.Depth));
                if (reader.FieldCount != ColumnCount)
                    throw new Exception(
                        string.Format(
                            "The procedure [Case].[GetPostilionData] asked not correct the number of columns for the ARN: {0} , FieldCount: {1}",
                            arn, reader.FieldCount));
            }
        }

        #endregion

        public PostilionData GetPostilionData(IncomingTranMASTERCARD incomingTran)
        {
            Complaint complaint;
            complaint = _dbContext.Complaints.FirstOrDefault(x => x.CaseId == incomingTran.CaseId);
            if (complaint == null) return null;
            if (string.IsNullOrWhiteSpace(complaint.PANMask))
                return GetPostilionData(complaint.ARN, null, null);
            var panFirst6 = complaint.PANMask.Substring(0, 6);
            var panLast4 = complaint.PANMask.Substring(12, 4);
            return GetPostilionData(complaint.ARN, panFirst6, panLast4);
        }

        public string GetPreviousTransactionAmount(string prevPostTranId)
        {
            string prevTransactionAmount;
            var longPrevPostTranId = long.Parse(prevPostTranId);
            var view = _dbContext.View_SELECTEDPOSTILIONDATA.FirstOrDefault(t => t.post_tran_id == longPrevPostTranId);
            if (view == null) return string.Empty;
            prevTransactionAmount = view.tran_amount_req.ToString();
            return prevTransactionAmount;
        }

        public string GetStructuredDataFieldValue(string fieldName, string strDataReq)
        {
            var start = 0;
            const int lengthSub = 1;
            while (start < strDataReq.Length)
            {
                var lengthOfLengthName = int.Parse(strDataReq.Substring(start, lengthSub));
                var length = int.Parse(strDataReq.Substring(start + lengthSub, lengthOfLengthName));
                var name = strDataReq.Substring(start + lengthSub + lengthOfLengthName, length);
                var lengthOfLengthField = int.Parse(strDataReq.Substring(start + lengthSub + lengthOfLengthName + length, lengthSub));
                var lengthField = int.Parse(strDataReq.Substring(start + lengthSub + lengthOfLengthName + length + lengthSub, lengthOfLengthField));
                var field = strDataReq.Substring(start + lengthSub + lengthOfLengthName + length + lengthSub + lengthOfLengthField, lengthField);
                if (name.Equals(fieldName))
                    return field;
                start = start + lengthSub + lengthOfLengthName + length + lengthSub + lengthOfLengthField + lengthField;
            }
            return " ";
        }

        public string GetParticipantId(string bin)
        {
            var item = _dbContext.BINLists.FirstOrDefault(x => x.BIN == bin);
            return item != null ? item.ParticipantId : null;
        }
    }
}
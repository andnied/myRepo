using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ComplaintTool.Common.Enum;
using Organization = ComplaintTool.Common.Enum.Organization;
using ComplaintTool.DataAccess.Model;
using ComplaintTool.DataAccess;
using ComplaintTool.DataAccess.Repos;

namespace ComplaintTool.Postilion.Outgoing
{
    public static class Tool
    {
        public static string GetParticipantId(string bin)
        {
            using (var entities = new ComplaintEntities())
            {
                var item = entities.BINLists.FirstOrDefault(x => x.BIN == bin);
                return item != null ? item.ParticipantId : null;
            }
        }

        public static string GetParticipantId(string bin, ComplaintEntities entities)
        {
                var item = entities.BINLists.FirstOrDefault(x => x.BIN == bin);
                return item != null ? item.ParticipantId : null;
        }

        public static void RepresentmentStatusUpdate(Representment representmentToUpdate, int status)
        {
            using (var entities = new ComplaintEntities())
            {
                var representment
                    = entities.Representments.FirstOrDefault(
                    x => x.RepresentmentId == representmentToUpdate.RepresentmentId);
                if (representment != null)
                {
                    representment.Status = status;
                }
                entities.SaveChanges();
            }
        }

        public static void FeeCollectionStatusUpdate(FeeCollection feeCollectionToUpdate, int status)
        {
            using (var entities = new ComplaintEntities())
            {
                var feeCollection
                    = entities.FeeCollections.FirstOrDefault(
                    x => x.FeeCollectionId == feeCollectionToUpdate.FeeCollectionId);
                if (feeCollection != null)
                {
                    feeCollection.Status = status;
                }
                entities.SaveChanges();
            }
        }

        public static Organization GetOrganizationByNumber(int number)
        {
            return (Organization) number;
        }

        /// <summary>
        /// Nazwa pliku z odpowiedzią będzie to counter, która nie będzie weryfikowana po stronie CT.
        /// Nazwa oryginalnego pliku z wymianą specjalną będzie miała max12 znaków.
        /// •	Pierwszy znak będzie identyfikował organizację:
        /// 1 – Visa enum Organization.VISA = 1
        /// 4 – MasterCard i Maestro enum Organization.MC = 4
        /// •	Drugi znak będzie identyfikował typ wymiany specjalnej:
        /// 1 – Reprezentacja
        /// 2 – Fee collection / funds disbursement
        /// •	Dziesięć kolejnych znaków counter.
        /// </summary>
        /// <returns></returns>
        public static string FileName(Organization organization, ReplyMode replyMode)
        {
            var count = 0;
            using (var unitOfWork = ComplaintUnitOfWork.Get())
            {
                switch (replyMode)
                {
                    case ReplyMode.Representment:
                        count = unitOfWork.Repo<RepresentmentRepo>().GetRepresentmentPostilionFilesCount();
                        break;
                    case ReplyMode.FeeCollection:
                        count = unitOfWork.Repo<RepresentmentRepo>().GetFeeCollectionPostilionFilesCount();
                        break;
                }
            }
            count++;
            return ((int)organization).ToString(CultureInfo.InvariantCulture) + ((int)replyMode).ToString(CultureInfo.InvariantCulture) + count.ToString(CultureInfo.InvariantCulture).PadLeft(10, '0') + ".csv";
        }

        /// <summary>
        /// 1 – Reprezentacja
        /// 2 / default– Fee collection / funds disbursement
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ReplyMode DetectFileMode(string message)
        {
            var charMode = message.Substring(1, 1);
            switch (charMode)
            {
                case "1":
                    return ReplyMode.Representment;
                default:
                    return ReplyMode.FeeCollection;
            }
        }

        public static string GetFeeCollectionControlNr(Organization organization, long feeCollectionId)
        {
            var controlNr = DateTime.Now.ToString("yyyyMMdd"); //50420400001
            var count = feeCollectionId;
            switch (organization)
            {
                case Organization.MC:
                    controlNr += "4";
                    controlNr += count.ToString(CultureInfo.InvariantCulture).PadLeft(9, '0');
                    return controlNr;
                case Organization.VISA:
                    controlNr += "1";
                    controlNr += count.ToString(CultureInfo.InvariantCulture).PadLeft(8, '0');
                    return controlNr;
                default:
                    return controlNr;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;

namespace ComplaintTool.Accounting.Currency
{
    public class NBPRssReader
    {
        public NBPExchangeRatesDaily ReadRSS(DateTime data)
        {
            string url = ConfigurationManager.AppSettings["NBPRssReader_Url"];
            string proxy = ConfigurationManager.AppSettings["NBPRssReader_Proxy"];

            string feedString = GetFeedStringUsingProxy(url, proxy);
            XDocument doc = GetNewestNBPExchangeRatesXMLFromFeed(feedString, data);

            List<NBPExchange> list = new List<NBPExchange>();

            list = GetNBPExchangeListFromDocument(doc);

            return CreateNBPExchangeRatesDaily(doc, list);
        }
        private string GetFeedStringUsingProxy(string url, string proxy)
        {
            var webProxy = new WebProxy(proxy);
            webProxy.UseDefaultCredentials = true;
            string feedString;
            using (var webClient = new WebClient())
            {
                webClient.Proxy = webProxy;
                // Download the feed as a string
                feedString = webClient.DownloadString(url);
            }
            return feedString;
        }

        private XDocument GetNewestNBPExchangeRatesXMLFromFeed(string feedString, DateTime data)
        {
            var stringReader = new StringReader(feedString);
            XDocument doc;
            using (XmlReader reader = XmlReader.Create(stringReader))
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                SyndicationItem item = feed.Items.Where(p => p.PublishDate.Date == data.Date).FirstOrDefault();

                // Get XML with newest exchange rates
                string xmlUri = item.Links.Where(p => p.Uri.AbsoluteUri.Contains(".xml")).FirstOrDefault().Uri.AbsoluteUri;
                doc = XDocument.Load(xmlUri);
            }
            return doc;
        }

        private List<NBPExchange> GetNBPExchangeListFromDocument(XDocument doc)
        {
            var currencies = from c in doc.Descendants("pozycja")
                             select new
                             {
                                 Name = c.Element("nazwa_waluty").Value,
                                 Conversion = c.Element("przelicznik").Value,
                                 CurrencyCode = c.Element("kod_waluty").Value,
                                 AverageExchangeRate = c.Element("kurs_sredni").Value
                             };


            List<NBPExchange> nbpExchangeRates = new List<NBPExchange>();
            foreach (var cur in currencies)
            {
                int conv;
                decimal avg;
                Decimal.TryParse(cur.AverageExchangeRate, out avg);
                Int32.TryParse(cur.Conversion, out conv);
                NBPExchange nbp = new NBPExchange() { Name = cur.Name, AverageExchangeRate = avg, Conversion = conv, CurrencyCode = cur.CurrencyCode };
                nbpExchangeRates.Add(nbp);
            }
            return nbpExchangeRates;
        }

        private NBPExchangeRatesDaily CreateNBPExchangeRatesDaily(XDocument doc, List<NBPExchange> list)
        {
            NBPExchangeRatesDaily file = new NBPExchangeRatesDaily();
            

            var date = from c in doc.Descendants("tabela_kursow")
                       select new
                       {
                           TableNumber = c.Element("numer_tabeli").Value,
                           Date = c.Element("data_publikacji").Value

                       };
            DateTime publishDate;
            DateTime.TryParse(date.FirstOrDefault().Date, out publishDate);
            file.PublishDate = publishDate;
            file.Rates = list;
            file.FileName = date.Select(p => "NBP_" +p.TableNumber+"_"+p.Date).FirstOrDefault();
            return file;
        }
    }
}

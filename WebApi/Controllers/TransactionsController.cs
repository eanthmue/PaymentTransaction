using Common.Entities;
using Common.Infrastructure.Services;
using CsvHelper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
using WebApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/transactions")]
    public class TransactionsController : ApiController
    {
        protected readonly ITransactionService transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [Route("currency/{currency}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByCurrency([FromUri] string currency = null)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                return BadRequest("currency is empty");
            }

            var trans = await this.transactionService.GetByCurrency(currency);
            var results = from row in trans
                          select new VMTransaction{ Id = row.TransactionId, Payment = row.Amount + row.CurrencyCode, Status = row.Status };

            return Ok(results);
        }

        [Route("from/{fromDate}/to/{toDate}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByDateRage([FromUri] string fromDate = null, string toDate = null)
        {
            DateTime fDate = new DateTime();
            DateTime tDate = new DateTime();
            string validationMessage = "";
            if (!DateTime.TryParse(fromDate, out fDate))
            {
                validationMessage = "From Date is invalid.";
            }

            if (!DateTime.TryParse(toDate, out tDate))
            {
                validationMessage += "<br/>To Date is invalid.";
            }

            if (!string.IsNullOrEmpty(validationMessage))
            {
                return BadRequest(validationMessage);
            }

            var trans = await this.transactionService.GetByDateRange(fDate, tDate);
            var results = from row in trans
                          select new VMTransaction { Id = row.TransactionId, Payment = row.Amount + row.CurrencyCode, Status = row.Status };
            return Ok(trans);
        }

        [Route("status/{status}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByStatus([FromUri] string status = null)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return BadRequest("status is empty");
            }

            var trans = await this.transactionService.GetByCurrency(status);
            var results = from row in trans
                          select new VMTransaction { Id = row.TransactionId, Payment = row.Amount + row.CurrencyCode, Status = row.Status };
            return Ok(trans);
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IHttpActionResult> Upload([FromUri] string status = null)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                IList<Transaction> records = new List<Transaction>();
                
                var postedFile = httpRequest.Files[0];
                var filePath = HttpContext.Current.Server.MapPath("~/ImportFiles/" + postedFile.FileName);
                postedFile.SaveAs(filePath);


                /////Process CSV file
                if (filePath.EndsWith("csv"))
                {
                    string csvData = File.ReadAllText(filePath);
                    Transaction transaction;
                    bool isValidFile = true;
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            string[] cells = row.Split(',');
                            if (isValidCSVRow(cells))
                            {
                                transaction = new Transaction();
                                transaction.TransactionId = cells[0];
                                transaction.Amount = decimal.Parse(cells[1]);
                                transaction.CurrencyCode = cells[2];
                                transaction.TransactionDate = DateTime.ParseExact(cells[3], "d/M/yyyy h:m", CultureInfo.InvariantCulture);
                                transaction.Status = cells[4];
                                records.Add(transaction);
                            }
                            else
                            {
                                Log.Information("Invalid record: " + row);
                                isValidFile = false;
                            }

                        }
                    }

                    if (isValidFile)
                    {
                        await this.transactionService.BulkInsert(records);
                        return Ok(records);
                    }
                    else
                    {
                        return BadRequest("Data import failed with invalid records.");
                    }

                }else if (filePath.EndsWith("xml"))
                {
                    string[] cells;
                    Transaction transaction;
                    bool isValidFile = true;
                    foreach (XElement transactionElement in XElement.Load(filePath).Elements("Transaction"))
                    {
                        cells = new string[5];
                        cells[0] = transactionElement.Attribute("id").Value;
                        
                        cells[1] = transactionElement.Element("TransactionDate").Value;
                        
                        XElement paymentDetailsElement = transactionElement.Element("PaymentDetails");
                        cells[2] = paymentDetailsElement.Element("Amount").Value;
                        cells[3] = paymentDetailsElement.Element("CurrencyCode").Value;
                        
                        cells[4] = transactionElement.Element("Status").Value;


                        if (isValidCSVRow(cells))
                        {
                            transaction = new Transaction();
                            transaction.TransactionId = cells[0];
                            transaction.Amount = decimal.Parse(cells[2]);
                            transaction.CurrencyCode = cells[3];
                            transaction.TransactionDate = DateTime.Parse(cells[1]);
                            transaction.Status = cells[4];
                            records.Add(transaction);
                        }
                        else
                        {
                            Log.Information("Invalid record: " +  string.Join("," ,cells));
                            isValidFile = false;
                        }
                    }

                    if (isValidFile)
                    {
                        await this.transactionService.BulkInsert(records);
                        return Ok(records);
                    }
                    else
                    {
                        return BadRequest("Data import failed with invalid records.");
                    }
                }
                else
                {
                    return BadRequest("Unknown Format");
                }
                
            }
            else
            {
                return BadRequest("No file found.");
            }
        }

        private void processCSVFile()
        {

        }

        private bool isValidCSVRow(string[] cells)
        {
            if (cells.Length == 5 && !string.IsNullOrEmpty(cells[0]) && !string.IsNullOrEmpty(cells[1]) && !string.IsNullOrEmpty(cells[2])
                && !string.IsNullOrEmpty(cells[3]) && !string.IsNullOrEmpty(cells[4]))
            {
                return true;
            }


            return false;
        }
    }
}

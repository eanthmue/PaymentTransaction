using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class VMTransaction
    {
        private string status;
        public string Id { get; set; }
        public string Payment { get; set; }
        public string Status {
            get
            {
                string tmp="";
                switch (status)
                {
                    case "Approved": tmp = "A";
                        break;
                    case "Failed":
                    case "Rejected": tmp = "R";
                        break;
                    case "Finished":
                    case "Done": tmp = "D";
                        break;
                }
                return tmp;
            }
            set
            {
                status = value;
            }    
        }
    }
}
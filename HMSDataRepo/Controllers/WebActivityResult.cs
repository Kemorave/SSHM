using System;
using System.Net;

namespace HMSDataRepo.Controllers
{
    public class WebActivityResult : EventArgs
    {
        public WebActivityResult(bool isSucc, HttpStatusCode? code)
        {
            IsSuccesfull = isSucc;
            this.Code = code;
        }
        public bool IsSuccesfull { get; }
        public HttpStatusCode? Code { get; }
    }
}
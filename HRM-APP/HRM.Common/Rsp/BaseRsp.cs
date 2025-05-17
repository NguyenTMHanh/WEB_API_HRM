using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Common.Rsp
{
    public class BaseRsp
    {
        public static bool Dev { get; set; }

        private readonly string err;
        private readonly string titleError;
        private string msg;
        public bool Succes { get; private set; }
        public string Code { get; set; }
        public string Message
        {
            get
            {
                if (Succes)
                {
                    return msg;
                }
                else
                {
                    return Dev ? msg : err;
                }
            }
        }
        public BaseRsp() 
        {
            Succes = true;
            msg = string.Empty;
            titleError = "Error";

            Dev = true;

            if (string.IsNullOrEmpty(err))
            {
                err = "Please update common error in Custom Settings";
            }
        }


        public BaseRsp (string message) : this()
        {
            msg = message;
        }


        public BaseRsp(string message, string titleError) : this(message)
        {
            this.titleError = titleError;
        }

        public void SetError(string message)
        {
            Succes = false;
            msg = message;
        }

        public void SetError(string code, string message)
        {
            Succes = false;
            Code = code;
            msg = message;
        }

        public void SetMessage(string message)
        {
            msg = message;
        }

        public void TestError()
        {
            SetError("We are testing to show error message, please ignore it...");
        }

        public string Variant
        {
            get
            {
                return Succes ? "success" : "error";
            }
        }

        public string Title
        {
            get
            {
                return Succes ? "Success" : titleError;
            }
        }
    }
}

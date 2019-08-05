using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.MobileModels
{
    public class MResponse
    {
        private bool _success;
        private string _message;

        public MResponse()
        {
        }

        public MResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; set; }
        public string Message {
            get {
                if (Success)
                
                    return "Success";
                
                else 
                    return "Fail";   
            }
            set {
                _message = value;
            }
        }


    }
}
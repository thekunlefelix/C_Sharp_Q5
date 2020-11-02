using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C_Sharp_Q5.Helpers
{
    public class Utils
    {
        public const int StatusCode_Success = 0;
        public const int StatusCode_ObjectNull = 1;
        public const int StatusCode_NameEmpty = 2;
        public const int StatusCode_NotFound = 3;

        public const string StatusMessage_Success = "THE OPERATION WAS SUCCESSFUL";
        public const string StatusMessage_ObjectNull = "COUNTRY IS NULL";
        public const string StatusMessage_NameEmpty = "COUNTRY NAME IS EMPTY";
        public const string StatusMessage_NotFound = "COUNTRY NOT FOUND";
    }
}

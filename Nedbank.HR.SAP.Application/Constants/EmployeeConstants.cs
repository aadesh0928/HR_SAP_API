using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.BAL.Constants
{
    public class FilterConstant
    {
        public const string MongoFieldName = "MongoFieldName";
        public const string Operator = "Operator";
        public const string OrOperator = "Or";
        public const string AndOperator = "And";

    }

    public class ResultConstant
    {
        public const string Success = "SUCCESS";
        public const string SuccessCode = "R000";
        public const string NotFound = "Not found";
        public const string NotFoundCode = "404";
        public const string Failed = "Failed";
        public const string FailedCode = "R111";
        public const string PartialSuccess = "PARTIAL SUCCESS";
        public const string PartialSuccessCode = "R001";
        public const string BadRequest = "BAD REQUEST";
        public const string BadRequestCode = "400";


    }
}

using System;

namespace WebApplicationNeoPharm.Exceptions
{
    public class BaseBusinessException : Exception
    {
        public const string errorCodeHeaderName = "errorCode";
        public const string paramsHeaderName = "errorsParams";
        public const string stringKeyCodeHeaderName = "StringKeyCode";
        public const string stringIsRevocable = "StringIsRevocable";

        //From enum EMessages
        public int ErrorEnumCode { get; set; }
        public string ErrorStringKeyCode { get; set; }
        public object[] optinalParameters { get; set; }
        public bool IsLogExceptionToTraceLogOnMethodExit { get; set; }
        // Can the operation be forced  despite business exception being thrown
        public bool IsRevocable { get; set; }

        public BaseBusinessException(string exceptionMessage) : base(exceptionMessage)
        {
        }
    }
}

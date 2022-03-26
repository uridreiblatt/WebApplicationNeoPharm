using System;

namespace WebApplicationNeoPharm.Exceptions
{

      /// <summary>
        /// Rafi 15.02.21 -> cannot be moved to .net STD since it uses the framework resource manager
        /// </summary>
        public class BusinessException : BaseBusinessException
        {
            public BusinessException(EMessages message, bool isRevocable = false) : base("")
            {
                IsRevocable = isRevocable;
                ErrorEnumCode = (int)message;
                ErrorStringKeyCode = message.ToString();
            }

            /// <summary>
            /// Throws a business exception -> will be transalted to user locale
            /// </summary>
            /// <param name="message"> Message from messages enum</param>
            /// <param name="optionalFormattedParameters"> for message like "Rate {0} has invalid start date {1}"</param>
            public BusinessException(EMessages message, params object[] optionalFormattedParameters) : base("")
            {
                ErrorEnumCode = (int)message;
                ErrorStringKeyCode = message.ToString();
                optinalParameters = optionalFormattedParameters;
            }

            private static string GetFormattedMessage(EMessages message)
            {
                try
                {
                    return  Enum.GetName(typeof(EMessages), 1);
                }
                catch (Exception ex)
                {
                    return $"Error formatting business exception. EMessage: {Convert.ToString(message)}  | Exception: {ex}";
                }
            }

            /// <summary>
            /// Handles the Business Exception in the trace log
            /// </summary>
            /// <returns></returns>
            public BusinessException LogExceptionToTraceLogOnMethodExit()
            {
                IsLogExceptionToTraceLogOnMethodExit = true;
                return this;
            }
        }
    }

using System;

namespace FinancialStructures.ReportingStructures
{
    public class ErrorReport : IComparable
    {
        public override string ToString()
        {
            return ErrorType.ToString() + " - " + Message;

        }

        /// <summary>
        /// Method of comparison
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is ErrorReport value)
            {
                if (value.ErrorType == ErrorType)
                {
                    return Message.CompareTo(value.Message);
                }

                return ErrorType.CompareTo(value.ErrorType);
            }

            return 0;
        }

        private ReportType fErrorType;

        public ReportType ErrorType
        {
            get { return fErrorType; }
            set { fErrorType = value; }
        }

        private string fMessage;
        public string Message
        {
            get { return fMessage; }
            set { fMessage = value; }
        }

        public ErrorReport()
        {
        }

        public ErrorReport(ReportType type, string msg)
        {
            ErrorType = type;
            Message = msg;
        }
    }
}

using System;

namespace FinancialStructures.Reporting
{
    public class ErrorReport : IComparable
    {
        public override string ToString()
        {
            return ErrorType.ToString() + " - " + ErrorLocation.ToString() + " - " + Message;

        }

        /// <summary>
        /// Method of comparison
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is ErrorReport value)
            {
                if (value.ErrorType.Equals(ErrorType))
                {
                    if (value.ErrorLocation.Equals(ErrorLocation))
                    {
                        return Message.CompareTo(value.Message);
                    }

                    return value.ErrorLocation.CompareTo(ErrorLocation);
                }

                return ErrorType.CompareTo(value.ErrorType);
            }

            return 0;
        }

        private Severity fErrorSeverity;

        public Severity ErrorSeverity
        {
            get { return fErrorSeverity; }
            set { fErrorSeverity = value; }
        }

        private ReportType fErrorType;

        public ReportType ErrorType
        {
            get { return fErrorType; }
            set { fErrorType = value; }
        }

        private Location fErrorLocation;

        public Location ErrorLocation
        {
            get { return fErrorLocation; }
            set { fErrorLocation = value; }
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

        public ErrorReport(ReportType type, Location errorLocation, string msg)
        {
            ErrorType = type;
            ErrorLocation = errorLocation;
            Message = msg;
        }

        public ErrorReport(Severity severity, ReportType type, Location errorLocation, string msg)
        {
            ErrorSeverity = severity;
            ErrorType = type;
            ErrorLocation = errorLocation;
            Message = msg;
        }
    }
}

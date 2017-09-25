using System;

namespace Kernel.Federation.Exceptions
{
    public class TimeRestrictionViolationException : FederationException
    {
        string _violatedIn;

        string _timeRestrictionName;

        DateTimeOffset _notBeforeValue;

        DateTimeOffset _violatedAt;

        TimeSpan _maxClockSkew;

        public TimeRestrictionViolationException(string timeRestrictionName, string violatedIn, DateTimeOffset notBeforeValue, DateTimeOffset now, TimeSpan maxClockSkew)
        {
            _timeRestrictionName = timeRestrictionName;

            _violatedIn = violatedIn;

            _notBeforeValue = notBeforeValue;

            _violatedAt = now;

            _maxClockSkew = maxClockSkew;
        }

        public override string Message
        {
            get
            {
                return string.Format("{0} restriction has been violated in {1} at {2}. Not before value: {3}. Clock skew: {4}.", _timeRestrictionName, _violatedIn, _violatedAt, _notBeforeValue, _maxClockSkew);
            }
        }
    }
}
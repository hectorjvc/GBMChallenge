using System;

namespace Utilities
{
    public class Guard
    {
        public static void ForLessEqualZero(int value, string parameterName)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} cannot be equal to ZERO");
        }

        public static void ForLessEqualZero(decimal value, string parameterName)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} cannot be equal to ZERO");
        } 

        public static void ForLessEqualZero(long value, string parameterName)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} cannot be equal to ZERO");
        }

        public static void MustBeEqualToZero(int value, string parameterName)
        {
            if (value != 0)
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} must be assigned value of ZERO");
        }

        public static void ForNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} cannot be Null or Empty");
        }

        public static void ForValidGuid(Guid value, string parameterName)
        {
            if (value == Guid.Empty)
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} cannot be Null or Empty");
        }

        public static void ForNullObject(object target, string parameterName)
        {
            if (target == null)
                throw new ArgumentNullException(parameterName, $"{parameterName} cannot be Null or Empty");
        }
    }
}
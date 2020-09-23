using System;

namespace AvaDevBox.Utils
{
    public static class Extensions
    {
        /// <summary>Limits a comparable value to the given upper and lower limits (inclusive).</summary>
        /// <typeparam name="T">IComparable Type to applied to.</typeparam>
        /// <param name="input">The input to limit to upper an lower limit.</param>
        /// <param name="lowerLimit">The lower limit.</param>
        /// <param name="upperLimit">The upper limit.</param>
        /// <returns>The value itself or either of the limits, if out of range.</returns>
        public static T LimitTo<T>(this T input, T lowerLimit, T upperLimit) where T : IComparable<T>
        {
            return input.LimitToAtLeast(lowerLimit).LimitToAtMost(upperLimit);
        }

        /// <summary>Limits a comparable value to the given lower limit (inclusive).</summary>
        /// <typeparam name="T">IComparable Type to applied to.</typeparam>
        /// <param name="input">The input to limit to lower limit.</param>
        /// <param name="lowerLimit">The lower limit to be not smaller than.</param>
        /// <returns>The value itself or the limit, if out of range.</returns>
        public static T LimitToAtLeast<T>(this T input, T lowerLimit) where T : IComparable<T>
        {
            return input.CompareTo(lowerLimit) < 0 ? lowerLimit : input;
        }

        /// <summary>Limits a comparable value to the given upper limit (inclusive).</summary>
        /// <typeparam name="T">IComparable Type to applied to.</typeparam>
        /// <param name="input">The input to limit to upper limit.</param>
        /// <param name="upperLimit">The upper limit to be not greater than.</param>
        /// <returns>The value itself or the limit, if out of range.</returns>
        public static T LimitToAtMost<T>(this T input, T upperLimit) where T : IComparable<T>
        {
            return input.CompareTo(upperLimit) > 0 ? upperLimit : input;
        }
    }
}

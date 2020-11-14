using System;

namespace FromLocalsToLocals.Utilities
{
    public static class StringExtensions
    {
        public static T ParseEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}

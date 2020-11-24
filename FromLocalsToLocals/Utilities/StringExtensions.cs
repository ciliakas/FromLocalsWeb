using Microsoft.AspNetCore.Http;
using SuppLocals;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FromLocalsToLocals.Utilities
{
    public static class StringExtensions
    {
        public static T ParseEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static bool ValidImage(this IFormFile image)
        {
            if (image != null)
            {
                if (image.Length > 0)
                {
                    var ext = Path.GetExtension(image.FileName).ToLowerInvariant();

                    if (string.IsNullOrEmpty(ext) || Config.permittedExtensions.Contains(ext))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

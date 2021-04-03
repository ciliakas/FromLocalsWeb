using System.Globalization;
using System.Threading; //using System.Web.SessionState;

namespace FromLocalsToLocals.Utilities
{
    public class CultureHelper
    {
        public static int CurrentCulture
        {
            get
            {
                if (Thread.CurrentThread.CurrentUICulture.Name == "en")
                    return 0;
                if (Thread.CurrentThread.CurrentUICulture.Name == "lt")
                    return 1;
                return 0;
            }
            set
            {
                if (value == 0)
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                else if (value == 1)
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("lt");
                else
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }
        }
    }
}
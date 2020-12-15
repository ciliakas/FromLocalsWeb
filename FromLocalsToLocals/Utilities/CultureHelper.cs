using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.SessionState;
using System.Threading;
using System.Globalization;

namespace FromLocalsToLocals.Utilities
{
    public class CultureHelper
    {

        public static int CurrentCulture
        {
            get
            {
                if (Thread.CurrentThread.CurrentUICulture.Name == "en")
                {
                    return 0;
                }
                else if (Thread.CurrentThread.CurrentUICulture.Name == "lt")
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value == 0)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                }
                else if (value == 1)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("lt");
                }
                else
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                }
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }
        }
    }
}

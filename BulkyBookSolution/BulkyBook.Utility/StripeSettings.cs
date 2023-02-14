using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Utility
{
    public class StripeSettings
    {
        // these properties should match the appsettings.json properties
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
}

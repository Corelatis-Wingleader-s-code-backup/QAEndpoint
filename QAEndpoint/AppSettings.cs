using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAEndpoint {
    public record AppSettings(
     string ServiceUrl,
     string AssemblyPath,
     string LicenseStart,
     string HostAddress);

    public static class AppHelper {
        public static AppSettings AppSettings { get; set; }
    }
}

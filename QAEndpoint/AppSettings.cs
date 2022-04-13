using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAEndpoint {
    //Step1.创建一个记录对象，其成员名称与json配置文件中的
    //配置项名称完全一致
    //*这里使用class也是可以的
    //*这个类的名称是可以自己决定的
    //*使用record可以阻止配置被初始化之后在其他地方被意外修改
    public record AppSettings {
        public string ServiceUrl { get; init; }
        public string AssemblyPath { get; init; }
        public string LicenseStart { get; init; }
        public string HostAddress { get; init; }
    }

    public static class AppHelper {
        public static AppSettings AppSettings { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace ArkDesktop.CoreKit
{
    public static class UpdateChecker
    {
        public static (string, string) GetUpdateInfo(List<PluginModuleInfo> modules = null, int helperVersion = 0)
        {
            var additionalParams = new StringBuilder();
            void AddParam(string guid, int version)
            {
                additionalParams.Append("&modules[]=");
                additionalParams.Append(guid);
                additionalParams.Append('|');
                additionalParams.Append(version);
            }
            if (modules != null)
                foreach (PluginModuleInfo i in modules)
                    AddParam(i.moduleGuid.ToString(), i.version);
            AddParam("00000000-0000-0000-0000-000000000000", helperVersion);

            WebClient client = new WebClient();
            try
            {
                var st = client.OpenRead("https://akd.huix.cc/api/GetUpdate.php?current=" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + additionalParams.ToString());
                string s;
                using (StreamReader sr = new StreamReader(st)) s = sr.ReadToEnd();
                if (s == "Latest") return ("Latest", null);
                var a = s.Split('|');
                if (a.Length < 2) return ("", null);
                if (a.Length > 2)
                {
                    string a1 = "";
                    for (int i = 1; i != a.Length; ++i) a1 += a[i] + '|';
                    a1 = a1.Substring(0, a1.Length - 1);
                }
                return (a[0], a[1]);
            }
            catch (Exception)
            {
                return ("", null);
            }
        }
        public static string CoreVersion { get => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); }

    }
}

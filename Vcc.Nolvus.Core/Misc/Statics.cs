using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Misc
{
    public class ENBs
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public static List<ENBs> GetAvailableENBs()
        {
            List<ENBs> ENBList = new List<ENBs>();

            ENBs PICHO = new ENBs { Code = "PICHO", Name = "PI-CHO" };
            ENBs RUDY = new ENBs { Code = "RUDY", Name = "Rudy" };
            ENBs BJORN = new ENBs { Code = "BJREG", Name = "Bjorn" };
            ENBs BJORNDARK = new ENBs { Code = "BJAGE", Name = "Bjorn Dark Ages" };
            ENBs BJORNWOLF = new ENBs { Code = "BJWOL", Name = "Bjorn The White Wolf Cut" };

            ENBList.Add(PICHO);
            ENBList.Add(RUDY);
            ENBList.Add(BJORN);
            ENBList.Add(BJORNDARK);
            ENBList.Add(BJORNWOLF);

            return ENBList;
        }

        public static string GetENBByCode(string Code)
        {
            return ENBs.GetAvailableENBs().Where(x => x.Code == Code).Select(x => x.Name).FirstOrDefault();
        }
    }

    public static class CDN
    {
        public static List<string> Get()
        {
            List<string> Result = new List<string>();

            Result.Add("Nexus CDN");
            Result.Add("Amsterdam");
            Result.Add("Prague");
            Result.Add("Chicago");
            Result.Add("Los Angeles");
            Result.Add("Miami");

            return Result;
        }
    }
}

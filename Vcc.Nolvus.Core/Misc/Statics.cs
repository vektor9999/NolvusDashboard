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
        public string Description { get; set; }

        public static List<ENBs> GetAvailableENBsForV5()
        {
            List<ENBs> ENBList = new List<ENBs>();

            ENBs PICHO = new ENBs { Code = "PICHO", Name = "PI-CHO", Description = string.Empty };
            ENBs RUDY = new ENBs { Code = "RUDY", Name = "Rudy", Description = string.Empty };
            ENBs BJORN = new ENBs { Code = "BJREG", Name = "Bjorn", Description = string.Empty };
            ENBs BJORNDARK = new ENBs { Code = "BJAGE", Name = "Bjorn Dark Ages", Description = string.Empty };
            ENBs BJORNWOLF = new ENBs { Code = "BJWOL", Name = "Bjorn The White Wolf Cut", Description = string.Empty };

            ENBList.Add(PICHO);
            ENBList.Add(RUDY);
            ENBList.Add(BJORN);
            ENBList.Add(BJORNDARK);
            ENBList.Add(BJORNWOLF);

            return ENBList;
        }

        public static List<ENBs> GetAvailableENBsForV6()
        {
            List<ENBs> ENBList = new List<ENBs>();

            ENBs CABBAGE = new ENBs { Code = "CABBAGE", Name = "Cabbage", Description = "The famous Cabbage ENB from Nexus. Its visual design aims to provide you with a vibrant, unique, and diverse Skyrim experience. Comes with the Nolvus Reshade preset designed for it. This is the version used in the Nolvus v6 showcase videos." };            
            ENBs KAUZ = new ENBs { Code = "KAUZ", Name = "Kauz", Description = "An ENB preset that utilizes the Silent Horizons 2 Shader Core. This ENB stands out with dramatic lighting and atmospheric color schemes while preserving playablity." };
            ENBs PICHO = new ENBs { Code = "PICHO", Name = "PiCho", Description = "Based on Silent Horizon. It is an ambitious project that aims to bring modern game graphics to Skyrim. Character, environment, and gameplay were all well-balanced. There are various customizations that are either the same as or different from the original." };
            ENBs AMON = new ENBs { Code = "AMON", Name = "Amon Reborn", Description = "The definitive lightweight AMON Preset for NAT 3 Weather with 100 weathers edited." };

            ENBList.Add(CABBAGE);            
            ENBList.Add(KAUZ);
            ENBList.Add(PICHO);
            ENBList.Add(AMON);

            return ENBList;
        }

        public static string GetENBByCode(string Code)
        {
            string Result =  ENBs.GetAvailableENBsForV5().Where(x => x.Code == Code).Select(x => x.Name).FirstOrDefault();                       

            if (Result == null)
            {
                Result = ENBs.GetAvailableENBsForV6().Where(x => x.Code == Code).Select(x => x.Name).FirstOrDefault();
            }

            return Result;
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

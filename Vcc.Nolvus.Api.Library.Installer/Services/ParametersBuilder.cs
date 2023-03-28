using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Api.Installer.Services
{
    public static class ParametersBuilder
    {

        public static string GetQueryStringFromParameters(Dictionary<string, object> Parameters)
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append("?");

            CultureInfo Culture = new CultureInfo("en-US");

            Culture.DateTimeFormat.DateSeparator = "-";

            int Counter = 0;

            foreach(KeyValuePair<string, object> Item in Parameters)
            {
                Builder.Append(Item.Key);
                Builder.Append("=");

                Type MyType = Item.Value.GetType();

                if (Item.Value is string)
                {
                    Builder.Append(Item.Value.ToString());
                }
                else if (Item.Value is int)
                {
                    Builder.Append(Item.Value.ToString());
                }
                else if (Item.Value is double)
                {
                    Builder.Append(System.Convert.ToDouble(Item.Value).ToString(Culture));
                }
                else if (Item.Value is DateTime)
                {
                    Builder.Append(System.Convert.ToDateTime(Item.Value).ToString(Culture));
                }
                else if (Item.Value is bool)
                {
                    Builder.Append(Item.Value.ToString());
                }

                if ( Counter != Parameters.Values.Count - 1)
                {
                    Builder.Append("&");
                }
                
                Counter++;
            }

            return Builder.ToString();
        }
    }
}

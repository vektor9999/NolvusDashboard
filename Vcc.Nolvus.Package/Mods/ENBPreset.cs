using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Package.Rules;
using Vcc.Nolvus.Package.Conditions;
using Vcc.Nolvus.Package.Patchers;
using System.Xml;
using ZetaLongPaths;

namespace Vcc.Nolvus.Package.Mods
{
    public class ENBPreset : NexusMod, IENBPreset
    {

        public bool Installed
        {
            get
            {
                return GetFieldValueByKey("EnbCode") == ServiceSingleton.Instances.WorkingInstance.Options.AlternateENB;
            }
        }

        public override void Load(XmlNode Node, List<InstallableElement> Elements)
        {
            base.Load(Node, Elements);            
        }

        protected override async Task PrepareDirectrory()
        {
            await Remove();
        }

        public override async Task Remove()
        {
            var Tsk = Task.Run(() =>
            {
                try
                {                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            await Tsk;
        }
    }
}

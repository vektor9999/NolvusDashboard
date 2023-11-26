using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Package.Rules;

namespace Vcc.Nolvus.Package.Mods
{
    public abstract class Software : InstallableElement
    {
        public List<Rule> Rules = new List<Rule>();

        public override bool IsInstallable()
        {
            return true;
        }

        public override void Load(XmlNode Node, List<InstallableElement> Elements)
        {
            base.Load(Node, Elements);

            #region Rules

            Rules.Clear();

            XmlNode RulesNode = Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Rules").FirstOrDefault();

            if (RulesNode != null)
            {
                foreach (XmlNode RuleNode in RulesNode.ChildNodes.Cast<XmlNode>().ToList())
                {
                    Rule Rule = Activator.CreateInstance(Type.GetType("Vcc.Nolvus.Package.Rules." + RuleNode["Type"].InnerText)) as Rule;

                    Rule.Load(RuleNode);

                    Rules.Add(Rule);
                }
            }

            #endregion

            Elements.Add(this);
        }

        public override string ArchiveFolder
        {
            get
            {
                return ServiceSingleton.Instances.WorkingInstance.ArchiveDir;
            }
        }

        protected override Task DoUnpack()
        {
            return Task.CompletedTask;
        }

        protected override Task DoPatch()
        {
            return Task.CompletedTask;
        }     

        public override Task Remove()
        {
            return Task.CompletedTask;
        }        
    }
}

using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Package.Conditions;

namespace Vcc.Nolvus.Package.Rules
{
    public abstract class Rule
    {
        public List<RuleCondition> Conditions = new List<RuleCondition>();

        public bool Force { get; set; }
        public virtual bool IsPriority
        {
            get { return false; }
        }        

        public virtual void Load(XmlNode Node)
        {
            Force = false;

            Conditions.Clear();

            if (Node["Force"] != null) Force = System.Convert.ToBoolean(Node["Force"].InnerText);            

            XmlNode ConditionsNode = Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Conditions").FirstOrDefault();

            if (ConditionsNode != null)
            {
                foreach (XmlNode ConditionNode in ConditionsNode.ChildNodes.Cast<XmlNode>().ToList())
                {
                    RuleCondition Condition = Activator.CreateInstance(Type.GetType("Vcc.Nolvus.Package.Conditions." + ConditionNode["Type"].InnerText)) as RuleCondition;

                    Condition.Load(ConditionNode);

                    Conditions.Add(Condition);
                }
            }            
        }

        public abstract void Execute(string GamePath, string ExtractDir, string ModDir, string InstanceDir);        

        protected virtual bool CanExecute(string GamePath, string InstallDir)
        {
            foreach(var Condition in Conditions)
            {
                if (!Condition.IsValid(GamePath, InstallDir))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

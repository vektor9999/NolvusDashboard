using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Interfaces;


namespace Vcc.Nolvus.Package.Mods
{
    public abstract class MOElement : InstallableElement, IMOElement
    {
        #region Fields

        private string RootDir = Path.Combine(ServiceSingleton.Instances.WorkingInstance.InstallDir, "MODS", "mods");

        protected const string MetaIni = @"[General]
modid={0}
version={1}
newestVersion=
category=0
installationFile={2}

[installedFiles]
size=0
1\modid={0}
1\fileid={3}";

        #endregion

        #region Properties

        public bool Display { get; set; }

        public string MoDirectoryFullName
        {
            get
            {
                return Path.Combine(RootDir, MoDirectoryName);
            }
        }

        public abstract string MoDirectoryName {get;}

        #endregion

        #region Methods       

        public override void Load(XmlNode Node, List<InstallableElement> Elements)
        {
            base.Load(Node, Elements);
            Display = System.Convert.ToBoolean(Node["Display"].InnerText);
        }

        protected abstract void CreateElementIni();

        #endregion


    }
}

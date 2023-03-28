using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.StockGame.Core;

namespace Vcc.Nolvus.StockGame.Meta
{
    public class PatchingInstruction
    {
        public PatcherAction Action { get; set; }
        public string PatchFile { get; set; }
        public GameFile SourceFile { get; set; }
        public GameFile DestFile { get; set; }
        public string DownLoadLink { get; set; }

        public void Parse(XmlNode Node)
        {            
            Action = (PatcherAction)Enum.Parse(typeof(PatcherAction), Node["Action"].InnerText);

            if (Node["SourceFile"] != null)
            {
                SourceFile = new GameFile();
                SourceFile.Parse(Node["SourceFile"]);
            }

            if (Node["DestFile"].ChildNodes.Count != 0)
            {
                DestFile = new GameFile();
                DestFile.Parse(Node["DestFile"]);
            }
            

            PatchFile = Node["PatchFile"].InnerText;
            DownLoadLink = Node["DownloadLink"].InnerText;
        }

        //private void DoDeleteFile(string Dir)
        //{
        //    File.Delete(Path.Combine(Dir, this.SourceFile.Name));
        //}

        //private void DoPatchFile(string GameDir, string StockGameDir)
        //{

        //}

        //public void Execute(string GameDir, string StockGameDir)
        //{
        //    switch (Action)
        //    {
        //        case PatcherAction.Delete:
        //            this.DoDeleteFile(StockGameDir);
        //            break;
        //        case PatcherAction.Patch:
        //            this.DoPatchFile(GameDir, StockGameDir);
        //            break;

        //    }
        //}
    }
}

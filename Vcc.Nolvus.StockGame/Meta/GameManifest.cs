using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace Vcc.Nolvus.StockGame.Meta
{
    public class GameManifest
    {
        public string Name { get; set; }
        public string ExeName { get; set; }
        public string Version { get; set; }
        public List<GameFile> Files { get; set; }
        public List<PatchingInstruction> Instructions { get; set; }

        public GameManifest()
        {
            Files = new List<GameFile>();
            Instructions = new List<PatchingInstruction>();
        }
        
        public void Parse(XmlNode Node)
        {
            Name =  Node["Name"].InnerText;
            ExeName = Node["ExeName"].InnerText;
            Version = Node["Version"].InnerText;
        }
        
        public void AddFile(XmlNode Node)
        {
            var GameFile = new GameFile();
            GameFile.Parse(Node);

            this.Files.Add(GameFile);
        }

        public void AddInstruction(XmlNode Node)
        {
            var PatchingInstruction = new PatchingInstruction();
            PatchingInstruction.Parse(Node);

            this.Instructions.Add(PatchingInstruction);
        }
    }
}

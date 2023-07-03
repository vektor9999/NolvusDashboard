using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Services;
using System.Xml;

namespace Vcc.Nolvus.Package.Rules
{
    public class DirectoryCopy : CopyRule
    {
        public bool IncludeRootDirectory { get; set; }

        public override void Load(XmlNode Node)
        {
            base.Load(Node);
            IncludeRootDirectory = System.Convert.ToBoolean(Node["IncludeRootDirectory"].InnerText);
        }

        private FileCopy CreateFileCopyRule(int Destination, string Source, string DestinationDirectory)
        {
            return new FileCopy {
                Destination = Destination,
                NewFileName = string.Empty,
                Source = Source,
                DestinationDirectory = DestinationDirectory
            };                    
        }

        public List<Rule> CreateFileRules(string ExtactDir, int Destination, string GamePath, string ModDir)
        {
            var Rules = new List<Rule>();

            if (this.CanExecute(GamePath, ModDir))
            {
                foreach (var File in ServiceSingleton.Files.GetFiles(Path.Combine(ExtactDir, this.Source)))
                {
                    var SourceDir = File.FullName.Replace(ExtactDir, string.Empty).Substring(1);
                    var DestDirectory = File.Directory.FullName.Replace(ExtactDir, string.Empty);

                    if (this.Source != string.Empty)
                    {
                        var Dir = Source;

                        if (IncludeRootDirectory)
                        {
                            Dir = Source.Replace(new DirectoryInfo(this.Source).Name, string.Empty);
                        }

                        if (Dir != string.Empty)
                        {
                            var regex = new Regex(Regex.Escape(Dir));
                            DestDirectory = regex.Replace(DestDirectory, string.Empty, 1);
                        }
                    }

                    DestDirectory = DestDirectory + DestinationDirectory;

                    Rules.Add(CreateFileCopyRule(Destination, SourceDir, DestDirectory.TrimStart('\\')));
                }
            }

            return Rules;
        }

        public override void Execute(string GamePath, string ExtractDir, string ModDir, string InstanceDir)
        {
            if (this.CanExecute(GamePath, ModDir))
            {
                string Destination = InstanceDir;

                if (this.Destination == 0)
                {
                    Destination = ModDir;
                }
                else if( this.Destination == 1)
                {
                    Destination = GamePath;
                }                

                if (!CopyToRoot)
                {
                    Destination = Path.Combine(Destination, DestinationDirectory);

                    Directory.CreateDirectory(Destination);
                }

                if (!this.IncludeRootDirectory)
                {
                    ServiceSingleton.Files.CopyFiles(Path.Combine(ExtractDir , this.Source), Destination, false);
                }
                else
                {
                    DirectoryInfo DirInfo = new DirectoryInfo(Path.Combine(ExtractDir, this.Source));

                    ServiceSingleton.Files.CopyFiles(Path.Combine(ExtractDir, this.Source), Path.Combine(Destination, DirInfo.Name), true);
                }
            }                                   
        }        
    }
}

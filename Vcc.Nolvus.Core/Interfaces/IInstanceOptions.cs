using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IInstanceOptions
    {
        string Nudity { get; set; }
        string AlternateENB { get; set; }
        string FantasyMode { get; set; }
        string HardcoreMode { get; set; }
        string AlternateLeveling { get; set; }
        string SkinType { get; set; }
        string AlternateStart { get; set; }
        string CombatAnimation { get; set; }
        string StancesPerksTree { get; set; }
        string DeleveledEnemies { get; set; }
        string Exhaustion { get; set; }
        string NerfPA { get; set; }
        string EnemiesResistance { get; set; }
        string Boss { get; set; }
        string Poise { get; set; }
        string Gore { get; set; }
        string CombatScaling { get; set; }
        string Controller { get; set; }
    }
}

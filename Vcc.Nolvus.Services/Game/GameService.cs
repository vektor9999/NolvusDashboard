using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Misc;

namespace Vcc.Nolvus.Services.Game
{
    public class GameService : IGameService
    {
        public const string SkyrimSE = "The Elder Scrolls V: Skyrim Special Edition";

        private static RegistryKey GetInstalledApplication(string AppName)
        {
            string displayName;
            RegistryKey key;

            key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            string[] Arr1 = key.GetSubKeyNames();
            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (AppName.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return subkey;
                }
            }

            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            string[] Arr2 = key.GetSubKeyNames();
            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (AppName.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return subkey;
                }
            }

            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            string[] Arr3 = key.GetSubKeyNames();
            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                if (AppName.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return subkey;
                }
            }

            return null;
        }

        public bool IsGameInstalled()
        {
            return GetInstalledApplication(SkyrimSE) != null;
        }

        public string GetSkyrimSEDirectory()
        {
            if (IsGameInstalled())
            {
                return GetInstalledApplication(SkyrimSE).GetValue("InstallLocation") as string;
            }

            return string.Empty;            
        }

        public List<string> GamePlugins
        {
            get
            {
                var Lines = new List<string>();

                Lines.Add("-0. MASTER FILES_separator");
                Lines.Add("*Creation Club: ccvsvsse004-beafarmer");
                Lines.Add("*Creation Club: ccvsvsse003-necroarts");
                Lines.Add("*Creation Club: ccvsvsse002-pets");
                Lines.Add("*Creation Club: ccvsvsse001-winter");
                Lines.Add("*Creation Club: cctwbsse001-puzzledungeon");
                Lines.Add("*Creation Club: ccrmssse001-necrohouse");
                Lines.Add("*Creation Club: ccqdrsse002-firewood");
                Lines.Add("*Creation Club: ccpewsse002-armsofchaos");
                Lines.Add("*Creation Club: ccmtysse002-ve");
                Lines.Add("*Creation Club: ccmtysse001-knightsofthenine");
                Lines.Add("*Creation Club: cckrtsse001_altar");
                Lines.Add("*Creation Club: ccfsvsse001-backpacks");
                Lines.Add("*Creation Club: ccffbsse002-crossbowpack");
                Lines.Add("*Creation Club: ccffbsse001-imperialdragon");
                Lines.Add("*Creation Club: cceejsse005-cave");
                Lines.Add("*Creation Club: cceejsse004-hall");
                Lines.Add("*Creation Club: cceejsse003-hollow");
                Lines.Add("*Creation Club: cceejsse002-tower");
                Lines.Add("*Creation Club: cceejsse001-hstead");
                Lines.Add("*Creation Club: ccedhsse003-redguard");
                Lines.Add("*Creation Club: ccedhsse002-splkntset");
                Lines.Add("*Creation Club: ccedhsse001-norjewel");
                Lines.Add("*Creation Club: cccbhsse001-gaunt");
                Lines.Add("*Creation Club: ccbgssse069-contest");
                Lines.Add("*Creation Club: ccbgssse068-bloodfall");
                Lines.Add("*Creation Club: ccbgssse067-daedinv");
                Lines.Add("*Creation Club: ccbgssse066-staves");
                Lines.Add("*Creation Club: ccbgssse064-ba_elven");
                Lines.Add("*Creation Club: ccbgssse063-ba_ebony");
                Lines.Add("*Creation Club: ccbgssse062-ba_dwarvenmail");
                Lines.Add("*Creation Club: ccbgssse061-ba_dwarven");
                Lines.Add("*Creation Club: ccbgssse060-ba_dragonscale");
                Lines.Add("*Creation Club: ccbgssse059-ba_dragonplate");
                Lines.Add("*Creation Club: ccbgssse058-ba_steel");
                Lines.Add("*Creation Club: ccbgssse057-ba_stalhrim");
                Lines.Add("*Creation Club: ccbgssse056-ba_silver");
                Lines.Add("*Creation Club: ccbgssse055-ba_orcishscaled");
                Lines.Add("*Creation Club: ccbgssse054-ba_orcish");
                Lines.Add("*Creation Club: ccbgssse053-ba_leather");
                Lines.Add("*Creation Club: ccbgssse052-ba_iron");
                Lines.Add("*Creation Club: ccbgssse051-ba_daedricmail");
                Lines.Add("*Creation Club: ccbgssse050-ba_daedric");
                Lines.Add("*Creation Club: ccbgssse045-hasedoki");
                Lines.Add("*Creation Club: ccbgssse043-crosselv");
                Lines.Add("*Creation Club: ccbgssse041-netchleather");
                Lines.Add("*Creation Club: ccbgssse040-advobgobs");
                Lines.Add("*Creation Club: ccbgssse038-bowofshadows");
                Lines.Add("*Creation Club: ccbgssse036-petbwolf");
                Lines.Add("*Creation Club: ccbgssse035-petnhound");
                Lines.Add("*Creation Club: ccbgssse034-mntuni");
                Lines.Add("*Creation Club: ccbgssse031-advcyrus");
                Lines.Add("*Creation Club: ccbgssse021-lordsmail");
                Lines.Add("*Creation Club: ccbgssse020-graycowl");
                Lines.Add("*Creation Club: ccbgssse019-staffofsheogorath");
                Lines.Add("*Creation Club: ccbgssse018-shadowrend");
                Lines.Add("*Creation Club: ccbgssse016-umbra");
                Lines.Add("*Creation Club: ccbgssse014-spellpack01");
                Lines.Add("*Creation Club: ccbgssse013-dawnfang");
                Lines.Add("*Creation Club: ccbgssse012-hrsarmrstl");
                Lines.Add("*Creation Club: ccbgssse011-hrsarmrelvn");
                Lines.Add("*Creation Club: ccbgssse010-petdwarvenarmoredmudcrab");
                Lines.Add("*Creation Club: ccbgssse008-wraithguard");
                Lines.Add("*Creation Club: ccbgssse007-chrysamere");
                Lines.Add("*Creation Club: ccbgssse006-stendarshammer");
                Lines.Add("*Creation Club: ccbgssse005-goldbrand");
                Lines.Add("*Creation Club: ccbgssse004-ruinsedge");
                Lines.Add("*Creation Club: ccbgssse003-zombies");
                Lines.Add("*Creation Club: ccbgssse002-exoticarrows");
                Lines.Add("*Creation Club: ccasvsse001-almsivi");
                Lines.Add("*Creation Club: ccafdsse001-dwesanctuary");
                Lines.Add("*Creation Club: ccqdrsse001-survivalmode");
                Lines.Add("*Creation Club: ccbgssse037-curios");
                Lines.Add("*Creation Club: ccbgssse025-advdsgs");
                Lines.Add("*Creation Club: ccbgssse001-fish");
                Lines.Add("*DLC: HearthFires");
                Lines.Add("*DLC: Dragonborn");
                Lines.Add("*DLC: Dawnguard");

                return Lines;
            }
        }

        public List<ModObject> GamePluginAsObjects()
        {           
            return GamePlugins.Skip(1).Select(x => {

                var Counter = 0;

                var Mod = new ModObject
                {
                    Selected = true,
                    Priority = Counter,
                    Name = x.Substring(1),
                    Category = "0. MASTER FILES",
                    Version = "NA",
                    Status = ModObjectStatus.OK,
                    StatusText = "OK"
                };

                Counter++;

                return Mod;
            }).ToList();
        }

        public List<string> GamePluginFiles
        {
            get
            {
                var Lines = new List<string>();

                Lines.Add("Skyrim.esm");
                Lines.Add("Update.esm");
                Lines.Add("Dawnguard.esm");
                Lines.Add("HearthFires.esm");
                Lines.Add("Dragonborn.esm");
                Lines.Add("ccasvsse001-almsivi.esm");
                Lines.Add("ccbgssse001-Fish.esm");
                Lines.Add("ccbgssse002-exoticarrows.esl");
                Lines.Add("ccbgssse003-zombies.esl");
                Lines.Add("ccbgssse004-ruinsedge.esl");
                Lines.Add("ccbgssse005-goldbrand.esl");
                Lines.Add("ccbgssse006-stendarshammer.esl");
                Lines.Add("ccbgssse007-chrysamere.esl");
                Lines.Add("ccbgssse010-petdwarvenarmoredmudcrab.esl");
                Lines.Add("ccbgssse011-hrsarmrelvn.esl");
                Lines.Add("ccbgssse012-hrsarmrstl.esl");
                Lines.Add("ccbgssse014-spellpack01.esl");
                Lines.Add("ccbgssse019-staffofsheogorath.esl");
                Lines.Add("ccbgssse020-graycowl.esl");
                Lines.Add("ccbgssse021-lordsmail.esl");
                Lines.Add("ccmtysse001-knightsofthenine.esl");
                Lines.Add("ccqrdsse001-SurvivalMode.esl");
                Lines.Add("cctwbsse001-puzzledungeon.esm");
                Lines.Add("cceejsse001-hstead.esm");
                Lines.Add("ccqdrsse002-firewood.esl");
                Lines.Add("ccbgssse018-shadowrend.esl");
                Lines.Add("ccbgssse035-petnhound.esl");
                Lines.Add("ccfsvsse001-backpacks.esl");
                Lines.Add("cceejsse002-tower.esl");
                Lines.Add("ccedhsse001-norjewel.esl");
                Lines.Add("ccvsvsse002-pets.esl");
                Lines.Add("ccbgssse037-Curios.esl");
                Lines.Add("ccbgssse034-mntuni.esl");
                Lines.Add("ccbgssse045-hasedoki.esl");
                Lines.Add("ccbgssse008-wraithguard.esl");
                Lines.Add("ccbgssse036-petbwolf.esl");
                Lines.Add("ccffbsse001-imperialdragon.esl");
                Lines.Add("ccmtysse002-ve.esl");
                Lines.Add("ccbgssse043-crosselv.esl");
                Lines.Add("ccvsvsse001-winter.esl");
                Lines.Add("cceejsse003-hollow.esl");
                Lines.Add("ccbgssse016-umbra.esm");
                Lines.Add("ccbgssse031-advcyrus.esm");
                Lines.Add("ccbgssse038-bowofshadows.esl");
                Lines.Add("ccbgssse040-advobgobs.esl");
                Lines.Add("ccbgssse050-ba_daedric.esl");
                Lines.Add("ccbgssse052-ba_iron.esl");
                Lines.Add("ccbgssse054-ba_orcish.esl");
                Lines.Add("ccbgssse058-ba_steel.esl");
                Lines.Add("ccbgssse059-ba_dragonplate.esl");
                Lines.Add("ccbgssse061-ba_dwarven.esl");
                Lines.Add("ccpewsse002-armsofchaos.esl");
                Lines.Add("ccbgssse041-netchleather.esl");
                Lines.Add("ccedhsse002-splkntset.esl");
                Lines.Add("ccbgssse064-ba_elven.esl");
                Lines.Add("ccbgssse063-ba_ebony.esl");
                Lines.Add("ccbgssse062-ba_dwarvenmail.esl");
                Lines.Add("ccbgssse060-ba_dragonscale.esl");
                Lines.Add("ccbgssse056-ba_silver.esl");
                Lines.Add("ccbgssse055-ba_orcishscaled.esl");
                Lines.Add("ccbgssse053-ba_leather.esl");
                Lines.Add("ccbgssse051-ba_daedricmail.esl");
                Lines.Add("ccbgssse057-ba_stalhrim.esl");
                Lines.Add("ccbgssse066-staves.esl");
                Lines.Add("ccbgssse067-daedinv.esm");
                Lines.Add("ccbgssse068-bloodfall.esl");
                Lines.Add("ccbgssse069-contest.esl");
                Lines.Add("ccvsvsse003-necroarts.esl");
                Lines.Add("ccvsvsse004-beafarmer.esl");
                Lines.Add("ccbgssse025-AdvDSGS.esm");
                Lines.Add("ccffbsse002-crossbowpack.esl");
                Lines.Add("ccbgssse013-dawnfang.esl");
                Lines.Add("ccrmssse001-necrohouse.esl");
                Lines.Add("ccedhsse003-redguard.esl");
                Lines.Add("cceejsse004-hall.esl");
                Lines.Add("cceejsse005-cave.esm");
                Lines.Add("cckrtsse001_altar.esl");
                Lines.Add("cccbhsse001-gaunt.esl");
                Lines.Add("ccafdsse001-dwesanctuary.esm");

                return Lines;
            }
        }
    }
}

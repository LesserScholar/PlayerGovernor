using HarmonyLib;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace PlayerGovernor
{
    public class SubModule : MBSubModuleBase
    {
        Harmony _harmony = new Harmony("PlayerGovernor");

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            _harmony.PatchAll();
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

        }


		[CommandLineFunctionality.CommandLineArgumentFunction("get_current_governor", "player_governor")]
		public static string GiveSettlementToPlayer(List<string> strings)
		{
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				return "Format is \"player_governor.get_current_governor [SettlementName/SettlementId]";
			}
			string text = CampaignCheats.ConcatenateString(strings);

			string text3 = text;
			Settlement settlement2 = MBObjectManager.Instance.GetObject<Settlement>(text3);
			if (settlement2 == null)
			{
				foreach (Settlement settlement3 in MBObjectManager.Instance.GetObjectTypeList<Settlement>())
				{
					if (settlement3.Name.ToString().Equals(text3, StringComparison.InvariantCultureIgnoreCase))
					{
						settlement2 = settlement3;
						break;
					}
				}
			}
			if (settlement2 == null)
			{
				return "Given settlement name or id could not be found.";
			}
            var gov = settlement2.Town.Governor;
            if (gov == null)
            {
                return "No governor appointed.";
            }
			return settlement2.Town.Governor.Name.ToString();
		}
	}

    [HarmonyPatch(typeof(Town), "Governor", MethodType.Getter)]

    public class TownPatch {

        public static void Postfix(ref Hero __result, Town __instance)
        {
            if (__result == null)
            {
                if (__instance.OwnerClan == Clan.PlayerClan && Hero.MainHero.CurrentSettlement == __instance.Settlement)
                {
                    __result = Hero.MainHero;
                }
            }
        }
    }
}
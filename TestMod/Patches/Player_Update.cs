namespace TestMod.Patches
{
    [HarmonyPatch(typeof(Player), nameof(Player.Update))]
    class Player_Update
    {
        private static SaveData saveData;
        private static Config cfg = Config.GetConfig();
        private static Player_Update pUpdate = new Player_Update();

        [HarmonyPostfix]
        internal static void PostFix(Player __instance)
        {
            if (Guard.IsGamePaused())
                return;

            cfg = Config.GetConfig();
            saveData = SaveData.GetSaveData();

            RPGKeyPress.ProcessKeys();

            XP_Events.DoubleXPEvent();

            pUpdate.UpdateMovement(__instance);
            pUpdate.UpdateHealth(__instance);
            pUpdate.UpdateSuffocation(__instance);

            Hints.CheckMSGs();
        }

        internal void UpdateMovement(Player __instance)
        {
            if (__instance.IsUnderwaterForSwimming() || __instance.motorMode == Player.MotorMode.Dive)  //add xp to swim speed
            {
                if (saveData.SwimSpeedLevel <= cfg.MaxSwimSpeedLevel)
                    saveData.SwimSpeed_XP += __instance.movementSpeed * cfg.XP_Multiplier;
            }
            else if (__instance.motorMode == Player.MotorMode.Walk || __instance.motorMode == Player.MotorMode.Run)  //add xp to walk speed
            {
                if (saveData.WalkSpeedLevel <= cfg.MaxWalkSpeedLevel)
                    saveData.WalkSpeed_XP += __instance.movementSpeed * cfg.XP_Multiplier;
            }

            Player.main.playerController.SetMotorMode(__instance.motorMode);
        }
    }
}

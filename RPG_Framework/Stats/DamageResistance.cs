using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework.Stats
{
    class DamageResistance
    {
        private static SaveData saveData;
        private static Config cfg = Config.GetConfig();

        public static float AddXP(float statValue, float amountToAdd)
        {
            float addXP = statValue + amountToAdd * cfg.XP_Multiplier;
            return addXP;
        }

        public static void ApplyResistance(Player __instance, DamageInfo damageInfo)
        {
            if(damageInfo == null) return;

            saveData = SaveData.GetSaveData();

            if (damageInfo.type == DamageType.Acid)
                ApplyAcidResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Cold)
                ApplyColdResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Collide)
                ApplyCollideResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Drill)
                ApplyDrillResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Electrical)
                ApplyElectricResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Explosive)
                ApplyExplosiveResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Fire)
                ApplyFireResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Heat)
                ApplyHeatResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.LaserCutter)
                ApplyLaserCutterResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Normal)
                ApplyNormalResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Poison)
                ApplyPoisonResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Pressure)
                ApplyPressureResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Puncture)
                ApplyPunctureResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Radiation)
                ApplyRadResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Smoke)
                ApplySmokeResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Starve)
                ApplyStarveResist(__instance, damageInfo);
            else if (damageInfo.type == DamageType.Undefined)
                ApplyUndefinedResist(__instance, damageInfo);

            SaveData.Save_SaveFile();
        }

        public static void IgnoreDamage(Player __instance, DamageInfo damageInfo, float amountToIgnore)
        {
            if (__instance.liveMixin.health <= 1 && damageInfo.damage >=1 && amountToIgnore != 100) return;

            float percentIgnore = amountToIgnore / 100;
            float numToIgnore = damageInfo.damage * percentIgnore;

            __instance.liveMixin.health += numToIgnore;
            damageInfo.damage -= numToIgnore;
        }

        public static void ApplyAcidResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.AcidResist_XP = AddXP(saveData.AcidResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Acid-Damage Resistance",
                Level = saveData.AcidResistLevel,
                MaxLevel = cfg.MaxAcidResistLevel,
                XP = saveData.AcidResist_XP,
                XPToNextLevel = saveData.AcidResist_XPToNextLevel,
                Modifier = cfg.AcidResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level)); 
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.AcidResistLevel = stat.Level;
            saveData.AcidResist_XP = stat.XP;
            saveData.AcidResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyColdResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.ColdResist_XP = AddXP(saveData.ColdResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Cold-Damage Resistance",
                Level = saveData.ColdResistLevel,
                MaxLevel = cfg.MaxColdResistLevel,
                XP = saveData.ColdResist_XP,
                XPToNextLevel = saveData.ColdResist_XPToNextLevel,
                Modifier = cfg.ColdResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.ColdResistLevel = stat.Level;
            saveData.ColdResist_XP = stat.XP;
            saveData.ColdResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyCollideResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.CollideResist_XP = AddXP(saveData.CollideResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Collision-Damage Resistance",
                Level = saveData.CollideResistLevel,
                MaxLevel = cfg.MaxCollideResistLevel,
                XP = saveData.CollideResist_XP,
                XPToNextLevel = saveData.CollideResist_XPToNextLevel,
                Modifier = cfg.CollideResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.CollideResistLevel = stat.Level;
            saveData.CollideResist_XP = stat.XP;
            saveData.CollideResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyDrillResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.DrillResist_XP = AddXP(saveData.DrillResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Drill-Damage Resistance",
                Level = saveData.DrillResistLevel,
                MaxLevel = cfg.MaxDrillResistLevel,
                XP = saveData.DrillResist_XP,
                XPToNextLevel = saveData.DrillResist_XPToNextLevel,
                Modifier = cfg.DrillResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.DrillResistLevel = stat.Level;
            saveData.DrillResist_XP = stat.XP;
            saveData.DrillResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyElectricResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.ElectricResist_XP = AddXP(saveData.ElectricResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Electric-Damage Resistance",
                Level = saveData.ElectricResistLevel,
                MaxLevel = cfg.MaxElectricResistLevel,
                XP = saveData.ElectricResist_XP,
                XPToNextLevel = saveData.ElectricResist_XPToNextLevel,
                Modifier = cfg.ElectricResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.ElectricResistLevel = stat.Level;
            saveData.ElectricResist_XP = stat.XP;
            saveData.ElectricResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyExplosiveResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.ExplosiveResist_XP = AddXP(saveData.ExplosiveResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Explosive-Damage Resistance",
                Level = saveData.ExplosiveResistLevel,
                MaxLevel = cfg.MaxExplosiveResistLevel,
                XP = saveData.ExplosiveResist_XP,
                XPToNextLevel = saveData.ExplosiveResist_XPToNextLevel,
                Modifier = cfg.ExplosiveResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.ExplosiveResistLevel = stat.Level;
            saveData.ExplosiveResist_XP = stat.XP;
            saveData.ExplosiveResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyFireResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.FireResist_XP = AddXP(saveData.FireResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Fire-Damage Resistance",
                Level = saveData.FireResistLevel,
                MaxLevel = cfg.MaxFireResistLevel,
                XP = saveData.FireResist_XP,
                XPToNextLevel = saveData.FireResist_XPToNextLevel,
                Modifier = cfg.FireResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.FireResistLevel = stat.Level;
            saveData.FireResist_XP = stat.XP;
            saveData.FireResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyHeatResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.HeatResist_XP = AddXP(saveData.HeatResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Heat-Damage Resistance",
                Level = saveData.HeatResistLevel,
                MaxLevel = cfg.MaxHeatResistLevel,
                XP = saveData.HeatResist_XP,
                XPToNextLevel = saveData.HeatResist_XPToNextLevel,
                Modifier = cfg.HeatResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.HeatResistLevel = stat.Level;
            saveData.HeatResist_XP = stat.XP;
            saveData.HeatResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyLaserCutterResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.LaserCutterResist_XP = AddXP(saveData.LaserCutterResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "LaserCutter-Damage Resistance",
                Level = saveData.LaserCutterResistLevel,
                MaxLevel = cfg.MaxLaserCutterResistLevel,
                XP = saveData.LaserCutterResist_XP,
                XPToNextLevel = saveData.LaserCutterResist_XPToNextLevel,
                Modifier = cfg.LaserCutterResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.LaserCutterResistLevel = stat.Level;
            saveData.LaserCutterResist_XP = stat.XP;
            saveData.LaserCutterResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyNormalResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.NormalResist_XP = AddXP(saveData.NormalResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Normal-Damage Resistance",
                Level = saveData.NormalResistLevel,
                MaxLevel = cfg.MaxNormalResistLevel,
                XP = saveData.NormalResist_XP,
                XPToNextLevel = saveData.NormalResist_XPToNextLevel,
                Modifier = cfg.NormalResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.NormalResistLevel = stat.Level;
            saveData.NormalResist_XP = stat.XP;
            saveData.NormalResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyPoisonResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.PoisonResist_XP = AddXP(saveData.PoisonResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Poison Resistance",
                Level = saveData.PoisonResistLevel,
                MaxLevel = cfg.MaxPoisonResistLevel,
                XP = saveData.PoisonResist_XP,
                XPToNextLevel = saveData.PoisonResist_XPToNextLevel,
                Modifier = cfg.PoisonResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.PoisonResistLevel = stat.Level;
            saveData.PoisonResist_XP = stat.XP;
            saveData.PoisonResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyPressureResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.PressureResist_XP = AddXP(saveData.PressureResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Pressure-Damage Resistance",
                Level = saveData.PressureResistLevel,
                MaxLevel = cfg.MaxPressureResistLevel,
                XP = saveData.PressureResist_XP,
                XPToNextLevel = saveData.PressureResist_XPToNextLevel,
                Modifier = cfg.PressureResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.PressureResistLevel = stat.Level;
            saveData.PressureResist_XP = stat.XP;
            saveData.PressureResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyPunctureResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.PunctureResist_XP = AddXP(saveData.PunctureResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Puncture-Damage Resistance",
                Level = saveData.PunctureResistLevel,
                MaxLevel = cfg.MaxPunctureResistLevel,
                XP = saveData.PunctureResist_XP,
                XPToNextLevel = saveData.PunctureResist_XPToNextLevel,
                Modifier = cfg.PunctureResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.PunctureResistLevel = stat.Level;
            saveData.PunctureResist_XP = stat.XP;
            saveData.PunctureResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyRadResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.RadResist_XP = AddXP(saveData.RadResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Radiation-Damage Resistance",
                Level = saveData.RadResistLevel,
                MaxLevel = cfg.MaxRadResistLevel,
                XP = saveData.RadResist_XP,
                XPToNextLevel = saveData.RadResist_XPToNextLevel,
                Modifier = cfg.RadResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.RadResistLevel = stat.Level;
            saveData.RadResist_XP = stat.XP;
            saveData.RadResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplySmokeResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.SmokeResist_XP = AddXP(saveData.SmokeResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Smoke-Damage Resistance",
                Level = saveData.SmokeResistLevel,
                MaxLevel = cfg.MaxSmokeResistLevel,
                XP = saveData.SmokeResist_XP,
                XPToNextLevel = saveData.SmokeResist_XPToNextLevel,
                Modifier = cfg.SmokeResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.SmokeResistLevel = stat.Level;
            saveData.SmokeResist_XP = stat.XP;
            saveData.SmokeResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyStarveResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.StarveResist_XP = AddXP(saveData.StarveResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Starvation-Damage Resistance",
                Level = saveData.StarveResistLevel,
                MaxLevel = cfg.MaxStarveResistLevel,
                XP = saveData.StarveResist_XP,
                XPToNextLevel = saveData.StarveResist_XPToNextLevel,
                Modifier = cfg.StarveResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.StarveResistLevel = stat.Level;
            saveData.StarveResist_XP = stat.XP;
            saveData.StarveResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }


        public static void ApplyUndefinedResist(Player __instance, DamageInfo damageInfo)
        {
            saveData.UndefinedResist_XP = AddXP(saveData.UndefinedResist_XP, damageInfo.damage);

            StatObject stat = new StatObject()
            {
                Name = "Undefined-Damage Resistance",
                Level = saveData.UndefinedResistLevel,
                MaxLevel = cfg.MaxUndefinedResistLevel,
                XP = saveData.UndefinedResist_XP,
                XPToNextLevel = saveData.UndefinedResist_XPToNextLevel,
                Modifier = cfg.UndefinedResistModifier
            };


            if (!StatMgr.CanLevelUp(stat))
            {
                IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);

            saveData.UndefinedResistLevel = stat.Level;
            saveData.UndefinedResist_XP = stat.XP;
            saveData.UndefinedResist_XPToNextLevel = stat.XPToNextLevel;

            Notify_ResistanceLevelUp(stat, gainedLevels, StatMgr.CalcResistance(stat.Level));
            IgnoreDamage(__instance, damageInfo, StatMgr.CalcResistance(stat.Level));
        }



        List<string> singleLevelNotifs = new List<string>
        {
            "{0} has gone up. Current resistance: {1}%",
            "{0} has gone up. Current resistance is {1}%",
            "{0} has gone up. Resistance is currently at {1}%",
            "{0} has gained a level. Current resistance: {1}%",
            "{0} has gained a level. Current resistance is {1}%",
            "{0} has gained a level. Resistance is currently at {1}%",
            "{0} has been raised. Current resistance: {1}%",
            "{0} has been raised. Current resistance is {1}%",
            "{0} has been raised. Resistance is currently at {1}%",
            "{0} has increased. Current resistance: {1}%",
            "{0} has increased. Current resistance is {1}%",
            "{0} has increased. Resistance is currently at {1}%",

            "{0} has gone up. Your current resistance: {1}%",
            "{0} has gone up. Your current resistance is {1}%",
            "{0} has gone up. Your resistance is currently at {1}%",
            "{0} has gained a level. Your current resistance: {1}%",
            "{0} has gained a level. Your current resistance is {1}%",
            "{0} has gained a level. Your resistance is currently at {1}%",
            "{0} has been raised. Your current resistance: {1}%",
            "{0} has been raised. Your current resistance is {1}%",
            "{0} has been raised. Your resistance is currently at {1}%",
            "{0} has increased. Your current resistance: {1}%",
            "{0} has increased. Your current resistance is {1}%",
            "{0} has increased. Your resistance is currently at {1}%",

            "Your {0} has gone up. Current resistance: {1}%",
            "Your {0} has gone up. Current resistance is {1}%",
            "Your {0} has gone up. Resistance is currently at {1}%",
            "Your {0} has gained a level. Current resistance: {1}%",
            "Your {0} has gained a level. Current resistance is {1}%",
            "Your {0} has gained a level. Resistance is currently at {1}%",
            "Your {0} has been raised. Current resistance: {1}%",
            "Your {0} has been raised. Current resistance is {1}%",
            "Your {0} has been raised. Resistance is currently at {1}%",
            "Your {0} has increased. Current resistance: {1}%",
            "Your {0} has increased. Current resistance is {1}%",
            "Your {0} has increased. Resistance is currently at {1}%",
        };

        List<string> multiLevelNotifs = new List<string>
        {
            "{0} has gone up by {1}. Current resistance: {2}%",
            "{0} has gone up by {1}. Current resistance is {2}%",
            "{0} has gone up by {1}. Resistance is currently at {2}%",
            "{0} has gained {1} levels. Current resistance: {2}%",
            "{0} has gained {1} levels. Current resistance is {2}%",
            "{0} has gained {1} levels. Resistance is currently at {2}%",
            "{0} has been raised by {1}. Current resistance: {2}%",
            "{0} has been raised by {1}. Current resistance is {2}%",
            "{0} has been raised by {1}. Resistance is currently at {2}%",
            "{0} has increased by {1}. Current resistance: {2}%",
            "{0} has increased by {1}. Current resistance is {2}%",
            "{0} has increased by {1}. Resistance is currently at {2}%",

            "{0} has gone up by {1}. Your current resistance: {2}%",
            "{0} has gone up by {1}. Your current resistance is {2}%",
            "{0} has gone up by {1}. Your resistance is currently at {2}%",
            "{0} has gained {1} levels. Your current resistance: {2}%",
            "{0} has gained {1} levels. Your current resistance is {2}%",
            "{0} has gained {1} levels. Your resistance is currently at {2}%",
            "{0} has been raised by {1}. Your current resistance: {2}%",
            "{0} has been raised by {1}. Your current resistance is {2}%",
            "{0} has been raised by {1}. Your resistance is currently at {2}%",
            "{0} has increased by {1}. Your current resistance: {2}%",
            "{0} has increased by {1}. Your current resistance is {2}%",
            "{0} has increased by {1}. Your resistance is currently at {2}%",

            "Your {0} has gone up by {1}. Current resistance: {2}%",
            "Your {0} has gone up by {1}. Current resistance is {2}%",
            "Your {0} has gone up by {1}. Resistance is currently at {2}%",
            "Your {0} has gained {1} levels. Current resistance: {2}%",
            "Your {0} has gained {1} levels. Current resistance is {2}%",
            "Your {0} has gained {1} levels. Resistance is currently at {2}%",
            "Your {0} has been raised by {1}. Current resistance: {2}%",
            "Your {0} has been raised by {1}. Current resistance is {2}%",
            "Your {0} has been raised by {1}. Resistance is currently at {2}%",
            "Your {0} has increased by {1}. Current resistance: {2}%",
            "Your {0} has increased by {1}. Current resistance is {2}%",
            "Your {0} has increased by {1}. Resistance is currently at {2}%",
        };

        public static void Notify_ResistanceLevelUp(StatObject stat, int gainedLevels, float currentResistance)
        {
            Random random = new Random();
            DamageResistance d = new DamageResistance();
            StatMgr mgr = new StatMgr();

            int responseNum = random.Next(0, d.singleLevelNotifs.Count - 1);
            if (gainedLevels == 1)
                Log.InGameMSG(String.Format(d.singleLevelNotifs[responseNum], stat.Name, currentResistance));
            else
                Log.InGameMSG(String.Format(d.multiLevelNotifs[responseNum], stat.Name, gainedLevels, currentResistance));


            if (stat.Level >= stat.MaxLevel)
            {
                int maxRandom = random.Next(0, mgr.maxLevelNotifs.Count - 1);
                Log.InGameMSG(String.Format(mgr.maxLevelNotifs[maxRandom], stat.Name) + ". Current resistance is at " + currentResistance + "%");
            }
        }
    }
}

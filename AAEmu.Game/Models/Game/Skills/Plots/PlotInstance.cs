﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Models.Game.World;

namespace AAEmu.Game.Models.Game.Skills.Plots
{
    public class PlotInstance
    {
        public ConcurrentBag<Task> Tasks;
        public CancellationToken Ct;
        public bool Canceled = false;

        public ConcurrentDictionary<uint, int> Tickets { get; set; }
        public PlotConditionsCache ConditionsCache { get; set; }
        public List<int> Variables { get; set; }
        public byte CombatDiceRoll { get; set; }

        public Skill ActiveSkill { get; set; }

        public Unit Caster { get; set; }
        public SkillCaster CasterCaster { get; set; }
        public BaseUnit Target { get; set; }
        public SkillCastTarget TargetCaster { get; set; }
        public SkillObject SkillObject { get; set; }

        public readonly object ConditionLock = new object();
        public readonly object TicketLock = new object();

        public List<GameObject> HitObjects { get; set; }

        public PlotInstance(Unit caster, SkillCaster casterCaster, BaseUnit target, SkillCastTarget targetCaster, SkillObject skillObject, Skill skill, CancellationToken ct)
        {
            Tasks = new ConcurrentBag<Task>();
            Ct = ct;
            Tickets = new ConcurrentDictionary<uint, int>();
            ConditionsCache = new PlotConditionsCache();
            Variables = new List<int>();

            Caster = caster;
            CasterCaster = casterCaster;
            Target = target;
            TargetCaster = targetCaster;
            SkillObject = skillObject;

            ActiveSkill = skill;
            HitObjects = new List<GameObject>();
        }
        public bool UseConditionCache(PlotCondition condition)
        {
            switch (condition.Kind)
            {
                case PlotConditionType.BuffTag:
                    return ConditionsCache.BuffTagCache.ContainsKey(condition.Param1);
            }
            return false;
        }
        public bool GetConditionCacheResult(PlotCondition condition)
        {
            try
            {
                switch (condition.Kind)
                {
                    case PlotConditionType.BuffTag:
                        return ConditionsCache.BuffTagCache[condition.Param1];
                }
            } 
            catch
            {
                NLog.LogManager.GetCurrentClassLogger().Error($"Id Doesnt Exist");
            }

            NLog.LogManager.GetCurrentClassLogger().Error($"Unsupported Condition Cache Kind - Returned False by Default");
            return false;
        }
        public void UpdateConditionCache(PlotCondition condition, bool result)
        {
            switch (condition.Kind)
            {
                case PlotConditionType.BuffTag:
                    ConditionsCache.BuffTagCache.TryAdd(condition.Param1, result);
                break;
            }
        }
    }
}

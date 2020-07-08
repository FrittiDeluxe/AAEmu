using System;
using AAEmu.Game.Core.Packets.G2C;
using AAEmu.Game.Models.Game.Skills.Plots.Type;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.Skills.Plots
{
    public class PlotEventCondition
    {
        public PlotCondition Condition { get; set; }
        public int Position { get; set; }
        public PlotEffectSource SourceId { get; set; }
        public PlotEffectTarget TargetId { get; set; }
        public bool NotifyFailure { get; set; }

        // TODO 1.2 // public bool NotifyFailure { get; set; }

        public bool CheckCondition(PlotInstance instance, PlotEventInstance eventInstance)
        {
            if (GetConditionResult(instance, eventInstance, this))
                return true;

            if (NotifyFailure)
                ;//Maybe do something here?
            
            return false;

        }
        
        private bool GetConditionResult(PlotInstance instance, PlotEventInstance eventInstance, PlotEventCondition condition)
        {
            lock (instance.ConditionLock)
            {
                var not = condition.Condition.NotCondition;
                //Check if condition was cached
                // if (instance.UseConditionCache(condition.Condition))
                // {
                //     var cacheResult = instance.GetConditionCacheResult(condition.Condition);
                //     //Apply not condition
                //     cacheResult = not ? !cacheResult : cacheResult;
                //
                //     return cacheResult;
                // }

                Unit source;
                switch (SourceId)
                {
                    case PlotEffectSource.OriginalSource:
                        source = instance.Caster;
                        break;
                    case PlotEffectSource.OriginalTarget:
                        source = (Unit) instance.Target;
                        break;
                    case PlotEffectSource.Source:
                        source = (Unit) eventInstance.Source;
                        break;
                    case PlotEffectSource.Target:
                        source = (Unit) eventInstance.Target;
                        break;
                    default:
                        throw new InvalidOperationException("This can't happen");
                }

                var result = true;
                foreach (var newTarget in eventInstance.EffectedTargets)
                {
                    BaseUnit target;
                    switch (TargetId)
                    {
                        case PlotEffectTarget.OriginalSource:
                            target = instance.Caster;
                            break;
                        case PlotEffectTarget.OriginalTarget:
                            target = instance.Target;
                            break;
                        case PlotEffectTarget.Source:
                            target = eventInstance.Source;
                            break;
                        case PlotEffectTarget.Target:
                            target = newTarget;
                            break;
                        case PlotEffectTarget.Location:
                            target = eventInstance.Target;
                            break;
                        default:
                            throw new InvalidOperationException("This can't happen");
                    }

                    if (condition.Condition.Check(source, instance.CasterCaster, target,
                        instance.TargetCaster, instance.SkillObject, condition))
                    {
                        continue;
                    }

                    result = false;
                    break;
                }
                
                //Check 
                // var result = condition.Condition.Check(instance.Caster, instance.CasterCaster, instance.Target, instance.TargetCaster, instance.SkillObject, condition);
                // if (result)
                // {
                //     //We need to undo the not condition to store in cache
                //     // instance.UpdateConditionCache(condition.Condition, !not);
                //     return true;
                // }
                //
                // // instance.UpdateConditionCache(condition.Condition, not);
                // return false;
                return result;
            }
        }
    }
}

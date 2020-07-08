using System;
using AAEmu.Game.Core.Managers;
using AAEmu.Game.Models.Game.Skills.Effects;
using AAEmu.Game.Models.Game.Skills.Plots.Type;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.Skills.Plots
{
    public class PlotEventEffect
    {
        public int Position { get; set; }
        public PlotEffectSource SourceId { get; set; }
        public PlotEffectTarget TargetId { get; set; }
        public uint ActualId { get; set; }
        public string ActualType { get; set; }

        public void ApplyEffect(PlotInstance instance, PlotEventInstance eventInstance, PlotEventTemplate evt, Skill skill, ref byte flag, ref bool appliedEffects)
        {
            var template = SkillManager.Instance.GetEffectTemplate(ActualId, ActualType);

            if (template is BuffEffect)
                flag = 6; //idk what this does?  
            if (template is SpecialEffect)
                appliedEffects = true;

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

                template.Apply(
                    source,
                    instance.CasterCaster,
                    target,
                    instance.TargetCaster,
                    new CastPlot(evt.PlotId, skill.TlId, evt.Id, skill.Template.Id), skill, instance.SkillObject, DateTime.Now);
            }
        }
    }
}

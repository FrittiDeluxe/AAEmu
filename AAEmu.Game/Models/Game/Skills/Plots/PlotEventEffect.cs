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
        public int SourceId { get; set; }
        public int TargetId { get; set; }
        public uint ActualId { get; set; }
        public string ActualType { get; set; }

        public void ApplyEffect(PlotInstance instance, PlotEventInstance eventInstance, PlotEventTemplate evt, Skill skill, ref byte flag, ref bool appliedEffects)
        {
            var template = SkillManager.Instance.GetEffectTemplate(ActualId, ActualType);

            if (template is BuffEffect)
                flag = 6; //idk what this does?  
            if (template is SpecialEffect)
                appliedEffects = true;

            // TODO: Update Source and Target here.
            // Given how source/target update is the same for Effects and Conditions, either use a common object and update above, or extension methods
            Unit source;
            
            switch (SourceId)
            {
                case 1:
                    source = instance.Caster;
                    break;
                case 2:
                    source = (Unit) instance.Target;
                    break;
                case 3:
                    source = (Unit) eventInstance.Source;
                    break;
                case 4:
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
                    case 1:
                        target = instance.Caster;
                        break;
                    case 2:
                        target = instance.Target;
                        break;
                    case 3:
                        target = eventInstance.Source;
                        break;
                    case 4:
                        target = newTarget;
                        break;
                    case 5:
                        target = eventInstance.Target;
                        break;
                    default:
                        throw new InvalidOperationException("This can't happen");
                }

                Console.WriteLine($"Effect: {this.ActualType} Source: {source.Name} Target: {target.Name}");

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

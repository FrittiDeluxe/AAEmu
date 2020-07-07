using System;
using System.Collections.Generic;
using System.Linq;
using AAEmu.Commons.Utils;
using AAEmu.Game.Core.Managers.World;
using AAEmu.Game.Models.Game.Faction;
using AAEmu.Game.Models.Game.Skills.Plots.Type;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Models.Game.Skills.Plots.UpdateTargetMethods;
using AAEmu.Game.Utils;
using AAEmu.Game.Models.Game.World;

namespace AAEmu.Game.Models.Game.Skills.Plots
{
    public class PlotEventInstance
    {
        public BaseUnit Source { get; set; }
        private BaseUnit PreviousSource { get; set; }
        public BaseUnit Target { get; set; }
        private BaseUnit PreviousTarget { get; set; }
        public List<BaseUnit> EffectedTargets { get; set; }

        public PlotEventInstance(PlotInstance instance)
        {
            EffectedTargets = new List<BaseUnit>();
            PreviousSource = instance.Caster;
            PreviousTarget = instance.Target;
        }

        public PlotEventInstance(PlotEventInstance eventInstance)
        {
            EffectedTargets = new List<BaseUnit>();
            PreviousSource = eventInstance.Source;
            PreviousTarget = eventInstance.Target;
            Source = eventInstance.Source;
            Target = eventInstance.Target;
        }

        public void UpdateSource(PlotEventTemplate template, PlotInstance instance)
        {
            switch ((PlotSourceUpdateMethodType)template.SourceUpdateMethodId)
            {
                case PlotSourceUpdateMethodType.OriginalSource:
                    Source = instance.Caster;
                    break;
                case PlotSourceUpdateMethodType.OriginalTarget:
                    Source = instance.Target;
                    break;
                case PlotSourceUpdateMethodType.PreviousSource:
                    Source = PreviousSource;
                    break;
                case PlotSourceUpdateMethodType.PreviousTarget:
                    Source = PreviousTarget;
                    break;
            }
        }

        public void UpdateTargets(PlotEventTemplate template, PlotInstance instance)
        {
            switch ((PlotTargetUpdateMethodType)template.TargetUpdateMethodId)
            {
                case PlotTargetUpdateMethodType.OriginalSource:
                    Target = instance.Caster;
                    EffectedTargets.Add(Target);
                    break;
                case PlotTargetUpdateMethodType.OriginalTarget:
                    Target = instance.Target;
                    EffectedTargets.Add(Target);
                    break;
                case PlotTargetUpdateMethodType.PreviousSource:
                    Target = PreviousSource;
                    EffectedTargets.Add(Target);
                    break;
                case PlotTargetUpdateMethodType.PreviousTarget:
                    Target = PreviousTarget;
                    EffectedTargets.Add(Target);
                    break;
                case PlotTargetUpdateMethodType.Area:
                    Target = UpdateAreaTarget(new PlotTargetAreaParams(template), instance);
                    break;
                case PlotTargetUpdateMethodType.RandomUnit:
                    Target = UpdateRandomUnitTarget(new PlotTargetRandomUnitParams(template), instance);
                    EffectedTargets.Add(Target);
                    break;
                case PlotTargetUpdateMethodType.RandomArea:
                    Target = UpdateRandomAreaTarget(new PlotTargetRandomAreaParams(template), instance);
                    break;
            }
        }

        private BaseUnit UpdateAreaTarget(PlotTargetAreaParams args, PlotInstance instance)
        {
            BaseUnit posUnit = new BaseUnit();
            posUnit.Position = new Point(Source.Position.X, Source.Position.Y, Source.Position.Z);
            posUnit.Region = Source.Region;
            posUnit.Position.ZoneId = Source.Position.ZoneId;
            posUnit.Position.WorldId = Source.Position.WorldId;
            //posUnit.Position.RotationZ = S
            var direction = MathUtil.ConvertDegreeToDirection(args.Angle);
            var newPos = MathUtil.AddDistanceToFront(args.Distance, posUnit.Position.X, posUnit.Position.Y, direction);

            posUnit.Position.X = newPos.Item1;
            posUnit.Position.Y = newPos.Item2;
            // posUnit.Position.Z = get heightmap value for x:y   
            
            if (args.MaxTargets == 0)
            {
                EffectedTargets.Add(posUnit);
                return posUnit;
            }
            
            // posUnit.Position.Z = get heightmap value for x:y     
            //TODO: Get Targets around posUnit?
            var unitsInRange = WorldManager.Instance.GetAround<BaseUnit>(posUnit, 5);

            // TODO : Filter min distance
            // TODO : Compute Unit Relation
            // TODO : Compute Unit Flag
            // unitsInRange = unitsInRange.Where(u => u.);

            EffectedTargets.AddRange(unitsInRange);

            return posUnit;
        }

        private BaseUnit UpdateRandomUnitTarget(PlotTargetRandomUnitParams args, PlotInstance instance)
        {
            //TODO for now we get all units in a 5 meters radius
            var randomUnits = WorldManager.Instance.GetAround<Unit>(Source, 5);

            var filteredUnits = FilterTargets(randomUnits, instance);
            var index = Rand.Next(0, randomUnits.Count);
            var randomUnit = filteredUnits.ElementAt(index);

            return randomUnit;
        }

        private BaseUnit UpdateRandomAreaTarget(PlotTargetRandomAreaParams args, PlotInstance instance)
        {
            BaseUnit posUnit = new BaseUnit();

            //TODO

            return posUnit;
        }

        private IEnumerable<Unit> FilterTargets(IEnumerable<Unit> units, PlotInstance instance)
        {
            var template = instance.ActiveSkill.Template;
            var filtered = units;
            if (!template.TargetAlive)
                filtered = filtered.Where(o => o.Hp == 0);
            if (!template.TargetDead)
                filtered = filtered.Where(o => o.Hp > 0);
            
            filtered = filtered
                .Where(o =>
                {
                    var relationState = instance.Caster.Faction.GetRelationState(o.Faction.Id);
                    if (relationState == RelationState.Neutral) // TODO ?
                        return false;
                    
                    switch (template.TargetRelation)
                    {
                        case SkillTargetRelation.Any:
                            return true;
                        case SkillTargetRelation.Friendly:
                            return relationState == RelationState.Friendly;
                        case SkillTargetRelation.Party:
                            return false; // TODO filter party member
                        case SkillTargetRelation.Raid:
                            return false; // TODO filter raid member
                        case SkillTargetRelation.Hostile:
                            return relationState == RelationState.Hostile;
                        default:
                            return true;
                    }
                });

            //Todo target unit type

            return filtered;
        }
    }
}

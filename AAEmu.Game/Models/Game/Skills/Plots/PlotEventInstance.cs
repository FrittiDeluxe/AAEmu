﻿using System.Collections.Generic;
using AAEmu.Game.Models.Game.Skills.Plots.Type;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Models.Game.Skills.Plots.UpdateTargetMethods;
using AAEmu.Game.Utils;

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
                    Target = UpdateAreaTarget(new PlotTargetAreaParams(template));
                    break;
                case PlotTargetUpdateMethodType.RandomUnit:
                    Target = UpdateRandomUnitTarget(new PlotTargetRandomUnitParams(template));
                    break;
                case PlotTargetUpdateMethodType.RandomArea:
                    Target = UpdateRandomAreaTarget(new PlotTargetRandomAreaParams(template));
                    break;
            }
        }

        private BaseUnit UpdateAreaTarget(PlotTargetAreaParams args)
        {
            BaseUnit posUnit = new BaseUnit();
            posUnit.Position = Source.Position;
            //posUnit.Position.RotationZ = S
            var direction = MathUtil.ConvertDegreeToDirection(args.Angle);
            var newPos = MathUtil.AddDistanceToFront(args.Distance, posUnit.Position.X, posUnit.Position.Y, direction);

            //TODO: Get Targets around posUnit?

            return posUnit;
        }

        private BaseUnit UpdateRandomUnitTarget(PlotTargetRandomUnitParams args)
        {
            BaseUnit randomUnit = new BaseUnit();

            //TODO

            return randomUnit;
        }

        private BaseUnit UpdateRandomAreaTarget(PlotTargetRandomAreaParams args)
        {
            BaseUnit posUnit = new BaseUnit();

            //TODO

            return posUnit;
        }
    }
}

﻿using System.Collections.Generic;
using AAEmu.Game.Models.Game.Skills.Plots.Type;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.Skills.Plots
{
    public class PlotEventInstance
    {
        public BaseUnit Source { get; set; }
        private BaseUnit PreviousSource { get; set; }
        public List<BaseUnit> Targets { get; set; }
        private List<BaseUnit> PreviousTargets { get; set; }

        public PlotEventInstance(PlotInstance instance)
        {
            Targets = new List<BaseUnit>();
            PreviousTargets = new List<BaseUnit>();
            PreviousTargets.Add(instance.Target);

            PreviousSource = instance.Caster;
        }
        public PlotEventInstance(PlotEventInstance eventInstance)
        {
            Source = eventInstance.Source;
            Targets = new List<BaseUnit>();
            PreviousSource = Source;
            PreviousTargets = new List<BaseUnit>(eventInstance.Targets);
        }

        public void UpdateSource(PlotEventTemplate template, PlotInstance instance)
        {
            switch((PlotSourceUpdateMethodType)template.SourceUpdateMethodId)
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
                    //Will there be multiple targets when this is called?
                    if (PreviousTargets.Count > 1)
                        NLog.LogManager.GetCurrentClassLogger().Error("Multiple Previous Targets For Case PreviousTarget.");
                    Source = PreviousTargets[0];
                    break;
            }
        }
        public void UpdateTargets(PlotEventTemplate template, PlotInstance instance)
        {
            switch ((PlotTargetUpdateMethodType)template.TargetUpdateMethodId)
            {
                case PlotTargetUpdateMethodType.OriginalSource:
                    Targets.Add(instance.Caster);
                    break;
                case PlotTargetUpdateMethodType.OriginalTarget:
                    Targets.Add(instance.Target);
                    break;
                case PlotTargetUpdateMethodType.PreviousSource:
                    Targets.Add(PreviousSource);
                    break;
                case PlotTargetUpdateMethodType.PreviousTarget:
                    Targets.AddRange(PreviousTargets);
                    break;
                case PlotTargetUpdateMethodType.Area:
                    //Todo
                    break;
                case PlotTargetUpdateMethodType.RandomUnit:
                    //Todo 
                    break;
                case PlotTargetUpdateMethodType.RandomArea:
                    //Todo
                    break;
            }
        }
    }
}

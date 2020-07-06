using System.Collections.Generic;
using AAEmu.Game.Models.Game.Skills.Plots.Type;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.Skills.Plots
{
    public class PlotEventInstance
    {
        public BaseUnit Source { get; set; }
        private BaseUnit PreviousSource { get; set; }
        public BaseUnit Target { get; set; }
        private BaseUnit PreviousTarget { get; set; }
        public List<BaseUnit> AreaTargets { get; set; }

        public PlotEventInstance(PlotInstance instance)
        {
            AreaTargets = new List<BaseUnit>();
            PreviousSource = instance.Caster;
            PreviousTarget = instance.Target;
        }
        public PlotEventInstance(PlotEventInstance eventInstance)
        {
            AreaTargets = new List<BaseUnit>();
            PreviousSource = eventInstance.Source;
            PreviousTarget = eventInstance.Target;
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
                    break;
                case PlotTargetUpdateMethodType.OriginalTarget:
                    Target = instance.Target;
                    break;
                case PlotTargetUpdateMethodType.PreviousSource:
                    Target = PreviousSource;
                    break;
                case PlotTargetUpdateMethodType.PreviousTarget:
                    Target = PreviousTarget;
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

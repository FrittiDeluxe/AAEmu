﻿namespace AAEmu.Game.Models.Game.Skills.Plots.UpdateTargetMethods
{
    public class PlotTargetRandomUnitParams : IPlotTargetParams
    {
        public uint AreaShapeId { get; set; } // TODO: Change to AreaShape object
        public bool HitOnce { get; set; }
        public SkillTargetRelation UnitRelationType { get; set; } // TODO: Change to enum
        public byte UnitTypeFlag { get; set; }

        public PlotTargetRandomUnitParams(PlotEventTemplate template)
        {
            AreaShapeId = (uint)template.TargetUpdateMethodParam1;
            HitOnce = template.TargetUpdateMethodParam2 == 1;
            UnitRelationType = (SkillTargetRelation)template.TargetUpdateMethodParam3;
            UnitTypeFlag = (byte)template.TargetUpdateMethodParam4;
        }
    }
}

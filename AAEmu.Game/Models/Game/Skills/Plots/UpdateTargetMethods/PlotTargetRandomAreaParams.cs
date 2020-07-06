namespace AAEmu.Game.Models.Game.Skills.Plots.UpdateTargetMethods
{
    public class PlotTargetRandomAreaParams
    {
        public uint AreaShapeId { get; set; } // TODO: Change to AreaShape object
        public int MaxTargets { get; set; }
        public int Angle { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }
        public bool HitOnce { get; set; }
        public int UnitRelationType { get; set; } // TODO: Change to enum
        public byte UnitTypeFlag { get; set; }


        public PlotTargetRandomAreaParams(PlotEventTemplate template)
        {
            AreaShapeId = (uint)template.TargetUpdateMethodParam1;
            MaxTargets = template.TargetUpdateMethodParam2;
            Angle = template.TargetUpdateMethodParam3;
            MinRange = template.TargetUpdateMethodParam4;
            MaxRange = template.TargetUpdateMethodParam5;
            HitOnce = template.TargetUpdateMethodParam6 == 1;
            UnitRelationType = template.TargetUpdateMethodParam7;
            UnitTypeFlag = (byte)template.TargetUpdateMethodParam8;
        }
    }
}

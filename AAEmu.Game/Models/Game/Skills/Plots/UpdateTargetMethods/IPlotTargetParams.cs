using System;
using System.Collections.Generic;
using System.Text;

namespace AAEmu.Game.Models.Game.Skills.Plots.UpdateTargetMethods
{
    interface IPlotTargetParams
    {
        uint AreaShapeId { get; set; } // TODO: Change to AreaShape object
        bool HitOnce { get; set; }
        SkillTargetRelation UnitRelationType { get; set; } // TODO: Change to enum
        byte UnitTypeFlag { get; set; }

    }
}

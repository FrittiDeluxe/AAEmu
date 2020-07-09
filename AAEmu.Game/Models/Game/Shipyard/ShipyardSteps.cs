namespace AAEmu.Game.Models.Game.Shipyards
{
    public class ShipyardSteps
    {
        public uint Id { get; set; }
        public uint ShipyardId { get; set; }
        public int Step { get; set; }
        public uint ModelId { get; set; }
        public uint SkillId { get; set; }
        public int NumActions { get; set; }
        public int MaxHp { get; set; }
    }
}

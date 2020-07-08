using AAEmu.Game.Core.Managers;
using AAEmu.Game.Models.Game;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Core.Managers.World;
using AAEmu.Game.Utils;

namespace AAEmu.Game.Scripts.Commands
{
    public class Distance : ICommand
    {
        public void OnLoad()
        {
            string[] names = { "distance"};
            CommandManager.Instance.Register(names, this);
        }

        public string GetCommandLineHelp()
        {
            return "(player)";
        }

        public string GetCommandHelpText()
        {
            return "Displays distance to your target.";
        }

        public void Execute(Character character, string[] args)
        {
            BaseUnit targetPlayer = character.CurrentTarget ?? character;

            var distance = MathUtil.CalculateDistance(character.Position, targetPlayer.Position, true);
            var pos = targetPlayer.Position;

            character.SendMessage("[Distance] |cFFFFFFFF{0}|r[Scale] {1}", distance , targetPlayer.Scale);
        }
    }
}

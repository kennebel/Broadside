using Godot;
using System;

namespace TheGameData
{
    [GlobalClass]
    public partial class GameConfig : Resource
    {
        public const string GameName = "Broadside";
        public const string GameVersion = "0.1a";

        [Export]
        public string PlayerName { get; set; }

        public GameConfig() : this(null) {}
        public GameConfig(string newPlayerName)
        {
            PlayerName = newPlayerName == null ? "" : newPlayerName;
        }
    }
}
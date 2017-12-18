using MiNET;
using MiNET.Blocks;
using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Worlds;
using System;

namespace SafeJoin
{
    public class SafeJoinMain : Plugin
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            ConsoleColor bfColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[SafeJoin] 플러그인 활성화!");
            Console.ForegroundColor = bfColor;
            Context.Server.PlayerFactory.PlayerCreated += PlayerFactory_PlayerCreated;
        }

        private void PlayerFactory_PlayerCreated(object sender, MiNET.PlayerEventArgs e)
        {
            e.Player.PlayerJoin += Player_PlayerJoin;
        }

        private void Player_PlayerJoin(object sender, PlayerEventArgs e)
        {
            Level level = e.Level;
            PlayerLocation location = e.Player.KnownPosition;
            PlayerLocation safeLoc = GetSafeLocation(level, location);
            e.Player.Teleport(safeLoc);
            e.Player.SendMessage(ChatColors.Aqua + "[SafeJoin] 안전한 장소로 이동했습니다.", MiNET.MessageType.Raw);
        }

        private PlayerLocation GetSafeLocation(Level level, PlayerLocation location)
        {
            while (!(level.GetBlock(location) is Air))
            {
                location = location.Clone() as PlayerLocation;
                location.Y++;
            }
            return location;
        }
    }
}

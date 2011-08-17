using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tad.Xna.Common.Avatars;
using Tad.Xna.Common.Cameras;

namespace Tad.Xna.Common
{
    public static class BasicSetup
    {
        public static void AddGodAvatarWithCamera(Game game)
        {
            Avatar.SetAvatar(new GodAvatar(game, new Vector3(0, 0, -50)));
            Avatar.Default.SetOrientation(Quaternion.CreateFromYawPitchRoll(0, 0, 0));
            game.Components.Add(Avatar.Default);

            Camera.SetCamera(new FirstPersonCamera(game));
            Camera.Default.AttachTo(Avatar.Default);
            game.Components.Add(Camera.Default);

            game.Activated += new EventHandler<EventArgs>(game_Activated);
            game.Deactivated += new EventHandler<EventArgs>(game_Deactivated);
        }

        static void game_Deactivated(object sender, EventArgs e)
        {
            Avatar.Default.CaptureMouse = false;
        }

        static void game_Activated(object sender, EventArgs e)
        {
            Avatar.Default.CaptureMouse = true;
        }
    }
}

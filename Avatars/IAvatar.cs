using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Tad.Xna.Common.Entities;

namespace Tad.Xna.Common.Avatars
{
    public interface IAvatar : IGameEntity
    {
        bool CaptureMouse { get; set; }
        Vector3 EyesPosition { get; }
        void AcceptInput(KeyboardState keyboard, GamePadState gamePad, MouseState mouse);
        void SetPosition(Vector3 position);
        void SetOrientation(Quaternion orientation);
        BoundingFrustum ViewFrustum { get; }
        BoundingFrustum ClippingFrustum { get; }
    }
}

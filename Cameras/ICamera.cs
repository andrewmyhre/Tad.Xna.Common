using Microsoft.Xna.Framework;
using Tad.Xna.Common.Entities;

namespace Tad.Xna.Common.Cameras
{
    public interface ICamera : IGameEntity, IUpdateable
    {
        float NearClip { get; }
        float FarClip { get; }
        Matrix ViewMatrix { get; }
        Matrix ProjectionMatrix { get; }
        void AttachTo(IGameEntity Entity);
        BoundingFrustum ViewFrustum { get; }
        BoundingFrustum ClippingFrustum { get; }
    }
}

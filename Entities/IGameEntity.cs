using System;
using Microsoft.Xna.Framework;

namespace Tad.Xna.Common.Entities
{
    public interface IGameEntity : IGameComponent
    {
        Vector3 Position {get;set;}
        Quaternion Orientation { get; set; }
        bool DrawWireframe { get; set; }
    }

    public class InformationEventArgs : EventArgs{
        public string Info {get;set;}
    }
}

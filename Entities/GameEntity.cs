using Microsoft.Xna.Framework;

namespace Tad.Xna.Common.Entities
{
    public abstract class GameEntity : DrawableGameComponent, IGameEntity
    {
        public GameEntity(Game game) : base(game)
        {
            Velocity = new Vector3();
            Position = new Vector3();
            Orientation = new Quaternion();
        }

        protected Quaternion _orientation;
        public Quaternion Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        protected Vector3 _position;
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        protected Vector3 _velocity;
        public Vector3 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public bool DrawWireframe { get; set; }
        public bool IsStatic = false;
    }
}

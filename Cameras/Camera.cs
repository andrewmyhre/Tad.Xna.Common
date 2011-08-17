namespace Tad.Xna.Common.Cameras
{
    public static class Camera
    {
        private static ICamera _default;
        public static void SetCamera(ICamera camera)
        {
            _default = camera;
        }
        public static ICamera Default { get { return _default; } }
    }
}
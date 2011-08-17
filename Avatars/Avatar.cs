namespace Tad.Xna.Common.Avatars
{
    public static class Avatar
    {
        private static IAvatar _defaultAvatar;
        public static void SetAvatar(IAvatar avatar)
        {
            _defaultAvatar = avatar;
        }
        public static IAvatar Default { get { return _defaultAvatar; } }
        public static void SetMouseCaptureMode(bool captureMouse)
        {
            _defaultAvatar.CaptureMouse = captureMouse;
        }
    }
}

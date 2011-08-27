using System.Collections.Generic;

namespace Tad.Xna.Common.Avatars
{
    public static class Avatar
    {
        private static IAvatar _currentAvatar;
        private static List<IAvatar> _avatars;
        private static int _currentAvatarIndex = 0;

        static Avatar()
        {
            _avatars = new List<IAvatar>();
        }

        public static void SetCurrent(IAvatar avatar)
        {
            if (_currentAvatar != null)
                _currentAvatar.CaptureMouse = false;

            _currentAvatar = avatar;
            _currentAvatar.CaptureMouse = true;
        }
        public static IAvatar AddAvatar(IAvatar avatar)
        {
            _avatars.Add(avatar);
            if (_avatars.Count == 1)
                SetCurrent(avatar);

            return avatar;
        }
        public static IAvatar NextAvatar()
        {
            if (_avatars.Count > _currentAvatarIndex+1)
            {
                _currentAvatarIndex++;
            } else
            {
                _currentAvatarIndex = 0;
            }
            SetCurrent(_avatars[_currentAvatarIndex]);
            return _avatars[_currentAvatarIndex];
        }
        public static IAvatar PreviousAvatar()
        {
            if (_currentAvatarIndex > 0)
            {
                _currentAvatarIndex--;
            } else
            {
                _currentAvatarIndex = _avatars.Count - 1;
            }
            SetCurrent(_avatars[_currentAvatarIndex]);
            return _avatars[_currentAvatarIndex];
        }
        public static IAvatar Current { get { return _currentAvatar; } }
        public static void SetMouseCaptureMode(bool captureMouse)
        {
            _currentAvatar.CaptureMouse = captureMouse;
        }
    }
}

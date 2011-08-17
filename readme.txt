Usages
======

Create a simple god avatar with camera
--------------------------------------

public class Game1 : Game
{
protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Tad.Xna.Common.BasicSetup.AddGodAvatarWithCamera(this);

            Components.Add(new TestObject(this));

            base.Initialize();
        }
}


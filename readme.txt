Usages
======

Create a simple god avatar with camera
--------------------------------------

public class Game1 : Game
{
	protected override void Initialize()
	{
		Tad.Xna.Common.BasicSetup.AddGodAvatarWithCamera(this);

		Components.Add(new TestEntity(this));

		base.Initialize();
	}
}


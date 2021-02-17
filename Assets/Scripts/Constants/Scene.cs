using System;

namespace Constants
{
    public enum Scene
    {
        Tutorial,
        Testing,
        Level1
    }

    public static class SceneHelper
    {
        public static string GetName(this Scene scene)
        {
            return scene switch
            {
                Scene.Tutorial => "Tutorial",
                Scene.Testing => "Testing",
                Scene.Level1 => "Level1",
                _ => throw new ArgumentOutOfRangeException(nameof(scene), scene, null)
            };
        }
    }
}
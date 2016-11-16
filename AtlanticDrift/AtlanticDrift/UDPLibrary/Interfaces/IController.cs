using Microsoft.Xna.Framework;
using AtlanticDrift;

namespace UDPLibrary
{
    public interface IController
    {
        void Update(GameTime gameTime, IActor actor);
        string GetID();
    }
}

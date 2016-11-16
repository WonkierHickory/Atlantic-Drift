using Microsoft.Xna.Framework;

//base class from which all drawn, collidable, 
//non-collidable, trigger volumes, and camera inherit
namespace UDPLibrary
{
    public interface IActor
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}

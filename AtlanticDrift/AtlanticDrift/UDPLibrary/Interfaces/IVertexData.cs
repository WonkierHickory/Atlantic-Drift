using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UDPLibrary
{
    public interface IVertexData
    {
        void Draw(GameTime gameTime, BasicEffect effect);
    }
}

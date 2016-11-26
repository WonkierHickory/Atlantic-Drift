using Microsoft.Xna.Framework;

namespace UDPLibrary
{
    public interface IController
    {
        string GetName();
        Actor GetParentActor();
        void SetParentActor(Actor parentActor);

        void Update(GameTime gameTime);
        object Clone();
    }
}

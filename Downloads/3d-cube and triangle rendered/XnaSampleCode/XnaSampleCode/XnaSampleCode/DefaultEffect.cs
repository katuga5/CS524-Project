using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JWalsh.XnaSamples
{
    class DefaultEffect : Effect, IEffectMatrices
    {
        EffectParameter world;
        EffectParameter projection;
        EffectParameter texture;

        public DefaultEffect(Effect effect)
            : base(effect)
        {
            world = Parameters["World"];
            projection = Parameters["Projection"];
            texture = Parameters["DiffuseTexture"];
        }

        public Matrix World
        {
            get { return world.GetValueMatrix(); }
            set { world.SetValue(value); }
        }

        public Matrix View
        {
            get { return Matrix.Identity; }
            set { }
        }

        public Matrix Projection
        {
            get { return projection.GetValueMatrix(); }
            set { projection.SetValue(value); }
        }

        public Texture2D Texture
        {
            get { return texture.GetValueTexture2D(); }
            set { texture.SetValue(value); }
        }
    }
}

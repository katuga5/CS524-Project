using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JWalsh.XnaSamples
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        KeyboardState prevKeyboardState = Keyboard.GetState();

        DefaultEffect effect;
        Matrix triangleTransform;
        Matrix rectangleTransform;

        VertexBuffer pyramidVB;
        IndexBuffer pyramidIB;
        VertexBuffer boxVB;
        IndexBuffer boxIB;
        TextureFilter textureFilter = TextureFilter.Linear;

        private float triangleAngle;
        private float rectangleAngle;
        bool isAnimating = true;
        float depth = -6.0f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            // Allow users to resize the window, and handle the Projection Matrix on Resize
            Window.Title = "3D Cube and Triangle Rendering";
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnClientSizeChanged;
        }

        protected bool IsFullScreen
        {
            get { return graphics.IsFullScreen; }
            set
            {
                if (value != graphics.IsFullScreen)
                {
                    // Toggle FullScreen, and Mouse Display, then apply the changes
                    // on the DeviceManager
                    graphics.IsFullScreen = !graphics.IsFullScreen;
                    IsMouseVisible = !IsMouseVisible;
                    graphics.ApplyChanges();
                }
            }
        }

        protected void OnClientSizeChanged(object sender, EventArgs e)
        {
            ResetProjection();
        }

        protected void ResetProjection()
        {
            Viewport viewport = graphics.GraphicsDevice.Viewport;

            // Set the Projection Matrix
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                (float)graphics.GraphicsDevice.Viewport.Width /
                (float)graphics.GraphicsDevice.Viewport.Height,
                0.1f,
                100.0f);
        }

        protected void ResetTranslation()
        {
            triangleTransform = Matrix.CreateTranslation(new Vector3(-1.5f, 0.0f, depth));
            rectangleTransform = Matrix.CreateTranslation(new Vector3(1.5f, 0.0f, depth));
        }

        protected override void Initialize()
        {
            ResetTranslation();

            Vector2 topLeft = new Vector2(0.0f, 0.0f);
            Vector2 topRight = new Vector2(1.0f, 0.0f);
            Vector2 bottomLeft = new Vector2(0.0f, 1.0f);
            Vector2 bottomRight = new Vector2(1.0f, 1.0f);

            VertexPositionTexture[] triangleData = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, 1.0f), bottomRight),
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, 1.0f), bottomLeft),
                new VertexPositionTexture(new Vector3(0.0f, 1.0f, 0.0f), topRight),
 
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, -1.0f), bottomRight),
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, 1.0f), bottomLeft),
                new VertexPositionTexture(new Vector3(0.0f, 1.0f, 0.0f), topRight),
 
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, -1.0f), bottomRight),
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, -1.0f), bottomLeft),
                new VertexPositionTexture(new Vector3(0.0f, 1.0f, 0.0f), topRight),
 
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, 1.0f), bottomRight),
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, -1.0f), bottomLeft),
                new VertexPositionTexture(new Vector3(0.0f, 1.0f, 0.0f), topRight),
            };

                    VertexPositionTexture[] boxData = new VertexPositionTexture[]
            {
                // Front Surface
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, 1.0f),bottomLeft),
                new VertexPositionTexture(new Vector3(-1.0f, 1.0f, 1.0f),topLeft),
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, 1.0f),bottomRight),
                new VertexPositionTexture(new Vector3(1.0f, 1.0f, 1.0f),topRight), 
 
                // Front Surface
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, -1.0f),bottomLeft),
                new VertexPositionTexture(new Vector3(1.0f, 1.0f, -1.0f),topLeft),
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, -1.0f),bottomRight),
                new VertexPositionTexture(new Vector3(-1.0f, 1.0f, -1.0f),topRight),
 
                // Left Surface
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, -1.0f),bottomLeft),
                new VertexPositionTexture(new Vector3(-1.0f, 1.0f, -1.0f),topLeft),
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, 1.0f),bottomRight),
                new VertexPositionTexture(new Vector3(-1.0f, 1.0f, 1.0f),topRight),
 
                // Right Surface
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, 1.0f),bottomLeft),
                new VertexPositionTexture(new Vector3(1.0f, 1.0f, 1.0f),topLeft),
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, -1.0f),bottomRight),
                new VertexPositionTexture(new Vector3(1.0f, 1.0f, -1.0f),topRight),
 
                // Top Surface
                new VertexPositionTexture(new Vector3(-1.0f, 1.0f, 1.0f),bottomLeft),
                new VertexPositionTexture(new Vector3(-1.0f, 1.0f, -1.0f),topLeft),
                new VertexPositionTexture(new Vector3(1.0f, 1.0f, 1.0f),bottomRight),
                new VertexPositionTexture(new Vector3(1.0f, 1.0f, -1.0f),topRight),
 
                // Bottom Surface
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, -1.0f),bottomLeft),
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, 1.0f),topLeft),
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, -1.0f),bottomRight),
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, 1.0f),topRight),
            };

            short[] pyramidIndices = new short[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

            short[] boxIndices = new short[] {
                0, 1, 2, 2, 1, 3,  
                4, 5, 6, 6, 5, 7,
                8, 9, 10, 10, 9, 11,
                12, 13, 14, 14, 13, 15,
                16, 17, 18, 18, 17, 19,
                20, 21, 22, 22, 21, 23
            };

            pyramidVB = new VertexBuffer(GraphicsDevice, VertexPositionTexture.VertexDeclaration, 12, BufferUsage.WriteOnly);
            pyramidIB = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, 12, BufferUsage.WriteOnly);

            pyramidVB.SetData(triangleData);
            pyramidIB.SetData(pyramidIndices);
            triangleData = null;
            pyramidIndices = null;

            boxVB = new VertexBuffer(GraphicsDevice, VertexPositionTexture.VertexDeclaration, 24, BufferUsage.WriteOnly);
            boxIB = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, 36, BufferUsage.WriteOnly);

            boxVB.SetData(boxData);
            boxIB.SetData(boxIndices);
            boxData = null;
            boxIndices = null;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Effect tempEffect = Content.Load<Effect>("Effects/Default");
            effect = new DefaultEffect(tempEffect);
            tempEffect = null;

            ResetProjection();
            //effect.Texture = Content.Load("Textures/Crate");
            GraphicsDevice.SamplerStates[0] = new SamplerState()
            {
                Filter = textureFilter
            };
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyboard = Keyboard.GetState();

            // If the user presses the escape key, close the game
            if (keyboard.IsKeyDown(Keys.Escape) )
                Exit();

            // If the user presses the F1 key, toggle FullScreen Mode
            if (keyboard.IsKeyDown(Keys.F11) && prevKeyboardState.IsKeyUp(Keys.F11))
                IsFullScreen = !IsFullScreen;

            if (keyboard.IsKeyDown(Keys.A) && prevKeyboardState.IsKeyUp(Keys.A))
                isAnimating = !isAnimating;

            if (keyboard.IsKeyDown(Keys.R) && prevKeyboardState.IsKeyUp(Keys.R))
            {
                triangleAngle = 0f;
                rectangleAngle = 0f;
            }

            if (keyboard.IsKeyDown(Keys.F) && prevKeyboardState.IsKeyUp(Keys.F))
            {
                textureFilter = textureFilter + 1;
                if (textureFilter > TextureFilter.MinPointMagLinearMipPoint)
                    textureFilter = TextureFilter.Linear;

                GraphicsDevice.SamplerStates[0] = new SamplerState()
                {
                    Filter = textureFilter
                };
            }

            if (keyboard.IsKeyDown(Keys.PageUp))
            {
                depth -= 0.05f;
                ResetTranslation();
            }

            if (keyboard.IsKeyDown(Keys.PageDown))
            {
                depth += 0.05f;
                ResetTranslation();
            }

            if (isAnimating)
            {
                triangleAngle += MathHelper.ToRadians(1.0f);
                if (triangleAngle > MathHelper.TwoPi)
                    triangleAngle -= MathHelper.TwoPi;

                rectangleAngle -= MathHelper.ToRadians(0.75f);
                if (rectangleAngle < 0.0f)
                    rectangleAngle += MathHelper.TwoPi;
            }

            // Cache the keyboard state for the next update cycle
            prevKeyboardState = keyboard;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            effect.World = Matrix.CreateRotationY(triangleAngle) * triangleTransform;
            effect.CurrentTechnique.Passes[0].Apply();
 
            GraphicsDevice.SetVertexBuffer(pyramidVB);
            GraphicsDevice.Indices = pyramidIB;
            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 5, 0, 4);
 
            effect.World = Matrix.CreateFromYawPitchRoll(rectangleAngle,
                        rectangleAngle, rectangleAngle) * rectangleTransform;
            effect.CurrentTechnique.Passes[0].Apply();
 
            GraphicsDevice.SetVertexBuffer(boxVB);
            GraphicsDevice.Indices = boxIB;
            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 24, 0, 12);
        
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EBC.Engine_Components;
using EBC.Physics;

namespace Graphics
{
    public class Game1 : Game
    {
        private Texture2D _whitePx;

        private Force[] _forces;

        //used for graphics scaling
        private float _maxComponent;
        private float _maxMoment;


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        public Game1(Force[] forces, float maxComponent, float maxMoment)
        {
            //set fields
            _forces = forces;
            _maxComponent = maxComponent;
            _maxMoment = maxMoment;

            _graphics = new GraphicsDeviceManager(this);

            //change resolution
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _whitePx = Content.Load<Texture2D>("whitePx"); 
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            
            foreach(Force force in _forces)
            {
                _spriteBatch.Draw
                    (
                    _whitePx,
                    new Vector2
                    (
                        _graphics.PreferredBackBufferWidth / 2 + force.Components.X * (_graphics.PreferredBackBufferWidth / (2 * _maxComponent)),
                        _graphics.PreferredBackBufferHeight / 2 +force.Components.Y * (_graphics.PreferredBackBufferHeight / (2 * _maxComponent))
                    ),
                    Color.White
                    );
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
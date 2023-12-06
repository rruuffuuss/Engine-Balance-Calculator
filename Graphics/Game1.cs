using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using EBC.Physics;
using Graphics.Components;
using Graphics.Components.Labels;
using Graphics.Holders;
using System;
using EBC.Engine_Components;

namespace Graphics
{
    public class Game1 : Game
    {
        private Force[] _forces;
        private ForceHolder _currentForce;
        private int _forceNumber;
        private float _crankRotation;
        private numHolder _crankRotHolder;

        private Graph _componentGraph;
        private Graph _momentGraph;

        private MouseState _newMouseState;
        private MouseState _oldMouseState;
        private KeyboardInput _keyboardInput;

        private  Graphics.Components.Labels.Label[] _labels;

        private GraphicsDeviceManager _graphics;
        private float _guiScale;

        private SpriteBatch _spriteBatch;
        private Texture2D _background;

        //theme colours
        private Color Colour1 = new Color(153, 162, 255);
        private Color Colour2 = new Color(153, 255, 182);
        private Color Colour3 = new Color(255, 152, 152);
        private Color Colour4 = new Color(255, 233, 153);

        public Game1(Engine engine, float RPM, int measurementNo)
        {
            Vector3 MaxComponents = Vector3.Zero;
            Vector3 MaxMoments = Vector3.Zero;
            float maxComponent = 0;
            float maxMoment = 0;

            
            Force[] forces = new Force[measurementNo];


            Vector3[] components = new Vector3[forces.Length];
            Vector3[] moments = new Vector3[forces.Length];


            for (int i = 0; i < measurementNo; i++)
            {


                forces[i] = engine.ComputeAllForces(((float)i / (float)measurementNo) * 360f, RPM);

                if (MathF.Abs(forces[i].Components.X) > maxComponent) maxComponent = MathF.Abs(forces[i].Components.X);
                if (MathF.Abs(forces[i].Components.Y) > maxComponent) maxComponent = MathF.Abs(forces[i].Components.Y);
                if (MathF.Abs(forces[i].Components.Z) > maxComponent) maxComponent = MathF.Abs(forces[i].Components.Z);

                if (MathF.Abs(forces[i].Moments.X) > maxMoment) maxMoment = MathF.Abs(forces[i].Moments.X);
                if (MathF.Abs(forces[i].Moments.Y) > maxMoment) maxMoment = MathF.Abs(forces[i].Moments.Y);
                if (MathF.Abs(forces[i].Moments.Z) > maxMoment) maxMoment = MathF.Abs(forces[i].Moments.Z);

                components[i] = forces[i].Components;
                moments[i] = forces[i].Moments;
            }

            _componentGraph = new Graph(components, maxComponent, engine);
            _momentGraph = new Graph(moments, maxMoment, engine);
            _forces = forces;


            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _forceNumber = 0;

            base.Initialize();

            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.HardwareModeSwitch = false;
            _graphics.ApplyChanges();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //load textures
            Texture2D _pointTexture = Content.Load<Texture2D>("point");
            Texture2D _currentPointTexture = Content.Load<Texture2D>("currentPoint");
            Texture2D _forceArrowTexture = Content.Load<Texture2D>("upArrow");
            Texture2D _engineLineTexture = Content.Load<Texture2D>("line");
            Texture2D _enginePistonTexture = Content.Load<Texture2D>("circle");
            _background = Content.Load<Texture2D>("layout");
            //load font
            SpriteFont _DM_Mono = Content.Load<SpriteFont>("DM Mono Regular");

            _guiScale = GraphicsDevice.DisplayMode.Width / 1920f;

            //init holders
            _currentForce = new ForceHolder();
            _currentForce.force = Force.GetForceEmpty();
            _crankRotHolder = new numHolder();

            //load components

            _componentGraph.LoadContent
            (
                _pointTexture,
                _currentPointTexture,
                _engineLineTexture,
                _currentPointTexture,
                _enginePistonTexture,
                Colour1,
                Colour3,
                Colour2,
                Colour4,
                Vector2.Multiply(new Vector2(0,0), _guiScale),//ON PURPOSE, GRAPH MUST BE "CUBE"
                Vector3.Multiply(new Vector3(720), _guiScale),
                0.5f
            );

            _momentGraph.LoadContent
            (
                _pointTexture,
                _forceArrowTexture,
                _engineLineTexture,
                _currentPointTexture,
                _enginePistonTexture,
                Colour3,
                Colour1,
                Colour4,
                Colour2,
                Vector2.Multiply(new Vector2(1200, 0), _guiScale),//ON PURPOSE, GRAPH MUST BE "CUBE"
                Vector3.Multiply(new Vector3(720),_guiScale),
                0.5f
            );

            _labels = new Components.Labels.Label[]
            {
                new CrankLabel
                (
                    Vector2.Multiply(new Vector2(1045,16),_guiScale),
                    _DM_Mono,
                    Color.Black,
                    "deg",
                    7,
                    _crankRotHolder
                ),
                new XCompLabel
                (
                    Vector2.Multiply(new Vector2(765,107), _guiScale),
                    _DM_Mono,
                    Color.Black,
                    "N",
                     8,
                    _currentForce
                ),
                new YCompLabel
                (
                    Vector2.Multiply(new Vector2(765,171), _guiScale),
                    _DM_Mono,
                    Color.Black,
                    "N",
                     8,
                    _currentForce
                ),
                new ZCompLabel
                (
                    Vector2.Multiply(new Vector2(765,234), _guiScale),
                    _DM_Mono,
                    Color.Black,
                    "N",
                     8,
                    _currentForce
                ),
                new XMomLabel
                (
                    Vector2.Multiply(new Vector2(1005,107), _guiScale),
                    _DM_Mono,
                    Color.Black,
                    "N",
                     8,
                    _currentForce
                ),
                new YMomLabel
                (
                    Vector2.Multiply(new Vector2(1005,171), _guiScale),
                    _DM_Mono,
                    Color.Black,
                    "N",
                     8,
                    _currentForce
                ),
                new ZMomLabel
                (
                    Vector2.Multiply(new Vector2(1005,234), _guiScale),
                    _DM_Mono,
                    Color.Black,
                    "N",
                     8,
                    _currentForce
                ),
            };

            //init keyboardInput object
            _keyboardInput = new KeyboardInput();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //go to next force 
            _oldMouseState = _newMouseState;
            _newMouseState = Mouse.GetState();

            _forceNumber = _keyboardInput.Update(_forceNumber);
            _forceNumber = Math.Clamp(_forceNumber >= _forces.Length ? _forceNumber - _forces.Length : _forceNumber, 0, _forces.Length);
            _currentForce.force = _forces[_forceNumber];

            _componentGraph.Update(_oldMouseState, _newMouseState);
            _momentGraph.Update(_oldMouseState, _newMouseState);

            

            _crankRotation = _forceNumber * _forces.Length / 360f;
            _crankRotHolder.value = _crankRotation;

            foreach(Components.Labels.Label label in _labels)
            {
                label.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background,Vector2.Zero, Color.White);


            _componentGraph.Draw(gameTime, _spriteBatch, _forceNumber);
            _momentGraph.Draw(gameTime, _spriteBatch, _forceNumber);

            foreach(Components.Labels.Label label in _labels)
            {
                label.Draw(gameTime, _spriteBatch);
            }

            

            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
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

            _graphics = new GraphicsDeviceManager(this);

            _componentGraph = new Graph(components, maxComponent, engine);
            _momentGraph = new Graph(moments, maxMoment, engine);

            _forces = forces;

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
            Texture2D _engineLineTexture = Content.Load<Texture2D>("line");
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
                _currentPointTexture,
                _engineLineTexture,
                _currentPointTexture,
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



        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //go to next force 
            _oldMouseState = _newMouseState;
            _newMouseState = Mouse.GetState();

            _componentGraph.Update(_oldMouseState, _newMouseState);
            _momentGraph.Update(_oldMouseState, _newMouseState);

            _forceNumber += 1;
            if(_forceNumber >= _forces.Length) _forceNumber = 0;
            _currentForce.force = _forces[_forceNumber];

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


        /// <summary>
        /// Tries to convert keyboard input to characters and prevents repeatedly returning the 
        /// same character if a key was pressed last frame, but not yet unpressed this frame.
        /// </summary>
        /// <param name="keyboard">The current KeyboardState</param>
        /// <param name="oldKeyboard">The KeyboardState of the previous frame</param>
        /// <param name="key">When this method returns, contains the correct character if conversion succeeded.
        /// Else contains the null, (000), character.</param>
        /// <returns>True if conversion was successful</returns>
        public static bool TryConvertKeyboardInput(KeyboardState keyboard, KeyboardState oldKeyboard, out char key)
        {
            Keys[] keys = keyboard.GetPressedKeys();
            bool shift = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);

            if (keys.Length > 0 && !oldKeyboard.IsKeyDown(keys[0]))
            {
                switch (keys[0])
                {
                    //Alphabet keys
                    case Keys.A: if (shift) { key = 'A'; } else { key = 'a'; } return true;
                    case Keys.B: if (shift) { key = 'B'; } else { key = 'b'; } return true;
                    case Keys.C: if (shift) { key = 'C'; } else { key = 'c'; } return true;
                    case Keys.D: if (shift) { key = 'D'; } else { key = 'd'; } return true;
                    case Keys.E: if (shift) { key = 'E'; } else { key = 'e'; } return true;
                    case Keys.F: if (shift) { key = 'F'; } else { key = 'f'; } return true;
                    case Keys.G: if (shift) { key = 'G'; } else { key = 'g'; } return true;
                    case Keys.H: if (shift) { key = 'H'; } else { key = 'h'; } return true;
                    case Keys.I: if (shift) { key = 'I'; } else { key = 'i'; } return true;
                    case Keys.J: if (shift) { key = 'J'; } else { key = 'j'; } return true;
                    case Keys.K: if (shift) { key = 'K'; } else { key = 'k'; } return true;
                    case Keys.L: if (shift) { key = 'L'; } else { key = 'l'; } return true;
                    case Keys.M: if (shift) { key = 'M'; } else { key = 'm'; } return true;
                    case Keys.N: if (shift) { key = 'N'; } else { key = 'n'; } return true;
                    case Keys.O: if (shift) { key = 'O'; } else { key = 'o'; } return true;
                    case Keys.P: if (shift) { key = 'P'; } else { key = 'p'; } return true;
                    case Keys.Q: if (shift) { key = 'Q'; } else { key = 'q'; } return true;
                    case Keys.R: if (shift) { key = 'R'; } else { key = 'r'; } return true;
                    case Keys.S: if (shift) { key = 'S'; } else { key = 's'; } return true;
                    case Keys.T: if (shift) { key = 'T'; } else { key = 't'; } return true;
                    case Keys.U: if (shift) { key = 'U'; } else { key = 'u'; } return true;
                    case Keys.V: if (shift) { key = 'V'; } else { key = 'v'; } return true;
                    case Keys.W: if (shift) { key = 'W'; } else { key = 'w'; } return true;
                    case Keys.X: if (shift) { key = 'X'; } else { key = 'x'; } return true;
                    case Keys.Y: if (shift) { key = 'Y'; } else { key = 'y'; } return true;
                    case Keys.Z: if (shift) { key = 'Z'; } else { key = 'z'; } return true;

                    //Decimal keys
                    case Keys.D0: if (shift) { key = ')'; } else { key = '0'; } return true;
                    case Keys.D1: if (shift) { key = '!'; } else { key = '1'; } return true;
                    case Keys.D2: if (shift) { key = '@'; } else { key = '2'; } return true;
                    case Keys.D3: if (shift) { key = '#'; } else { key = '3'; } return true;
                    case Keys.D4: if (shift) { key = '$'; } else { key = '4'; } return true;
                    case Keys.D5: if (shift) { key = '%'; } else { key = '5'; } return true;
                    case Keys.D6: if (shift) { key = '^'; } else { key = '6'; } return true;
                    case Keys.D7: if (shift) { key = '&'; } else { key = '7'; } return true;
                    case Keys.D8: if (shift) { key = '*'; } else { key = '8'; } return true;
                    case Keys.D9: if (shift) { key = '('; } else { key = '9'; } return true;

                    //Decimal numpad keys
                    case Keys.NumPad0: key = '0'; return true;
                    case Keys.NumPad1: key = '1'; return true;
                    case Keys.NumPad2: key = '2'; return true;
                    case Keys.NumPad3: key = '3'; return true;
                    case Keys.NumPad4: key = '4'; return true;
                    case Keys.NumPad5: key = '5'; return true;
                    case Keys.NumPad6: key = '6'; return true;
                    case Keys.NumPad7: key = '7'; return true;
                    case Keys.NumPad8: key = '8'; return true;
                    case Keys.NumPad9: key = '9'; return true;

                    //Special keys
                    case Keys.OemTilde: if (shift) { key = '~'; } else { key = '`'; } return true;
                    case Keys.OemSemicolon: if (shift) { key = ':'; } else { key = ';'; } return true;
                    case Keys.OemQuotes: if (shift) { key = '"'; } else { key = '\''; } return true;
                    case Keys.OemQuestion: if (shift) { key = '?'; } else { key = '/'; } return true;
                    case Keys.OemPlus: if (shift) { key = '+'; } else { key = '='; } return true;
                    case Keys.OemPipe: if (shift) { key = '|'; } else { key = '\\'; } return true;
                    case Keys.OemPeriod: if (shift) { key = '>'; } else { key = '.'; } return true;
                    case Keys.OemOpenBrackets: if (shift) { key = '{'; } else { key = '['; } return true;
                    case Keys.OemCloseBrackets: if (shift) { key = '}'; } else { key = ']'; } return true;
                    case Keys.OemMinus: if (shift) { key = '_'; } else { key = '-'; } return true;
                    case Keys.OemComma: if (shift) { key = '<'; } else { key = ','; } return true;
                    case Keys.Space: key = ' '; return true;
                }
            }

            key = (char)0;
            return false;
        }
    }
}
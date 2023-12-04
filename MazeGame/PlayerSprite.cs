using Maze;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeGame
{
    public delegate void KeyChange();
    public class PlayerSprite : DrawableGameComponent
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private int _steps;
        private int _offset = 16;
        public int Steps { get { return _steps; } }

        private ContentManager _content;
        private SpriteBatch _spriteBatch;
        private InputManager _inputManager;
        private IPlayer _player;
        private IMap _maze;

        private Texture2D _playerTexture;
        private Texture2D _tileTexture;

        private Vector2 _playerPosition;
        private Vector2 _previousPostion;
        private Vector2 _tilePostion;
        private Vector2[,] _tilePostions;

        public PlayerSprite(MazeGame game, IMap maze, Vector2[,] _tilePositions) : base(game)
        {
            _content = game.Content;
            _content.RootDirectory = game.Content.RootDirectory;

            _maze = maze;
            _player = _maze.Player;
            _tilePostions = _tilePositions;
            _steps = 0;
            Game.Components.Add(this);
        }

        public override void Initialize()
        {
            _playerPosition = _tilePostions[_player.StartX, _player.StartY];
            _playerPosition.X += _offset;
            _playerPosition.Y += _offset;
            _tilePostion = new Vector2(0, 0);
            _previousPostion = new Vector2(0, 0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(base.GraphicsDevice);
            _playerTexture = _content.Load<Texture2D>("Player");
            _tileTexture = _content.Load<Texture2D>("Path");

            _inputManager = InputManager.Instance;
            _inputManager.AddKeyHandler(Keys.Right, Right);
            _inputManager.AddKeyHandler(Keys.Left, Left);
            _inputManager.AddKeyHandler(Keys.Up, Up);
            _inputManager.AddKeyHandler(Keys.Down, Down);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (_player != null)
            {
                _inputManager.Update();
                _playerPosition = _tilePostions[_player.Position.X, _player.Position.Y];
                _playerPosition.X += _offset;
                _playerPosition.Y += _offset;
                _tilePostion = _previousPostion;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_player != null)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_playerTexture, _playerPosition, new Rectangle(0, 0, 32, 32), Color.White, _player.GetRotation(), new Vector2(_offset, _offset), 1, SpriteEffects.None, 1);
                _logger.Debug("Drew Player in Draw");
                _previousPostion = _playerPosition;
                if (_tilePostion != _playerPosition && _tilePostion != new Vector2(0, 0))
                {
                    _spriteBatch.Draw(_tileTexture, _tilePostion, new Rectangle(0, 0, 32, 32), Color.White, 0, new Vector2(_offset, _offset), 1, SpriteEffects.None, 1);
                    _logger.Debug("Drew previous player position as tile in Update");
                    _steps++;
                }
                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public void Clear()
        {
            _maze = null;
            _player = null;
        }

        private void Left()
        {
            if (_player != null)
            {
                _player.TurnLeft();
            }
        }
        private void Right()
        {
            if (_player != null)
            {
                _player.TurnRight();
            }
        }
        private void Up()
        {
            if (_player != null)
            {
                _player.MoveForward();
            }
        }
        private void Down()
        {
            if (_player != null)
            {
                _player.MoveBackward();
            }
        }
    }
}

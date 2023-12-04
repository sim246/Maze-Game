using Maze;
using MazeHuntKill;
using MazeRecursion;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Windows.Forms;

namespace MazeGame;

public class MazeGame : Game
{
    private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

    private GraphicsDeviceManager _graphics;
    private PlayerSprite _playerSprite;
    private SpriteBatch _spriteBatch;
    private IMap _maze;
    private readonly int _screen = 800;

    private SpriteFont _font;
    private Texture2D _wallTexture;
    private Texture2D _tileTexture;
    private Texture2D _goalTexture;
    private Vector2 _goalPosition;
    private Vector2[,] _tilePositions;

    enum GameState { Menu, LoadFile, Recursive, HuntKill, Size, Height, Width, Play, End }
    private GameState _state;
    private Color _colorLoad;
    private Color _colorRecursive;
    private Color _colorHuntKill;
    private KeyboardState _previousState;
    private OpenFileDialog openFileDialog;
    private string _filePath;
    private int _height = 0;
    private int _width = 0;

    public MazeGame()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            IsFullScreen = false,
            PreferredBackBufferWidth = _screen,
            PreferredBackBufferHeight = _screen
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _state = GameState.Menu;
        _colorRecursive = Color.Black;
        _colorHuntKill = Color.Black;
        _colorLoad = Color.Black;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _wallTexture = Content.Load<Texture2D>("Wall");
        _tileTexture = Content.Load<Texture2D>("Path");
        _goalTexture = Content.Load<Texture2D>("Goal");
        _font = Content.Load<SpriteFont>("font");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
        {
            _logger.Info("Game exited because Escape key was pressed");
            Exit();
        }

        var state = Keyboard.GetState();

        if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up) & !_previousState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
        {
            if (_state == GameState.Menu)
            {
                if (_colorLoad == Color.Black && _colorRecursive == Color.AliceBlue)
                {
                    _colorLoad = Color.AliceBlue;
                    _colorRecursive = Color.Black;
                    _colorHuntKill = Color.Black;
                }
                else if (_colorRecursive == Color.Black && _colorHuntKill == Color.AliceBlue)
                {
                    _colorLoad = Color.Black;
                    _colorRecursive = Color.AliceBlue;
                    _colorHuntKill = Color.Black;
                }
                else if (_colorHuntKill == Color.Black)
                {
                    _colorLoad = Color.Black;
                    _colorRecursive = Color.Black;
                    _colorHuntKill = Color.AliceBlue;
                }
            }
            if (_state == GameState.Height)
            {
                _height++;
            }
            if (_state == GameState.Width)
            {
                _width++;
            }
        }

        if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down) & !_previousState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
        {
            if (_colorHuntKill == Color.Black && _colorRecursive == Color.AliceBlue)
            {
                _colorLoad = Color.Black;
                _colorRecursive = Color.Black;
                _colorHuntKill = Color.AliceBlue;
            }
            else if (_colorRecursive == Color.Black && _colorLoad == Color.AliceBlue)
            {
                _colorLoad = Color.Black;
                _colorRecursive = Color.AliceBlue;
                _colorHuntKill = Color.Black;
            }
            else if (_colorLoad == Color.Black)
            {
                _colorLoad = Color.AliceBlue;
                _colorRecursive = Color.Black;
                _colorHuntKill = Color.Black;
            }
            if (_state == GameState.Height)
            {
                _height--;
            }
            if (_state == GameState.Width)
            {
                _width--;
            }
        }

        if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) & !_previousState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
        {
            if (_state == GameState.Menu)
            {
                if (_colorLoad == Color.AliceBlue)
                {
                    OpenFileDialogForm();
                    _colorLoad = Color.Black;
                    _state = GameState.LoadFile;
                }
                else if (_colorRecursive == Color.AliceBlue || _colorHuntKill == Color.AliceBlue)
                {
                    _state = GameState.Height;
                }
            }
            if (_state == GameState.Height && _height > 1)
            {
                _state = GameState.Width;
            }
            if (_state == GameState.Width && _width > 1)
            {
                _state = GameState.Size;
            }
            if (_state == GameState.Size)
            {
                if (_height > 1 && _width > 1)
                {
                    if (_colorHuntKill == Color.AliceBlue)
                    {
                        _colorHuntKill = Color.Black;
                        _state = GameState.HuntKill;
                    }
                    if (_colorRecursive == Color.AliceBlue)
                    {
                        _colorRecursive = Color.Black;
                        _state = GameState.Recursive;
                    }
                }
            }
        }

        _previousState = state;

        if (_state == GameState.LoadFile)
        {
            try
            {
                IMapProvider map = new MazeFromFile.MazeFromFile(mapPath: _filePath);
                _maze = new Map(map);
                _maze.CreateMap();

                _graphics.PreferredBackBufferWidth = _maze.Height * 32;
                _graphics.PreferredBackBufferHeight = _maze.Width * 32;
                _graphics.ApplyChanges();
                _tilePositions = new Vector2[_maze.Height, _maze.Width];
                _playerSprite = new PlayerSprite(this, _maze, _tilePositions);

                _state = GameState.Play;
            }
            catch
            {
                _logger.Info("Game shows menu because map failed to load, file was invalid");
                _state = GameState.Menu;
            }
        }

        if (_state == GameState.Recursive)
        {
            IMapProvider mazeRecursive = new MazeRecursive();
            _maze = new Map(mazeRecursive);
            _maze.CreateMap(_height, _width);

            _graphics.PreferredBackBufferWidth = _maze.Height * 32;
            _graphics.PreferredBackBufferHeight = _maze.Width * 32;
            _graphics.ApplyChanges();
            _tilePositions = new Vector2[_maze.Height, _maze.Width];
            _playerSprite = new PlayerSprite(this, _maze, _tilePositions);

            _state = GameState.Play;
        }

        if (_state == GameState.HuntKill)
        {
            IMapProvider mazeHuntKill = new HuntKill();
            _maze = new Map(mazeHuntKill);
            _maze.CreateMap(_height, _width);

            _graphics.PreferredBackBufferWidth = _maze.Height * 32;
            _graphics.PreferredBackBufferHeight = _maze.Width * 32;
            _graphics.ApplyChanges();
            _tilePositions = new Vector2[_maze.Height, _maze.Width];
            _playerSprite = new PlayerSprite(this, _maze, _tilePositions);

            _state = GameState.Play;
        }

        if (_state == GameState.Play)
        {
            if (_maze.IsGameFinished)
            {
                _maze = null;
                _filePath = null;
                _playerSprite.Clear();
                _playerSprite = null;
                _height = 0;
                _width = 0;
                _colorLoad = Color.Black;
                _colorRecursive = Color.Black;
                _colorHuntKill = Color.Black;
                _state = GameState.Menu;
            }
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (_state == GameState.Height || _state == GameState.Width)
        {
            GraphicsDevice.Clear(Color.BlanchedAlmond);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, "Inpu Size", new Vector2(200, 200), Color.BurlyWood);
            _spriteBatch.DrawString(_font, "Inpu Size: " + _height + "x" + _width, new Vector2(200, 400), Color.BurlyWood);
            _spriteBatch.End();
        }
        if (_state == GameState.Menu)
        {
            _graphics.PreferredBackBufferWidth = _screen;
            _graphics.PreferredBackBufferHeight = _screen;
            _graphics.ApplyChanges();
            GraphicsDevice.Clear(Color.BlanchedAlmond);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, "Menu", new Vector2(50, 0), Color.BurlyWood);
            _spriteBatch.DrawString(_font, "Load Maze File", new Vector2(50, 100), _colorLoad);
            _spriteBatch.DrawString(_font, "Draw Recursive Maze", new Vector2(50, 200), _colorRecursive);
            _spriteBatch.DrawString(_font, "Draw HuntKill Maze", new Vector2(50, 300), _colorHuntKill);
            _spriteBatch.End();
        }
        if (_state == GameState.Play)
        {
            GraphicsDevice.Clear(Color.BlanchedAlmond);
            DrawMaze();
        }
        base.Draw(gameTime);
    }

    public void OpenFileDialogForm()
    {
        openFileDialog = new OpenFileDialog
        {
            InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.ToString(),
            RestoreDirectory = true,
            DefaultExt = "txt",
            Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            _filePath = openFileDialog.FileName;
        }
    }

    private void DrawMaze()
    {
        int spaceX = 0;
        int spaceY = 0;
        for (int i = 0; i < _maze.Height; i++)
        {
            for (int j = 0; j < _maze.Width; j++)
            {
                _tilePositions[i, j] = new Vector2(i + spaceX, j + spaceY);
                if (_maze.Goal.Equals(new MapVector(i, j)))
                {
                    _goalPosition = _tilePositions[i, j];
                }
                spaceY += 31;
            }
            spaceY = 0;
            spaceX += 31;
        }
        _spriteBatch.Begin();
        for (int i = 0; i < _maze.Height; i++)
        {
            for (int j = 0; j < _maze.Width; j++)
            {
                if (_maze.MapGrid[i, j] == Block.Solid)
                {
                    _spriteBatch.Draw(_wallTexture, _tilePositions[i, j], new Rectangle(0, 0, 32, 32), Color.White);
                }
                else if (_maze.MapGrid[i, j] == Block.Empty)
                {
                    _spriteBatch.Draw(_tileTexture, _tilePositions[i, j], new Rectangle(0, 0, 32, 32), Color.White);
                }
            }
        }
        _spriteBatch.Draw(_goalTexture, _goalPosition, new Rectangle(0, 0, 32, 32), Color.White);
        _spriteBatch.End();
    }
}
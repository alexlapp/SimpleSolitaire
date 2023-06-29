using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleSolitaire.MonoGame.Extensions;

namespace SimpleSolitaire.MonoGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static Color ClearColor { get; } = new Color(22, 128, 17);

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
#if DEBUG
            SynchronizeWithVerticalRetrace = false
#else
            SynchronizeWithVerticalRetrace = true
#endif
        };
        _graphics.ApplyChanges();
        this.IsFixedTimeStep = false;
        
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
    }

#if DEBUG
    private TimeSpan _fpsTracker = TimeSpan.Zero;
    private int _frameCount = 0;
#endif

    protected override void Update(GameTime gameTime)
    {
#if DEBUG
        _fpsTracker += gameTime.ElapsedGameTime;
        if (_fpsTracker >= TimeSpan.FromSeconds(1))
        {
            Debug.WriteLine($"FPS: {_frameCount}");
            _fpsTracker = TimeSpan.Zero;
            _frameCount = 0;
        }   
#endif
        
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);

#if DEBUG
        _frameCount++;
#endif
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ClearColor);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}

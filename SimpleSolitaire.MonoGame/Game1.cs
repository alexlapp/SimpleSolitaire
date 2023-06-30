using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleSolitaire.Engine.Cards;
using SimpleSolitaire.MonoGame.Extensions;

namespace SimpleSolitaire.MonoGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _cardSpritesheet;
    private Pile _testPileOne;
    private Pile _testPileTwo;

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
        Random r = new Random();
        OrderingStrategy anyOrder = (Card top, Card bottom) => true; 
        _testPileOne = new Pile(anyOrder, PileFlow.Downwards);
        _testPileTwo = new Pile(anyOrder, PileFlow.Rightwards);
        
        for (int i = 0; i < 5; i++)
        {
            _testPileOne.Cards.Add(new Card(r.Next(1, 14), (Suit)Enum.GetValues(typeof(Suit)).GetValue(r.Next(4))));
            
            if (i % 2 == 0) 
                _testPileTwo.Cards.Add(new Card(r.Next(1, 14), (Suit)Enum.GetValues(typeof(Suit)).GetValue(r.Next(4))));
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _cardSpritesheet = Content.Load<Texture2D>("card_spritesheet");
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
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(Vector3.One * 2));

        DrawCard(_spriteBatch, new Card(12, Suit.Diamonds), new Vector2(10, 10));
        DrawPile(_spriteBatch, _testPileOne, new Vector2(50, 50));
        DrawPile(_spriteBatch, _testPileTwo, new Vector2(100, 50));

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawPile(SpriteBatch spriteBatch, Pile pile, Vector2 position)
    {
        var offset = Vector2.Zero;
        switch (pile.PileFlow)
        {
            case (PileFlow.Downwards): 
                offset = new Vector2(0, 13);
                break;
            
            case (PileFlow.Rightwards): 
                offset = new Vector2(12, 0);
                break;
        }
        
        for (int c = 0; c < pile.Cards.Count; c++)
        {
            DrawCard(spriteBatch, pile.Cards[c], position + (offset * c));
        }
    }

    private void DrawCard(SpriteBatch spriteBatch, Card card, Vector2 position)
    {
        var sourceRectangle = new Rectangle(0, 0, 32, 48);
        sourceRectangle.Y = (card.Rank - 1) * 48;
        switch (card.Suit)
        {  
            case (Suit.Spades): sourceRectangle.X = 0;
                break;
            case (Suit.Diamonds): sourceRectangle.X = 32 * 1;
                break;
            case (Suit.Clubs): sourceRectangle.X = 32 * 2;
                break;
            case (Suit.Hearts): sourceRectangle.X = 32 * 3;
                break;
        }
        
        spriteBatch.Draw(_cardSpritesheet, position, sourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
    }
}

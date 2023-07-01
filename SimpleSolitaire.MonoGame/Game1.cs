using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleSolitaire.Engine.Cards;
using SimpleSolitaire.MonoGame.Extensions;

namespace SimpleSolitaire.MonoGame;

// TODO: Move these classes
public class GraphicalCard : ICard
{
    public Rectangle Area { get; set; }
    public int Rank { get; }
    public Suit Suit { get; }
    public CardColor CardColor => Suit == Suit.Spades || Suit == Suit.Clubs ? CardColor.Black : CardColor.Red;

    public GraphicalCard(int rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
        Area = Rectangle.Empty;
    }

    public GraphicalCard(int rank, Suit suit, Rectangle area) : this(rank, suit)
    {
        Area = area;
    }
}

public class GraphicalPile : Pile<GraphicalCard>
{
    private Rectangle _area;

    public Rectangle Area
    {
        get => _area;
        set => _area = value;
    }

    public void AddCard(GraphicalCard card)
    {
        var offset = Point.Zero;
        switch (PileFlow)
        {
            case (PileFlow.Downwards): 
                offset = new Point(0, 13);
                break;
            
            case (PileFlow.Rightwards): 
                offset = new Point(12, 0);
                break;
        }

        var startPosition = Area.Location;

        card.Area = new Rectangle(startPosition + (new Point(offset.X * Cards.Count, offset.Y * Cards.Count)), card.Area.Size);
        Cards.Add(card);
    }

    public GraphicalPile(OrderingStrategy<GraphicalCard> orderingStrategy, PileFlow pileFlow = PileFlow.Stack,
        AcceptStrategy<GraphicalCard>? acceptFunction = null, EmptyAcceptStrategy<GraphicalCard>? emptyAcceptStrategy = null,
        CardAvailabilityStrategy<GraphicalCard>? cardAvailabilityStrategy = null) : base(orderingStrategy, pileFlow, acceptFunction,
        emptyAcceptStrategy, cardAvailabilityStrategy)
    {
    }

    public void SetPosition(Point position)
    {
        _area.X = position.X;
        _area.Y = position.Y;
    }
}

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _cardSpritesheet;
    private List<GraphicalPile> _piles;

    private Rectangle r = new Rectangle(10, 10, 20, 20);

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
        OrderingStrategy<GraphicalCard> anyOrder = (GraphicalCard top, GraphicalCard bottom) => true; 
        var testPileOne = new GraphicalPile(anyOrder, PileFlow.Downwards);
        testPileOne.Area = new Rectangle(50, 50, 32, 48);
        var testPileTwo = new GraphicalPile(anyOrder, PileFlow.Rightwards);
        testPileTwo.Area = new Rectangle(100, 50, 32, 48);
        
        for (int i = 0; i < 5; i++)
        {
            testPileOne.AddCard(new GraphicalCard(r.Next(1, 14), (Suit)Enum.GetValues(typeof(Suit)).GetValue(r.Next(4))));
            
            if (i % 2 == 0) 
                testPileTwo.AddCard(new GraphicalCard(r.Next(1, 14), (Suit)Enum.GetValues(typeof(Suit)).GetValue(r.Next(4))));
        }

        _piles = new List<GraphicalPile>() { testPileOne, testPileTwo };

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

    private bool _leftButtonWasPressed = false;

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
        var leftButtonPressed = Mouse.GetState().LeftButton == ButtonState.Pressed;

        if (leftButtonPressed && !_leftButtonWasPressed)
        {
            var mousePos = Mouse.GetState().Position;
            foreach (var pile in _piles)
            {
                // TODO: Clicky logic here
            }
        }

        _leftButtonWasPressed = leftButtonPressed;
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

        foreach (var pile in _piles)
        {
            DrawPile(_spriteBatch, pile);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawPile(SpriteBatch spriteBatch, GraphicalPile gPile)
    {
        foreach (var card in gPile.Cards)
        {
            DrawGraphicalCard(spriteBatch, card);
        }
    }

    private void DrawGraphicalCard(SpriteBatch spriteBatch, GraphicalCard gCard)
    {
        var sourceRectangle = new Rectangle(0, 0, 32, 48);
        sourceRectangle.Y = (gCard.Rank - 1) * 48;
        switch (gCard.Suit)
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
        
        spriteBatch.Draw(_cardSpritesheet, gCard.Area.Location.ToVector2(), sourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
    }
}

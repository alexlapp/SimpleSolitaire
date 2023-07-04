using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleSolitaire.Engine.Cards;
using SimpleSolitaire.MonoGame.Cards;
using SimpleSolitaire.MonoGame.Extensions;

namespace SimpleSolitaire.MonoGame;

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
        var r = new Random();
        OrderingStrategy anyOrder = (Card top, Card bottom) => true; 
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
    private GraphicalPile? dragPile = null;
    private GraphicalPile? dragStartPile = null;
    private Point dragOffset = Point.Zero;

    private (GraphicalPile?, int cardIndex) GetTopCardAtPoint(Point testPoint, bool includeEmptyPile = false)
    {
        foreach (var pile in _piles)
        {
            for (var i = pile.Cards.Count - 1; i >= 0; i--)
            {
                var gCard = pile.Cards[i];
                if (testPoint.X >= gCard.Area.X
                    && testPoint.X <= gCard.Area.X + gCard.Area.Width
                    && testPoint.Y >= gCard.Area.Y
                    && testPoint.Y <= gCard.Area.Y + gCard.Area.Height)
                {
                    Debug.WriteLine($"CLICK: {gCard.ToString()}");
                    return (pile, i);
                }
            }
            if (includeEmptyPile 
                && testPoint.X >= pile.Area.X
                && testPoint.X <= pile.Area.X + pile.Area.Width
                && testPoint.Y >= pile.Area.Y
                && testPoint.Y <= pile.Area.Y + pile.Area.Height)
            {
                return (pile, -1);
            }
        }

        return (null, -1);
    }

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
        var mousePos = (Mouse.GetState().Position.ToVector2() / 2).ToPoint();

        GraphicalPile? clickedPile = null;
        int cardIndex = -1;

        if (leftButtonPressed && !_leftButtonWasPressed)
        {
            (clickedPile, cardIndex) = GetTopCardAtPoint(mousePos);
        }

        if (clickedPile != null && dragPile == null)
        {
            dragStartPile = clickedPile;
            dragPile = clickedPile.PileFromIndex(cardIndex);
            dragOffset = mousePos - dragPile.Area.Location;
        }

        dragPile?.SetPosition(mousePos - dragOffset);

        if (_leftButtonWasPressed && !leftButtonPressed && dragPile != null && dragStartPile != null)
        {
            var (dropPile, _) = GetTopCardAtPoint(mousePos, includeEmptyPile: true);

            var targetPile = dropPile ?? dragStartPile;
            
            foreach (var card in dragPile.Cards)
            {
                targetPile.AddCard(card);
            }

            dragPile = null;
            dragStartPile = null;
            dragOffset = Point.Zero;
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
            _spriteBatch.DrawPile(_cardSpritesheet, pile);
        }
        
        if (dragPile != null)
            _spriteBatch.DrawPile(_cardSpritesheet, dragPile);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

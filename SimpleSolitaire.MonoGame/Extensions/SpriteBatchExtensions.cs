using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleSolitaire.Engine.Cards;
using SimpleSolitaire.MonoGame.Cards;

namespace SimpleSolitaire.MonoGame.Extensions;

public static class SpriteBatchExtensions
{
    public static void DrawPile(this SpriteBatch spritebatch, Texture2D cardSpritesheet, GraphicalPile pile)
    {
        spritebatch.DrawCard(cardSpritesheet, new GraphicalCard(16, Suit.Spades, pile.Area));
        foreach (var card in pile.Cards)
        {
            spritebatch.DrawCard(cardSpritesheet, card);
        }
    }

    public static void DrawCard(this SpriteBatch spriteBatch, Texture2D cardSpritesheet, GraphicalCard card)
    {
        var sourceRectangle = new Rectangle(0, (card.Rank - 1) * 48, 32, 48);
        switch (card.Suit)
        {
            case Suit.Spades: sourceRectangle.X = 0;
                break;
            case Suit.Diamonds: sourceRectangle.X = 32 * 1;
                break;
            case Suit.Clubs: sourceRectangle.X = 32 * 2;
                break;
            case Suit.Hearts: sourceRectangle.X = 32 * 3;
                break;
        }

        spriteBatch.Draw(cardSpritesheet, card.Area.Location.ToVector2(), sourceRectangle, Color.White, 0.0f,
            Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
    }
    
    public static void DrawOutlinedString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position,
        Vector2 origin, int outlineWidth = 1)
    {
        spriteBatch.DrawString(font, text, position + new Vector2(outlineWidth, -outlineWidth), Color.Black, 0, origin,
            1.0f, SpriteEffects.None, 0.5f);
        spriteBatch.DrawString(font, text, position + new Vector2(-outlineWidth, outlineWidth), Color.Black, 0, origin,
            1.0f, SpriteEffects.None, 0.5f);
        spriteBatch.DrawString(font, text, position - (Vector2.One * outlineWidth), Color.Black, 0, origin,
            1.0f, SpriteEffects.None, 0.5f);
        spriteBatch.DrawString(font, text, position + (Vector2.One * outlineWidth), Color.Black, 0, origin,
            1.0f, SpriteEffects.None, 0.5f);
        spriteBatch.DrawString(font, text, position + new Vector2(0, outlineWidth), Color.Black, 0, origin,
            1.0f, SpriteEffects.None, 0.5f);
        spriteBatch.DrawString(font, text, position + new Vector2(0, -outlineWidth), Color.Black, 0, origin,
            1.0f, SpriteEffects.None, 0.5f);
        spriteBatch.DrawString(font, text, position + new Vector2(outlineWidth, 0), Color.Black, 0, origin,
            1.0f, SpriteEffects.None, 0.5f);
        spriteBatch.DrawString(font, text, position + new Vector2(-outlineWidth, 0), Color.Black, 0, origin,
            1.0f, SpriteEffects.None, 0.5f);
        spriteBatch.DrawString(font, text, position, Color.White, 0, origin,
            1.0f, SpriteEffects.None, 0.5f);
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleSolitaire.MonoGame.Extensions;

public static class SpriteBatchExtensions
{
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
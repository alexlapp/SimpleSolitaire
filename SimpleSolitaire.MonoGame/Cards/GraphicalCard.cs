using Microsoft.Xna.Framework;
using SimpleSolitaire.Engine.Cards;

namespace SimpleSolitaire.MonoGame.Cards;

public class GraphicalCard : Card
{
    public Rectangle Area { get; set; }

    public GraphicalCard(int rank, Suit suit, Rectangle? area = null) : base(rank, suit)
    {
        Area = area ?? new Rectangle(0, 0, 32, 48);
    }
}
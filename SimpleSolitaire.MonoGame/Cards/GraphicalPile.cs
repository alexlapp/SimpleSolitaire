using System.Linq;
using Microsoft.Xna.Framework;
using SimpleSolitaire.Engine.Cards;

namespace SimpleSolitaire.MonoGame.Cards;



public class GraphicalPile : Pile<GraphicalCard>
{
    private Rectangle _area;

    public Rectangle Area
    {
        get => _area;
        set => _area = value;
    }

    private Point GetCardPosition(int cardIndex)
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

        return Area.Location + new Point(offset.X * cardIndex, offset.Y * cardIndex);
    } 

    public override void AddCard(GraphicalCard card)
    {
        var position = GetCardPosition(_cards.Count);
        card.Area = new Rectangle(position, card.Area.Size);
        
        base.AddCard(card);
    }

    private void UpdatePositions()
    {
        for (var i = 0; i < _cards.Count; i++)
        {
            _cards[i].Area = new Rectangle(GetCardPosition(i), _cards[i].Area.Size);
        }
    }

    public void SetPosition(Point position)
    {
        _area.X = position.X;
        _area.Y = position.Y;
        
        UpdatePositions();
    }

    public GraphicalPile(OrderingStrategy orderingStrategy, PileFlow pileFlow = PileFlow.Stack,
        AcceptStrategy? acceptFunction = null, EmptyAcceptStrategy? emptyAcceptStrategy = null,
        CardAvailabilityStrategy? cardAvailabilityStrategy = null) : base(orderingStrategy, pileFlow, acceptFunction,
        emptyAcceptStrategy, cardAvailabilityStrategy)
    {
    }

    public GraphicalPile(GraphicalPile other) : base(other)
    {
        _area = other._area;
    }

    public override GraphicalPile PileFromIndex(int index)
    {
        var result = new GraphicalPile(this)
        {
            _cards = TakeCardsFromIndex(index)
        };
        result._area = new Rectangle(result._cards.First().Area.Location, result.Area.Size);

        return result;
    }
}
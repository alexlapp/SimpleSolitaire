using System.Collections.Generic;

namespace SimpleSolitaire.Engine.Cards;

public class Pile
{
    private List<Card> _cards;
    private OrderingStrategy _orderingStrategy;
    private AcceptStrategy _acceptFunction;
    private EmptyAcceptStrategy _emptyAcceptStrategy;
    private CardAvailabilityStrategy _cardAvailabilityStrategy;

    public Pile(OrderingStrategy orderingStrategy, AcceptStrategy? acceptFunction = null, EmptyAcceptStrategy? emptyAcceptStrategy = null, CardAvailabilityStrategy? cardAvailabilityStrategy = null)
    {
        _cards = new List<Card>();
        
        _orderingStrategy = orderingStrategy;
        _emptyAcceptStrategy = emptyAcceptStrategy ?? EmptyAcceptStrategies.Any;
        _acceptFunction = acceptFunction ?? ((List<Card> pileCards, List<Card> incoming) =>
        {
            if (!(pileCards.Any() || !incoming.Any())) return false;

            return _orderingStrategy(pileCards.Last(), incoming.First());
        });
        _cardAvailabilityStrategy = cardAvailabilityStrategy ?? ((List<Card> cards, int cardIndex) =>
        {
            if (cardIndex < 0 || cardIndex >= cards.Count) return false;
            if (cardIndex == cards.Count - 1) return true;

            for (int i = cardIndex; i < cards.Count - 1; i++)
            {
                if (!_orderingStrategy(cards[i], cards[i + 1]))
                    return false;
            }

            return true;
        });
    }
}
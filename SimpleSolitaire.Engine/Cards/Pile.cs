using System.Collections.Generic;

namespace SimpleSolitaire.Engine.Cards;

public enum PileFlow
{
    Stack,
    Downwards,
    Rightwards,
}

public class Pile
{
    private List<Card> _cards;
    private OrderingStrategy _orderingStrategy;
    private AcceptStrategy _acceptFunction;
    private EmptyAcceptStrategy _emptyAcceptStrategy;
    private CardAvailabilityStrategy _cardAvailabilityStrategy;

    public List<Card> Cards => _cards;
    
    public PileFlow PileFlow { get; }

    public bool CanAcceptPile(Pile incoming) =>
        _cards.Any() ? _acceptFunction(_cards, incoming.Cards) : _emptyAcceptStrategy(incoming.Cards);

    public bool IsCardAvailable(int cardIndex) => _cardAvailabilityStrategy(_cards, cardIndex);

    public Pile(OrderingStrategy orderingStrategy, PileFlow pileFlow = PileFlow.Stack, AcceptStrategy? acceptFunction = null, EmptyAcceptStrategy? emptyAcceptStrategy = null, CardAvailabilityStrategy? cardAvailabilityStrategy = null)
    {
        _cards = new List<Card>();

        PileFlow = pileFlow;
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
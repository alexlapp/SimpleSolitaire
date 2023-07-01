using System.Collections.Generic;

namespace SimpleSolitaire.Engine.Cards;

public enum PileFlow
{
    Stack,
    Downwards,
    Rightwards,
}

public class Pile<TCard> where TCard : ICard
{
    private List<TCard> _cards;
    private OrderingStrategy<TCard> _orderingStrategy;
    private AcceptStrategy<TCard> _acceptFunction;
    private EmptyAcceptStrategy<TCard> _emptyAcceptStrategy;
    private CardAvailabilityStrategy<TCard> _cardAvailabilityStrategy;

    public List<TCard> Cards => _cards;
    
    public PileFlow PileFlow { get; }

    public bool CanAcceptPile(Pile<TCard> incoming)  =>
        _cards.Any() ? _acceptFunction(_cards, incoming.Cards) : _emptyAcceptStrategy(incoming.Cards);

    public bool IsCardAvailable(int cardIndex) => _cardAvailabilityStrategy(_cards, cardIndex);

    public Pile(OrderingStrategy<TCard> orderingStrategy, PileFlow pileFlow = PileFlow.Stack, AcceptStrategy<TCard>? acceptFunction = null, EmptyAcceptStrategy<TCard>? emptyAcceptStrategy = null, CardAvailabilityStrategy<TCard>? cardAvailabilityStrategy = null)
    {
        _cards = new List<TCard>();

        PileFlow = pileFlow;
        _orderingStrategy = orderingStrategy;
        _emptyAcceptStrategy = emptyAcceptStrategy ?? ((List<TCard> card) => true);
        _acceptFunction = acceptFunction ?? ((List<TCard> pileCards, List<TCard> incoming) =>
        {
            if (!(pileCards.Any() || !incoming.Any())) return false;

            return _orderingStrategy(pileCards.Last(), incoming.First());
        });
        _cardAvailabilityStrategy = cardAvailabilityStrategy ?? ((List<TCard> cards, int cardIndex) =>
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
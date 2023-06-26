namespace CardLib
{
    #region Card class enums

    public enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades,
        Red,
        Black
    }

    public enum SuitNoJokers
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    public enum Rank
    {
        Ace = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Joker
    }

    public enum RankNoJokers
    {
        Ace = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }

    public enum Face
    {
        Up,
        Down
    }
    #endregion

    #region Deck class enums

    public enum DeckSize
    {
        RegularDeck,
        RegularDeckWithJokers,
        Durak36Deck,
        Durak20Deck
    }

    #endregion

    #region Player class enums
    public enum PlayerType
    {
        Human,
        AI,
        None
    }
    #endregion

}

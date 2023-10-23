namespace FairPlayImporter.Model
{
    public class PlayerHand
    {
        public PlayerHand(IList<CardInHand> cards)
        {
            Cards = cards;
        }

        public IList<CardInHand> Cards { get; set; }
    }
}

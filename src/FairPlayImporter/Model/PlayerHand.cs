namespace FairPlayImporter.Model
{
    public class PlayerHand
    {
        public PlayerHand(List<CardInHand> cards)
        {
            Cards = cards;
        }

        public List<CardInHand> Cards { get; set; }
    }
}

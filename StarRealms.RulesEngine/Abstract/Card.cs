namespace StarRealms.RulesEngine.Abstract
{
    public abstract class Card
    {
        public abstract string Description { get; }
        public abstract Faction Faction { get; }
        public abstract string ImagePath { get; }
        public abstract string Name { get; }
        public abstract int Price { get; }
        public abstract void OnPlay(Game g);

        public override string ToString()
        {
            return this.Name;
        }
    }
}
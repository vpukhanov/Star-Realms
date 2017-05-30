namespace StarRealms.RulesEngine.Abstract
{
    /// <summary>
    /// Класс, определяющий карту в игре.
    /// </summary>
    public abstract class Card
    {
        /// <summary>
        /// Текстовое описание карты. Используется во всплывающих
        /// подсказках и диалогах выбора карты
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Фракция, которой принадлежит карта (ее цвет)
        /// </summary>
        public abstract Faction Faction { get; }

        /// <summary>
        /// Путь к изображению карты
        /// </summary>
        public abstract string ImagePath { get; }

        /// <summary>
        /// Название карты
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Стоимость покупки карты из торгового ряда
        /// </summary>
        public abstract int Price { get; }

        /// <summary>
        /// Метод, вызываемый при разыгрывании карты. Может активировать
        /// ее эффекты
        /// </summary>
        /// <param name="g">Текущая игра</param>
        public abstract void OnPlay(Game g);

        public override string ToString()
        {
            return this.Name;
        }
    }
}
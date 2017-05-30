using StarRealms.RulesEngine.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StarRealms.RulesEngine
{
    /// <summary>
    /// Класс вспомогательных методов. Также является связью между
    /// графическим интерфейсом пользователя и игроком <see cref="StarRealms.RulesEngine.Players.HumanPlayer"/>
    /// в игре
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Метод GUI, позволяющий игроку выбрать одну карту из списка карт
        /// </summary>
        public static Func<List<Card>, Card> userCardChoiceMethod = null;

        /// <summary>
        /// Метод GUI, позволяющий уведомить игрока о каком-то событии
        /// </summary>
        public static Action<string> userNotifyMethod = null;

        /// <summary>
        /// Метод GUI, позволяющий игроку сделать выбор из предложенных вариантов
        /// </summary>
        public static Func<List<string>, int> userStringChoiceMethod = null;

        /// <summary>
        /// Метод GUI, позволяющий игроку сделать выбор - да или нет
        /// </summary>
        public static Func<string, string, bool> userYesNoChoiceMethod = null;

        private static Random random = new Random();

        /// <summary>
        /// Наполнить ObservableCollection <paramref name="dest"/> содержимым перечислимой
        /// коллекции <paramref name="src"/>.
        /// </summary>
        /// <typeparam name="T">Тип элементов содержимого коллекций</typeparam>
        /// <param name="dest">Куда необходимо скопировать содержимое</param>
        /// <param name="src">Источник данных</param>
        public static void CopyFromListToObservableCollection<T>(ObservableCollection<T> dest, IEnumerable<T> src)
        {
            foreach (T element in src)
            {
                dest.Add(element);
            }
        }

        /// <summary>
        /// Получить правильное окончание множественного числа для существительного
        /// </summary>
        /// <param name="number">Число предметов</param>
        /// <param name="strFormOne">Форма окончания для числа 1 (например, яблоко)</param>
        /// <param name="strFormFour">Форма окончания для числа 4 (например, яблока)</param>
        /// <param name="strFormFive">Форма окончания для числа 5 (например, яблок)</param>
        /// <returns>Множественное число существительного, соответствующее числу <paramref name="number"/></returns>
        public static string GetPlural(int number, string strFormOne, string strFormFour, string strFormFive)
        {
            number %= 100;

            if (number >= 11 && number <= 19)
            {
                return strFormFive;
            }
            else
            {
                number %= 10;

                if (number == 1)
                    return strFormOne;
                else if (number >= 2 && number <= 4)
                    return strFormFour;
                else
                    return strFormFive;
            }
        }

        /// <summary>
        /// Метод, перемешивающий коллекцию IList
        /// </summary>
        /// <typeparam name="T">Тип элементов, хранящихся в коллекции</typeparam>
        /// <param name="list">Коллекция для перемешивания</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
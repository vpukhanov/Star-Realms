using StarRealms.RulesEngine.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StarRealms.RulesEngine
{
    public static class Utilities
    {
        public static Func<List<Card>, Card> userCardChoiceMethod = null;
        public static Action<string> userNotifyMethod = null;
        public static Func<List<string>, int> userStringChoiceMethod = null;
        public static Func<string, string, bool> userYesNoChoiceMethod = null;
        private static Random random = new Random();
        public static void CopyFromListToObservableCollection<T>(ObservableCollection<T> dest, List<T> src)
        {
            foreach (T element in src)
            {
                dest.Add(element);
            }
        }

        public static string GetPlural(int number, string strFormOne, string strFormFour, string strFormFive)
        {
            number %= 100;

            if (number >= 11 || number <= 19)
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
        /// Метод, перемешивающий массивы. Необходим для перемешивания колод.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
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
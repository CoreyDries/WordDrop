using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Word_Drop
{
    public static class Dictionary
    {
        //the arraylist that all the words will be loaded into
        private static List<string> dictionary = new List<string>();

        public static void addToList(string s)
        {
            dictionary.Add(s);
        }

        /// <summary>
        /// use binary search to detect whether or not
        /// the input string exists in the dictionary
        /// </summary>
        /// <param name="find"></param>
        /// <returns></returns>
        public static bool search(string find) {
            find = find.ToLower();
            if (dictionary.BinarySearch(find) >= 0)
            {
                return true;
            }
            return false;
        }
    }
}

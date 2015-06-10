using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Word_Drop
{
    class Tile
    {
        private char letter; //contains the character value of the tile
        private int val; //contains the points value of the tile
        private int xCoord; //the column that the tile got put into
        private int yCoord; //the row that the tile got put into
        public static Random r = new Random(); //the generator for making new random tiles

        /// <summary>
        /// accessor for the letter field
        /// </summary>
        public char Letter
        {
            get
            {
                return letter;
            }
            set
            {
                letter = value;
            }
        }

        /// <summary>
        /// accessor for the val field
        /// </summary>
        public int Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
            }
        }

        /// <summary>
        /// accessors for the pt field
        /// this gets and sets the location of a particular tile
        /// after it has been placed on the board
        /// </summary>
        public Point pt
        {
            get
            {
                return new Point(xCoord, yCoord);
            }
        }
        public int ptX
        {
            set
            {
                xCoord = value;
            }
        }
        public int ptY
        {
            set
            {
                yCoord = value;
            }
        }
        /*
         * The arrays that represent character rarity and points;
         * they're indexed in such a way that they're translatable
         * and you can even look and see that D has 4 rarity and 2 points
         */

        //the characters
        private static char[] chars = { 'A', 'B', 'C', 'D', 'E', 'F',
                                   'G', 'H', 'I', 'J', 'K',
                                   'L', 'M', 'N', 'O', 'P',
                                   'Q', 'R', 'S', 'T', 'U',
                                   'V', 'W', 'X', 'Y', 'Z' };
        //rarity is based on the likelyhood out of 98 (the number of non-blank tiles in scrabble) to draw each tile
        private static int[] rarity = { 9, 2, 2, 4, 12, 2,
                                   3, 2, 9, 1, 1,
                                   4, 2, 6, 8, 2,
                                   1, 6, 4, 6, 4,
                                   2, 2, 1, 2, 1 };
        //points is based on the tiles in the classic scrabble game and the user gains points based on these numbers when they complete a word
        private static int[] points = { 1, 3, 3, 2, 1, 4,
                                    2, 4, 1, 8, 5,
                                    1, 3, 1, 1, 3,
                                    10, 1, 1, 1, 1,
                                    4, 2, 8, 4, 10 };

        /// <summary>
        /// The tile constructor, it generates a random letter
        /// and assigns this letter and its associated points value to the tile
        /// </summary>
        public Tile()
        {
            //change to 100 for blank, and add a blank char to all arrays
            int temp = r.Next(98);
            int current = 0;
            for (int i = 0; i < 26; i++) //for each letter in the alphabet
            {
                //go through the rarity list
                current += rarity[i];
                //accept the first letter that gets above the random value
                if (temp <= current)
                {
                    letter = chars[i];
                    val = points[i];
                    break;
                }
            }
        }

        /// <summary>
        /// Translates a character to its points value for characters so they don't have to be tile objects
        /// </summary>
        /// <param name="a">the character to find the points for</param>
        /// <returns>the points value that char 'a' is worth</returns>
        public static int getScore(char a)
        {
            for (int i = 0; i < 26; i++)
            {
                if (a == chars[i])
                {
                    return points[i];
                }
            }
                return 0;
        }
    }
}

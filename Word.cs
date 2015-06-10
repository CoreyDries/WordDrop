using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Word_Drop
{
    class Word
    {
        private String w; // the word part of a word
        // the tiles that make up each individual letter
        // this allows me to keep track of the position of a word on the board
        private ArrayList letters = new ArrayList();
        // a total score to be associated with a word
        private int s;

        /// <summary>
        /// Accessor for the word part of the word
        /// </summary>
        public String word
        {
            get
            {
                return w;
            }
            set
            {
                w = value;
            }
        }

        /// <summary>
        /// Accessor for a word's score
        /// </summary>
        public int score
        {
            get
            {
                return s;
            }
            set
            {
                s = value;
            }
        }

        /// <summary>
        /// Allows you to add a tile (letter) to the word
        /// </summary>
        /// <param name="t">the tile to be added</param>
        public void setTile(Tile t)
        {
            letters.Add(t);
        }

        /// <summary>
        /// Access to the list of tiles representing the word
        /// </summary>
        /// <returns>the tile list</returns>
        public ArrayList getLetters()
        {
            return letters;
        }
    }
}

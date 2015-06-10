using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Word_Drop
{
    class Hand
    {
        private Tile[] tiles = new Tile[7]; //the 7 tiles that the player has access to
        private int points = 0; //the points that the player/hand currently has

        public int Points
        {
            get
            {
                return points;
            }
            set
            {
                points += value;
            }
        }

        /// <summary>
        /// Initializes a hand, by drawing 7 tiles for it.
        /// Each new tile is passed to the tile class, creating a random tile
        /// </summary>
        public Hand()
        {
            for (int i = 0; i < 7; i++)
            {
                tiles[i] = new Tile();
            }
        }

        /// <summary>
        /// Draws a new tile for the hand to replace the one that was played
        /// </summary>
        /// <param name="place">the index that needs to be redrawn</param>
        /// <returns>the new total score for the hand</returns>
        public int playTile(int place)
        {
            tiles[place] = new Tile();
            return points;
        }

        /// <summary>
        /// Simply returns the tile object at the specified location in the player's hand
        /// </summary>
        /// <param name="place">The location in the hand to return</param>
        /// <returns>The tile at the specified location</returns>
        public Tile getTile(int place)
        {
            return tiles[place];
        }
    }
}

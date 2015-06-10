using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Word_Drop
{
    class Board
    {
        private const int lastCol = 7;     //the width parameter of the board, from 0
        private const int lastRow = 7;     // the height parameter of the board, from 0
        private const int minSize = 2;      // mimimum amount of letters in a word
        private List<Tile> pts = new List<Tile>();
        private List<String> messageQueue = new List<String>();
        private Tile[ , ] board = new Tile[ 8, 8 ];
        private int totalMoves =  64;

        public Board()
        {
            for ( int i = 0; i <= lastCol; i++ )
            {
                for ( int j = 0; j <= lastRow; j++ )
                {
                    this.board[ i, j ] = new Tile();
                    board[ i, j ].Letter = '\0';
                }
            }
        }

        public int Moves
        {
            get
            {
                return totalMoves;
            }
        }

        public void addMessage( List<Word> words )
        {
            String message = "";
            int score = words[ 0 ].score;
            if ( words.Count == 2 )
                score += words[ 1 ].score;
            if ( score > 0 )
            {
                message += " played " + words[ 0 ].word;
                if ( words.Count > 1 )
                    message += " and " + words[ 1 ].word;
                message += " for " + score + " points.";
                messageQueue.Add( message );

                if ( messageQueue.Count > 4 )
                    messageQueue.RemoveAt( 0 );
            }
        }

        public void addName( String name )
        {
            messageQueue[ messageQueue.Count - 1 ] = name + messageQueue[ messageQueue.Count - 1 ];
        }

        public List<String> getMessages()
        {
            return messageQueue;
        }

        public bool isFull( int column )
        {
            if ( board[ column, lastRow ].Letter == '\0' )
            {
                return false;
            }
            return true;
        }

        public int getBottom( int column )
        {
            for ( int i = 0; i <= lastRow; i++ )
            {
                if ( board[ column, i ].Letter == '\0' )
                {
                    return i;
                }
            }
            //this means the row is full
            return -1;
        }

        public List<Tile> getDroppedTile()
        {
            List<Tile> tiles = new List<Tile>();
            foreach ( Tile t in board )
            {
                if ( t.Letter != '\0' )
                    tiles.Add( t );
            }
            return tiles;
        }

        /// <summary>
        /// Drops row tile with character value 'drop' into the column 'column'
        /// </summary>
        /// <param name="column">the column to drop the tile in</param>
        /// <param name="drop">which tile to drop in the column</param>
        public List<Tile> dropTile( int column, Tile drop, Hand hand )
        {
            totalMoves--;
            for ( int i = 0; i <= lastRow; i++ )
            {
                if ( board[ column, i ].Letter == '\0' )
                {
                    Tile newTile = new Tile();
                    newTile.Letter = drop.Letter;
                    newTile.Value = drop.Value;
                    newTile.ptX = column;
                    newTile.ptY = i;
                    board[ column, i ] = newTile;

                    Word finalWord = getScore( findWords( newTile ), true );
                    hand.Points = finalWord.score;
                    pts.Clear();

                    foreach ( Tile t in finalWord.getLetters() )
                    {
                        if ( t.Letter != '\0' )
                        {
                            bool found = false;
                            foreach ( Tile current in pts )
                            {
                                if ( t.pt.X == current.pt.X && t.pt.Y == current.pt.Y )
                                    found = true;
                            }
                            if (!found)
                                pts.Add( t ); //this is going to be an arraylist of tiles
                        }
                    }
                    return pts;
                }
            }
            return pts; //should be returning nothing, but will never reach this line assuming you can't drop in a full column
        }

        public int peekDrop( int column, Tile drop, Hand hand )
        {
            for ( int i = 0; i <= lastRow; i++ )
            {
                if ( board[ column, i ].Letter == '\0' )
                {
                    Tile newTile = new Tile();
                    newTile.Letter = drop.Letter;
                    newTile.Value = drop.Value;
                    newTile.ptX = column;
                    newTile.ptY = i;
                    board[ column, i ] = newTile;
                    Word finalWord = getScore( findWords( newTile ), false );
                    board[ column, i ].Letter = '\0';
                    return finalWord.score;
                }
            }
            return 0;
        }

        private ArrayList[] findWords( Tile tile )
        {
            ArrayList[] words = new ArrayList[ 4 ];

            // tile.pt.X == dropCol
            // tile.pt.Y == dropRow

            words[ 0 ] = new ArrayList();  // vertical words
            words[ 1 ] = new ArrayList();  // horizontal words
            words[ 2 ] = new ArrayList();  // diagonal upper-left to bottom-right words
            words[ 3 ] = new ArrayList();  // diagonal lower-left to upper-right words
            Word tempWord = new Word();

            // check vertical
            // for each potential word
            // if (findWord(potential word))
            // insert potential word into words[0]

            for ( int row = 0; row <= lastRow + 1; row++ )
            {
                for ( int wordSpot = tile.pt.Y + 1 - row; wordSpot <= tile.pt.Y; wordSpot++ )
                {
                    tempWord = new Word();
                    for ( int wordOffset = 0; wordOffset < row; wordOffset++ )
                    {
                        if ( wordSpot + wordOffset >= 0 && wordSpot + wordOffset <= lastRow )
                        {
                            tempWord.word = board[ tile.pt.X, wordSpot + wordOffset ].Letter + tempWord.word;
                            tempWord.setTile( board[ tile.pt.X, wordSpot + wordOffset ] );
                        }
                    }

                    if ( !tempWord.word.Contains( '\0' ) && tempWord.word.Length >= minSize )
                    {
                        if ( Dictionary.search( tempWord.word ) )
                        {
                            words[ 0 ].Add( tempWord );
                        }
                    }
                }
            }


            // check horizontal
            // for each potential word
            // if (findWord(potential word))
            // insert potential word into words[1]

            //for (int column = 0; column <= lastCol + 1; column++)
            //{
            //    for (int wordSpot = tile.pt.X + 1 - column; wordSpot <= tile.pt.X; wordSpot++)
            //    {
            //        tempWord = new Word();
            //        for (int wordOffset = 0; wordOffset < column; wordOffset++)
            //        {
            //            if (wordSpot + wordOffset >= 0 && wordSpot + wordOffset <= lastCol)
            //            {
            //                tempWord.word += board[wordSpot + wordOffset, tile.pt.Y].Letter;
            //                tempWord.setTile(board[wordSpot + wordOffset, tile.pt.Y]);
            //            }
            //        }
            //
            //        if (!tempWord.word.Contains('\0') && tempWord.word.Length >= minSize)
            //        {
            //            if (Dictionary.search(tempWord.word))
            //            {
            //                words[1].Add(tempWord);
            //            }
            //        }
            //    }
            //}

            int leftBounds = 0;
            for ( int i = tile.pt.X; i >= 0; i-- )
            {
                if ( board[ i, tile.pt.Y ].Letter == '\0' )
                {
                    leftBounds = i + 1;
                    break;
                }
            }
            int rightBounds = lastCol;
            for ( int i = tile.pt.X; i <= lastCol; i++ )
            {
                if ( board[ i, tile.pt.Y ].Letter == '\0' )
                {
                    rightBounds = i - 1;
                    break;
                }
            }
            int maxSize = rightBounds - leftBounds + 1;

            for ( int wordSize = minSize; wordSize <= maxSize; wordSize++ )
            {
                for ( int offset = Math.Max( ( tile.pt.X + 1 - wordSize ), leftBounds );
                    offset <= Math.Min( ( rightBounds - wordSize + 1 ), tile.pt.X ); offset++ )
                {
                    tempWord = new Word();
                    for ( int i = offset; i < ( offset + wordSize ); i++ )
                    {
                        tempWord.word += board[ i, tile.pt.Y ].Letter;
                        tempWord.setTile( board[ i, tile.pt.Y ] );
                    }
                    if ( Dictionary.search( tempWord.word ) )
                        words[ 1 ].Add( tempWord );
                }
            }

            /*
            // check diagonal upper-left to bottom-right
            // for each potential word
                // if (findWord(potential word))
                    // insert potential word into words[2]

            // temporary variables needed for diagonal detection
            int startBoundRow = tile.pt.Y;
            int startBoundCol = tile.pt.X;
            int endBoundRow = tile.pt.Y;
            int endBoundCol = tile.pt.X;
            int positionOfTileInDiagTemp;
            int[] diagTemp;

            // initialization of the starting bounds of the diagonal span to search through
            while ((startBoundRow > 0) && (startBoundCol > 0) &&
                (board[startBoundRow - 1, startBoundCol - 1] != '\0'))
            {
                startBoundRow--;
                startBoundCol--;
            }

            // initialization of the ending bounds of the diagonal span to search through
            while ((endBoundRow < lastRow) && (endBoundCol < lastCol) &&
                (board[endBoundRow + 1, endBoundCol + 1] != '\0'))
            {
                endBoundRow++;
                endBoundCol++;
            }

            // initialization of the temporary array representing the diagonal span
            positionOfTileInDiagTemp = startBoundCol - tile.pt.X; //pos of the dropped tile in that temporary array

            diagTemp = new int[endBoundRow - startBoundRow + 1];
            for (int i = 0; startBoundRow <= endBoundRow; i++)
            {
                diagTemp[i] = board[startBoundRow++, startBoundCol++];
            }

            // Searching for words in the diagTemp array
            for (int begin = 0; begin <= positionOfTileInDiagTemp; begin++)
            {
                for (int end = positionOfTileInDiagTemp; end <= diagTemp.Length; end++ )
                {
                    tempWord = "";
                    for (int i = begin; i <= end; i++)
                    {
                        //tempWord += diagTemp[i];    // EXCEPTION
                    }
                    if (Dictionary.search(tempWord))
                    {
                        words[2].Add(tempWord);
                        Console.WriteLine("word found!");
                    }
                }
            }

            
             
            // check diagonal lower-left to upper-right
            // for each potential word
                // if (findWord(potential word))
                    // insert potential word into words[3]

            startBoundRow = tile.pt.Y;
            startBoundCol = tile.pt.X;
            endBoundRow = tile.pt.Y;
            endBoundCol = tile.pt.X;

            // initialization of the starting bounds of the diagonal span to search through
            while ((startBoundRow < lastRow) && (startBoundCol > 0) &&
                (board[startBoundRow + 1, startBoundCol - 1] != '\0'))
            {
                startBoundRow++;
                startBoundCol--;
            }

            // initialization of the ending bounds of the diagonal span to search through
            while ((endBoundRow > 0) && (endBoundCol < lastCol) &&
                (board[endBoundRow - 1, endBoundCol + 1] != '\0'))
            {
                endBoundRow--;
                endBoundCol++;
            }

            // initialization of the temporary array representing the diagonal span
            positionOfTileInDiagTemp = startBoundCol - tile.pt.X; //pos of the dropped tile in that temporary array

            diagTemp = new int[endBoundRow - startBoundRow + 1];
            for (int i = 0; startBoundCol <= endBoundCol; i++)         // POSSIBLE LOGIC ERROR
            {
                diagTemp[i] = board[startBoundRow--, startBoundCol++];  // EXCEPTION
            }

            // Searching for words in the diagTemp array
            for (int begin = 0; begin <= positionOfTileInDiagTemp; begin++)
            {
                for (int end = positionOfTileInDiagTemp; end <= diagTemp.Length; end++)
                {
                    tempWord = "";
                    for (int i = begin; i <= end; i++)
                    {
                       // tempWord += diagTemp[i];        // EXCEPTION
                    }
                    if (Dictionary.search(tempWord))
                    {
                        words[3].Add(tempWord);
                    }
                }
            }
             * */
            return words;
        }

        private Word getScore( ArrayList[] words, bool mess )
        {
            int tempScore = 0;
            Word finalWord = new Word();
            List<Word> myWords = new List<Word>();

            //comments are an example being worked through

            foreach ( ArrayList wordset in words ) //cat fish dog, cheese pie grapes, dirt stone clay, orange red blue
            {
                Word tempWord = new Word();
                foreach ( Word w in wordset ) //cat, then fish, then dog
                {
                    tempScore = 0;
                    foreach ( char c in w.word ) //c, then a, then t
                    {
                        tempScore += Tile.getScore( c ); //add points for c, a, t
                    }
                    if ( tempScore > tempWord.score ) //check if cat or fish or dog is worth more points
                    {
                        tempWord = w;
                        tempWord.score = tempScore;
                    }
                }
                finalWord.score += tempWord.score; //add the best result of cat fish dog to the return variable
                if ( tempWord.score != 0 )
                {
                    myWords.Add( tempWord );
                }
                foreach ( Tile t in tempWord.getLetters() )
                {
                    finalWord.setTile( t );
                }
            }
            if ( mess && myWords.Count != 0 )
                addMessage( myWords );
            return finalWord;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;

namespace Word_Drop
{
    class MainView : View
    {
        private Point coords = new Point();
        private Point animatedTile = new Point();
        private Paint myPaint = new Paint();
        private Paint highlightPaint = new Paint();
        private Paint compHighlightPaint = new Paint();
        private Bitmap[] letters = new Bitmap[26];
        private Bitmap grid;
        private Bitmap menu;
        private Bitmap victory;
        private Point screenSize = new Point();
        private int width;
        private int currentTile = -1;
        private int currentPlayer = 0;
        private int mode; //1 for menu, 2 3 4 for local multiplayer, 5 6 7 for ai's
        private int[] scores = new int[4];
        private bool dropping = false;
        private bool isOver = false; //depicts whether or not the game has ended

        private List<Point> highlights = new List<Point>();
        private List<Point> compHighlights = new List<Point>();
        private List<Hand> players = new List<Hand>();
        private Board board;

        public Point Coords
        {
            get
            {
                return coords;
            }
            set
            {
                coords = value;
                Invalidate();
            }
        }

        public Point Animated
        {
            get
            {
                return animatedTile;
            }
            set
            {
                animatedTile = value;
                Invalidate();
            }
        }

        public bool Drop
        {
            get
            {
                return dropping;
            }
            set
            {
                dropping = value;
            }
        }

        public bool IsOver
        {
            get
            {
                return isOver;
            }
            set
            {
                isOver = value;
            }
        }

        public int Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
                if( value == 1 )
                {
                    //stop players from taking turns
                    isOver = false;
                    currentTile = -1;
                }
                else
                {
                    //create new game, MAKE SURE TO RESET ALL VARIABLES
                    isOver = false;
                    currentTile = -1;
                    currentPlayer = 0;
                    for( int i = 0; i < 4; i++ )
                        scores[i] = 0;
                    highlights = new List<Point>();
                    compHighlights = new List<Point>();
                    board = new Board();
                    players = new List<Hand>();
                    players.Add( new Hand() );
                    players.Add( new Hand() );
                    if( value == 3 || value == 4 )
                        players.Add( new Hand() );
                    if( value == 4 )
                        players.Add( new Hand() );
                }
            }
        }

        public int SelectTile
        {
            get
            {
                return currentTile;
            }
            set
            {
                currentTile = value;
                Invalidate();
            }
        }

        public int tileWidth
        {
            get
            {
                return width;
            }
        }

        public MainView( Context context )
            : base( context )
        {
            players.Add( new Hand() );
            players.Add( new Hand() );
            board = new Board();
            mode = 1;

            myPaint.SetARGB( 255, 0, 128, 128 );
            myPaint.SetStyle( Paint.Style.Stroke );
            myPaint.StrokeWidth = 2;
            myPaint.TextSize = 20;
            myPaint.SetTypeface( null );

            highlightPaint.SetARGB( 128, 0, 0, 196 );
            highlightPaint.SetStyle( Paint.Style.Fill );
            compHighlightPaint.SetARGB( 128, 196, 0, 0 );
            compHighlightPaint.SetStyle( Paint.Style.Fill );

            var metrics = Resources.DisplayMetrics;
            screenSize.X = metrics.WidthPixels;
            screenSize.Y = metrics.HeightPixels;

            width = ( screenSize.X / 8 );

            grid = BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.grid );
            menu = BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.Menu );
            victory = BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.Victory );
            letters[0] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.A ), width, width, false );
            letters[1] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.B ), width, width, false );
            letters[2] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.C ), width, width, false );
            letters[3] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.D ), width, width, false );
            letters[4] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.E ), width, width, false );
            letters[5] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.F ), width, width, false );
            letters[6] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.G ), width, width, false );
            letters[7] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.H ), width, width, false );
            letters[8] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.I ), width, width, false );
            letters[9] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.J ), width, width, false );
            letters[10] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.K ), width, width, false );
            letters[11] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.L ), width, width, false );
            letters[12] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.M ), width, width, false );
            letters[13] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.N ), width, width, false );
            letters[14] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.O ), width, width, false );
            letters[15] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.P ), width, width, false );
            letters[16] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.Q ), width, width, false );
            letters[17] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.R ), width, width, false );
            letters[18] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.S ), width, width, false );
            letters[19] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.T ), width, width, false );
            letters[20] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.U ), width, width, false );
            letters[21] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.V ), width, width, false );
            letters[22] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.W ), width, width, false );
            letters[23] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.X ), width, width, false );
            letters[24] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.Y ), width, width, false );
            letters[25] = Bitmap.CreateScaledBitmap( BitmapFactory.DecodeResource( context.Resources, Resource.Drawable.Z ), width, width, false );
        }

        protected override void OnDraw( Canvas canvas )
        {
            base.OnDraw( canvas );
            if( Mode == 1 )
            {
                canvas.DrawBitmap( menu, 0, 0, myPaint );
                canvas.DrawText( "Mode: " + Mode.ToString(), 0, 40, myPaint );
            }
            else
            {
                canvas.DrawBitmap( grid, 0, 0, myPaint );
                String outputScores = "";
                outputScores += "Player 1: " + players[0].Points.ToString();
                if( mode == 5 || mode == 6 || mode == 7 )
                    outputScores += "  Comp: " + players[1].Points.ToString();
                if( mode == 2 || mode == 3 || mode == 4 )
                    outputScores += "   Player 2: " + players[1].Points.ToString();
                if( mode == 3 || mode == 4 )
                    outputScores += "  Player 3: " + players[2].Points.ToString();
                if( mode == 4 )
                    outputScores += "  Player 4: " + players[3].Points.ToString();
                canvas.DrawText( outputScores, 0, 9 * tileWidth + 20, myPaint );

                int messageSpot = 11 * tileWidth;
                foreach( String s in board.getMessages() )
                {
                    canvas.DrawText( s, 0, messageSpot, myPaint );
                    messageSpot += 20;
                }

                if( !dropping && currentTile != -1 )
                    canvas.DrawBitmap( letters[players[currentPlayer].getTile( currentTile ).Letter - 65], coords.X - ( width / 2 ), coords.Y - ( width / 2 ), myPaint );
                if( dropping )
                {
                    canvas.DrawBitmap( letters[players[currentPlayer].getTile( currentTile ).Letter - 65], animatedTile.X, animatedTile.Y, myPaint );
                }

                for( int i = 0; i < 7; i++ )
                {
                    if( i != currentTile )
                    {
                        canvas.DrawBitmap( letters[players[currentPlayer].getTile( i ).Letter - 65], width / 2 + width * i, width * 19 / 2, myPaint );
                    }
                }
                foreach( Tile t in board.getDroppedTile() )
                    canvas.DrawBitmap( letters[t.Letter - 65], t.pt.X * tileWidth, ( 8 - t.pt.Y ) * tileWidth, myPaint );

                foreach( Point p in highlights )
                    canvas.DrawRect( p.X * width, ( 8 - p.Y ) * width, p.X * width + width, ( 8 - p.Y ) * width + width, highlightPaint );

                foreach( Point p in compHighlights )
                    canvas.DrawRect( p.X * width, ( 8 - p.Y ) * width, p.X * width + width, ( 8 - p.Y ) * width + width, compHighlightPaint );
                if( isOver )
                {
                    canvas.DrawBitmap( victory, width, 3 * width, myPaint );
                    canvas.DrawText( "Game Over", (int)( 3.5 * width ), 4 * width, myPaint );
                    if( mode != 4 )
                        players.Add( new Hand() );
                    if( mode != 4 && mode != 3 )
                        players.Add( new Hand() );
                    int temp = 0;
                    if( players[0].Points < players[1].Points )
                        temp = 1;
                    if( players[temp].Points < players[2].Points )
                        temp = 2;
                    if( players[temp].Points < players[3].Points )
                        temp = 3;
                    if (mode == 2 || mode == 3 || mode == 4)
                        canvas.DrawText( "Player " + (temp + 1) + " wins!", 3 * width, 5 * width, myPaint );
                    else
                    {
                        if (temp == 0)
                            canvas.DrawText( "Player 1 wins!", 3 * width, 5 * width, myPaint );
                        else
                            canvas.DrawText( "Computer wins!", 3 * width, 5 * width, myPaint );
                    }
                }
            }
        }

        public int getBottom( int column )
        {
            return board.getBottom( column );
        }

        public void playTile( int column )
        {
            if( !board.isFull( column ) )
            {
                highlights.Clear();
                foreach( Tile t in board.dropTile( column, players[currentPlayer].getTile( currentTile ), players[currentPlayer] ) )
                {
                    highlights.Add( new Point( t.pt.X, t.pt.Y ) );
                }

                scores[currentPlayer] = players[currentPlayer].playTile( currentTile );

                if( highlights.Count != 0 )
                    board.addName( "Player " + ( currentPlayer + 1 ).ToString() );

                if( mode == 2 || mode == 3 || mode == 4 )
                {
                    if( ++currentPlayer == mode )
                    {
                        currentPlayer = 0;
                    }
                }
                if( mode == 5 || mode == 6 || mode == 7 )
                {
                    compHighlights.Clear();
                    List<int> scoreList = new List<int>();
                    List<Point> tracking = new List<Point>();
                    scoreList.Add( 0 );
                    bool inserted = false;
                    for( int tiles = 0; tiles < 6; tiles++ )
                    {
                        for( int boardSpot = 0; boardSpot < 7; boardSpot++ )
                        {
                            int temp = board.peekDrop( boardSpot, players[1].getTile( tiles ), players[1] );
                            if( temp == 0 )
                                continue;
                            inserted = false;
                            for( int i = 0; i < scoreList.Count; i++ )
                            {
                                if( temp < scoreList[i] )
                                {
                                    inserted = true;
                                    scoreList.Insert( i, temp );
                                    tracking.Insert( i, new Point( tiles, boardSpot ) );
                                    break;
                                }
                            }
                            if( !inserted )
                            {
                                scoreList.Add( temp );
                                tracking.Add( new Point( tiles, boardSpot ) );
                            }
                        }
                    }

                    if( tracking.Count == 0 )
                    {
                        board.dropTile( 3, players[1].getTile( 0 ), players[1] );
                    }
                    else
                    {
                        if( mode == 5 )
                        {
                            foreach( Tile t in board.dropTile( tracking[1].Y, players[1].getTile( tracking[1].X ), players[1] ) )
                            {
                                compHighlights.Add( new Point( t.pt.X, t.pt.Y ) );
                            }
                        }
                        if( mode == 6 )
                        {
                            foreach( Tile t in board.dropTile( tracking[tracking.Count / 2].Y, players[1].getTile( tracking[tracking.Count / 2].X ), players[1] ) )
                            {
                                compHighlights.Add( new Point( t.pt.X, t.pt.Y ) );
                            }
                        }
                        if( mode == 7 )
                        {
                            //TODO: firx error when close to top of screen
                            foreach( Tile t in board.dropTile( tracking[tracking.Count - 1].Y, players[1].getTile( tracking[tracking.Count - 1].X ), players[1] ) )
                            {
                                compHighlights.Add( new Point( t.pt.X, t.pt.Y ) );
                            }
                        }
                        if( compHighlights.Count != 0 )
                            board.addName( "Comp" );
                    }
                }
                if( board.Moves == 0 )
                    isOver = true;
            }
        }
    }
}

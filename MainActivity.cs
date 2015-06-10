using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using System.IO;
using System.Timers;
using System.Threading.Tasks;
using System.Threading;

namespace Word_Drop
{
    [Activity( Label = "Word_Drop", MainLauncher = true, Icon = "@drawable/icon" )]
    public class MainActivity : Activity
    {

        private int tileWidth;
        private int bottom;
        private CancellationTokenSource _cancellationTokenSource;
        MainView img;

        protected override void OnCreate( Bundle bundle )
        {
            RequestWindowFeature( WindowFeatures.NoTitle );
            this.Window.AddFlags( WindowManagerFlags.Fullscreen );
            base.OnCreate( bundle );


            using( StreamReader sr = new StreamReader( Assets.Open( "Dictionary.txt" ) ) )
            {
                string content;
                while( ( content = sr.ReadLine() ) != null )
                    Dictionary.addToList( content );
            }

            img = new MainView( this );
            SetContentView( img );

            //var ad = new AdView(img.Context);
            //ad.AdSize = AdSize.SmartBanner;
            //ad.AdUnitId = "pub-3642540719713162";
            //var requestbuilder = new AdRequest.Builder();
            //ad.LoadAd(requestbuilder.Build());

            tileWidth = img.tileWidth;

            img.Touch += async ( sender, e ) =>
            {
                Point p = new Point( (int)e.Event.RawX, (int)e.Event.RawY );
                switch( e.Event.Action )
                {
                    case MotionEventActions.Down:
                        {
                            if( !img.Drop )
                            {
                                if( img.Mode == 1 )
                                {
                                    //detect all menu clicks here
                                    if( isInBounds( p, 1, 3, 8, 9 ) )
                                        img.Mode = 2;
                                    if( isInBounds( p, 3, 5, 8, 9 ) )
                                        img.Mode = 3;
                                    if( isInBounds( p, 5, 7, 8, 9 ) )
                                        img.Mode = 4;
                                    if( isInBounds( p, 1, 4, 5, 6 ) )
                                        img.Mode = 5;
                                    if( isInBounds( p, 4, 7, 5, 6 ) )
                                        img.Mode = 6;
                                    if( isInBounds( p, 1, 5, 6, 7 ) )
                                        img.Mode = 7;
                                }
                                else if( img.IsOver )
                                {
                                    if( isInBounds( p, 7, 8, 11, 12 ) )
                                        img.Mode = 1;
                                }
                                else
                                {
                                    if( isInBounds( p, 0.5, 7.5, 9.5, 10.5 ) )
                                    {
                                        img.Coords = new Point( (int)e.Event.RawX, (int)e.Event.RawY );
                                        img.SelectTile = ( (int)e.Event.RawX - tileWidth / 2 ) / tileWidth;
                                    }
                                    if( isInBounds( p, 7, 8, 11, 12 ) )
                                        img.Mode = 1;
                                }
                            }
                            break;
                        }
                    case MotionEventActions.Move:
                        {
                            if( !img.Drop )
                            {
                                if( img.SelectTile != -1 )
                                    img.Coords = new Point( (int)e.Event.RawX, (int)e.Event.RawY );
                            }
                            break;
                        }
                    case MotionEventActions.Up:
                        {
                            if( !img.Drop )
                            {
                                if( e.Event.RawY < tileWidth && img.SelectTile != -1 )
                                {
                                    img.Drop = true;
                                    img.Animated = new Point( ( ( (int)e.Event.RawX ) / tileWidth ) * tileWidth, 0 );
                                    bottom = img.getBottom( (int)e.Event.RawX / tileWidth );

                                    if( bottom != -1 )
                                    {
                                        _cancellationTokenSource = new CancellationTokenSource();
                                        await Start( _cancellationTokenSource.Token );

                                        _cancellationTokenSource.Dispose();
                                    }

                                    img.Animated = new Point( -tileWidth, 0 );
                                    img.Drop = false;

                                    img.playTile( (int)e.Event.RawX / tileWidth );
                                }

                                img.SelectTile = -1;
                            }
                            break;
                        }
                }
            };
        }

        private async Task Start( CancellationToken cancel = default (CancellationToken) )
        {
            while( !cancel.IsCancellationRequested )
            {
                if( img.Animated.Y < ( 8 - bottom ) * tileWidth )
                {
                    await Task.Delay( 10, cancel );
                    img.Animated = new Point( img.Animated.X, img.Animated.Y + tileWidth / 4 );
                    RunOnUiThread( () => img.Invalidate() );
                }
                else
                {
                    _cancellationTokenSource.Cancel();
                }
            }
        }

        private bool isInBounds( Point p, double left, double right, double top, double bottom )
        {
            if( p.X >= left * tileWidth && p.X < right * tileWidth && p.Y >= top * tileWidth && p.Y < bottom * tileWidth )
                return true;
            return false;
        }
    }
}

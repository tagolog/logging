using System;
using System.Timers;
using Common.Logging;
using Tagolog.Examples.CloudHosting.Emulator.Helpers;

namespace Tagolog.Examples.CloudHosting.Emulator.Model
{
    class VirtualMachine
    {
        public VirtualMachine( string userId, string name )
        {
            UserId = userId;
            Name = name;

            _timer.Elapsed += OnTimerElapsed;
        }

        public string UserId { get; private set; }
        string Name { get; set; }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        void OnTimerElapsed( object sender, ElapsedEventArgs e )
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                try
                {
                    scope.Tags[ AppTag.Application ] = DataCenter.ApplicationName;
                    scope.Tags[ AppTag.VirtualMachine ] = Name;

                    var random = RandomHelper.GenerateInt( 0, 100 );
                    if ( random < 30 )
                        _logger.Info( "Low memory and CPU usage." );
                    if ( random < 60 )
                        _logger.Warn( "Medium memory and CPU usage." );
                    else
                        throw new InvalidOperationException( "High memory and CPU usage." );
                }
                catch ( InvalidOperationException ex )
                {
                    _logger.Error( ex.Message, ex );
                }
            }
        }

        readonly RandomTimer _timer = new RandomTimer( 1, 3 );
        readonly ILog _logger = LogManager.GetLogger( typeof( VirtualMachine ) );
    }
}

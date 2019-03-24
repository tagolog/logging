using System;
using System.Threading;
using System.Collections.Generic;
using Tagolog.Examples.CloudHosting.Emulator.Model;
using Tagolog.Examples.CloudHosting.Emulator.Helpers;
using Tagolog.Examples.CloudHosting.EmulatorInterface;

namespace Tagolog.Examples.CloudHosting.Emulator
{
    class WebInterface
    {
        public WebInterface( IEmulatorLogger emulatorLogger )
        {
            _emulatorLogger = emulatorLogger;
            _dataCenter = new DataCenter( emulatorLogger );
        }

        /// <summary>
        /// Emulates process of creating a new user.
        /// Password is skipped since not used in emulation.
        /// </summary>
        public void RegisterNewUser( string email, string firstName, string lastName )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                scope.Tags[ AppTag.Email ] = email;
                scope.Tags[ AppTag.FirstName ] = firstName;
                scope.Tags[ AppTag.LastName ] = lastName;
                _emulatorLogger.Info( "Request to register new user." );

                lock ( _locker )
                {
                    var user = new User( email );
                    _users.Add( email, user );
                    // Bind further log entries to user ID.
                    scope.Tags[ AppTag.UserId ] = user.UserId;
                }

                _emulatorLogger.Info( "New user registered." );
            }
        }

        /// <summary>
        /// Emulates authentication (login process) for existing user.
        /// Password is skipped since not used in emulation.
        /// </summary>
        /// <param name="email">User email that is used as a login.</param>
        /// <returns>
        /// Session ID in case of successful authentication.
        /// Null or empty string if authentication fails.
        /// </returns>
        public string Authenticate( string email )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                scope.Tags[ AppTag.Email ] = email;

                // Bind further log entries to user ID.
                lock ( _locker )
                {
                    scope.Tags[ AppTag.UserId ] = _users[ email ].UserId;
                }

                if ( RandomHelper.GenerateBoolean() )
                {
                    var sessionId = Guid.NewGuid().ToString( "D" );
                    lock ( _locker )
                    {
                        _sessions.Add( sessionId, email );
                    }

                    scope.Tags[ AppTag.SessionId ] = sessionId;
                    _emulatorLogger.Info( "Authentication succeeded." );

                    return sessionId;
                }
                else
                {
                    _emulatorLogger.Error( "Authentication failed." );
                    return null;
                }
            }
        }

        public bool OrderNewVirtualMachine( string sessionId, string virtualMachineName, decimal amount )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                var user = GetUserBySession( sessionId );
                scope.Tags[ AppTag.UserId ] = user.UserId;
                scope.Tags[ AppTag.SessionId ] = sessionId;

                _emulatorLogger.InfoFormat( "Redirect user to external payment system. Amount = {0}", amount );

                if ( RandomHelper.GenerateBoolean() )
                {
                    scope.Tags[ AppTag.VirtualMachine ] = virtualMachineName;
                    _emulatorLogger.Info( "Creating virtual machine..." );
                    ThreadPool.QueueUserWorkItem( _ => _dataCenter.CreateNewVirtualMachine( user.UserId, virtualMachineName ) );
                    return true;
                }

                var errorId = Guid.NewGuid().ToString( "D" );
                scope.Tags[ AppTag.ErrorId ] = errorId;
                _emulatorLogger.Error( "Payment failed. Unique error ID was displayed." );
                return false;
            }
        }

        public void StartVirtualMachine( string sessionId, string virtualMachineName )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                scope.Tags[ AppTag.SessionId ] = sessionId;
                scope.Tags[ AppTag.UserId ] = GetUserBySession( sessionId ).UserId;
                scope.Tags[ AppTag.VirtualMachine ] = virtualMachineName;
                _emulatorLogger.Info( "User requested to start virtual machine." );

                ThreadPool.QueueUserWorkItem( _ => _dataCenter.StartVirtualMachine( virtualMachineName ) );
            }
        }

        public void StopVirtualMachine( string sessionId, string virtualMachineName )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                scope.Tags[ AppTag.SessionId ] = sessionId;
                scope.Tags[ AppTag.UserId ] = GetUserBySession( sessionId ).UserId;
                scope.Tags[ AppTag.VirtualMachine ] = virtualMachineName;
                _emulatorLogger.Info( "User requested to stop virtual machine." );

                ThreadPool.QueueUserWorkItem( _ => _dataCenter.StopVirtualMachine( virtualMachineName ) );
            }
        }

        User GetUserBySession( string sessionId )
        {
            lock ( _locker )
            {
                var email = _sessions[ sessionId ];
                return _users[ email ];
            }
        }

        static ITagScope CreateTagScope( string operation )
        {
            return TagScopeManager.CreateScope( new Dictionary<string, string>
            {
                { AppTag.Application, "WebInterface" },
                { AppTag.Operation, operation }
            } );
        }

        readonly IEmulatorLogger _emulatorLogger;
        readonly DataCenter _dataCenter;

        readonly object _locker = new object(); 

        /// <summary>
        /// Map email to user instance.
        /// </summary>
        readonly Dictionary<string, User> _users = new Dictionary<string, User>();

        /// <summary>
        /// Map sessionId to email.
        /// </summary>
        readonly Dictionary<string, string> _sessions = new Dictionary<string, string>();
    }
}

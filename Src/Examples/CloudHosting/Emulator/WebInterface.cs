using System;
using System.Threading;
using System.Collections.Generic;
using Common.Logging;
using Tagolog.Examples.CloudHosting.Emulator.Model;
using Tagolog.Examples.CloudHosting.Emulator.Helpers;

namespace Tagolog.Examples.CloudHosting.Emulator
{
    static class WebInterface
    {
        /// <summary>
        /// Emulates process of creating a new user.
        /// Password is skipped since not used in emulation.
        /// </summary>
        public static void RegisterNewUser( string email, string firstName, string lastName )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                scope.Tags[ AppTag.Email ] = email;
                scope.Tags[ AppTag.FirstName ] = firstName;
                scope.Tags[ AppTag.LastName ] = lastName;
                Logger.Info( "Request to register new user." );

                lock ( Locker )
                {
                    var user = new User( email );
                    Users.Add( email, user );
                    // Bind further log entries to user ID.
                    scope.Tags[ AppTag.UserId ] = user.UserId;
                }

                Logger.Info( "New user registered." );
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
        public static string Authenticate( string email )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                scope.Tags[ AppTag.Email ] = email;

                // Bind further log entries to user ID.
                lock ( Locker )
                {
                    scope.Tags[ AppTag.UserId ] = Users[ email ].UserId;
                }

                if ( RandomHelper.GenerateBoolean() )
                {
                    var sessionId = Guid.NewGuid().ToString( "D" );
                    lock ( Locker )
                    {
                        Sessions.Add( sessionId, email );
                    }

                    scope.Tags[ AppTag.SessionId ] = sessionId;
                    Logger.Info( "Authentication succeeded." );

                    return sessionId;
                }
                else
                {
                    Logger.Error( "Authentication failed." );
                    return null;
                }
            }
        }

        public static bool OrderNewVirtualMachine( string sessionId, string virtualMachineName, decimal amount )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                var user = GetUserBySession( sessionId );
                scope.Tags[ AppTag.UserId ] = user.UserId;
                scope.Tags[ AppTag.SessionId ] = sessionId;

                Logger.InfoFormat( "Redirect user to external payment system. Amount = {0}", amount );

                if ( RandomHelper.GenerateBoolean() )
                {
                    scope.Tags[ AppTag.VirtualMachine ] = virtualMachineName;
                    Logger.Info( "Creating virtual machine..." );
                    ThreadPool.QueueUserWorkItem( _ => DataCenter.CreateNewVirtualMachine( user.UserId, virtualMachineName ) );
                    return true;
                }

                var errorId = Guid.NewGuid().ToString( "D" );
                scope.Tags[ AppTag.ErrorId ] = errorId;
                Logger.Error( "Payment failed. Unique error ID was displayed." );
                return false;
            }
        }

        public static void StartVirtualMachine( string sessionId, string virtualMachineName )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                scope.Tags[ AppTag.SessionId ] = sessionId;
                scope.Tags[ AppTag.UserId ] = GetUserBySession( sessionId ).UserId;
                scope.Tags[ AppTag.VirtualMachine ] = virtualMachineName;
                Logger.Info( "User requested to start virtual machine." );

                ThreadPool.QueueUserWorkItem( _ => DataCenter.StartVirtualMachine( virtualMachineName ) );
            }
        }

        public static void StopVirtualMachine( string sessionId, string virtualMachineName )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                scope.Tags[ AppTag.SessionId ] = sessionId;
                scope.Tags[ AppTag.UserId ] = GetUserBySession( sessionId ).UserId;
                scope.Tags[ AppTag.VirtualMachine ] = virtualMachineName;
                Logger.Info( "User requested to stop virtual machine." );

                ThreadPool.QueueUserWorkItem( _ => DataCenter.StopVirtualMachine( virtualMachineName ) );
            }
        }

        #region Helpers

        static User GetUserBySession( string sessionId )
        {
            lock ( Locker )
            {
                var email = Sessions[ sessionId ];
                return Users[ email ];
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

        #endregion // Helpers

        readonly static object Locker = new object(); 

        /// <summary>
        /// Map email to user instance.
        /// </summary>
        readonly static Dictionary<string, User> Users = new Dictionary<string, User>();

        /// <summary>
        /// Map sessionId to email.
        /// </summary>
        readonly static Dictionary<string, string> Sessions = new Dictionary<string, string>();

        readonly static ILog Logger = LogManager.GetLogger( typeof( WebInterface ) );
    }
}

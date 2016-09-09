using System.Collections.Generic;
using Common.Logging;
using Tagolog.Examples.CloudHosting.Emulator.Helpers;
using Tagolog.Examples.CloudHosting.Emulator.Model;

namespace Tagolog.Examples.CloudHosting.Emulator
{
    internal static class DataCenter
    {
        public static void CreateNewVirtualMachine( string userId, string virtualMachineName )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                lock ( VirtualMachinesLock )
                {
                    var virtualMachine = new VirtualMachine( userId, virtualMachineName );
                    VirtualMachines[ virtualMachineName ] = virtualMachine;

                    scope.Tags[ AppTag.UserId ] = userId;
                    scope.Tags[ AppTag.VirtualMachine ] = virtualMachineName;
                    Logger.Info( "New virtual machine was created." );
                }
            }
        }

        public static void StartVirtualMachine( string virtualMachineName )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                lock ( VirtualMachinesLock )
                {
                    var virtualMachine = VirtualMachines[ virtualMachineName ];

                    scope.Tags[ AppTag.UserId ] = virtualMachine.UserId;
                    scope.Tags[ AppTag.VirtualMachine ] = virtualMachineName;
                    Logger.Info( "Starting virtual machine..." );

                    virtualMachine.Start();
                }
            }
        }

        public static void StopVirtualMachine( string virtualMachineName )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                lock ( VirtualMachinesLock )
                {
                    var virtualMachine = VirtualMachines[ virtualMachineName ];

                    scope.Tags[ AppTag.UserId ] = virtualMachine.UserId;
                    scope.Tags[ AppTag.VirtualMachine ] = virtualMachineName;
                    Logger.Info( "Stopping virtual machine..." );

                    virtualMachine.Stop();
                }
            }
        }

        static ITagScope CreateTagScope( string operation )
        {
            return TagScopeManager.CreateScope( new Dictionary<string, string>
            {
                { AppTag.Application, ApplicationName },
                { AppTag.Operation, operation }
            } );
        }

        public static string ApplicationName
        {
            get { return typeof( DataCenter ).Name; }
        }

        readonly static object VirtualMachinesLock = new object();
        readonly static Dictionary<string, VirtualMachine> VirtualMachines = new Dictionary<string, VirtualMachine>();
        readonly static ILog Logger = LogManager.GetLogger( typeof( DataCenter ) );
    }
}

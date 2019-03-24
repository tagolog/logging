using System.Collections.Generic;
using Tagolog.Examples.CloudHosting.Emulator.Model;
using Tagolog.Examples.CloudHosting.Emulator.Helpers;
using Tagolog.Examples.CloudHosting.EmulatorInterface;

namespace Tagolog.Examples.CloudHosting.Emulator
{
    class DataCenter
    {
        public DataCenter( IEmulatorLogger emulatorLogger )
        {
            _emulatorLogger = emulatorLogger;
        }

        public void CreateNewVirtualMachine( string userId, string virtualMachineName )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                lock ( _virtualMachinesLock )
                {
                    var virtualMachine = new VirtualMachine( userId, virtualMachineName );
                    _virtualMachines[ virtualMachineName ] = virtualMachine;

                    scope.Tags[ AppTag.UserId ] = userId;
                    scope.Tags[ AppTag.VirtualMachine ] = virtualMachineName;
                    _emulatorLogger.Info( "New virtual machine was created." );
                }
            }
        }

        public void StartVirtualMachine( string virtualMachineName )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                lock ( _virtualMachinesLock )
                {
                    var virtualMachine = _virtualMachines[ virtualMachineName ];

                    scope.Tags[ AppTag.UserId ] = virtualMachine.UserId;
                    scope.Tags[ AppTag.VirtualMachine ] = virtualMachineName;
                    _emulatorLogger.Info( "Starting virtual machine..." );

                    virtualMachine.Start();
                }
            }
        }

        public void StopVirtualMachine( string virtualMachineName )
        {
            using ( var scope = CreateTagScope( ClassHelper.GetCurrentMethodName() ) )
            {
                lock ( _virtualMachinesLock )
                {
                    var virtualMachine = _virtualMachines[ virtualMachineName ];

                    scope.Tags[ AppTag.UserId ] = virtualMachine.UserId;
                    scope.Tags[ AppTag.VirtualMachine ] = virtualMachineName;
                    _emulatorLogger.Info( "Stopping virtual machine..." );

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

        readonly IEmulatorLogger _emulatorLogger;
        readonly object _virtualMachinesLock = new object();
        readonly Dictionary<string, VirtualMachine> _virtualMachines = new Dictionary<string, VirtualMachine>();
    }
}

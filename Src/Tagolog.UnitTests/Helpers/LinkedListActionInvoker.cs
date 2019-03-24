using System;
using System.Collections.Generic;

namespace Tagolog.UnitTests.Helpers
{
    class LinkedListActionInvoker
    {
        public static void Invoke<T>( LinkedListNode<T> headElement, Action<LinkedListNode<T>> action )
        {
            action.Invoke( headElement );
        }
    }
}

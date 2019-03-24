using System;
using System.Collections.Generic;
using Tagolog.UnitTests.Model;

namespace Tagolog.UnitTests.Helpers
{
    class LinkedListRecursiveActionInvoker
    {
        public static void Invoke( LinkedListNode<TagTestCase> headElement, Action<LinkedListNode<TagTestCase>> action )
        {
            action.Invoke( headElement );
        }
    }
}

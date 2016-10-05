using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tagolog.UnitTest.Model;

namespace Tagolog.UnitTest.Tests.Helper
{
    class LinkedListRecursiveActionInvoker
    {
        public static void Invoke( LinkedListNode<TagTestCase> headElement, Action<LinkedListNode<TagTestCase>> action )
        {
            action.Invoke( headElement );
        }
    }
}

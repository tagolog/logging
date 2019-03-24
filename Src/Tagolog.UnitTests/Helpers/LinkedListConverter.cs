using System.Collections.Generic;

namespace Tagolog.UnitTests.Helpers
{
    class LinkedListConverter
    {
        /// <summary>
        /// Converts collection to linked list.
        /// </summary>
        /// <param name="collection">Collection of tag test cases.</param>
        /// <returns>Linked list of tag test cases.</returns>
        public static LinkedList<T> ToLinkedList<T>( IEnumerable<T> collection )
        {
            var linkedList = new LinkedList<T>();

            foreach ( var item in collection )
                linkedList.AddLast( item );

            return linkedList;
        }
    }
}

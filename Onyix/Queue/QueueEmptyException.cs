using System;

namespace Onyix.Queue
{
    public class QueueEmptyException : Exception
    {
        public QueueEmptyException() { }
        public QueueEmptyException(string message) : base(message) { }
    }
}

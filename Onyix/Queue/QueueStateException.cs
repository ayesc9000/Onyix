using System;

namespace Onyix.Queue
{
    public class QueueStateException : Exception
    {
        public QueueStateException() { }
        public QueueStateException(string message) : base(message) { }
    }
}

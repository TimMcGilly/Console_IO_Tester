namespace Console_IO_Tester
{
    using System;

    public partial class IO_Exception_Check
    {
        [Serializable]
        public class StartAlreadyRunning : Exception
        {
            public StartAlreadyRunning() { }
            public StartAlreadyRunning(string message) : base(message) { }
            public StartAlreadyRunning(string message, Exception inner) : base(message, inner) { }
            protected StartAlreadyRunning(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}

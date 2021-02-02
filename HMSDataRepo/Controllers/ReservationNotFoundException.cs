using System;
using System.Runtime.Serialization;

namespace HMSDataRepo.Controllers
{
    [Serializable]
    internal class ReservationNotFoundException : Exception
    {
        public ReservationNotFoundException()
        {
        }

        public ReservationNotFoundException(string message) : base(message)
        {
        }

        public ReservationNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReservationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
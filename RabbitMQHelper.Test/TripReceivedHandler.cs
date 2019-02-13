using Common;
using System;

namespace RabbitMQHelper.Test
{
    public class TripReceivedHandler : IReceivedHandler<NetstarTrip>
    {
        public void Process(NetstarTrip t)
        {
            Console.WriteLine(t);
        }
    }
}

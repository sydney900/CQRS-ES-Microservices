using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQHelper
{
    public class MyConnectFactory : IMyConnectFactory
    {
        private readonly string hostName;
        private readonly int port;
        private readonly string userName;
        private readonly string password;
        private readonly string vhost;
        private readonly int timeOutSeconds;

        public MyConnectFactory(string hostName, int port, string userName, string password, string vhost, int timeOutSeconds)
        {
            this.hostName = hostName;
            this.port = port;
            this.userName = userName;
            this.password = password;
            this.vhost = vhost;
            this.timeOutSeconds = timeOutSeconds;
        }

        public ConnectionFactory CreateConnectFactory()
        {
            var factory = new ConnectionFactory()
            {
                HostName = hostName,
            };
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                factory.UserName = userName;
                factory.Password = password;
            }
            if (!string.IsNullOrEmpty(vhost))
            {
                factory.VirtualHost = vhost;
            }
            factory.Port = port <= 0 ? 5672 : port;
            factory.RequestedConnectionTimeout = timeOutSeconds * 1000;

            return factory;

        }
    }
}

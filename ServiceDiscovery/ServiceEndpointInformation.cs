namespace ServiceDiscovery
{
    public class ServiceEndpointInformation
    {
        public ServiceEndpointInformation(ServiceEnum name, string host, int port)
        {
            Name = name;
            Host = host;
            Port = port;
        }

        public ServiceEnum Name { get; } 
        public string Host { get; }
        public int Port { get; }
    }
}
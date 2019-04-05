namespace RestCommunication
{
    public interface IResilientRestClientFactory
    {
        ResilientRestClient CreateResilientHttpClient();
    }
} 
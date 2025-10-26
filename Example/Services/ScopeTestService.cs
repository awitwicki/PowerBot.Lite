namespace Example.Services;

public class ScopeTestService : IScopeTestService
{
    private readonly string _id = Guid.NewGuid().ToString()[..4];

    public string GetId()
    {
        return _id;
    }
}

using Testcontainers.PostgreSql;
using Xunit;

namespace TestMvCMovie.Utils;
// ReSharper disable once ClassNeverInstantiated.Global
public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer container =
        new PostgreSqlBuilder()
            .WithCleanUp(false)
            .WithAutoRemove(false)
            .Build();
 
    public string ConnectionString => container.GetConnectionString();
    public string ContainerId => $"{container.Id}";
    public Task InitializeAsync()
        => container.StartAsync();
    public Task DisposeAsync()
        => container.DisposeAsync().AsTask();
}
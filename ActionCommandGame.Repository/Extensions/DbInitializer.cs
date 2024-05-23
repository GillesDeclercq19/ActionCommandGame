using ActionCommandGame.Repository.Core;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;

public class DbInitializer
{
    //seed de database async
    private readonly IServiceProvider _serviceProvider;

    public DbInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task InitializeAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ActionButtonGameDbContext>();
            await dbContext.InitializeAsync(scope.ServiceProvider);
        }
    }
}
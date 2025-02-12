using Microsoft.Extensions.DependencyInjection;
using System;
using TreasureMap.Interfaces;
using TreasureMap.Models;
using TreasureMap.Services;

class Program
{
    static void Main(string[] args)
    {
        using var serviceProvider = ConfigureServices();

        var gameService = serviceProvider.GetRequiredService<IGameService>();

        gameService.StartGame();
    }

    private static ServiceProvider ConfigureServices()
    {
        return new ServiceCollection()
                    .AddScoped<IMapService, MapService>()           
                    .AddScoped<IMovementService, MovementService>()  
                    .AddScoped<IGameService, GameService>()
                    .BuildServiceProvider();
    }

}

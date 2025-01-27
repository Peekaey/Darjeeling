﻿using Darjeeling.Interfaces.Repositories;
using Darjeeling.Models.Entities;

namespace Darjeeling.DataContext.Repositories;

public class FCGuildServerRepository : IFCGuildServerRepository
{
    private readonly Darjeeling.Repositories.DataContext _context;
    private readonly ILogger<FCGuildServerRepository> _logger;
    
    public FCGuildServerRepository(Darjeeling.Repositories.DataContext context, ILogger<FCGuildServerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task AddAsync(FCGuildServer server)
    {
        await _context.FCGuildServers.AddAsync(server);
    }
    
    public async Task RemoveAsync(FCGuildServer server)
    {
        _context.FCGuildServers.RemoveRange(server);
    }
    
    
}
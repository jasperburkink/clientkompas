﻿using System.Text.Json;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Domain.CVS.Domain;

namespace Infrastructure.Services.Menu
{
    public class MenuService(IFileService fileService) : IMenuService
    {
        private const string CONFIG_FILE_PATH = "Services/Menu/menu-config.json";

        public IEnumerable<MenuItem> GetMenuByRole(string role)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            var configPath = Path.Combine(basePath, CONFIG_FILE_PATH);

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException($"Menu configuration file not found at path: {configPath}");
            }

            var jsonContent = fileService.ReadAllText(configPath);

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var menuConfig = JsonSerializer.Deserialize<MenuConfig>(jsonContent, serializerOptions);

            if (menuConfig != null && menuConfig.Roles.TryGetValue(role, out var menuItems))
            {
                return menuItems;
            }

            return [];
        }
    }
}

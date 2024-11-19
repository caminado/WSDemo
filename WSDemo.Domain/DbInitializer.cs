namespace WSDemo.Domain;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (context.FolderItems.Any())
        {
            return;   // DB has been seeded
        }

        List<FolderItem> defaultFolderItems = [
            new FolderItem { Name = "DefFolderItem 1"  },
            new FolderItem { Name = "DefFolderItem 2"  },
            new FolderItem { Name = "DefFolderItem 3"  },
            new FolderItem { Name = "DefFolderItem 4"  },
            new FolderItem { Name = "DefFolderItem 5"  },
            new FolderItem { Name = "DefFolderItem 6"  },
            new FolderItem { Name = "DefFolderItem 7"  },
            new FolderItem { Name = "DefFolderItem 8"  },
            new FolderItem { Name = "DefFolderItem 9"  },
            new FolderItem { Name = "DefFolderItem 10"  },
            new FolderItem { Name = "DefFolderItem 11"  },
            ];

        await context.AddRangeAsync(defaultFolderItems);
        await context.SaveChangesAsync();
    }
}

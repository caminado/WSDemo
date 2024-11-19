using Microsoft.EntityFrameworkCore;
using WSDemo.Domain;
using WSDemo.RepositoryLayer;

namespace WSDemo.UnitTests;

public class FolderItemsRepositoryTests
{
    [Fact]
    public async Task GetByIdAsync_WithWrongId_ShouldReturnEmpty()
    {
        using var ctx = GetMemoryContext();
        var repo = new FolderItemsRepository(ctx);
        List<Guid> ids = new List<Guid>();
        for (int i = 0; i < 10; i++)
        {
            var id = await repo.InsertAsync(new FolderItem { Name = $"Name{i}" });
            ids.Add(id);
        }

        var folderItem1 = await repo.GetByIdAsync(Guid.Empty);
        Assert.Null(folderItem1);

        var folderItem2 = await repo.GetByIdAsync(Guid.NewGuid());
        Assert.Null(folderItem2);
    }

    [Fact]
    public async Task InsertAsync_ShouldFindAllItems()
    {
        using var ctx = GetMemoryContext();
        var repo = new FolderItemsRepository(ctx);
        List<Guid> ids = new List<Guid>();
        for (int i = 0; i < 10; i++)
        {
            var id = await repo.InsertAsync(new FolderItem { Name = $"Name{i}" });
            ids.Add(id);
        }
        for (int i = 0; i < 10; i++)
        {
            var folderItem = await repo.GetByIdAsync(ids[i]);
            Assert.NotNull(folderItem);
        }
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        using var ctx = GetMemoryContext();
        var repo = new FolderItemsRepository(ctx);
        var id = await repo.InsertAsync(new FolderItem { Name = $"Name1" });
        var folderItem = await repo.GetByIdAsync(id);
        folderItem.Name = "NewName";
        await repo.UpdateAsync(folderItem);
        var folderItemAfterUpdate = await repo.GetByIdAsync(id);
        Assert.NotNull(folderItemAfterUpdate);
        Assert.Equal("NewName", folderItemAfterUpdate.Name);
        Assert.Null(folderItemAfterUpdate.ParentId);
    }

    [Fact]
    public async Task DeleteAsync_ShouldPersistChanges()
    {
        using var ctx = GetMemoryContext();
        var repo = new FolderItemsRepository(ctx);
        var id = await repo.InsertAsync(new FolderItem { Name = $"Name1" });
        var folderItem = await repo.GetByIdAsync(id);
        await repo.UpdateAsync(folderItem);
        await repo.DeleteAsync(id);
        var folderItemAfterDelete = await repo.GetByIdAsync(id);
        Assert.Null(folderItemAfterDelete);
    }

    [Fact]
    public async Task GetByParentIdAsync_ShouldFindAllParticularItems()
    {
        using var ctx = GetMemoryContext();
        var repo = new FolderItemsRepository(ctx);

        var rootId1 = await repo.InsertAsync(new FolderItem { Name = $"Root1" });
        var rootId2 = await repo.InsertAsync(new FolderItem { Name = $"Root2" });

        List<Guid> ids = new List<Guid>();
        for (int i = 0; i < 10; i++)
        {
            var id = await repo.InsertAsync(new FolderItem { Name = $"Name{i}", ParentId= rootId1 });
            ids.Add(id);
        }

        var allInRoot1 = await repo.GetByParentIdAsync(rootId1, null, null);

        Assert.NotNull(allInRoot1);
        Assert.Equal(10, allInRoot1.Count());
        Assert.All(allInRoot1, x => Assert.Equal( x.ParentId!.Value, rootId1));
    }

    [Fact]
    public async Task GetByParentIdAsync_WithPagingParameters_ShouldFindAllParticularItems()
    {
        using var ctx = GetMemoryContext();
        var repo = new FolderItemsRepository(ctx);

        var rootId1 = await repo.InsertAsync(new FolderItem { Name = $"Root1" });
        var rootId2 = await repo.InsertAsync(new FolderItem { Name = $"Root2" });

        List<Guid> ids = new List<Guid>();
        for (int i = 0; i < 10+1; i++)
        {
            var id = await repo.InsertAsync(new FolderItem { Name = $"Name{i}", ParentId = rootId1 });
            ids.Add(id);
        }

        var allInRoot1 = await repo.GetByParentIdAsync(rootId1, 10, 0);

        Assert.NotNull(allInRoot1);
        Assert.Equal(10, allInRoot1.Count());
        
        allInRoot1 = await repo.GetByParentIdAsync(rootId1, 10, 1);

        Assert.NotNull(allInRoot1);
        Assert.Equal(1, allInRoot1.Count());

        allInRoot1 = await repo.GetByParentIdAsync(rootId1, 10, 2);

        Assert.Equal(0, allInRoot1.Count());
    }

    [Fact]
    public async Task GetByParentIdAsync_WithMoving_ShouldReturnMovedItem()
    {
        using var ctx = GetMemoryContext();
        var repo = new FolderItemsRepository(ctx);

        var rootId1 = await repo.InsertAsync(new FolderItem { Name = $"Root1" });
        var rootId2 = await repo.InsertAsync(new FolderItem { Name = $"Root2" });

        List<Guid> ids = new List<Guid>();
        for (int i = 0; i < 10; i++)
        {
            var id = await repo.InsertAsync(new FolderItem { Name = $"Name{i}", ParentId = rootId1 });
            ids.Add(id);
        }


        var folderItem = await repo.GetByIdAsync(ids.First());
        folderItem.ParentId = rootId2;
        await repo.UpdateAsync(folderItem);

        var allInRoot1 = await repo.GetByParentIdAsync(rootId1, null, null);
        Assert.NotNull(allInRoot1);
        Assert.Equal(10-1, allInRoot1.Count());

        var allInRoot2 = await repo.GetByParentIdAsync(rootId2, null, null);
        Assert.NotNull(allInRoot2);
        Assert.Equal(1, allInRoot2.Count());
    }

    private ApplicationDbContext GetMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .Options;
        return new ApplicationDbContext(options);
    }
}
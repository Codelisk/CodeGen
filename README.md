# CodeGen

Declare your [Id]:

```csharp
public class BaseDto
{
    [Id]
    public Guid Id { get; set; }
}
``` 
Add your Dtos:

    ```[Dto]
    public partial class CategoryDto : BaseDto
    {
        public string Name { get; set; }
        public List<ProductDto> Products { get; set; }
    }```

Add your Entities
```
[Entity(typeof(CategoryDto))]
public partial class Category : BaseEntity
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public ICollection<Product> Product { get; set; }
}
```

Declare your [BaseContext]:

```
[BaseContext]
public partial class BaseContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseInMemoryDatabase("TEST");
    }
    public BaseContext(DbContextOptions<BaseContext> opt)
    : base(opt)
    {
    }
}
```

Declare your [DefaultController]:

```
[DefaultController]
[Route("[controller]")]
public class DefaultController<T, TKey, TEntity> : Microsoft.AspNetCore.Mvc.Controller
    where T : BaseDto
    where TEntity : class
    where TKey : IComparable
{
    protected readonly IDefaultManager<T, TKey, TEntity> _manager;

    public DefaultController(IDefaultManager<T, TKey, TEntity> manager)
    {
        _manager = manager;
    }
}
```

Declare your [DefaultController] [Delete] [GetAll] [Save] [Get]:

```
[DefaultManager]
public class DefaultManager<TDto, TKey, TEntity> : IDefaultManager<TDto,TKey,TEntity> where TDto : class where TEntity : class
{
    private readonly IDefaultRepository<TEntity, TKey> _repo;
    private readonly IMapper _mapper;

    public DefaultManager(IDefaultRepository<TEntity, TKey> Repo, IMapper mapper)
    {
        _repo = Repo;
        _mapper = mapper;
    }
    [Delete]
    public Task Delete(TDto t)
    {
        return _repo.Delete(_mapper.Map<TEntity>(t));
    }
    [GetAll]
    public async Task<List<TDto>> GetAll()
    {
        return _mapper.Map<List<TDto>>(await _repo.GetAll());
    }
    [Save]
    public async Task<TDto> Save(TDto t)
    {
        return _mapper.Map<TDto>(await _repo.Save(_mapper.Map<TEntity>(t)));
    }
    [Get]
    public async Task<TDto> Get(TKey id)
    {
        return _mapper.Map<TDto>(await _repo.Get(id));
    }
}
```

Declare your [DefaultRepository] [Delete] [GetAll] [Save] [Get]:

```
[DefaultRepository]
public class DefaultRepository<T, TKey> : IDefaultRepository<T, TKey> where T : class
{
    private readonly BaseContext _context;

    public DefaultRepository(BaseContext context)
    {
        _context = context;
    }
    [Save]
    public async Task<T> Save(T t)
    {
        var result = _context.Add(t);
        await _context.SaveChangesAsync();
        return result.Entity;
    }
    [Get]
    public async Task<T> Get(TKey id)
    {
        return await _context.FindAsync<T>(id);
    }
    [GetAll]
    public async Task<List<T>> GetAll()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }
    [Delete]
    public async Task Delete(T t)
    {
        _context.Remove(t);
        await _context.SaveChangesAsync();
    }
}
```

Declare your [DefaultRepository] [UserDto]:
```
[DefaultController]
[UserDto]
public class DefaultUserController<T, TKey, TEntity> : DefaultController<T, TKey, TEntity>
    where T : BaseDto
    where TEntity : class
    where TKey : IComparable
{
    public DefaultUserController(IDefaultManager<T, TKey, TEntity> manager) : base(manager)
    {
    }
}
```


Declare your [DefaultManager] [UserDto]:
```
[DefaultManager]
[UserDto]
public class DefaultUserManager<TDto, TKey, TEntity> : DefaultManager<TDto, TKey, TEntity> where TDto : class where TEntity : class
{
    public DefaultUserManager(IDefaultRepository<TEntity, TKey> repo, IMapper mapper) : base(repo, mapper)
    {
    }
}
```

Declare your [DefaultRepository] [UserDto]:
```
[DefaultRepository]
[UserDto]
public class DefaultUserRepository<T, TKey> : DefaultRepository<T, TKey> where T : class
{
    public DefaultUserRepository(BaseContext context) : base(context)
    {
    }
}
```

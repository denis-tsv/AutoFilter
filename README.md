<table>
<tr>
<td>

 ![Logo](https://github.com/denis-tsv/AutoFilter/blob/master/assets/logo/logo-128x128.png?raw=true) 

</td>
<td>

# AutoFilter

AutoFilter allows to create LINQ Expression by filter DTO. You can use this expression to generate SQL using any ORM (tested for Entity Framework Core and Entity Framework 6.3). Or you can use expression to filter collection of entities in memory.
Also AutoFilter contains useful implementation of OrderBy and OrderByDescending methods which takes name of property for sorting. 
And killer feature of AutoFilter is implementation of Specification pattern based on LINQ Expressions. It used in advanced scenarios as combination of specification, automapper and autofilter, combination of several queries wrapped in specifications, filtering by navigation properties in one-to-many and many-to-many scenarios. AutoFilter used in open-source project [ContestantRegister](https://github.com/denis-tsv/ContestantRegister).

[![Nuget](https://img.shields.io/nuget/v/AutoFilter.Sql?logo=nuget)](https://www.nuget.org/packages/AutoFilter.Sql/)

</td>
</tr>
</table>

# Usage

Every enterprise application has a lot of lookups and a lot of lookups contains filters. For example, in e-shop filter by product will look like this.

Model 

```csharp
public class Product
{
    public string Name { get; set; }
    public int Cost { get; set; }
}
```

Filter DTO 

```csharp
public class ProductFilter
{
    public string Name { get; set; }
    public int? CostFrom { get; set; }
    public int? CostTo { get; set; }
}
```

Controller which returns products by filter DTO query 

```csharp
public class ProductController : Controller
{
    [HttpGet]
    public async Task<IEnumerable<Product>> GetProducts([FromQuery]ProductFilter filter)
    {
        var products = DbContext.Products;
        
        if (!string.IsNullOrEmpty(filter.Name))
            products = products.Where(x => x.Name.Contains(filter.Name));
            
        if (filter.CostFrom.HasValue)
            products = products.Where(x => x.Cost >= filter.CostFrom.Value);
        
        if (filter.CostTo.HasValue)
            products = products.Where(x => x.Cost <= filter.CostTo.Value);
        
        return products.ToListAsync();
    }
}
```

AutoFilter allows to automatically generate LINQ expression by filter DTO like this.

```csharp
public class ProductController : Controller
{
    [HttpGet]
    public async Task<IEnumerable<Product>> GetProducts([FromQuery]ProductFilter filter)
    {
        return DbContext
            .Products
            .AutoFilter(filter) // <- AutoFilter in action
            .ToListAsync();
    }
}
```

AutoFilter allows not only create a LINQ Expression for ORM but it also allows to create an expression to filter objects in memory. Expression for in memory filtering using IEnumerable will contain null checks for string and navigation properties unlike expression for ORM filtering using IQuerable. For in-memory filtration expression will be compiled to delegate.

```csharp
public class ProductController : Controller
{
    private static List<Product> _cachedProducts; // cached in memory list of products
    [HttpGet]
    public async Task<IEnumerable<Product>> GetProducts([FromQuery]ProductFilter filter)
    {
        return _cachedProducts
            .AutoFilter(filter) // <- AutoFilter for collection in memory. string and navigation properties will be checked on null
            .ToListAsync();
    }
}
```

## String comparison

AutoFilter allows to compare strings using StartsWith and Contains modes. StartsWith mode is default. To enable Contains mode you need to add FilterProperty attribute to corresponding property of filter DTO.

```csharp
public class ProductFilter
{
    [FilterProperty(StringFilter = StringFilterCondition.Contains)]
    public string Name { get; set; }
}
```

Also by default string comparison is case sensitive. You can enable ignore case option using IgnoreCase property of FilterProperty attribute.

```csharp
public class ProductFilter
{
    [FilterProperty(IgnoreCase = true)]
    public string Name { get; set; }
}
```

And if name of filter property does not correspond to name of entity property then you can use TargetPropertyName property of FilterProperty attribute to set name of entity property for fitper DTO property.

```csharp
public class ProductFilter
{
    [FilterProperty(TargetPropertyName = "Name")] // or TargetPropertyName = nameof(Product.Name)
    public string ProductName { get; set; }
}
```

## Search by text (google like)
AutoFilter does't support google-like search by text. It is better to combine search by text and AutoFilter.
```csharp
public class ProductController : Controller
{
    [HttpGet]
    public async Task<IEnumerable<Product>> GetProducts([FromQuery]ProductFilter filter)
    {
        IQueryable<Product> = DbContext.Products;

        if (!string.IsNullOrEmpty(filter.Text))
            query = query.Where(x => x.Name.Contains(filter.Text) || x.Category.Name.StartsWith(filter.Text));

        query = query.AutoFilter(filter);  //Ignore Text property

        return query.ToListAsync();
    }
}
```

## Value object comparison

AutoFilter allows to compare all value types (bool, DateTime, numeric types as int, double, decimal) using options Equal, Less, LessOrEqual, Greater, GreaterOrEqual. Equal is default. You can use FilterCondition property of FilterProperty attribute to set option for value type comparison.

```csharp
public class ProductFilter
{
    [FilterProperty(TargetPropertyName = "Cost", FilterCondition = FilterCondition.GreaterOrEqual)]
    public int? CostFrom { get; set; }
    
    [FilterProperty(TargetPropertyName = "Cost", FilterCondition = FilterCondition.LessOrEqual)]
    public int? CostTo { get; set; }
}
```

## Range
AutoFilter allows to specify Range for value types. One of properties From and To for Range object must be specified. AutoFilter fill generate query Cost >= @from AND Cost <= @to
```csharp
public class ProductFilter
{
    public Range<int> Cost { get; set; } // From, To
}
```

## Invalid cases 

If entity does not contain property whist name corresponds to filter property name then exception will be thrown. 

## Composite kind

AutoFilter includes in LINQ expression properties of filter DTO which contains not null values. If two or more properties has not null values then filter conditions combined using AND option by default. But you also can use OR option as a parameter of AutoFilter extension method.

```csharp
return DbContext
    .Products
    .AutoFilter(filter, ComposeKind.Or) 
    .ToListAsync();
```

## Navigation properties

If our product has a navigation property Producer, then we can include in filter property to search by producer name using NavigationProperty attribute.

```csharp
public class Producer
{
    public string Name { get; set; }
}

public class Product
{
    public Producer Producer { get; set; }
}

public class ProductFilter
{
    [NavigationProperty("Producer", TargetPropertyName = "Name")] 
    public string ProducerName { get; set; }
}
```

Debt of nested properties is not limited. If producer has a Country navigation property then you can specify a property of filter DTO to search by country name.

```csharp
public class Country
{
    public string Name { get; set; }
}

public class Producer
{
    public Country Country { get; set; }
}

public class Product
{
    public Producer Producer { get; set; }
}

public class ProductFilter
{
    [NavigationProperty("Producer.Country", TargetPropertyName = "Name")] 
    public string ProducerName { get; set; }
}
```

## Converter

If value type in filter DTO does not correspond to value in entity property (for example, filter property contains enum's name but entity property contains enum's value) then you have to use converter. You need to implement IFilterValueConverter interface and use ConvertFilter attribute to specify this implementation for property of filter DTO.

```csharp
public enum ProductState
{
    Available,
    NotAvailable
}

public class Product
{
    public ProductState State { get; set; }
}

public class StringToEnumConverter : IFilterValueConverter
{
    public object Convert(object value)
    {
        return Enum.Parse(typeof(ProductState), (string)value);
    }
}

public class ProductFilter
{
    [ConvertFilter(typeof(StringToEnumConverter))] 
    public string State { get; set; }
}
```

## NotAutoFiltered attribute
If your DTO contains properties for filtering and some other properties which you need to exclude from autofiltering (for example property for sorting) then you can use attribute NotAutoFiltered for properties which you want to exclude.

```csharp

public class ProductFilter
{    
    [NotAutoFiltered] //property will be excluded from autofiltering because it has NotAutoFiltered attrubute
    public string OrderBy { get; set; } //Name of property for sorting, for example Cost
    
    public int Cost { get; set; }
}

```

## Default values
FilterPropertyAttribute class contains three static properties which allows to manage default behavior of AutoFilter.
- DefaultStringFilterCondition allows to set default value for StringFilterCondition property, by default DefaultStringFilterCondition contains StartsWith value.
- DefaultIgnoreCase allows to set default value for IgnoreCase property, by default DefaultIgnoreCase contains false value.
- DefaultFilterCondition allows to set default value for FilterCondition property, by default DefaultFilterCondition contains Equal value.

You can set values to default properties during application configuration, for exampe in Startup class for ASP.NET Core.

## Caching

AutoFilter used reflection to get filter DTO metadata. This metadata cached to increase speed of filtering. All caches are enabled by default and can be disabled using IsEnabled static property.
TypeInfoCache contains cached list of properties for each filter DTO type. 
FilterPropertyCache contains list of properties for filter DTO type with corresponding FilterProperty attribute or it inheritor (NavigationProperty, ConvertFilter).

## Thread safety 
Single instance of FilterProperty attribute used to generate LINQ expressions for all filter DTOs. Method GetExpression of FilterProperty attribute depends only from parameters and don't change any common state. So using a cache fo FilterProperty attributes is thread safe.

# Order by property name
Often we need to sort query results by some property name. AutoFilter contains useful implementations of OrderBy and OrderByDescending methods for IEnumerable and IQuerable interfaces. 

```csharp
public class ProductController : Controller
{
    private static IEnumerable<Products> ProductsCache; 
    
    [HttpGet]
    public async Task<IEnumerable<Product>> GetFavouriteProducts(string orderByPropertyName)
    {   
        return ProductsCache // sample for IEnumerable
            .OrderByDescending(orderByPropertyName) // OrderBy(orderByPropertyName) also available
            .ToListAsync();
    }
    
    [HttpGet]
    public async Task<IEnumerable<Product>> GetProducts([FromQuery]ProductFilter filter)
    {   
        return DbContext
            .Products // sample for IQueryable
            .AutoFilter(filter)
            .OrderBy("Cost") // OrderByDescending("Cost") also available
            .ToListAsync();
    }
}
```

# Specification

## Basic usage

In many scenarios queries contains duplicated filter conditions. For example we can hide product from e-shop visitors using IsAvailable option. Ant this option will duplicate in all queries which returns list of products.


```csharp
public class Product
{
    public bool IsAvailable { get; set; }
    public string Name { get; set; }
}

public class ProductController : Controller
{
    [HttpGet]
    public async Task<IEnumerable<Product>> GetProductsByName(string name)
    {
        return DbContext
            .Products
            .Where(x => x.IsAvailable && x.Name.Contains(name))            
            .ToListAsync();
    }
    
    [HttpGet]
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return DbContext
            .Products
            .Where(x => x.IsAvailable)            
            .ToListAsync();
    }
}
```

But this condition also can change. For example we can add IsAvailable property for Producer. This way we can hide products using IsAvailable property of product or IsAvailable property of Producer.

```csharp
public class Producer
{
    public bool IsAvailable { get; set; }
}

public class Product
{
    public bool IsAvailable { get; set; }
    public string Name { get; set; }
    public Producer Producer { get; set; }
}

public class ProductController : Controller
{
    [HttpGet]
    public async Task<IEnumerable<Product>> GetProductsByName(string name)
    {
        return DbContext
            .Products
            .Where(x => x.IsAvailable && x.Producer.IsAvailable && // duplicated query
                x.Name.Contains(name))            
            .ToListAsync();
    }
    
    [HttpGet]
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return DbContext
            .Products
            .Where(x => x.IsAvailable && x.Producer.IsAvailable) // duplicated query
            .ToListAsync();
    }
}
```

AutoFilter contains implementation of Specification pattern allows to encapsulate condition and combine it with ofter conditions. You can combine specifications using operators && (AND), || (OR) and ! (NOT).

```csharp

public class ProductController : Controller
{
    private static Spec<Product> IsProductAvailable = new Spec<Product>(x => x.IsAvailable && x.Producer.IsAvailable);
    private static Spec<Product> ProductByName(string name)
    {
        return new Spec<Product>(x => x.Name.Contains(name));
    }
    
    [HttpGet]
    public async Task<IEnumerable<Product>> GetProductsByName(string name)
    {
        return DbContext
            .Products
            .Where(IsProductAvailable && ProductByName(name)) // combination of specifications using && (AND) operator
            .ToListAsync();
    }
    
    [HttpGet]
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return DbContext
            .Products
            .Where(IsProductAvailable) 
            .ToListAsync();
    }
}
```

## Specification and AutoFilter

Specification contains search options whist depends on application logic and user can't change them. But filter DTO contains search options specified by user. This way you can first apply specification and after that apply auto filter.

```csharp

public class ProductController : Controller
{
    private static Spec<Product> IsProductAvailable = new Spec<Product>(x => x.IsAvailable && x.Producer.IsAvailable);
    
    [HttpGet]
    public async Task<IEnumerable<Product>> GetProducts([FromQuery]ProductFilter filter)
    {
        return DbContext
            .Products
            .Where(IsProductAvailable) // specification
            .AutoFilter(filter) // autofilter
            .ToListAsync();
    }
}
```

## Specification and AutoFilter with AutoMapper
Often controller's method needs to return DTO instead of domain entity (for example we need to hide from e-shop visitor properties IsAvailable and CreationDate of Product). In this case we need to apply specification for domain entity, after that apply mapping and apply autofilter for DTOs. Filtering it is client logic and DTO property names could differ from domain entity property names, that's why we apply autofilter after mapping.

```csharp

public class Product
{
    public int Id { get; set; }
    public int Name { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreationDate { get; set; }
}

public class ProductDto
{
    public int Id { get; set; }
    public int Name { get; set; }
}

public class ProductController : Controller
{
    private static Spec<Product> IsProductAvailable = new Spec<Product>(x => x.IsAvailable && x.Producer.IsAvailable);
    
    [HttpGet]
    public async Task<IEnumerable<ProductDto>> GetProducts([FromQuery]ProductFilter filter)
    {
        return DbContext
            .Products
            .Where(IsProductAvailable) // specification fitler for domain entities
            .ProjectTo<ProductDto>(Mapper.ConfigurationProvider) // AutoMapper mapping from domain entities to DTOs
            .AutoFilter(filter) // autofilter for DTOs
            .ToListAsync();
    }
}
```

## Advanced scenarios (killer feature)

Let's imagine that we hide products at our e-shop using only property IsAvailable for Producer. In this case we can receive list of available products this way:

```csharp

public class ProductController : Controller
{
    private static Spec<Product> IsProductAvailable = new Spec<Product>(x => x.Producer.IsAvailable);
    
    [HttpGet]
    public async Task<IEnumerable<Product>> GetProducts()
    {
        return DbContext
            .Products
            .Where(IsProductAvailable) // specification            
            .ToListAsync();
    }
}
```

But in this case specification depends only for producer but not product. And AutoFilter allows to create specification like this:
```csharp

public class ProductController : Controller
{
    private static Spec<Producer> IsProducerAvailable = new Spec<Producer>(x => x.IsAvailable);
    
    [HttpGet]
    public async Task<IEnumerable<Product>> GetProducts()
    {
        return DbContext
            .Products
            .Where(x => x.Producer, IsProducerAvailable) // specification for Producer but not product
            .ToListAsync();
    }
}
```

AutoFilter also allows this kind of specifications for many to many relationships too. For example if we have an entity Category and many-to-many relationship between Product and Category, and Category also has IsAvailable option then we can show available products this way:


```csharp

public class Category
{
    public bool IsAvailable { get; set; }
}

public class ProductCategory
{
    public Category Category { get; set; }
    public Product Product { get; set; }
}

public class Product
{
    public ICollection Categories { get; set; }
}

public class ProductController : Controller
{
    private static Spec<Category> IsCategoryAvailable = new Spec<Category>(x => x.IsAvailable);
    
    [HttpGet]
    public async Task<IEnumerable<Product>> GetProducts()
    {
        return DbContext
            .Products
            .WhereAny(x => x.Categories, IsCategoryAvailable) // specification for Category and extension method WhenAny
            .ToListAsync();
    }
}
```

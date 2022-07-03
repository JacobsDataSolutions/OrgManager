using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Common.Mapping;
using JDS.OrgManager.Domain.Models;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JDS.OrgManager.Application.UnitTests
{
    public class TestDomainBaseClass
    {
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
    }

    public class TestDomainChildClass
    {
        public Guid GuidProperty { get; set; }
    }

    public class Item : DomainEntity<Item>
    {
        private ItemCategory category = default!;

        private List<StockTransaction> stockTransactions = new();

        private List<PlannedTransaction> plannedTransactions = new();

        public decimal Price { get; init; }
        public decimal Cost { get; init; }
        public int MaxOrderQty { get; init; }
        public ItemType ItemType { get; init; }
        public ItemCategory Category { get => category; init => category = value; }
        public List<StockTransaction> StockTransactions { get => stockTransactions; init => stockTransactions = value; }
        public List<PlannedTransaction> PlannedTransactions { get => plannedTransactions; init => plannedTransactions = value; }
        public int StockQty { get => CalculateStockQty(); private set => CalculateStockQty(); }
        public int AvailableQty { get => CalculateAvailableQty(); private set => CalculateAvailableQty(); }

        private int CalculateStockQty() => 1;
        private int CalculateAvailableQty() => 1;
    }

    public class ItemCategory : DomainEntity<ItemCategory>
    {
    }
    public class StockTransaction : DomainEntity<StockTransaction>
    {
    }
    public class PlannedTransaction : DomainEntity<PlannedTransaction>
    {
    }
    public class ItemType : DomainEntity<ItemType>
    {
    }

    public class Book : Item
    {
        public string Title { get; init; } = default!;
        public string Author { get; init; } = default!;
    }

    public class ItemViewModel : IViewModel
    {
        public int Id { get; set; }
        public decimal Price { get; init; }
        public decimal Cost { get; init; }
        public int MaxOrderQty { get; init; }
        public ItemType ItemType { get; init; }
    }

    public class BookViewModel : ItemViewModel
    {
        public string Title { get; init; } = default!;
        public string Author { get; init; } = default!;
        public decimal Price { get; init; }
        public decimal Cost { get; init; }
        public int MaxOrderQty { get; init; }
        public int StockQty { get; private set; }
        public int AvailableQty { get; private set; }
    }

    public partial class ItemDomainEntityToViewModelMapper : MapperBase<Item, ItemViewModel>, IDomainEntityToViewModelMapper<Item, ItemViewModel>
    {

    }

    public partial class ItemDomainEntityToViewModelMapper
    {
        protected override TypeAdapterSetter<Item, ItemViewModel> Configure(TypeAdapterSetter<Item, ItemViewModel> typeAdapterSetter)
            => typeAdapterSetter.Include<Book, BookViewModel>();
    }

    public class MiscMapperTests
    {
        private readonly ServiceCollection services;
        private readonly ServiceProvider serviceProvider;

        public MiscMapperTests()
        {
            services = new ServiceCollection();
            services.AddSingleton<IModelMapper, ModelMapper>();
            services.AddSingleton<IDomainEntityToViewModelMapper<Item, ItemViewModel>, ItemDomainEntityToViewModelMapper>();

            services.AddScoped<IMapper, Mapper>();
            serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void ItemDomainToViewModelMapper_BookItem_BookViewModelReturned()
        {
            var modelMapper = serviceProvider.GetRequiredService<IModelMapper>() ?? throw new ArgumentNullException(nameof(IModelMapper));
            var itemDomainEntityToViewModelMapper = serviceProvider.GetRequiredService<IDomainEntityToViewModelMapper<Item, ItemViewModel>>();
            var mapper = serviceProvider.GetRequiredService<IMapper>();
            mapper.Config.NewConfig<Item, ItemViewModel>().Include<Book, BookViewModel>();

            var book = new Book
            {
                Id = 3,
                Price = 5.0M,
                Cost = 3.0M,
                MaxOrderQty = 5,
                Title = "Title",
                Author = "Author"
            };

            var item = book as Item;

            void AssertBookProperties(BookViewModel bookViewModel)
            {
                Assert.NotNull(bookViewModel);
                Assert.True(bookViewModel is BookViewModel);
                Assert.Equal(3, bookViewModel.Id);
                Assert.Equal(5.0M, bookViewModel.Price);
                Assert.Equal(3.0M, bookViewModel.Cost);
                Assert.Equal(5, bookViewModel.MaxOrderQty);
                Assert.Equal("Author", bookViewModel.Author);
                Assert.Equal("Title", bookViewModel.Title);
            }

            // This works. itemViewModel3 is a BookViewModel
            var itemViewModel3 = mapper.Map<Item, ItemViewModel>(book);
            Assert.True(itemViewModel3 is BookViewModel);
            AssertBookProperties(itemViewModel3 as BookViewModel);

            // This works. itemViewModel4 is a BookViewModel
            var itemViewModel4 = mapper.Map<Item, ItemViewModel>(item);
            Assert.True(itemViewModel3 is BookViewModel);
            AssertBookProperties(itemViewModel3 as BookViewModel);

            // This works after updating MapperBase'T to explicitly provide both TSource and TDestination.
            var itemViewModel2 = itemDomainEntityToViewModelMapper.Map(item);
            Assert.True(itemViewModel2 is BookViewModel);
            AssertBookProperties(itemViewModel2 as BookViewModel);

            // This works after updating MapperBase'T to explicitly provide both TSource and TDestination.
            var itemViewModel = modelMapper.MapDomainEntityToViewModel<Item, ItemViewModel>(item);
            Assert.True(itemViewModel is BookViewModel);
            AssertBookProperties(itemViewModel as BookViewModel);
        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using PowerBot.Lite.Attributes;
using PowerBot.Lite.Attributes.AttributeValidators;
using Telegram.Bot.Types;
using Xunit;

namespace PowerBot.Lite.Tests;

internal class AllowFilter : IHandlerFilter
{
    public Task<bool> FilterAsync(Update update) => Task.FromResult(true);
}

internal class DenyFilter : IHandlerFilter
{
    public Task<bool> FilterAsync(Update update) => Task.FromResult(false);
}

public class AttributeValidatorFilterTests
{
    private class TestHandlerWithFilter
    {
        [HandlerFilter<AllowFilter>]
        public Task FilteredMethod() => Task.CompletedTask;

        [HandlerFilter<AllowFilter>]
        [HandlerFilter<DenyFilter>]
        public Task MultiFilteredMethod() => Task.CompletedTask;

        public Task NoFilterMethod() => Task.CompletedTask;
    }

    [Fact]
    public void GetHandlerFilterTypes_WithSingleFilter_ReturnsSingleType()
    {
        var method = typeof(TestHandlerWithFilter).GetMethod(nameof(TestHandlerWithFilter.FilteredMethod))!;
        var filterTypes = AttributeValidators.GetHandlerFilterTypes(method);
        Assert.Single(filterTypes);
        Assert.Equal(typeof(AllowFilter), filterTypes.First());
    }

    [Fact]
    public void GetHandlerFilterTypes_WithMultipleFilters_ReturnsAllTypes()
    {
        var method = typeof(TestHandlerWithFilter).GetMethod(nameof(TestHandlerWithFilter.MultiFilteredMethod))!;
        var filterTypes = AttributeValidators.GetHandlerFilterTypes(method);
        Assert.Equal(2, filterTypes.Count());
        Assert.Contains(typeof(AllowFilter), filterTypes);
        Assert.Contains(typeof(DenyFilter), filterTypes);
    }

    [Fact]
    public void GetHandlerFilterTypes_WithNoFilter_ReturnsEmpty()
    {
        var method = typeof(TestHandlerWithFilter).GetMethod(nameof(TestHandlerWithFilter.NoFilterMethod))!;
        var filterTypes = AttributeValidators.GetHandlerFilterTypes(method);
        Assert.Empty(filterTypes);
    }
}

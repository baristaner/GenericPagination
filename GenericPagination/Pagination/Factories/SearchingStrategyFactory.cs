using GenericPagination.Pagination.Interfaces;
using GenericPagination.Pagination.Strategies.Searching;

namespace Pagination.Factories;

public class SearchingStrategyFactory
{
    public static ISearchingStrategy CreateStrategy(string strategyType)
    {
        return strategyType.ToLower() switch
        {
            "custom" => new CustomSearchingStrategy(),
            _ => new DefaultSearchingStrategy(),
        };
    }
}

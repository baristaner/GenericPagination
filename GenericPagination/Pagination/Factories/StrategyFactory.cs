using GenericPagination.Pagination.Interfaces;
using GenericPagination.Pagination.Strategies.Sorting;

namespace GenericPagination.Pagination.Factories;

public class StrategyFactory
{
    public ISortingStrategy GetSortingStrategy(string strategyType)
    {
        return strategyType switch
        {
            "custom" => new CustomSortingStrategy(),
            _ => new DefaultSortingStrategy(),
        };
    }
}

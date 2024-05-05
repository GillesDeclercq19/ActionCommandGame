using System.Linq;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Mappings;

public static class QueryableMappingExtensions
{
    public static IQueryable<PlayerResult> MapToResults(this IQueryable<Player> query)
    {
        return query.Select(ProjectionExpressions.ProjectToPlayerResult());
    }

    public static IQueryable<ItemResult> MapToResults(this IQueryable<Item> query)
    {
        return query.Select(ProjectionExpressions.ProjectToItemResult());
    }

    public static IQueryable<PlayerItemResult> MapToResults(this IQueryable<PlayerItem> query)
    {
        return query.Select(ProjectionExpressions.ProjectToPlayerItemResult());
    }

    public static IQueryable<NegativeGameEventResult> MapToResults(this IQueryable<NegativeGameEvent> query)
    {
        return query.Select(ProjectionExpressions.ProjectToNegativeGameEventResult());
    }

    public static IQueryable<PositiveGameEventResult> MapToResults(this IQueryable<PositiveGameEvent> query)
    {
        return query.Select(ProjectionExpressions.ProjectToPositiveEventResult());
    }
}
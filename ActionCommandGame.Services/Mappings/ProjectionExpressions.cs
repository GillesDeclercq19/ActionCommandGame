using System;
using System.Linq;
using System.Linq.Expressions;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Services.Model.Results.DTO;

namespace ActionCommandGame.Services.Mappings;

public static class ProjectionExpressions
{
    public static Expression<Func<Player, PlayerResult>> ProjectToPlayerResult()
    {
        return entity => new PlayerResult
        {
            Id = entity.Id,
            Name = entity.Name,
            Money = entity.Money,
            Experience = entity.Experience,
            LastActionExecutedDateTime = entity.LastActionExecutedDateTime,
            CurrentAttackPlayerItemId = entity.CurrentAttackPlayerItemId,
            CurrentDefensePlayerItemId = entity.CurrentDefensePlayerItemId,
            CurrentFuelPlayerItemId = entity.CurrentFuelPlayerItemId,
            Inventory = entity.Inventory.Select(item => new ItemDto
            {
                Id = item.Id,
                Name = item.Item.Name
            }).ToList(),
        };
    }

    public static Expression<Func<Item, ItemResult>> ProjectToItemResult()
    {
        return entity => new ItemResult
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            Fuel = entity.Fuel,
            Attack = entity.Attack,
            Defense = entity.Defense,
            ActionCooldownSeconds = entity.ActionCooldownSeconds,
            PlayerItems = entity.PlayerItems.Select(item => new ItemDto
            {
                Id = item.Id,
                Name = item.Item.Name
            }).ToList(),
        };
    }

    public static Expression<Func<PlayerItem, PlayerItemResult>> ProjectToPlayerItemResult()
    {
        return entity => new PlayerItemResult
        {
            Id = entity.Id,
            PlayerId = entity.PlayerId,
            ItemId = entity.ItemId,
            RemainingAttack = entity.RemainingAttack,
            RemainingDefense = entity.RemainingDefense,
            RemainingFuel = entity.RemainingFuel
        };
    }

    public static Expression<Func<NegativeGameEvent, NegativeGameEventResult>> ProjectToNegativeGameEventResult()
    {
        return entity => new NegativeGameEventResult
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            DefenseWithGearDescription = entity.DefenseWithGearDescription,
            DefenseWithoutGearDescription = entity.DefenseWithoutGearDescription,
            DefenseLoss = entity.DefenseLoss,
            Probability = entity.Probability
        };
    }

    public static Expression<Func<PositiveGameEvent, PositiveGameEventResult>> ProjectToPositiveEventResult()
    {
        return entity => new PositiveGameEventResult
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Money = entity.Money,
            Experience = entity.Experience,
            Probability = entity.Probability
        };
    }
}
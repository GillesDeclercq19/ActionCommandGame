using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Extensions;

public static class ProjectionExpressions
{
    public static Expression<Func<Player, PlayerResult>> ProjectToPlayerResult()
    {
        return entity => new PlayerResult
        {
            Id = entity.Id,
            Name = entity.Name,
            Zeni = entity.Zeni,
            UserId = entity.UserId,
            Experience = entity.Experience,
            LastActionExecutedDateTime = entity.LastActionExecutedDateTime,
            CurrentKiPlayerItem = entity.CurrentKiPlayerItem.RemainingKi,
            CurrentKiPlayerItemName = entity.CurrentKiPlayerItem.Item.Name,
            CurrentAttackPlayerItem = entity.CurrentAttackPlayerItem.RemainingAttack,
            CurrentAttackPlayerItemName = entity.CurrentAttackPlayerItem.Item.Name,
            CurrentDefensePlayerItem = entity.CurrentDefensePlayerItem.RemainingDefense,
            CurrentDefensePlayerItemName = entity.CurrentDefensePlayerItem.Item.Name,
            Inventory = entity.Inventory.Select(pi => new PlayerItemResult
            {
                Id = pi.Id,
                PlayerId = pi.PlayerId,
                PlayerName = entity.Name,
                ItemId = pi.ItemId,
                ItemName = pi.Item.Name,
                ItemDescription = pi.Item.Description,
                RemainingKi = pi.RemainingKi,
                RemainingAttack = pi.RemainingAttack,
                RemainingDefense = pi.RemainingDefense
            }).ToList() ?? new List<PlayerItemResult>()
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
            Ki = entity.Ki,
            Attack = entity.Attack,
            Defense = entity.Defense,
            ActionCooldownSeconds = entity.ActionCooldownSeconds,
        };
    }

    public static Expression<Func<PlayerItem, PlayerItemResult>> ProjectToPlayerItemResult()
    {
        return entity => new PlayerItemResult
        {
            Id = entity.Id,
            PlayerId = entity.PlayerId,
            PlayerName = entity.Player.Name,
            ItemId = entity.ItemId,
            ItemName = entity.Item.Name,
            ItemDescription = entity.Item.Description,
            RemainingAttack = entity.RemainingAttack,
            RemainingDefense = entity.RemainingDefense,
            RemainingKi = entity.RemainingKi
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
            Zeni = entity.Zeni,
            Experience = entity.Experience,
            Probability = entity.Probability
        };
    }
}
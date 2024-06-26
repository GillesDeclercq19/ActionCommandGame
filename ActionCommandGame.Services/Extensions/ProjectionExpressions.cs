﻿using System;
using System.Linq;
using System.Linq.Expressions;
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
            KiItemName = entity.CurrentKiPlayerItem.Item.Name,
            CurrentAttackPlayerItem = entity.CurrentAttackPlayerItem.RemainingAttack,
            AttItemName = entity.CurrentAttackPlayerItem.Item.Name,
            CurrentDefensePlayerItem = entity.CurrentDefensePlayerItem.RemainingDefense,
            DefItemName = entity.CurrentDefensePlayerItem.Item.Name,
            Inventory = entity.Inventory.Select(pi => new PlayerItemResult
            {
                Id = pi.Id,
                ItemName = pi.Item.Name,
                RemainingKi = pi.RemainingKi,
                RemainingAttack = pi.RemainingAttack,
                RemainingDefense = pi.RemainingDefense
            }).ToList()
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
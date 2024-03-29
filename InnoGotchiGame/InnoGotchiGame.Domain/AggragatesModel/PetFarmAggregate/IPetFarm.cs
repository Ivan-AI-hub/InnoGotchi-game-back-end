﻿using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;
using InnoGotchiGame.Domain.AggragatesModel.UserAggregate;

namespace InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate
{
    public interface IPetFarm
    {
        int Id { get; }
        string Name { get; set; }
        DateTime CreateDate { get; }

        int OwnerId { get; }
        IUser Owner { get; }

        IEnumerable<IPet> Pets { get; }
    }
}
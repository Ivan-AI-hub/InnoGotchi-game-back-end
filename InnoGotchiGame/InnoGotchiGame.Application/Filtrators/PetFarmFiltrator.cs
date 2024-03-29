﻿using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;

namespace InnoGotchiGame.Application.Filtrators
{
    public class PetFarmFiltrator : Filtrator<IPetFarm>
    {
        public string Name { get; set; } = "";
        internal override IQueryable<IPetFarm> Filter(IQueryable<IPetFarm> farms)
        {
            return farms.Where(x => x.Name.Contains(Name));
        }
    }
}

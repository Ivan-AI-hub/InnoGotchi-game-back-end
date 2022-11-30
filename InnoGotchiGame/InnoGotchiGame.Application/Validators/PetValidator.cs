﻿using FluentValidation;
using InnoGotchiGame.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoGotchiGame.Application.Validators
{
	public class PetValidator : AbstractValidator<Pet>
	{
		public PetValidator() 
		{
			RuleFor(pet => pet.Statistic.Name)
				.NotEmpty()
				.NotNull();

			RuleFor(pet => pet.Statistic.BornDate)
				.NotEmpty()
				.NotNull();

			RuleFor(pet => pet.Statistic.IsAlive)
				.NotEmpty()
				.NotNull();

			RuleFor(pet => pet.Statistic.FeedingCount)
				.NotEmpty()
				.NotNull();

			RuleFor(pet => pet.Statistic.DrinkingCount)
				.NotEmpty()
				.NotNull();

			RuleFor(pet => pet.Statistic.FirstHappinessDay)
				.NotEmpty()
				.NotNull();

			RuleFor(pet => pet.Statistic.DateLastFeed)
				.NotEmpty()
				.NotNull();

			RuleFor(pet => pet.Statistic.DateLastDrink)
				.NotEmpty()
				.NotNull();
		}
	}
}
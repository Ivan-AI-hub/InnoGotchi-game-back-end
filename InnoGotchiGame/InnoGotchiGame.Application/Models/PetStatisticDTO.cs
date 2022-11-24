﻿namespace InnoGotchiGame.Application.Models
{
	public record PetStatisticDTO
	{
		public string? Name { get; set; }
		public int Age { get; set; }
		public bool IsAlive { get; set; }
		public DateTime FirstHappinessDay { get; set; }
		public DateTime DataLastFeed { get; set; }
		public DateTime DataLastDrink { get; set; }
	}
}
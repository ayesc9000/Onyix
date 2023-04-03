/* Onyix - An open-source Discord bot
 * Copyright (C) 2022 Liam "AyesC" Hogan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Threading.Tasks;

namespace Onyix.Commands;

/// <summary>
/// A command module for rolling dice
/// </summary>
public class Roll : ApplicationCommandModule
{
	/// <summary>
	/// Reply to an interaction to roll a die
	/// </summary>
	/// <param name="ctx">The context for this interaction</param>
	/// <param name="amount">The amount of dice to roll</param>
	/// <param name="sides">The amount of sides per die</param>
	/// <param name="starting">Add or subtract a specific amount from the result</param>
	/// <returns>An asynchronous task representing this reply</returns>
	[SlashCommand("roll", "Rolls a dice", true)]
	public static async Task Execute(InteractionContext ctx,
		[Option("amount", "The amount of dice to roll")] long amount,
		[Option("sides", "The amount of sides per die")] long sides,
		[Option("starting", "Add or subtract a specific amount from the result")] long starting)
	{
		// Check if values are within acceptable range
		// These are here to prevent someone from attempting to create a
		// result that overflows the maximum amount allowed in a 32-bit int
		if (amount is < 1 or > 128)
		{
			await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
				.WithTitle("Error")
				.WithDescription("You may only roll 1 to 128 dice at a time.")
				.WithColor(Colors.Red), true);

			return;
		}

		if (sides is < 2 or > 128)
		{
			await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
				.WithTitle("Error")
				.WithDescription("Dice may only have 2 to 128 sides.")
				.WithColor(Colors.Red), true);

			return;
		}

		if (starting is < (-10000) or > 10000)
		{
			await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
				.WithTitle("Error")
				.WithDescription("The starting value must be within -10,000 to 10,000.")
				.WithColor(Colors.Red), true);

			return;
		}

		// Calculate result
		int result = (int)starting;
		Random rand = new();

		for (long i = 0; i < amount; i++)
		{
			result += rand.Next(1, (int)sides);
		}

		// Reply with embed
		await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
			.WithTitle("Dice roll")
			.WithColor(Colors.Grey)
			.AddField("Dice", amount.ToString(), true)
			.AddField("Sides", sides.ToString(), true)
			.AddField("Starting value", starting.ToString(), true)
			.AddField("Result", result.ToString(), false));
	}
}

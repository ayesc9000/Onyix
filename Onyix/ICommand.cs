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

using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onyix
{
	/// <summary>
	/// Represents a slash command
	/// </summary>
	public interface ICommand
	{
		/// <summary>
		/// The name of the command
		/// </summary>
		public string Name
		{
			get;
		}

		/// <summary>
		/// The description of the command
		/// </summary>
		public string Description
		{
			get;
		}

		/// <summary>
		/// Should the command be usable in direct messages
		/// </summary>
		public bool UseInDMs
		{
			get;
		}

		/// <summary>
		/// The options for this command
		/// </summary>
		public List<SlashCommandOptionBuilder>? Options
		{
			get;
		}

		/// <summary>
		/// The executer method that is ran when the command is used
		/// </summary>
		public Task Execute(Client client, SocketSlashCommand command);
	}
}

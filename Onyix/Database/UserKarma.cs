/* Onyix - An open-source Discord bot
 * Copyright (C) 2023 Liam "AyesC" Hogan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using Microsoft.EntityFrameworkCore;

namespace Onyix.Database
{
	/// <summary>
	/// Represents a user's global karma
	/// </summary>
	[Index(nameof(UserId), IsUnique = true)]
	public class UserKarma
	{
		public int Id { get; set; }
		public ulong UserId { get; set; } = 0;
		public long Upvotes { get; set; } = 0;
		public long Downvotes { get; set; } = 0;
		public long Awards { get; set; } = 0;
		public long Posts { get; set; } = 0;
		public long Removed { get; set; } = 0;
	}
}

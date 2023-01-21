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
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */


using Microsoft.EntityFrameworkCore;

namespace Onyix.Entities
{
	[Index(nameof(UserId), IsUnique = true)]
	public class UserKarma
	{
		public int Id { get; set; }

		public ulong UserId { get; set; }

		public long Upvotes { get; set; }

		public long Downvotes { get; set; }

		public long Awards { get; set; }

		public long Posts { get; set; }

		public long Removed { get; set; }

		public UserKarma(ulong user)
		{
			UserId = user;
			Upvotes = 0;
			Downvotes = 0;
			Awards = 0;
			Posts = 0;
			Removed = 0;
		}
	}
}

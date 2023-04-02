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

using DSharpPlus.Entities;

namespace Onyix;

/// <summary>
/// A class with pre-defined DiscordColor types
/// </summary>
public static class Colors
{
	public static DiscordColor Gray => new("#606266");
	public static DiscordColor Green => new("#32a852");
	public static DiscordColor Yellow => new("#edbc28");
	public static DiscordColor Red => new("#e83c3c");
}

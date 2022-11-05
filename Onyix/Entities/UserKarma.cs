using LiteDB;

namespace Onyix.Entities
{
	public class UserKarma
	{
		public ObjectId Id { get; set; }
		public ulong UserId { get; set; }
		public long Upvotes { get; set; }
		public long Downvotes { get; set; }
		public long Awards { get; set; }
		public long Posts { get; set; }
		public long Removed { get; set; }

		public UserKarma()
		{
			UserId = 0;
			Upvotes = 0;
			Downvotes = 0;
			Awards = 0;
			Posts = 0;
			Removed = 0;
		}
	}
}

namespace Reversid.Misskey.Entities
{
	public class Invitation
	{
		public string CreatedAt { get; set; }
		public string ParentId { get; set; }
		public string ChildId { get; set; }
		public string Id { get; set; }

		public User Parent { get; set; }
		public User Child { get; set; }
	}
}

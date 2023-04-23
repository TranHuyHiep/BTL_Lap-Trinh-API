namespace BTLWeb.Models
{
    public partial class Comment
    {

        public long ID { get; set; }
        
        public string CommentMsg { get; set; }
        public Nullable<System.DateTime> CommentDate { get; set; }

        public string ProductID { get; set; }

        public long UserId { get; set; }

        public long ParentID { get; set; }
        public string FullName { get; set; }
    }
}

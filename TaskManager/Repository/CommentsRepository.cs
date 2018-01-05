using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entity;

namespace TaskManager.Repository
{
    public class CommentsRepository : BaseRepository<Comment>
    {
        public CommentsRepository(string filePath) : base(filePath)
        {}

        public override void PopulateEntity(StreamReader sr, Comment item)
        {
            item.Id = Convert.ToInt32(sr.ReadLine());
            item.RelatedTaskId = Convert.ToInt32(sr.ReadLine());
            item.CreatorId = Convert.ToInt32(sr.ReadLine());
            item.CommentText = sr.ReadLine();
            item.CommentDate = Convert.ToDateTime(sr.ReadLine());
        }

        public override void WriteEntity(StreamWriter sw, Comment item)
        {
            sw.WriteLine(item.Id);
            sw.WriteLine(item.RelatedTaskId);
            sw.WriteLine(item.CreatorId);
            sw.WriteLine(item.CommentText);
            sw.WriteLine(item.CommentDate);
        }
    }
}

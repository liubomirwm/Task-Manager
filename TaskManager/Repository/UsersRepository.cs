using System;
using System.IO;
using TaskManager.Entity;

namespace TaskManager.Repository
{
    public class UsersRepository : BaseRepository<User>
    {
        public UsersRepository(string filePath) : base(filePath)
        {}

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            try
            {
                while (!sr.EndOfStream)
                {
                    User user = new User();
                    user.Id = Convert.ToInt32(sr.ReadLine());
                    user.FullName = sr.ReadLine();
                    user.Username = sr.ReadLine();
                    user.Password = sr.ReadLine();
                    user.IsAdmin = Convert.ToBoolean(sr.ReadLine());
                    if (user.Username == username && user.Password == password)
                    {
                        return user;
                    }
                }
            }
            finally
            {
                sr.Dispose();
                fs.Dispose();
            }
            // if credentials not found
            return null;
        }

        public override void PopulateEntity(StreamReader sr, User item)
        {
            item.Id = Convert.ToInt32(sr.ReadLine());
            item.FullName = sr.ReadLine();
            item.Username = sr.ReadLine();
            item.Password = sr.ReadLine();
            item.IsAdmin = Convert.ToBoolean(sr.ReadLine());
        }

        public override void WriteEntity(StreamWriter sw, User item)
        {
            sw.WriteLine(item.Id);
            sw.WriteLine(item.FullName);
            sw.WriteLine(item.Username);
            sw.WriteLine(item.Password);
            sw.WriteLine(item.IsAdmin);
        }
    }
}

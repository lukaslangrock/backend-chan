using System.Runtime.InteropServices.JavaScript;

namespace backend.database;

using System.Data.SQLite;

public static class DB
{
   private static readonly string DbPath = "resources/database.db";
   private static readonly string DbCreationPath = "resources/createDB.sql";
   private static SQLiteConnection Connection;
   
   static DB()
   {
      Connection = CreateConnection();
   }
   
   private static SQLiteConnection CreateConnection()
   {
      SQLiteConnection sqliteConn;

      sqliteConn = new SQLiteConnection("Data Source=" + DbPath + ";Version=3;New=True;Compress=True;");

      try
      {
         sqliteConn.Open();
      }
      catch (Exception ex)
      {
         Console.WriteLine(ex.ToString());
      }
      return sqliteConn;
   }

   public static bool CheckUserCredentials(string username, string password)
   {
      return ExecuteQuery("SELECT * FROM user WHERE username=" + username + " AND password=" + password, reader => {}) > 0;
   }

   public static User? GetUserById(int id)
   {
      User? user = null;
      ExecuteQuery("SELECT * FROM user WHERE id=" + id, reader =>
      {
         user = new User(reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetInt32(4) == 1 ? OnlineStatus.Online : OnlineStatus.Offline);
      });
      
      return user;
   }

   public static Room? GetRoomById(int id)
   {
      Room? room = null;
      ExecuteQuery("SELECT * FROM room WHERE id=" + id, reader =>
      {
         room = new Room(reader.GetInt32(0), reader.GetString(1));
      });

      return room;
   }

   public static Message[] GetMessages(int roomId, long startTimestamp, long endTimestamp)
   {
      List<Message> messages = new List<Message>();
      ExecuteQuery(
         "SELECT * FROM message WHERE roomid=" + roomId + " AND timestamp>=" + startTimestamp + " AND timestamp<=" +
         endTimestamp,
         reader =>
         {
            messages.Add(new Message(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3),
               reader.GetString(4)));
         });

      return messages.ToArray();
   }

   public static OnlineStatus? GetOnlineStatus(int userId)
   {
      return GetUserById(userId)?.OnlineStatus;
   }

   public static User[] GetRoomMembers(int roomId)
   {
      List<User> roomMembers = new List<User>();

      ExecuteQuery("SELECT * FROM userroom WHERE roomId=" + roomId, reader =>
      {
         UserRoom userRoom = new UserRoom(reader.GetInt32(0), reader.GetInt32(1));

         User? user = GetUserById(userRoom.UserId);
         if(user != null)
            roomMembers.Add(user);
      });

      return roomMembers.ToArray();
   }

   public static int CreateDB()
   {
      return ExecuteNonQuery(File.ReadAllText(DbCreationPath));
   }
   
   public static int ExecuteNonQuery(string sql)
   {
      SQLiteCommand sqliteCmd = Connection.CreateCommand();
      sqliteCmd.CommandText = sql;
      return sqliteCmd.ExecuteNonQuery();
   }
   
   public static int ExecuteQuery(string sql, Action<SQLiteDataReader> lineProcessor)
   {
      SQLiteCommand sqliteCmd  = Connection.CreateCommand();
      sqliteCmd.CommandText = sql;

      SQLiteDataReader reader = sqliteCmd.ExecuteReader();

      int numRows = 0;
      for (; reader.Read(); numRows++)
      {
         lineProcessor(reader);
      }

      return numRows;
   }
}
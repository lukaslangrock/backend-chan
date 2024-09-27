namespace backend.database;

using System.Data.SQLite;

public static class Database
{
   private static readonly string DbPath = "database.db";
   private static readonly string DbCreationPath = "createDB.sql";
   private static SQLiteConnection Connection;
   
   static Database()
   {
      Connection = CreateConnection();
   }

   private static void CreateDB()
   {
      // TODO add code for db creation
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

   public static void ExecuteNonQuery(string sql)
   {
      SQLiteCommand sqliteCmd = Connection.CreateCommand();
      sqliteCmd.CommandText = sql;
      sqliteCmd.ExecuteNonQuery();
   }
   
   public static void ExecuteQuery(string sql)
   {
      SQLiteCommand sqliteCmd  = Connection.CreateCommand();
      sqliteCmd.CommandText = sql;

      SQLiteDataReader reader = sqliteCmd.ExecuteReader();
      while (reader.Read())
      {
         string myreader = reader.GetString(0);
         Console.WriteLine(myreader);
      }

      Connection.Close();
   }
}
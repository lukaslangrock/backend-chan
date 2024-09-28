using System.Data.Entity;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using backend;
using backend.database;

DB.CreateDB();

WebHandler.Run(args);


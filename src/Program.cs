using backend;
using backend.database;

LlmController.Init();

DB.CreateDB();

WebHandler.Run(args);

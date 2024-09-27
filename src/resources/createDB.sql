CREATE TABLE IF NOT EXISTS "User" (
                                      "id" INTEGER NOT NULL,
                                      "username" TEXT NOT NULL UNIQUE,
                                      "password" TEXT NOT NULL,
                                      "displayname" TEXT NOT NULL,
                                      "onlinestatus" INTEGER NOT NULL,
                                      PRIMARY KEY("id")
    );

CREATE TABLE IF NOT EXISTS "Message" (
                                         "id" INTEGER NOT NULL UNIQUE,
                                         "timestamp" INTEGER NOT NULL,
                                         "roomid" INTEGER NOT NULL,
                                         "senderid" INTEGER NOT NULL,
                                         "text" TEXT NOT NULL,
                                         PRIMARY KEY("id"),
    FOREIGN KEY ("roomid") REFERENCES "Room"("id")
    ON UPDATE NO ACTION ON DELETE NO ACTION,
    FOREIGN KEY ("senderid") REFERENCES "User"("id")
    ON UPDATE NO ACTION ON DELETE NO ACTION
    );

CREATE TABLE IF NOT EXISTS "Room" (
                                      "id" INTEGER NOT NULL UNIQUE,
                                      "displayname" TEXT NOT NULL,
                                      PRIMARY KEY("id"),
    FOREIGN KEY ("id") REFERENCES "RoomUser"("roomid")
    ON UPDATE NO ACTION ON DELETE NO ACTION
    );

CREATE TABLE IF NOT EXISTS "RoomUser" (
                                          "userid" INTEGER NOT NULL UNIQUE,
                                          "roomid" INTEGER NOT NULL,
                                          PRIMARY KEY("userid", "roomid"),
    FOREIGN KEY ("userid") REFERENCES "User"("id")
    ON UPDATE NO ACTION ON DELETE NO ACTION
    );
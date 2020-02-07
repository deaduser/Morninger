DROP TABLE "Users";
CREATE TABLE "Users" (
	"Id"			INTEGER DEFAULT 0,
	"FirstName"		TEXT DEFAULT "0",
	"LastName"		TEXT DEFAULT "0",
	"Username"		TEXT DEFAULT "0",
	"LastUpdate"	TEXT DEFAULT "0",
	PRIMARY KEY("Id")
);

DROP TABLE "Months";
CREATE TABLE "Months" (
	"UserId"		INTEGER DEFAULT 0,
	"Year"			INTEGER DEFAULT 0,
	"Month"			INTEGER DEFAULT 0,
	"Done"			INTEGER DEFAULT 0,
	"DayOff"		INTEGER DEFAULT 0,
	"LastUpdate"	TEXT DEFAULT "0",
	PRIMARY KEY("Month","Year","UserId")
);
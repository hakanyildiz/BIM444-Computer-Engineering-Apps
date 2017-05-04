USE duyarliol
GO

CREATE TABLE auths(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[authtype] [int] NULL,
	[userid] [int] NULL,
	[email] [nvarchar](150) NULL,
	[accesstoken] [nvarchar](500) NULL,
	[status] [int] NULL,
	[date] [datetime] NULL,
 CONSTRAINT [PK_authsid] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

create table auths(
	id int identity(1,1) not null,
	authtype int null,
	userid int null,
	email nvarchar(150) null,
	accesstoken nvarchar(500) null,
	status int null,
	date datetime null
	); 



CREATE TABLE users(
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[apikey] [nvarchar](250) NULL,
	[apisecret] [nvarchar](250) NULL,
	[fullname] [nvarchar](12) NULL,
	[password] [nvarchar](250) NULL,
	[registerdate] [datetime] NULL,
	[lastlogindate] [datetime] NULL,
	[status] [int] NULL,
	[namesurname] [nvarchar](150) NULL,
	[birthday] [datetime] NULL,
	[avatar] [nvarchar](150) NULL,
	[background] [nvarchar](150) NULL,
	[url] [nvarchar](150) not null
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


create table users(
	id int identity (1,1) not null,
	apikey nvarchar(250) null,
	apisecret nvarchar(250) null,
	fullname nvarchar(12) null,
	password nvarchar(250) null,
	registerdate datetime null,
	lastlogindate datetime null,
	status int null,
	namesurname nvarchar(150) null,
	birthday datetime null,
	avatar nvarchar(150) null,
	background nvarchar(150) null
);


create table userincome(
	id int identity (1,1) not null,
	jobtype nvarchar(50) null,
	monthlyincome float null,
	monthlyadditionalincome float null,
	createddate datetime null,
	updatedate datetime null,
	userid int foreign key references users(id)
);

select * from userincome

create table useroutcome(
	id int identity (1,1) not null,
	houserent float null,
	electricbill float null,
	waterbill float null,
	gasbill float null,
	internetbill float null,
	gsmbill float null,
	otherbills float null,
	individualexpense float null,
	marketexpense float null,
	createddate datetime null,
	updatedate datetime null,
	userid int foreign key references users(id)
);

select * from useroutcome

create table usercreditcards(
	id int identity(1,1) not null,
	bankname nvarchar(50) null,
	cardlimit float null,
	carddebt float null,
	createddate datetime null,
	updatedate datetime null,
	userid int foreign key references users(id)
);

select * from usercreditcards


/* wish list table creation */
create table wishlist(
	id int identity(1,1) not null,
	ordername nvarchar(200) not null,
	ordercount int not null,
	orderprice float not null,
	orderdate datetime not null,
	pending int default(1),
	userid int foreign key references users(id) not null
);

select * from wishlist

/* answer list table creation */

create table answerlist(
	id int identity(1,1) not null,
	question nvarchar(30) not null,
	answer nvarchar(10) not null,
	date datetime not null,
	userid int foreign key references users(id) not null
);

select * from answerlist


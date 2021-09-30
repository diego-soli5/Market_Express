USE master;
GO

DROP DATABASE MARKET_EXPRESS;
GO

CREATE DATABASE MARKET_EXPRESS;
GO

USE [MARKET_EXPRESS]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[Id] [uniqueidentifier] NOT NULL,
	[ClientId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[Detail] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUser]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUser](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](40) NOT NULL,
	[Identification] [varchar](12) NOT NULL,
	[Email] [varchar](40) NOT NULL,
	[Phone] [varchar](40) NOT NULL,
	[Password] [varchar](80) NOT NULL,
	[Type] [varchar](15) NOT NULL,
	[Status] [varchar](11) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[AddedBy] [varchar](12) NULL,
	[ModifiedBy] [varchar](12) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Identification] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUser_Role]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUser_Role](
	[Id] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[AppUserId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Article]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Article](
	[Id] [uniqueidentifier] NOT NULL,
	[CategoryId] [uniqueidentifier] NULL,
	[Description] [varchar](255) NOT NULL,
	[BarCode] [varchar](255) NOT NULL,
	[Price] [decimal](19, 2) NOT NULL,
	[Image] [varchar](30) NULL,
	[AutoSync] [bit] NOT NULL,
	[AutoSyncDescription] [bit] NOT NULL,
	[Status] [varchar](11) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[AddedBy] [varchar](12) NULL,
	[ModifiedBy] [varchar](12) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[BarCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Binnacle_Access]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Binnacle_Access](
	[Id] [uniqueidentifier] NOT NULL,
	[AppUserId] [uniqueidentifier] NOT NULL,
	[EntryDate] [datetime] NOT NULL,
	[ExitDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Binnacle_Movement]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Binnacle_Movement](
	[Id] [uniqueidentifier] NOT NULL,
	[AppUserId] [uniqueidentifier] NOT NULL,
	[MovementDate] [datetime] NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[Detail] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[Id] [uniqueidentifier] NOT NULL,
	[ClientId] [uniqueidentifier] NOT NULL,
	[OpeningDate] [datetime] NOT NULL,
	[Status] [varchar](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart_Detail]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart_Detail](
	[Id] [uniqueidentifier] NOT NULL,
	[CartId] [uniqueidentifier] NOT NULL,
	[ArticleId] [uniqueidentifier] NOT NULL,
	[Quantity] [decimal](19, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[Description] [varchar](200) NULL,
	[Status] [varchar](11) NOT NULL,
	[Image] [varchar](30) NULL,
	[CreationDate] [datetime] NOT NULL,
	[AddedBy] [varchar](12) NULL,
	[ModifiedBy] [varchar](12) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[Id] [uniqueidentifier] NOT NULL,
	[AppUserId] [uniqueidentifier] NOT NULL,
	[ClientCode] [varchar](10) NULL,
	[AutoSync] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[AppUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order_Detail]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order_Detail](
	[Id] [uniqueidentifier] NOT NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[ArticleId] [uniqueidentifier] NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[BarCode] [varchar](255) NOT NULL,
	[Price] [decimal](19, 2) NOT NULL,
	[Quantity] [decimal](19, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permission]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permission](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[Description] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[Description] [varchar](200) NULL,
	[CreationDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role_Permission]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role_Permission](
	[Id] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[PermissionId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_Order]    Script Date: 26/09/2021 13:12:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Order](
	[Id] [uniqueidentifier] NOT NULL,
	[ClientId] [uniqueidentifier] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[Total] [decimal](19, 2) NOT NULL,
	[Status] [varchar](9) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Address] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[AppUser] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[AppUser_Role] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Article] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Binnacle_Access] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Binnacle_Movement] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Cart] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Cart_Detail] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Category] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Client] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Order_Detail] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Permission] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Role] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Role_Permission] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[TB_Order] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Address]  WITH CHECK ADD FOREIGN KEY([ClientId])
REFERENCES [dbo].[Client] ([Id])
GO
ALTER TABLE [dbo].[AppUser_Role]  WITH CHECK ADD FOREIGN KEY([AppUserId])
REFERENCES [dbo].[AppUser] ([Id])
GO
ALTER TABLE [dbo].[AppUser_Role]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[Article]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Binnacle_Access]  WITH CHECK ADD FOREIGN KEY([AppUserId])
REFERENCES [dbo].[AppUser] ([Id])
GO
ALTER TABLE [dbo].[Binnacle_Movement]  WITH CHECK ADD FOREIGN KEY([AppUserId])
REFERENCES [dbo].[AppUser] ([Id])
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([ClientId])
REFERENCES [dbo].[Client] ([Id])
GO
ALTER TABLE [dbo].[Cart_Detail]  WITH CHECK ADD FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Article] ([Id])
GO
ALTER TABLE [dbo].[Cart_Detail]  WITH CHECK ADD FOREIGN KEY([CartId])
REFERENCES [dbo].[Cart] ([Id])
GO
ALTER TABLE [dbo].[Client]  WITH CHECK ADD FOREIGN KEY([AppUserId])
REFERENCES [dbo].[AppUser] ([Id])
GO
ALTER TABLE [dbo].[Order_Detail]  WITH CHECK ADD FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Article] ([Id])
GO
ALTER TABLE [dbo].[Order_Detail]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[TB_Order] ([Id])
GO
ALTER TABLE [dbo].[Role_Permission]  WITH CHECK ADD FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permission] ([Id])
GO
ALTER TABLE [dbo].[Role_Permission]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[TB_Order]  WITH CHECK ADD FOREIGN KEY([ClientId])
REFERENCES [dbo].[Client] ([Id])
GO
ALTER TABLE [dbo].[AppUser]  WITH CHECK ADD  CONSTRAINT [CHK_AppUser_Status] CHECK  (([Status]='ACTIVADO' OR [Status]='DESACTIVADO'))
GO
ALTER TABLE [dbo].[AppUser] CHECK CONSTRAINT [CHK_AppUser_Status]
GO
ALTER TABLE [dbo].[AppUser]  WITH CHECK ADD  CONSTRAINT [CHK_AppUser_Type] CHECK  (([Type]='ADMINISTRADOR' OR [Type]='CLIENTE'))
GO
ALTER TABLE [dbo].[AppUser] CHECK CONSTRAINT [CHK_AppUser_Type]
GO
ALTER TABLE [dbo].[Article]  WITH CHECK ADD  CONSTRAINT [CHK_Article_Status] CHECK  (([Status]='ACTIVADO' OR [Status]='DESACTIVADO'))
GO
ALTER TABLE [dbo].[Article] CHECK CONSTRAINT [CHK_Article_Status]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [CHK_Cart_Status] CHECK  (([Status]='ABIERTO' OR [Status]='CERRADO'))
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [CHK_Cart_Status]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [CHK_Category_Status] CHECK  (([Status]='ACTIVADO' OR [Status]='DESACTIVADO'))
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [CHK_Category_Status]
GO
ALTER TABLE [dbo].[TB_Order]  WITH CHECK ADD  CONSTRAINT [CHK_Order_Status] CHECK  (([Status]='PENDIENTE' OR [Status]='TERMINADO' OR [Status]='CANCELADO'))
GO
ALTER TABLE [dbo].[TB_Order] CHECK CONSTRAINT [CHK_Order_Status]
GO















INSERT INTO Permission(Id,Name) VALUES
('03B93003-B8F0-4315-A0C0-449E5058F23A','Permiso 1'),
('1AAF51B4-B054-4487-9FC5-26B96886E737','Permiso 2'),
('C236C5C0-004C-4B1C-A3E7-0E427E9F9593','Permiso 3'),
('0C13E8A6-3A0A-419E-9D18-778D8DFC87D6','Permiso 4'),
('9E66A172-D4C0-4FFB-B2F8-59471A826C17','Permiso 5');
GO

INSERT INTO Role(Id,Name,CreationDate) VALUES
('EC81F591-CA24-4A80-8D54-316897E9015C','Todos los permisos',GETDATE()),
('DF22A0FF-6FD5-4422-977D-49B99D3D71C2','Permiso 1 y 2',GETDATE()),
('C4934D01-205D-4888-827C-8787AB6B3CEE','Permiso 4 y 5',GETDATE());
GO

INSERT INTO Role_Permission(RoleId,PermissionId) VALUES
('EC81F591-CA24-4A80-8D54-316897E9015C','03B93003-B8F0-4315-A0C0-449E5058F23A'), --Todos los permisos
('EC81F591-CA24-4A80-8D54-316897E9015C','1AAF51B4-B054-4487-9FC5-26B96886E737'), --Todos los permisos
('EC81F591-CA24-4A80-8D54-316897E9015C','C236C5C0-004C-4B1C-A3E7-0E427E9F9593'), --Todos los permisos
('EC81F591-CA24-4A80-8D54-316897E9015C','0C13E8A6-3A0A-419E-9D18-778D8DFC87D6'), --Todos los permisos
('EC81F591-CA24-4A80-8D54-316897E9015C','9E66A172-D4C0-4FFB-B2F8-59471A826C17'), --Todos los permisos
('DF22A0FF-6FD5-4422-977D-49B99D3D71C2','03B93003-B8F0-4315-A0C0-449E5058F23A'), --Permiso 1 y 2
('DF22A0FF-6FD5-4422-977D-49B99D3D71C2','1AAF51B4-B054-4487-9FC5-26B96886E737'), --Permiso 1 y 2
('C4934D01-205D-4888-827C-8787AB6B3CEE','0C13E8A6-3A0A-419E-9D18-778D8DFC87D6'), --Permiso 4 y 5
('C4934D01-205D-4888-827C-8787AB6B3CEE','9E66A172-D4C0-4FFB-B2F8-59471A826C17'); --Permiso 4 y 5
GO

INSERT INTO AppUser(Id,Name,Identification,Email,Phone,Password,Type,Status,CreationDate,AddedBy) VALUES
('EA16E721-5E1D-EC11-9953-3863BBBB3AE0','Luis Diego Solís Camacho','1-1731-0010','1diego321@gmail.com','83358092','10000.+UfMddrk8Z1k7UZBQDNPvA==.705k1c4kJPT9uYc77Fkjw2/VAl257UUmJkSj0jGY/Zo=','ADMINISTRADOR','ACTIVADO',GETDATE(),'SYSTEM');
GO

INSERT INTO AppUser_Role(RoleId,AppUserId) VALUES
('EC81F591-CA24-4A80-8D54-316897E9015C','EA16E721-5E1D-EC11-9953-3863BBBB3AE0') --Todos los permisos
--('DF22A0FF-6FD5-4422-977D-49B99D3D71C2','EA16E721-5E1D-EC11-9953-3863BBBB3AE0'), --Permiso 1 y 2
--('C4934D01-205D-4888-827C-8787AB6B3CEE','EA16E721-5E1D-EC11-9953-3863BBBB3AE0') --Permiso 4 y 5
GO

INSERT INTO Client(AppUserId,AutoSync) VALUES
('EA16E721-5E1D-EC11-9953-3863BBBB3AE0',0);  
GO

----------------------------------------------------------------------------------------------------------------------------------------











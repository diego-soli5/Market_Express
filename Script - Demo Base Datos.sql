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
	[Alias] [varchar](10) NULL,
	[Identification] [varchar](12) NOT NULL,
	[Email] [varchar](40) NOT NULL,
	[Phone] [varchar](40) NOT NULL,
	[Password] [varchar](80) NOT NULL,
	[Type] [varchar](15) NOT NULL,
	[Status] [varchar](11) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[AddedBy] [varchar](40) NOT NULL,
	[ModifiedBy] [varchar](40) NULL,
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
	[ModificationDate] [datetime] NULL,
	[AddedBy] [varchar](40) NOT NULL,
	[ModifiedBy] [varchar](40) NULL,
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
	[PerformedBy] [varchar](40) NOT NULL, --Se elimina la relacion FK con la tb AppUser para poder incluir los movimientos de SYSTEM
	[MovementDate] [datetime] NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[Detail] [varchar](600) NOT NULL,
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
	[Description] [varchar](255) NULL,
	[Status] [varchar](11) NOT NULL,
	[Image] [varchar](50) NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[AddedBy] [varchar](40) NOT NULL,
	[ModifiedBy] [varchar](40) NULL,
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
	[Description] [varchar](255) NULL,
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
	[Description] [varchar](255) NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[AddedBy] [varchar](40) NOT NULL,
	[ModifiedBy] [varchar](40) NULL,
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
/****** Object:  Table [dbo].[Slider]    Script Date: 26/09/2021 13:12:40 ******/
CREATE TABLE [dbo].[Slider](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Image] [varchar](50) NOT NULL,
	[Status] [varchar](11) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ModificationDate] [datetime] NULL,
	[AddedBy] [varchar](40) NOT NULL,
	[ModifiedBy] [varchar](40) NULL,
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
ALTER TABLE [dbo].[Slider] ADD  DEFAULT (newsequentialid()) FOR [Id]
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
ALTER TABLE [dbo].[Slider]  WITH CHECK ADD  CONSTRAINT [CHK_Slider_Status] CHECK  (([Status]='ACTIVADO' OR [Status]='DESACTIVADO'))
GO
ALTER TABLE [dbo].[Slider] CHECK CONSTRAINT [CHK_Slider_Status]
GO















----------------------------------------------------------------------------------------------------------------------------------------











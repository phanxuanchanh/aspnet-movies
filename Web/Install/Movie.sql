/****** Object:  Table [dbo].[Cast]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cast](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[description] [ntext] NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_Cast] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CastOfFilm]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CastOfFilm](
	[filmId] [varchar](100) NOT NULL,
	[castId] [bigint] NOT NULL,
	[role] [nvarchar](50) NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_CastOfFilm] PRIMARY KEY CLUSTERED 
(
	[filmId] ASC,
	[castId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[description] [ntext] NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryDistribution]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryDistribution](
	[categoryId] [int] NOT NULL,
	[filmId] [varchar](100) NOT NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_CategoryDistribution] PRIMARY KEY CLUSTERED 
(
	[categoryId] ASC,
	[filmId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[description] [ntext] NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Director]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Director](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[description] [ntext] NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_Director] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DirectorOfFilm]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DirectorOfFilm](
	[filmId] [varchar](100) NOT NULL,
	[directorId] [bigint] NOT NULL,
	[role] [nvarchar](50) NOT NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_DirectorOfFilm] PRIMARY KEY CLUSTERED 
(
	[filmId] ASC,
	[directorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Film]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Film](
	[ID] [varchar](100) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[description] [ntext] NULL,
	[countryId] [int] NOT NULL,
	[productionCompany] [nvarchar](50) NOT NULL,
	[languageId] [int] NOT NULL,
	[releaseDate] [varchar](10) NOT NULL,
	[upvote] [bigint] NULL,
	[downvote] [bigint] NULL,
	[views] [bigint] NULL,
	[duration] [varchar](20) NULL,
	[thumbnail] [varchar](100) NULL,
	[source] [varchar](100) NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_Film] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Language]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Language](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[description] [ntext] NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentInfo]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentInfo](
	[userId] [varchar](100) NOT NULL,
	[paymentMethodId] [int] NOT NULL,
	[cardNumber] [varchar](50) NOT NULL,
	[cvv] [varchar](5) NOT NULL,
	[owner] [varchar](100) NOT NULL,
	[expirationDate] [varchar](30) NOT NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_PaymentInfo] PRIMARY KEY CLUSTERED 
(
	[userId] ASC,
	[paymentMethodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethod]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethod](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](30) NOT NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_PaymentMethod] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[ID] [varchar](100) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tag]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tag](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[description] [ntext] NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TagDistribution]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagDistribution](
	[tagId] [bigint] NOT NULL,
	[filmId] [varchar](100) NOT NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_TagDistribution] PRIMARY KEY CLUSTERED 
(
	[tagId] ASC,
	[filmId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [varchar](100) NOT NULL,
	[userName] [varchar](50) NOT NULL,
	[surName] [nvarchar](50) NULL,
	[middleName] [nvarchar](50) NULL,
	[name] [nvarchar](50) NULL,
	[email] [nvarchar](100) NOT NULL,
	[phoneNumber] [char](11) NULL,
	[password] [varchar](100) NOT NULL,
	[salt] [varchar](100) NOT NULL,
	[description] [ntext] NULL,
	[activated] [bit] NOT NULL,
	[roleId] [varchar](100) NOT NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserReaction]    Script Date: 1/25/2022 1:48:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserReaction](
	[filmId] [varchar](100) NOT NULL,
	[userId] [varchar](100) NOT NULL,
	[upvoted] [bit] NULL,
	[downvoted] [bit] NULL,
	[createAt] [datetime] NULL,
	[updateAt] [datetime] NULL,
 CONSTRAINT [PK_UserReaction] PRIMARY KEY CLUSTERED 
(
	[filmId] ASC,
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[CastOfFilm]  WITH CHECK ADD  CONSTRAINT [FK_CastOfFilm_Cast] FOREIGN KEY([castId])
REFERENCES [dbo].[Cast] ([ID])
GO
ALTER TABLE [dbo].[CastOfFilm] CHECK CONSTRAINT [FK_CastOfFilm_Cast]
GO
ALTER TABLE [dbo].[CastOfFilm]  WITH CHECK ADD  CONSTRAINT [FK_CastOfFilm_Film] FOREIGN KEY([filmId])
REFERENCES [dbo].[Film] ([ID])
GO
ALTER TABLE [dbo].[CastOfFilm] CHECK CONSTRAINT [FK_CastOfFilm_Film]
GO
ALTER TABLE [dbo].[CategoryDistribution]  WITH CHECK ADD  CONSTRAINT [FK_CategoryDistribution_Category] FOREIGN KEY([categoryId])
REFERENCES [dbo].[Category] ([ID])
GO
ALTER TABLE [dbo].[CategoryDistribution] CHECK CONSTRAINT [FK_CategoryDistribution_Category]
GO
ALTER TABLE [dbo].[CategoryDistribution]  WITH CHECK ADD  CONSTRAINT [FK_CategoryDistribution_Film] FOREIGN KEY([filmId])
REFERENCES [dbo].[Film] ([ID])
GO
ALTER TABLE [dbo].[CategoryDistribution] CHECK CONSTRAINT [FK_CategoryDistribution_Film]
GO
ALTER TABLE [dbo].[DirectorOfFilm]  WITH CHECK ADD  CONSTRAINT [FK_DirectorOfFilm_Director] FOREIGN KEY([directorId])
REFERENCES [dbo].[Director] ([ID])
GO
ALTER TABLE [dbo].[DirectorOfFilm] CHECK CONSTRAINT [FK_DirectorOfFilm_Director]
GO
ALTER TABLE [dbo].[DirectorOfFilm]  WITH CHECK ADD  CONSTRAINT [FK_DirectorOfFilm_Film] FOREIGN KEY([filmId])
REFERENCES [dbo].[Film] ([ID])
GO
ALTER TABLE [dbo].[DirectorOfFilm] CHECK CONSTRAINT [FK_DirectorOfFilm_Film]
GO
ALTER TABLE [dbo].[Film]  WITH CHECK ADD  CONSTRAINT [FK_Film_Country] FOREIGN KEY([countryId])
REFERENCES [dbo].[Country] ([ID])
GO
ALTER TABLE [dbo].[Film] CHECK CONSTRAINT [FK_Film_Country]
GO
ALTER TABLE [dbo].[Film]  WITH CHECK ADD  CONSTRAINT [FK_Film_Language] FOREIGN KEY([languageId])
REFERENCES [dbo].[Language] ([ID])
GO
ALTER TABLE [dbo].[Film] CHECK CONSTRAINT [FK_Film_Language]
GO
ALTER TABLE [dbo].[PaymentInfo]  WITH CHECK ADD  CONSTRAINT [FK_PaymentInfo_PaymentMethod] FOREIGN KEY([paymentMethodId])
REFERENCES [dbo].[PaymentMethod] ([ID])
GO
ALTER TABLE [dbo].[PaymentInfo] CHECK CONSTRAINT [FK_PaymentInfo_PaymentMethod]
GO
ALTER TABLE [dbo].[PaymentInfo]  WITH CHECK ADD  CONSTRAINT [FK_PaymentInfo_User] FOREIGN KEY([userId])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[PaymentInfo] CHECK CONSTRAINT [FK_PaymentInfo_User]
GO
ALTER TABLE [dbo].[TagDistribution]  WITH CHECK ADD  CONSTRAINT [FK_TagDistribution_Film] FOREIGN KEY([filmId])
REFERENCES [dbo].[Film] ([ID])
GO
ALTER TABLE [dbo].[TagDistribution] CHECK CONSTRAINT [FK_TagDistribution_Film]
GO
ALTER TABLE [dbo].[TagDistribution]  WITH CHECK ADD  CONSTRAINT [FK_TagDistribution_Tag] FOREIGN KEY([tagId])
REFERENCES [dbo].[Tag] ([ID])
GO
ALTER TABLE [dbo].[TagDistribution] CHECK CONSTRAINT [FK_TagDistribution_Tag]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([roleId])
REFERENCES [dbo].[Role] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[UserReaction]  WITH CHECK ADD  CONSTRAINT [FK_UserReaction_Film] FOREIGN KEY([filmId])
REFERENCES [dbo].[Film] ([ID])
GO
ALTER TABLE [dbo].[UserReaction] CHECK CONSTRAINT [FK_UserReaction_Film]
GO
ALTER TABLE [dbo].[UserReaction]  WITH CHECK ADD  CONSTRAINT [FK_UserReaction_User] FOREIGN KEY([userId])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserReaction] CHECK CONSTRAINT [FK_UserReaction_User]
GO

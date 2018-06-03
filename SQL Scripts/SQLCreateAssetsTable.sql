USE [ae_code_challange]
GO

/****** Object:  Table [dbo].[Assets]    Script Date: 6/2/2018 9:26:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Assets](
	[pk] [int] IDENTITY(1,1) NOT NULL,
	[X] [float] NOT NULL,
	[Y] [float] NOT NULL,
	[Name] [varchar](255) NULL,
	[AccountID] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Asset_pk] PRIMARY KEY CLUSTERED 
(
	[pk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Assets]  WITH CHECK ADD  CONSTRAINT [FK_Assets_Accounts] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Accounts] ([AccountID])
GO

ALTER TABLE [dbo].[Assets] CHECK CONSTRAINT [FK_Assets_Accounts]
GO



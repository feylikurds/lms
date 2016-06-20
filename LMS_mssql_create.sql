CREATE TABLE [LmsUser] (
	Id int NOT NULL,
	Email nvarchar(255) NOT NULL,
	Name nvarchar(255) NOT NULL,
	CourseId int NOT NULL,
	UserType int NOT NULL,
  CONSTRAINT [PK_LMSUSER] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Course] (
	Id int NOT NULL,
	Name nvarchar(255) NOT NULL,
	Description nvarchar(255) NOT NULL,
	StartDate datetime NOT NULL,
	EndDate datetime NOT NULL,
  CONSTRAINT [PK_COURSE] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Module] (
	Id int NOT NULL,
	Name nvarchar(255) NOT NULL,
	Description nvarchar(255) NOT NULL,
	StartDate datetime NOT NULL,
	EndDate datetime NOT NULL,
	CourseId int NOT NULL,
  CONSTRAINT [PK_MODULE] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Activity] (
	Id int NOT NULL,
	Name nvarchar(255) NOT NULL,
	StartDate datetime NOT NULL,
	EndDate datetime NOT NULL,
	Description nvarchar(255) NOT NULL,
	ModuleId int NOT NULL,
  CONSTRAINT [PK_ACTIVITY] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
CREATE TABLE [Document] (
	Id int NOT NULL,
	Name nvarchar(255) NOT NULL,
	Description nvarchar(255) NOT NULL,
	UploadTime datetime NOT NULL,
	UploaderId int NOT NULL,
	Deadline datetime,
	DocumentData binary NOT NULL,
	DocumentType int NOT NULL,
	CourseId int,
	ModuleId int,
	ActivityId int,
  CONSTRAINT [PK_DOCUMENT] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO
ALTER TABLE [LmsUser] WITH CHECK ADD CONSTRAINT [LmsUser_fk0] FOREIGN KEY ([CourseId]) REFERENCES [Course]([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [LmsUser] CHECK CONSTRAINT [LmsUser_fk0]
GO


ALTER TABLE [Module] WITH CHECK ADD CONSTRAINT [Module_fk0] FOREIGN KEY ([CourseId]) REFERENCES [Course]([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [Module] CHECK CONSTRAINT [Module_fk0]
GO

ALTER TABLE [Activity] WITH CHECK ADD CONSTRAINT [Activity_fk0] FOREIGN KEY ([ModuleId]) REFERENCES [Module]([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [Activity] CHECK CONSTRAINT [Activity_fk0]
GO

ALTER TABLE [Document] WITH CHECK ADD CONSTRAINT [Document_fk0] FOREIGN KEY ([UploaderId]) REFERENCES [LmsUser]([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [Document] CHECK CONSTRAINT [Document_fk0]
GO
ALTER TABLE [Document] WITH CHECK ADD CONSTRAINT [Document_fk1] FOREIGN KEY ([CourseId]) REFERENCES [Course]([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [Document] CHECK CONSTRAINT [Document_fk1]
GO
ALTER TABLE [Document] WITH CHECK ADD CONSTRAINT [Document_fk2] FOREIGN KEY ([ModuleId]) REFERENCES [Module]([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [Document] CHECK CONSTRAINT [Document_fk2]
GO
ALTER TABLE [Document] WITH CHECK ADD CONSTRAINT [Document_fk3] FOREIGN KEY ([ActivityId]) REFERENCES [Activity]([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [Document] CHECK CONSTRAINT [Document_fk3]
GO


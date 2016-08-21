
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/15/2016 20:53:03
-- Generated from EDMX file: C:\Users\asachs\Source\Workspaces\ImHereForFree\ImHereNonProfit\ImHereNonProfit.Model\AppModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [C:\Intel\ImHereForFree_FinalDB.mdf];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CommiteeCommitteeActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommitteeActivities] DROP CONSTRAINT [FK_CommiteeCommitteeActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_CommitteeCommitteeReport]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommitteeReports] DROP CONSTRAINT [FK_CommitteeCommitteeReport];
GO
IF OBJECT_ID(N'[dbo].[FK_CommitteeCommitteeDocument]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommitteeDocuments] DROP CONSTRAINT [FK_CommitteeCommitteeDocument];
GO
IF OBJECT_ID(N'[dbo].[FK_ExpensForCommittee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommitteeExpenses] DROP CONSTRAINT [FK_ExpensForCommittee];
GO
IF OBJECT_ID(N'[dbo].[FK_BasicUserDonation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Donations] DROP CONSTRAINT [FK_BasicUserDonation];
GO
IF OBJECT_ID(N'[dbo].[FK_MemberCommittee_Member]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MemberCommittee] DROP CONSTRAINT [FK_MemberCommittee_Member];
GO
IF OBJECT_ID(N'[dbo].[FK_MemberCommittee_Committee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MemberCommittee] DROP CONSTRAINT [FK_MemberCommittee_Committee];
GO
IF OBJECT_ID(N'[dbo].[FK_MemberUser_inherits_BasicUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BasicUsers_MemberUser] DROP CONSTRAINT [FK_MemberUser_inherits_BasicUser];
GO
IF OBJECT_ID(N'[dbo].[FK_ExecutiveUser_inherits_MemberUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BasicUsers_ExecutiveUser] DROP CONSTRAINT [FK_ExecutiveUser_inherits_MemberUser];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Committees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Committees];
GO
IF OBJECT_ID(N'[dbo].[Donations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Donations];
GO
IF OBJECT_ID(N'[dbo].[CommitteeActivities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommitteeActivities];
GO
IF OBJECT_ID(N'[dbo].[CommitteeReports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommitteeReports];
GO
IF OBJECT_ID(N'[dbo].[CommitteeDocuments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommitteeDocuments];
GO
IF OBJECT_ID(N'[dbo].[CommitteeExpenses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommitteeExpenses];
GO
IF OBJECT_ID(N'[dbo].[BasicUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BasicUsers];
GO
IF OBJECT_ID(N'[dbo].[BasicUsers_MemberUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BasicUsers_MemberUser];
GO
IF OBJECT_ID(N'[dbo].[BasicUsers_ExecutiveUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BasicUsers_ExecutiveUser];
GO
IF OBJECT_ID(N'[dbo].[MemberCommittee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MemberCommittee];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Committees'
CREATE TABLE [dbo].[Committees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CurrentYearBudget] int  NULL,
    [ChairId] int  NOT NULL
);
GO

-- Creating table 'Donations'
CREATE TABLE [dbo].[Donations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BasicUserId] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [Amount] int  NOT NULL,
    [Currency] nvarchar(max)  NOT NULL,
    [DonorId] int  NOT NULL
);
GO

-- Creating table 'CommitteeActivities'
CREATE TABLE [dbo].[CommitteeActivities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CommiteeId] int  NOT NULL,
    [ActivityDate] datetime  NOT NULL,
    [ReportedDate] datetime  NOT NULL,
    [Subject] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Tags] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CommitteeReports'
CREATE TABLE [dbo].[CommitteeReports] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CommitteeId] int  NOT NULL
);
GO

-- Creating table 'CommitteeDocuments'
CREATE TABLE [dbo].[CommitteeDocuments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CommitteeId] int  NOT NULL,
    [Tags] nvarchar(max)  NOT NULL,
    [LastModified] datetime  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CommitteeExpenses'
CREATE TABLE [dbo].[CommitteeExpenses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] int  NOT NULL,
    [Currency] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [CommitteeId] int  NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'BasicUsers'
CREATE TABLE [dbo].[BasicUsers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserType] int  NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Phone] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [Zipcode] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'BasicUsers_MemberUser'
CREATE TABLE [dbo].[BasicUsers_MemberUser] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'BasicUsers_ExecutiveUser'
CREATE TABLE [dbo].[BasicUsers_ExecutiveUser] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'MemberCommittee'
CREATE TABLE [dbo].[MemberCommittee] (
    [Members_Id] int  NOT NULL,
    [MemberOf_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Committees'
ALTER TABLE [dbo].[Committees]
ADD CONSTRAINT [PK_Committees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [BasicUserId] in table 'Donations'
ALTER TABLE [dbo].[Donations]
ADD CONSTRAINT [PK_Donations]
    PRIMARY KEY CLUSTERED ([Id], [BasicUserId] ASC);
GO

-- Creating primary key on [Id], [CommiteeId] in table 'CommitteeActivities'
ALTER TABLE [dbo].[CommitteeActivities]
ADD CONSTRAINT [PK_CommitteeActivities]
    PRIMARY KEY CLUSTERED ([Id], [CommiteeId] ASC);
GO

-- Creating primary key on [Id] in table 'CommitteeReports'
ALTER TABLE [dbo].[CommitteeReports]
ADD CONSTRAINT [PK_CommitteeReports]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CommitteeDocuments'
ALTER TABLE [dbo].[CommitteeDocuments]
ADD CONSTRAINT [PK_CommitteeDocuments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CommitteeExpenses'
ALTER TABLE [dbo].[CommitteeExpenses]
ADD CONSTRAINT [PK_CommitteeExpenses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BasicUsers'
ALTER TABLE [dbo].[BasicUsers]
ADD CONSTRAINT [PK_BasicUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BasicUsers_MemberUser'
ALTER TABLE [dbo].[BasicUsers_MemberUser]
ADD CONSTRAINT [PK_BasicUsers_MemberUser]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BasicUsers_ExecutiveUser'
ALTER TABLE [dbo].[BasicUsers_ExecutiveUser]
ADD CONSTRAINT [PK_BasicUsers_ExecutiveUser]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Members_Id], [MemberOf_Id] in table 'MemberCommittee'
ALTER TABLE [dbo].[MemberCommittee]
ADD CONSTRAINT [PK_MemberCommittee]
    PRIMARY KEY CLUSTERED ([Members_Id], [MemberOf_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CommiteeId] in table 'CommitteeActivities'
ALTER TABLE [dbo].[CommitteeActivities]
ADD CONSTRAINT [FK_CommiteeCommitteeActivity]
    FOREIGN KEY ([CommiteeId])
    REFERENCES [dbo].[Committees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommiteeCommitteeActivity'
CREATE INDEX [IX_FK_CommiteeCommitteeActivity]
ON [dbo].[CommitteeActivities]
    ([CommiteeId]);
GO

-- Creating foreign key on [CommitteeId] in table 'CommitteeReports'
ALTER TABLE [dbo].[CommitteeReports]
ADD CONSTRAINT [FK_CommitteeCommitteeReport]
    FOREIGN KEY ([CommitteeId])
    REFERENCES [dbo].[Committees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommitteeCommitteeReport'
CREATE INDEX [IX_FK_CommitteeCommitteeReport]
ON [dbo].[CommitteeReports]
    ([CommitteeId]);
GO

-- Creating foreign key on [CommitteeId] in table 'CommitteeDocuments'
ALTER TABLE [dbo].[CommitteeDocuments]
ADD CONSTRAINT [FK_CommitteeCommitteeDocument]
    FOREIGN KEY ([CommitteeId])
    REFERENCES [dbo].[Committees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommitteeCommitteeDocument'
CREATE INDEX [IX_FK_CommitteeCommitteeDocument]
ON [dbo].[CommitteeDocuments]
    ([CommitteeId]);
GO

-- Creating foreign key on [CommitteeId] in table 'CommitteeExpenses'
ALTER TABLE [dbo].[CommitteeExpenses]
ADD CONSTRAINT [FK_ExpensForCommittee]
    FOREIGN KEY ([CommitteeId])
    REFERENCES [dbo].[Committees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExpensForCommittee'
CREATE INDEX [IX_FK_ExpensForCommittee]
ON [dbo].[CommitteeExpenses]
    ([CommitteeId]);
GO

-- Creating foreign key on [BasicUserId] in table 'Donations'
ALTER TABLE [dbo].[Donations]
ADD CONSTRAINT [FK_BasicUserDonation]
    FOREIGN KEY ([BasicUserId])
    REFERENCES [dbo].[BasicUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BasicUserDonation'
CREATE INDEX [IX_FK_BasicUserDonation]
ON [dbo].[Donations]
    ([BasicUserId]);
GO

-- Creating foreign key on [Members_Id] in table 'MemberCommittee'
ALTER TABLE [dbo].[MemberCommittee]
ADD CONSTRAINT [FK_MemberCommittee_Member]
    FOREIGN KEY ([Members_Id])
    REFERENCES [dbo].[BasicUsers_MemberUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [MemberOf_Id] in table 'MemberCommittee'
ALTER TABLE [dbo].[MemberCommittee]
ADD CONSTRAINT [FK_MemberCommittee_Committee]
    FOREIGN KEY ([MemberOf_Id])
    REFERENCES [dbo].[Committees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MemberCommittee_Committee'
CREATE INDEX [IX_FK_MemberCommittee_Committee]
ON [dbo].[MemberCommittee]
    ([MemberOf_Id]);
GO

-- Creating foreign key on [ChairId] in table 'Committees'
ALTER TABLE [dbo].[Committees]
ADD CONSTRAINT [FK_CommitteeChair]
    FOREIGN KEY ([ChairId])
    REFERENCES [dbo].[BasicUsers_ExecutiveUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommitteeChair'
CREATE INDEX [IX_FK_CommitteeChair]
ON [dbo].[Committees]
    ([ChairId]);
GO

-- Creating foreign key on [Id] in table 'BasicUsers_MemberUser'
ALTER TABLE [dbo].[BasicUsers_MemberUser]
ADD CONSTRAINT [FK_MemberUser_inherits_BasicUser]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[BasicUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'BasicUsers_ExecutiveUser'
ALTER TABLE [dbo].[BasicUsers_ExecutiveUser]
ADD CONSTRAINT [FK_ExecutiveUser_inherits_MemberUser]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[BasicUsers_MemberUser]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
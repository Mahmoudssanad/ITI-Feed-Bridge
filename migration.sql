IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [ImgUrl] nvarchar(max) NULL,
    [BirthDate] date NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Carts] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Carts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Carts_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Donations] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [ImgURL] nvarchar(max) NULL,
    [ExpirDate] date NOT NULL,
    [Quantity] decimal(18,2) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [Phone] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Donations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Donations_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
);

CREATE TABLE [Notifications] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [RedirectUrl] nvarchar(max) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Notifications_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
);

CREATE TABLE [Parteners] (
    [Id] int NOT NULL IDENTITY,
    [ImageUrl] nvarchar(max) NOT NULL,
    [RedirectUrl] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Parteners] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Parteners_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
);

CREATE TABLE [Reviews] (
    [Id] int NOT NULL IDENTITY,
    [FeedBackMsg] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [StarsNumber] int NOT NULL,
    [UserID] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Reviews] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Reviews_AspNetUsers_UserID] FOREIGN KEY ([UserID]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [StaticPages] (
    [Id] int NOT NULL IDENTITY,
    [Content1] nvarchar(max) NOT NULL,
    [Content2] nvarchar(max) NOT NULL,
    [VideoUrl] nvarchar(max) NOT NULL,
    [PartenerBackgroundImageUrl] nvarchar(max) NOT NULL,
    [HomePageBackgroundImageUrl] nvarchar(max) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_StaticPages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_StaticPages_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
);

CREATE TABLE [Supports] (
    [Id] int NOT NULL IDENTITY,
    [Amount] decimal(18,2) NOT NULL,
    [PaymentDate] datetime2 NOT NULL,
    [PaymentMethod] nvarchar(max) NOT NULL,
    [TransactionId] nvarchar(max) NOT NULL,
    [Status] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Supports] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Supports_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
);

CREATE TABLE [Orders] (
    [Id] int NOT NULL IDENTITY,
    [Quantity] decimal(18,2) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [CartId] int NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Orders_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_Orders_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [Carts] ([Id])
);

CREATE TABLE [Products] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [ImgURL] nvarchar(max) NULL,
    [Quantity] decimal(18,2) NOT NULL,
    [ExpirDate] date NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [DonationId] int NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_Donations_DonationId] FOREIGN KEY ([DonationId]) REFERENCES [Donations] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [OrderProducts] (
    [OrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Quantity] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_OrderProducts] PRIMARY KEY ([OrderId], [ProductId]),
    CONSTRAINT [FK_OrderProducts_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderProducts_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ProductCarts] (
    [Id] int NOT NULL IDENTITY,
    [Quantity] decimal(18,2) NOT NULL,
    [CartId] int NOT NULL,
    [ProductId] int NOT NULL,
    CONSTRAINT [PK_ProductCarts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductCarts_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [Carts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProductCarts_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

CREATE UNIQUE INDEX [IX_Carts_UserId] ON [Carts] ([UserId]);

CREATE INDEX [IX_Donations_UserId] ON [Donations] ([UserId]);

CREATE INDEX [IX_Notifications_UserId] ON [Notifications] ([UserId]);

CREATE INDEX [IX_OrderProducts_ProductId] ON [OrderProducts] ([ProductId]);

CREATE INDEX [IX_Orders_CartId] ON [Orders] ([CartId]);

CREATE INDEX [IX_Orders_UserId] ON [Orders] ([UserId]);

CREATE INDEX [IX_Parteners_UserId] ON [Parteners] ([UserId]);

CREATE INDEX [IX_ProductCarts_CartId] ON [ProductCarts] ([CartId]);

CREATE INDEX [IX_ProductCarts_ProductId] ON [ProductCarts] ([ProductId]);

CREATE INDEX [IX_Products_DonationId] ON [Products] ([DonationId]);

CREATE INDEX [IX_Reviews_UserID] ON [Reviews] ([UserID]);

CREATE UNIQUE INDEX [IX_StaticPages_UserId] ON [StaticPages] ([UserId]);

CREATE INDEX [IX_Supports_UserId] ON [Supports] ([UserId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250905220009_New database', N'9.0.8');

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] ON;
INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
VALUES (N'1', NULL, N'Admin', N'ADMIN'),
(N'2', NULL, N'Delivery', N'DELIVERY'),
(N'3', NULL, N'User', N'USER');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] OFF;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250906083703_AddRolesAndSeed', N'9.0.8');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250906085529_UpdateSeedOrModels', N'9.0.8');

ALTER TABLE [Parteners] ADD [Name] nvarchar(max) NOT NULL DEFAULT N'';

CREATE TABLE [Reports] (
    [Id] int NOT NULL IDENTITY,
    [title] nvarchar(max) NOT NULL,
    [content] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Reports] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Reports_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Reports_UserId] ON [Reports] ([UserId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250906202750_PartenerNameAttributeAdded', N'9.0.8');

ALTER TABLE [StaticPages] DROP CONSTRAINT [FK_StaticPages_AspNetUsers_UserId];

DROP INDEX [IX_StaticPages_UserId] ON [StaticPages];

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[StaticPages]') AND [c].[name] = N'VideoUrl');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [StaticPages] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [StaticPages] ALTER COLUMN [VideoUrl] nvarchar(max) NULL;

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[StaticPages]') AND [c].[name] = N'UserId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [StaticPages] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [StaticPages] ALTER COLUMN [UserId] nvarchar(450) NULL;

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[StaticPages]') AND [c].[name] = N'PartenerBackgroundImageUrl');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [StaticPages] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [StaticPages] ALTER COLUMN [PartenerBackgroundImageUrl] nvarchar(max) NULL;

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[StaticPages]') AND [c].[name] = N'HomePageBackgroundImageUrl');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [StaticPages] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [StaticPages] ALTER COLUMN [HomePageBackgroundImageUrl] nvarchar(max) NULL;

CREATE UNIQUE INDEX [IX_StaticPages_UserId] ON [StaticPages] ([UserId]) WHERE [UserId] IS NOT NULL;

ALTER TABLE [StaticPages] ADD CONSTRAINT [FK_StaticPages_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250907012014_StaticPageNullablePropsAltered', N'9.0.8');

ALTER TABLE [Notifications] ADD [CreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Notifications] ADD [IsRead] bit NOT NULL DEFAULT CAST(0 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250907081842_NotificationNewPropertiesAdded', N'9.0.8');

ALTER TABLE [AspNetUsers] ADD [IsFrozen] bit NOT NULL DEFAULT CAST(0 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250907105707_Add IsFrozen attribute', N'9.0.8');

COMMIT;
GO


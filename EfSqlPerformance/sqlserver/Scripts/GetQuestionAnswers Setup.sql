DROP INDEX [IX_Posts_LastActivityDate_ParentId_INC_OwnerUserId_Title] ON [dbo].[Posts];
GO

CREATE NONCLUSTERED INDEX [IX_Posts_LastActivityDate_ParentId_INC_OwnerUserId_Title] ON [dbo].[Posts]
(
    [ParentId] ASC
);
GO

DROP INDEX IF EXISTS [IX_Posts_LastActivityDate_ParentId_INC_OwnerUserId_Title] ON [dbo].[Posts];
GO

CREATE NONCLUSTERED INDEX [IX_Posts_LastActivityDate_ParentId_INC_OwnerUserId_Title] ON [dbo].[Posts]
(
    [LastActivityDate] ASC,
    [ParentId] ASC
)
INCLUDE([OwnerUserId], [Title])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, 
      ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY];
GO

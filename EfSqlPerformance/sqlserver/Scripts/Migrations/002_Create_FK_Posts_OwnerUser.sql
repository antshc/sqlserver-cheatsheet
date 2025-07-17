UPDATE dbo.Posts
SET OwnerUserId = NULL
WHERE OwnerUserId IS NOT NULL
  AND OwnerUserId NOT IN (SELECT Id FROM dbo.Users);

ALTER TABLE dbo.Posts
    ADD CONSTRAINT FK_Posts_OwnerUser
        FOREIGN KEY (OwnerUserId) REFERENCES dbo.Users(Id)
            ON DELETE SET NULL;
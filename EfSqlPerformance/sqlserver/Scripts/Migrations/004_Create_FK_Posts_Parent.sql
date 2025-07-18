UPDATE dbo.Posts
SET ParentId = null
WHERE ParentId NOT IN (SELECT Id FROM dbo.Posts);

ALTER TABLE dbo.Posts
    ADD CONSTRAINT FK_Posts_Parent
        FOREIGN KEY (ParentId) REFERENCES dbo.Posts(Id)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION;
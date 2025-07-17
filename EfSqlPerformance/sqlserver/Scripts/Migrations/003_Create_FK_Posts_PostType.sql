ALTER TABLE dbo.Posts
    ADD CONSTRAINT FK_Posts_PostType
        FOREIGN KEY (PostTypeId) REFERENCES dbo.PostTypes (Id)
            ON DELETE NO ACTION;
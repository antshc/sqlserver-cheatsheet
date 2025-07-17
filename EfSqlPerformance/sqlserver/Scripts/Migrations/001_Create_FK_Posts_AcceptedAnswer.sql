UPDATE dbo.Posts
SET AcceptedAnswerId = NULL
WHERE AcceptedAnswerId = 0;

UPDATE dbo.Posts
SET AcceptedAnswerId = NULL
WHERE AcceptedAnswerId IS NOT NULL
  AND AcceptedAnswerId NOT IN (SELECT Id FROM dbo.Posts);

ALTER TABLE dbo.Posts
    ADD CONSTRAINT FK_Posts_AcceptedAnswer
        FOREIGN KEY (AcceptedAnswerId) REFERENCES dbo.Posts(Id)
            ON DELETE NO ACTION;
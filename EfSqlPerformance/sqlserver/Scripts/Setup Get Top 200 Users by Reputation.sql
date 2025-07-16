DROP INDEX IF EXISTS IX_Users_Reputation_Includes ON dbo.Users;
GO

CREATE NONCLUSTERED INDEX IX_Users_Reputation_Includes
ON dbo.Users (Reputation DESC)
INCLUDE (DisplayName, Id);
GO

-- Drop function if it exists
IF OBJECT_ID('dbo.GenerateAboutMeChunk', 'FN') IS NOT NULL
DROP FUNCTION dbo.GenerateAboutMeChunk;
GO

-- Create function
CREATE FUNCTION dbo.GenerateAboutMeChunk (
    @RepeatCount INT,
    @Seed UNIQUEIDENTIFIER
)
    RETURNS NVARCHAR(MAX)
                    AS
BEGIN
    DECLARE @Result NVARCHAR(MAX);

    SET @Result = REPLICATE(
        ISNULL(
            CHOOSE(
                ABS(CHECKSUM(@Seed)) % 20 + 1,
                '<p>I am software developer primarily working with Microsoft technologies and you can find me on <a href="http://twitter.com/johnkilmister" rel="nofollow noreferrer">Twitter</a></p>',
                '<p>#SOreadytohelp</p>',
                'Check out my <a href="http://tinyurl.com/5jaae2" rel="nofollow">free ebook</a>',
                '<p>Solution Architect on the Parature team at Microsoft based in Washington, DC.<br/><br/>Coder for 25 years, .Net developer since 2001.<br/><br/>Twitter: @justcallme98<br/><br/>Xbox Live Gamertag: ExitNinetyEight</p>',
                '<p>Full stack developer, living in NC, working remotely for a social media analytics company headquartered in Bethesda, MD.<br/>Most proficient with WISC (<strong>W</strong>indows, <strong>I</strong>IS, <strong>S</strong>QL Server, <strong>C</strong>Sharp) stack, dabble with the rest.</p>',
                '<p><a href="http://www.patrickhyatt.com" rel="nofollow noreferrer">http://www.patrickhyatt.com</a></p>',
                '<p>I&#39;m a professional developer that used to work in PHP every day. These days I work primarily with JavaScript-based technologies and Front-End software.</p>',
                '<p>I&#39;m currently studying computer engineering at the University of Michigan. My interests include: machine learning, data mining, hardware design, investing, and music.</p>',
                '<p>Livin&#39; and breathin&#39; .NET!</p>',
                '<p>Love clean code and great UX. Passionate about mentoring junior devs and scaling systems.</p>',
                '<p>Senior backend engineer with a focus on distributed systems and microservices architecture. Blogging at <a href="https://techwithme.dev" rel="nofollow noreferrer">techwithme.dev</a></p>',
                '<p>Frontend enthusiast, React/Angular wizard. Also love CSS animations 💫.</p>',
                '<p>Always exploring AI and ML trends. Open-source contributor at <a href="https://github.com/opensource-dev" rel="nofollow noreferrer">GitHub</a></p>',
                '<p>Father, musician, and Python developer. Sharing thoughts on <a href="https://dev.to/janedoe" rel="nofollow noreferrer">Dev.to</a></p>',
                '<p>Currently working on Kubernetes deployments for fintech apps. Love solving scalability issues 🔥</p>',
                '<p>Enjoy creating APIs that developers love. Advocate for RESTful design and GraphQL where needed.</p>',
                '<p>Cloud engineer (AWS/Azure). Helping companies with their cloud migration journeys.</p>',
                '<p>JavaScript fanboy and occasional Node.js trainer. Also host of the <strong>DevTalks Podcast</strong>.</p>',
                '<p>I build mobile apps with Flutter and Swift. Designer at heart ❤️</p>',
                '<p>Hiker, photographer, and developer. My portfolio: <a href="https://johnsmith.dev" rel="nofollow noreferrer">johnsmith.dev</a></p>'
            ),
            '<p>Currently working on Kubernetes deployments for fintech apps. Love solving scalability issues</p>'
        ) + CONVERT(NVARCHAR(36), @Seed), -- Append GUID for uniqueness
        @RepeatCount
    );

RETURN @Result;
END;
GO

-- Use function to update AboutMe
WITH TopUsers AS (
    SELECT TOP (200) Id, AboutMe
    FROM Users
    ORDER BY Reputation DESC
)
UPDATE TopUsers
SET AboutMe = REPLICATE(dbo.GenerateAboutMeChunk(1024 * 1024, NEWID()), 10);
GO


WITH TopUsers AS (
    SELECT TOP (200) Id, AboutMe, Reputation
    FROM Users
    ORDER BY Reputation DESC
)SELECT
    MAX(DATALENGTH(AboutMe)) AS MaxSizeBytes,
    MAX(LEN(AboutMe)) AS MaxLengthChars
FROM TopUsers
WHERE AboutMe IS NOT NULL;
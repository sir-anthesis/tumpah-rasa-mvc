-- CREATINGG TABLES
-- Create tb_admin table
CREATE TABLE tb_admin (
    id_admin INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(255),
    email VARCHAR(255),
    password VARCHAR(255)
);

-- Create tb_recipe table
CREATE TABLE tb_recipe (
    id_recipe INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(255),
    thumbnail VARCHAR(255),
    description TEXT,
    created_at DATETIME,
    id_admin INT FOREIGN KEY REFERENCES tb_admin(id_admin),
    rating FLOAT
);

-- Create tb_member table
CREATE TABLE tb_member (
    id_member INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(255),
    email VARCHAR(255),
    password VARCHAR(255)
);

-- Create tb_comment table
CREATE TABLE tb_comment (
    id_comment INT PRIMARY KEY IDENTITY(1,1),
    id_member INT FOREIGN KEY REFERENCES tb_member(id_member),
    id_recipe INT FOREIGN KEY REFERENCES tb_recipe(id_recipe),
    comment TEXT,
    rating INT,
    created_at DATETIME
);

-- Create tb_loved table
CREATE TABLE tb_loved (
    id_member INT FOREIGN KEY REFERENCES tb_member(id_member),
    id_recipe INT FOREIGN KEY REFERENCES tb_recipe(id_recipe),
    loved_at DATETIME,
    PRIMARY KEY (id_member, id_recipe)
);


--INSERTING VALUES
--Insert tb_admin
INSERT INTO tb_admin values ('admin1', 'admin1@gmail.com', 'admin1');

--Insert tb_member
INSERT INTO tb_member values ('akun1', 'akun1@gmail.com', 'akun1');


--TRIGGERS
--Create trigger UpdateRecipeRating
CREATE TRIGGER UpdateRecipeRating
ON tb_comment
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @RecipeID INT,
            @Rating FLOAT,
            @AvgRating FLOAT;

    -- Get the newly inserted values
    SELECT @RecipeID = id_recipe, @Rating = rating
    FROM inserted;

    -- Check if it's the first comment for the recipe
    IF NOT EXISTS (SELECT 1 FROM tb_comment WHERE id_recipe = @RecipeID AND id_comment <> (SELECT MAX(id_comment) FROM tb_comment WHERE id_recipe = @RecipeID))
    BEGIN
        -- Update tb_recipe with the rating from tb_comment
        UPDATE tb_recipe
        SET rating = @Rating
        WHERE id_recipe = @RecipeID;
    END
    ELSE
    BEGIN
        -- Calculate the average rating for the recipe and round it to one decimal place
        SELECT @AvgRating = ROUND(AVG(CAST(rating AS FLOAT)), 1)  -- Ensure floating-point division
        FROM tb_comment
        WHERE id_recipe = @RecipeID;

        -- Update tb_recipe with the average rating
        UPDATE tb_recipe
        SET rating = @AvgRating
        WHERE id_recipe = @RecipeID;
    END
END;

--Create trigger before insert tb_loved
CREATE TRIGGER trg_insert_tb_loved
ON tb_loved
INSTEAD OF INSERT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE EXISTS (
            SELECT 1
            FROM tb_loved l
            WHERE l.id_member = i.id_member
            AND l.id_recipe = i.id_recipe
        )
    )
    BEGIN
        RAISERROR ('You already loved this recipe', 16, 1);
    END
    ELSE
    BEGIN
        INSERT INTO tb_loved (id_member, id_recipe, loved_at)
        SELECT id_member, id_recipe, loved_at
        FROM inserted;
    END
END;


-- Add cascade on delete
ALTER TABLE tb_comment
ADD CONSTRAINT FK_tb_comment_tb_recipe FOREIGN KEY (id_recipe)
REFERENCES tb_recipe(id_recipe)
ON DELETE CASCADE;

ALTER TABLE tb_loved
ADD CONSTRAINT FK_tb_loved_tb_recipe FOREIGN KEY (id_recipe)
REFERENCES tb_recipe(id_recipe)
ON DELETE CASCADE;





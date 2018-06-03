use ae_code_challange
-- Queries

-- Instructions:
-- 1.
-- Create a query to find accounts that have more than 5 assets.
-- 2. 
-- Create a query to get X, Y coordinates and asset name for an account ID. 



-- Query 1. Find accounts that have more than 5 assets.
SELECT
    Accounts.AccountID, count(Assets.AccountID) as AccountIDCount
FROM
    accounts
        JOIN
        assets ON accounts.AccountID = Assets.AccountID
GROUP BY Accounts.AccountID
having count(Assets.AccountID) >= 5


-- Query 2. Get X, Y coordinates and asset name for an account ID. 
declare @AccountID varchar(255)
-- Please enter accountID to search for as the text of @AccountID below, replace 'ABC21' which is just an example. 
-- In a production product, this would be a stored proc that would take the accountID as a parameter.
select @AccountID = 'ABC21'  

select Assets.X, Assets.Y, Assets.Name AssetName, Assets.AccountID from accounts  
join assets on Accounts.AccountID = Assets.AccountID
where Accounts.AccountID = @AccountID

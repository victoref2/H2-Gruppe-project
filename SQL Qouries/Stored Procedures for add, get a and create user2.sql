-- AddVehicle procedure
DROP PROCEDURE IF EXISTS AddVehicle;
GO
CREATE PROCEDURE AddVehicle
    @Name VARCHAR(100),
    @KM VARCHAR(20),
    @RegistrationNumber VARCHAR(20),
    @AgeGroup VARCHAR(20),
    @TowHook BIT,
    @DriversLicenceClass VARCHAR(5),
    @EngineSize VARCHAR(10), 
    @KmL DECIMAL(10, 2),
    @FuelType VARCHAR(20),
    @EnergyClass VARCHAR(10)
AS
BEGIN
    BEGIN TRY
        INSERT INTO Vehicles (Name, KM, RegistrationNumber, AgeGroup, TowHook, DriversLicenceClass, EngineSize, KmL, FuelType, EnergyClass)
        VALUES (@Name, @KM, @RegistrationNumber, @AgeGroup, @TowHook, @DriversLicenceClass, @EngineSize, @KmL, @FuelType, @EnergyClass);

        SELECT SCOPE_IDENTITY() AS NewVehicleId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- AddHeavyVehicle procedure
DROP PROCEDURE IF EXISTS AddHeavyVehicle;
GO
CREATE PROCEDURE AddHeavyVehicle
    @VehicleId INT,
    @MaxLoadCapacity INT,
    @NumberOfAxles INT
AS
BEGIN
    BEGIN TRY
        INSERT INTO HeavyVehicles (VehicleId, MaxLoadCapacity, NumberOfAxles)
        VALUES (@VehicleId, @MaxLoadCapacity, @NumberOfAxles);

        SELECT SCOPE_IDENTITY() AS NewHeavyVehicleId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- AddBus procedure
DROP PROCEDURE IF EXISTS AddBus;
GO
CREATE PROCEDURE AddBus
    @HeavyVehicleId INT,
    @Height DECIMAL(10, 2),
    @Weight DECIMAL(10, 2),
    @Length DECIMAL(10, 2),
    @NumberOfSeats INT,
    @NumberOfSleepingPlaces INT,
    @HasToilet BIT
AS
BEGIN
    BEGIN TRY
        INSERT INTO Buses (HeavyVehicleId, Height, Weight, Length, NumberOfSeats, NumberOfSleepingPlaces, HasToilet)
        VALUES (@HeavyVehicleId, @Height, @Weight, @Length, @NumberOfSeats, @NumberOfSleepingPlaces, @HasToilet);

        SELECT SCOPE_IDENTITY() AS NewBusId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- AddTruck procedure
DROP PROCEDURE IF EXISTS AddTruck;
GO
CREATE PROCEDURE AddTruck
	@HeavyVehicleId INT,
	@Height DECIMAL(10, 2),
	@Weight DECIMAL(10, 2),
	@Length DECIMAL(10, 2),
	@LoadCapacity DECIMAL(10, 2)
AS
BEGIN
	BEGIN TRY
	INSERT INTO Trucks(HeavyVehicleId, Height, Weight, Length, LoadCapacity)
	VALUES(@HeavyVehicleId, @Height, @Weight, @Length, @LoadCapacity)

	SELECT SCOPE_IDENTITY() AS NewTruckId;
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH;
END;
GO

-- AddPrivateVehicles procedure
DROP PROCEDURE IF EXISTS AddPrivateVehicles;
GO
CREATE PROCEDURE AddPrivateVehicles
	@NormalVehiclesId INT,
	@IsofixMount BIT

AS
BEGIN
	BEGIN TRY
	INSERT INTO PrivateVehicles(NormalVehicleId,IsofixMount)
	VALUES (@NormalVehiclesId, @IsofixMount)

	SELECT SCOPE_IDENTITY() AS NewPrivateVehicleId;
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH;
END;
GO

-- AddCommercialVehicles procedure
DROP PROCEDURE IF EXISTS AddCommercialVehicles;
GO
CREATE PROCEDURE AddCommercialVehicles
	@NormalVehicleId INT,
	@RollCage BIT,
	@LoadCapacity INT

AS
BEGIN
	BEGIN TRY
	INSERT INTO CommercialVehicles(NormalVehicleId, RollCage, LoadCapacity)
	VALUES (@NormalVehicleId, @RollCage, @LoadCapacity)

	SELECT SCOPE_IDENTITY() AS NewCommercialVehicleId;
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH;
END;
GO

DROP PROCEDURE IF EXISTS GetABus;
GO

CREATE PROCEDURE GetABus
    @BusId INT
AS
BEGIN
    BEGIN TRY
        SELECT B.BusId, 
               B.Height, 
               B.Weight, 
               B.Length, 
               B.NumberOfSeats, 
               B.NumberOfSleepingPlaces, 
               B.HasToilet, 
               HV.MaxLoadCapacity, 
               HV.NumberOfAxles,
               V.VehicleId, 
               V.Name AS VehicleName, 
               V.KM, 
               V.RegistrationNumber, 
               V.AgeGroup, 
               V.TowHook, 
               V.DriversLicenceClass, 
               V.EngineSize, 
               V.KmL, 
               V.FuelType, 
               V.EnergyClass
        FROM Buses B
        JOIN HeavyVehicles HV ON B.HeavyVehicleId = HV.HeavyVehicleId
        JOIN Vehicles V ON HV.VehicleId = V.VehicleId
        WHERE B.BusId = @BusId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO


-- GetATruck procedure
DROP PROCEDURE IF EXISTS GetATruck;
GO
CREATE PROCEDURE GetATruck
    @TruckId INT
AS
BEGIN
    BEGIN TRY
        SELECT T.TruckId, 
               T.Height, 
               T.Weight, 
               T.Length, 
               T.LoadCapacity, 
               HV.MaxLoadCapacity, 
               HV.NumberOfAxles, 
               V.VehicleId, 
               V.Name AS VehicleName, 
               V.KM, 
               V.RegistrationNumber, 
               V.AgeGroup, 
               V.TowHook, 
               V.DriversLicenceClass, 
               V.EngineSize, 
               V.KmL, 
               V.FuelType, 
               V.EnergyClass
        FROM Trucks T
        JOIN HeavyVehicles HV ON T.HeavyVehicleId = HV.HeavyVehicleId
        JOIN Vehicles V ON HV.VehicleId = V.VehicleId
        WHERE T.TruckId = @TruckId;
    END TRY
    BEGIN CATCH
        THROW; 
    END CATCH;
END;
GO

-- GetACommercialVehicle procedure
DROP PROCEDURE IF EXISTS GetACommercialVehicle;
GO
CREATE PROCEDURE GetACommercialVehicle
    @CommercialVehicleId INT
AS
BEGIN
    BEGIN TRY
        SELECT CV.CommercialVehicleId, 
               CV.RollCage, 
               CV.LoadCapacity, 
               NV.NormalVehicleId, 
               NV.NumberOfSeats, 
               NV.TrunkDimensions, 
               NV.IsCommercial, 
               V.VehicleId, 
               V.Name AS VehicleName, 
               V.KM, 
               V.RegistrationNumber, 
               V.AgeGroup, 
               V.TowHook, 
               V.DriversLicenceClass, 
               V.EngineSize, 
               V.KmL, 
               V.FuelType, 
               V.EnergyClass
        FROM CommercialVehicles CV
        JOIN NormalVehicles NV ON CV.NormalVehicleId = NV.NormalVehicleId
        JOIN Vehicles V ON NV.VehicleId = V.VehicleId
        WHERE CV.CommercialVehicleId = @CommercialVehicleId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- GetAPrivateVehicle procedure
DROP PROCEDURE IF EXISTS GetAPrivateVehicle;
GO
CREATE PROCEDURE GetAPrivateVehicle
    @PrivateVehicleId INT
AS
BEGIN
    BEGIN TRY
        SELECT PV.PrivateVehicleId, 
               PV.IsofixMount, 
               NV.NormalVehicleId, 
               NV.NumberOfSeats, 
               NV.TrunkDimensions, 
               NV.IsCommercial, 
               V.VehicleId, 
               V.Name AS VehicleName, 
               V.KM, 
               V.RegistrationNumber, 
               V.AgeGroup, 
               V.TowHook, 
               V.DriversLicenceClass, 
               V.EngineSize, 
               V.KmL, 
               V.FuelType, 
               V.EnergyClass
        FROM PrivateVehicles PV
        JOIN NormalVehicles NV ON PV.NormalVehicleId = NV.NormalVehicleId
        JOIN Vehicles V ON NV.VehicleId = V.VehicleId
        WHERE PV.PrivateVehicleId = @PrivateVehicleId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

DROP PROCEDURE IF EXISTS CreateAUser;
GO
CREATE PROCEDURE CreateAUser
    @LoginName NVARCHAR(128),
    @Password NVARCHAR(128)
AS
BEGIN
    BEGIN TRY
        -- Step 1: Create the login
        DECLARE @Sql NVARCHAR(MAX);
        SET @Sql = 'CREATE LOGIN [' + @LoginName + '] WITH PASSWORD = ''' + @Password + ''';';
        EXEC sp_executesql @Sql;

        -- Step 2: Create the user in the specified database
        SET @Sql = 'USE [AutoAuctionDB]; CREATE USER [' + @LoginName + '] FOR LOGIN [' + @LoginName + '];';
        EXEC sp_executesql @Sql;

        -- Step 3: Grant EXECUTE permission
        SET @Sql = 'GRANT EXECUTE TO [' + @LoginName + '];';
        EXEC sp_executesql @Sql;

        -- Step 4: Deny SELECT on tables
        SET @Sql = 'DENY SELECT ON SCHEMA::dbo TO [' + @LoginName + '];';
        EXEC sp_executesql @Sql;

        -- Step 5: Deny INSERT on tables
        SET @Sql = 'DENY INSERT ON SCHEMA::dbo TO [' + @LoginName + '];';
        EXEC sp_executesql @Sql;

        -- Step 6: Deny UPDATE on tables
        SET @Sql = 'DENY UPDATE ON SCHEMA::dbo TO [' + @LoginName + '];';
        EXEC sp_executesql @Sql;

        -- Step 7: Deny DELETE on tables
        SET @Sql = 'DENY DELETE ON SCHEMA::dbo TO [' + @LoginName + '];';
        EXEC sp_executesql @Sql;

        PRINT 'User created successfully.';
    END TRY
    BEGIN CATCH
        PRINT 'Error occurred: ' + ERROR_MESSAGE();
    END CATCH;
END;
GO

-- Get all Trucks with relevant information
DROP PROCEDURE IF EXISTS GetAllTrucks;
GO
CREATE PROCEDURE GetAllTrucks
AS
BEGIN
    SELECT 
        T.TruckId,
        T.Height, 
        T.Weight, 
        T.Length, 
        T.LoadCapacity, 
        HV.MaxLoadCapacity, 
        HV.NumberOfAxles,
        V.VehicleId, 
        V.Name AS VehicleName, 
        V.KM, 
        V.RegistrationNumber, 
        V.AgeGroup, 
        V.TowHook, 
        V.DriversLicenceClass, 
        V.EngineSize, 
        V.KmL, 
        V.FuelType, 
        V.EnergyClass
    FROM Trucks T
    JOIN HeavyVehicles HV ON T.HeavyVehicleId = HV.HeavyVehicleId
    JOIN Vehicles V ON HV.VehicleId = V.VehicleId;
END;
GO


-- Edit a Truck and its related Vehicle and Heavy Vehicle details
DROP PROCEDURE IF EXISTS EditTruck;
GO
CREATE PROCEDURE EditTruck
    @TruckId INT,
    @Height DECIMAL(10, 2),
    @Weight DECIMAL(10, 2),
    @Length DECIMAL(10, 2),
    @LoadCapacity DECIMAL(10, 2),
    @MaxLoadCapacity INT,
    @NumberOfAxles INT,
    @VehicleName VARCHAR(100),
    @KM VARCHAR(20),
    @RegistrationNumber VARCHAR(20),
    @AgeGroup VARCHAR(20),
    @FuelType VARCHAR(20),
    @EnergyClass VARCHAR(10)
AS
BEGIN
    BEGIN TRY
        -- Update Trucks table
        UPDATE Trucks
        SET Height = @Height, Weight = @Weight, Length = @Length, LoadCapacity = @LoadCapacity
        WHERE TruckId = @TruckId;

        -- Update HeavyVehicles table
        UPDATE HeavyVehicles
        SET MaxLoadCapacity = @MaxLoadCapacity, NumberOfAxles = @NumberOfAxles
        WHERE HeavyVehicleId = (SELECT HeavyVehicleId FROM Trucks WHERE TruckId = @TruckId);

        -- Update Vehicles table
        UPDATE Vehicles
        SET Name = @VehicleName, KM = @KM, RegistrationNumber = @RegistrationNumber, 
            AgeGroup = @AgeGroup, FuelType = @FuelType, EnergyClass = @EnergyClass
        WHERE VehicleId = (SELECT VehicleId FROM HeavyVehicles WHERE HeavyVehicleId = 
            (SELECT HeavyVehicleId FROM Trucks WHERE TruckId = @TruckId));
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Delete a Truck and related records from HeavyVehicles and Vehicles
DROP PROCEDURE IF EXISTS DeleteATruck;
GO
CREATE PROCEDURE DeleteATruck
    @TruckId INT
AS
BEGIN
    BEGIN TRY
        -- Delete the Truck, this will also delete associated HeavyVehicle and Vehicle due to cascading delete
        DELETE FROM Trucks WHERE TruckId = @TruckId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

DROP PROCEDURE IF EXISTS GetAllBuses;
GO
CREATE PROCEDURE GetAllBuses
AS
BEGIN
    SELECT 
        B.BusId,
        B.Height, 
        B.Weight, 
        B.Length, 
        B.NumberOfSeats, 
        B.NumberOfSleepingPlaces, 
        B.HasToilet,
        HV.MaxLoadCapacity, 
        HV.NumberOfAxles,
        V.VehicleId, 
        V.Name AS VehicleName, 
        V.KM, 
        V.RegistrationNumber, 
        V.AgeGroup, 
        V.TowHook, 
        V.DriversLicenceClass, 
        V.EngineSize, 
        V.KmL, 
        V.FuelType, 
        V.EnergyClass
    FROM Buses B
    JOIN HeavyVehicles HV ON B.HeavyVehicleId = HV.HeavyVehicleId
    JOIN Vehicles V ON HV.VehicleId = V.VehicleId;
END;
GO


-- Edit a Bus and its related Vehicle and Heavy Vehicle details
DROP PROCEDURE IF EXISTS EditBus;
GO
CREATE PROCEDURE EditBus
    @BusId INT,
    @Height DECIMAL(10, 2),
    @Weight DECIMAL(10, 2),
    @Length DECIMAL(10, 2),
    @NumberOfSeats INT,
    @NumberOfSleepingPlaces INT,
    @HasToilet BIT,
    @MaxLoadCapacity INT,
    @NumberOfAxles INT,
    @VehicleName VARCHAR(100),
    @KM VARCHAR(20),
    @RegistrationNumber VARCHAR(20),
    @AgeGroup VARCHAR(20),
    @FuelType VARCHAR(20),
    @EnergyClass VARCHAR(10)
AS
BEGIN
    BEGIN TRY
        UPDATE Buses
        SET Height = @Height, 
            Weight = @Weight, 
            Length = @Length, 
            NumberOfSeats = @NumberOfSeats, 
            NumberOfSleepingPlaces = @NumberOfSleepingPlaces, 
            HasToilet = @HasToilet
        WHERE BusId = @BusId;

        UPDATE HeavyVehicles
        SET MaxLoadCapacity = @MaxLoadCapacity, 
            NumberOfAxles = @NumberOfAxles
        WHERE HeavyVehicleId = (SELECT HeavyVehicleId FROM Buses WHERE BusId = @BusId);

        UPDATE Vehicles
        SET Name = @VehicleName, 
            KM = @KM, 
            RegistrationNumber = @RegistrationNumber, 
            AgeGroup = @AgeGroup, 
            FuelType = @FuelType, 
            EnergyClass = @EnergyClass
        WHERE VehicleId = (SELECT VehicleId FROM HeavyVehicles WHERE HeavyVehicleId = 
            (SELECT HeavyVehicleId FROM Buses WHERE BusId = @BusId));
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO


-- Delete a Bus and related records from HeavyVehicles and Vehicles
DROP PROCEDURE IF EXISTS DeleteABuS;
GO
CREATE PROCEDURE DeleteABus
    @BusId INT
AS
BEGIN
    BEGIN TRY

        DELETE FROM Buses WHERE BusId = @BusId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Get all Commercial Vehicles with relevant information
DROP PROCEDURE IF EXISTS GetAllCommercialVehicles;
GO
CREATE PROCEDURE GetAllCommercialVehicles
AS
BEGIN
    SELECT 
        CV.CommercialVehicleId,
        CV.RollCage, 
        CV.LoadCapacity, 
        NV.NumberOfSeats, 
        NV.TrunkDimensions,
        V.Name AS VehicleName, 
        V.RegistrationNumber, 
        V.KM, 
        V.AgeGroup, 
        V.FuelType, 
        V.EnergyClass
    FROM CommercialVehicles CV
    JOIN NormalVehicles NV ON CV.NormalVehicleId = NV.NormalVehicleId
    JOIN Vehicles V ON NV.VehicleId = V.VehicleId;
END;
GO

-- Edit a Commercial Vehicle and its related Vehicle and Normal Vehicle details
DROP PROCEDURE IF EXISTS EditCommercialVehicle;
GO
CREATE PROCEDURE EditCommercialVehicle
    @CommercialVehicleId INT,
    @RollCage BIT,
    @LoadCapacity INT,
    @NumberOfSeats INT,
    @TrunkDimensions VARCHAR(50),
    @VehicleName VARCHAR(100),
    @KM VARCHAR(20),
    @RegistrationNumber VARCHAR(20),
    @AgeGroup VARCHAR(20),
    @FuelType VARCHAR(20),
    @EnergyClass VARCHAR(10)
AS
BEGIN
    BEGIN TRY
        -- Update CommercialVehicles table
        UPDATE CommercialVehicles
        SET RollCage = @RollCage, LoadCapacity = @LoadCapacity
        WHERE CommercialVehicleId = @CommercialVehicleId;

        -- Update NormalVehicles table
        UPDATE NormalVehicles
        SET NumberOfSeats = @NumberOfSeats, TrunkDimensions = @TrunkDimensions
        WHERE NormalVehicleId = (SELECT NormalVehicleId FROM CommercialVehicles WHERE CommercialVehicleId = @CommercialVehicleId);

        -- Update Vehicles table
        UPDATE Vehicles
        SET Name = @VehicleName, KM = @KM, RegistrationNumber = @RegistrationNumber, 
            AgeGroup = @AgeGroup, FuelType = @FuelType, EnergyClass = @EnergyClass
        WHERE VehicleId = (SELECT VehicleId FROM NormalVehicles WHERE NormalVehicleId = 
            (SELECT NormalVehicleId FROM CommercialVehicles WHERE CommercialVehicleId = @CommercialVehicleId));
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Delete a Commercial Vehicle and related records from NormalVehicles and Vehicles
DROP PROCEDURE IF EXISTS DeleteACommercialVehicle;
GO
CREATE PROCEDURE DeleteACommercialVehicle
    @CommercialVehicleId INT
AS
BEGIN
    BEGIN TRY
        -- Delete the Commercial Vehicle, this will also delete associated NormalVehicle and Vehicle due to cascading delete
        DELETE FROM CommercialVehicles WHERE CommercialVehicleId = @CommercialVehicleId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Get all Private Vehicles with relevant information
DROP PROCEDURE IF EXISTS GetAllPrivateVehicles;
GO
CREATE PROCEDURE GetAllPrivateVehicles
AS
BEGIN
    SELECT 
        PV.PrivateVehicleId, 
               PV.IsofixMount, 
               NV.NormalVehicleId, 
               NV.NumberOfSeats, 
               NV.TrunkDimensions, 
               NV.IsCommercial, 
               V.VehicleId, 
               V.Name AS VehicleName, 
               V.KM, 
               V.RegistrationNumber, 
               V.AgeGroup, 
               V.TowHook, 
               V.DriversLicenceClass, 
               V.EngineSize, 
               V.KmL, 
               V.FuelType, 
               V.EnergyClass
        FROM PrivateVehicles PV
        JOIN NormalVehicles NV ON PV.NormalVehicleId = NV.NormalVehicleId
        JOIN Vehicles V ON NV.VehicleId = V.VehicleId
END;
GO

-- Edit a Private Vehicle and its related Vehicle and Normal Vehicle details
DROP PROCEDURE IF EXISTS EditPrivateVehicle;
GO
CREATE PROCEDURE EditPrivateVehicle
    @PrivateVehicleId INT,
    @IsofixMount BIT,
    @NumberOfSeats INT,
    @TrunkDimensions VARCHAR(50),
    @VehicleName VARCHAR(100),
    @KM VARCHAR(20),
    @RegistrationNumber VARCHAR(20),
    @AgeGroup VARCHAR(20),
    @FuelType VARCHAR(20),
    @EnergyClass VARCHAR(10)
AS
BEGIN
    BEGIN TRY
        -- Update PrivateVehicles table
        UPDATE PrivateVehicles
        SET IsofixMount = @IsofixMount
        WHERE PrivateVehicleId = @PrivateVehicleId;

        -- Update NormalVehicles table
        UPDATE NormalVehicles
        SET NumberOfSeats = @NumberOfSeats, TrunkDimensions = @TrunkDimensions
        WHERE NormalVehicleId = (SELECT NormalVehicleId FROM PrivateVehicles WHERE PrivateVehicleId = @PrivateVehicleId);

        -- Update Vehicles table
        UPDATE Vehicles
        SET Name = @VehicleName, KM = @KM, RegistrationNumber = @RegistrationNumber, 
            AgeGroup = @AgeGroup, FuelType = @FuelType, EnergyClass = @EnergyClass
        WHERE VehicleId = (SELECT VehicleId FROM NormalVehicles WHERE NormalVehicleId = 
            (SELECT NormalVehicleId FROM PrivateVehicles WHERE PrivateVehicleId = @PrivateVehicleId));
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Delete a Private Vehicle and related records from NormalVehicles and Vehicles
DROP PROCEDURE IF EXISTS DeleteAPrivateVehicle;
GO
CREATE PROCEDURE DeleteAPrivateVehicle
    @PrivateVehicleId INT
AS
BEGIN
    BEGIN TRY
        -- Delete the Private Vehicle, this will also delete associated NormalVehicle and Vehicle due to cascading delete
        DELETE FROM PrivateVehicles WHERE PrivateVehicleId = @PrivateVehicleId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Create a User
DROP PROCEDURE IF EXISTS CreateUser;
GO
CREATE PROCEDURE CreateUser
    @UserName NVARCHAR(100),
    @Password NVARCHAR(255),
    @CorporateUser BIT,
    @Balance DECIMAL(10, 2),
    @Mail NVARCHAR(100)
AS
BEGIN
    BEGIN TRY
        INSERT INTO Users (UserName, Password, CorporateUser, Balance, Mail)
        VALUES (@UserName, @Password, @CorporateUser, @Balance, @Mail);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Get all Users
DROP PROCEDURE IF EXISTS GetAllUsers;
GO
CREATE PROCEDURE GetAllUsers
AS
BEGIN
    SELECT UserId, UserName, CorporateUser, Balance, Mail FROM Users;
END;
GO

-- Get a User by UserId
DROP PROCEDURE IF EXISTS GetUserById;
GO
CREATE PROCEDURE GetUserById
    @UserId INT
AS
BEGIN
    SELECT UserId, UserName, CorporateUser, Balance, Mail
    FROM Users
    WHERE UserId = @UserId;
END;
GO

DROP PROCEDURE IF EXISTS GetUserByName;
GO
CREATE PROCEDURE GetUserByName
	@UserName NVARCHAR(100)
AS
BEGIN
	SELECT UserId, UserName, PassWord, CorporateUser, Balance, Mail
	FROM Users
	WHERE UserName = @UserName;
END;
GO

-- Update a User
-- Drop the procedure if it already exists
DROP PROCEDURE IF EXISTS UpdateUser;
GO

-- Create the new stored procedure
CREATE PROCEDURE UpdateUser
    @UserId INT,
    @UserName NVARCHAR(100),
    @Password NVARCHAR(255),  -- Consider using NVARCHAR(128) if your passwords are limited
    @CorporateUser BIT,
    @Balance DECIMAL(10, 2),
    @Mail NVARCHAR(100)
AS
BEGIN
    BEGIN TRY
        -- Step 1: Update the user in the Users table
        UPDATE Users
        SET UserName = @UserName,
            Password = @Password,
            CorporateUser = @CorporateUser,
            Balance = @Balance,
            Mail = @Mail
        WHERE UserId = @UserId;

        -- Step 2: Update SQL Server login (if necessary)
        DECLARE @Sql NVARCHAR(MAX);
        SET @Sql = 'ALTER LOGIN [' + @UserName + '] WITH PASSWORD = ''' + @Password + ''';';
        EXEC sp_executesql @Sql;

        PRINT 'User updated successfully.';
    END TRY
    BEGIN CATCH
        PRINT 'Error occurred: ' + ERROR_MESSAGE();
    END CATCH;
END;
GO


-- Delete a User
DROP PROCEDURE IF EXISTS DeleteUser;
GO
CREATE PROCEDURE DeleteUser
    @UserId INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM Users WHERE UserId = @UserId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Create a Corporate User
DROP PROCEDURE IF EXISTS CreateCorporateUser;
GO
CREATE PROCEDURE CreateCorporateUser
    @UserId INT,
    @Credit DECIMAL(10, 2),
    @CVRNumber NVARCHAR(20)
AS
BEGIN
    BEGIN TRY
        INSERT INTO CorporateUsers (UserId, Credit, CVRNumber)
        VALUES (@UserId, @Credit, @CVRNumber);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Get all Corporate Users
DROP PROCEDURE IF EXISTS GetAllCorporateUsers;
GO
CREATE PROCEDURE GetAllCorporateUsers
AS
BEGIN
    SELECT CorporateUserId, UserId, Credit, CVRNumber FROM CorporateUsers;
END;
GO

-- Get a Corporate User by CorporateUserId
DROP PROCEDURE IF EXISTS GetCorporateUserById;
GO
CREATE PROCEDURE GetCorporateUserById
    @CorporateUserId INT
AS
BEGIN
    SELECT CorporateUserId, UserId, Credit, CVRNumber
    FROM CorporateUsers
    WHERE CorporateUserId = @CorporateUserId;
END;
GO

-- Update a Corporate User
DROP PROCEDURE IF EXISTS UpdateCorporateUser;
GO
CREATE PROCEDURE UpdateCorporateUser
    @CorporateUserId INT,
    @UserId INT,
    @Credit DECIMAL(10, 2),
    @CVRNumber NVARCHAR(20)
AS
BEGIN
    BEGIN TRY
        UPDATE CorporateUsers
        SET UserId = @UserId,
            Credit = @Credit,
            CVRNumber = @CVRNumber
        WHERE CorporateUserId = @CorporateUserId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Delete a Corporate User
DROP PROCEDURE IF EXISTS DeleteCorporateUser;
GO
CREATE PROCEDURE DeleteCorporateUser
    @CorporateUserId INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM CorporateUsers WHERE CorporateUserId = @CorporateUserId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Create a Private User
DROP PROCEDURE IF EXISTS CreatePrivateUser;
GO
CREATE PROCEDURE CreatePrivateUser
    @UserId INT,
    @CPRNumber NVARCHAR(11)
AS
BEGIN
    BEGIN TRY
        INSERT INTO PrivateUsers (UserId, CPRNumber)
        VALUES (@UserId, @CPRNumber);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Get all Private Users
DROP PROCEDURE IF EXISTS GetAllPrivateUsers;
GO
CREATE PROCEDURE GetAllPrivateUsers
AS
BEGIN
    SELECT PrivateUserId, UserId, CPRNumber FROM PrivateUsers;
END;
GO

-- Get a Private User by PrivateUserId
DROP PROCEDURE IF EXISTS GetPrivateUserById;
GO
CREATE PROCEDURE GetPrivateUserById
    @PrivateUserId INT
AS
BEGIN
    SELECT PrivateUserId, UserId, CPRNumber
    FROM PrivateUsers
    WHERE PrivateUserId = @PrivateUserId;
END;
GO

-- Delete a Private User
DROP PROCEDURE IF EXISTS DeletePrivateUser;
GO
CREATE PROCEDURE DeletePrivateUser
    @PrivateUserId INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM PrivateUsers WHERE PrivateUserId = @PrivateUserId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Create an Auction
DROP PROCEDURE IF EXISTS sp_AddAuction;
GO
CREATE PROCEDURE sp_AddAuction
    @VehicleId INT,
    @SellerUserId INT,
    @Price DECIMAL(18, 2),
    @ClosingDate DATETIME,
    @BuyerUserId INT
AS
BEGIN
    INSERT INTO Auctions (VehicleId, SellerUserId, Price, ClosingDate, BuyerUserId)
    VALUES (@VehicleId, @SellerUserId, @Price, @ClosingDate, @BuyerUserId);

    SELECT SCOPE_IDENTITY();
END
GO

-- Get all Auctions
DROP PROCEDURE IF EXISTS sp_GetAllAuctions;
GO
CREATE PROCEDURE sp_GetAllAuctions
AS
BEGIN
    SELECT * FROM Auctions;
END
GO

-- Delete an Auction
DROP PROCEDURE IF EXISTS sp_DeleteAuction;
GO
CREATE PROCEDURE sp_DeleteAuction
    @AuctionId INT
AS
BEGIN
    DELETE FROM Auctions
    WHERE AuctionId = @AuctionId;
END
GO

-- Get Seller Auctions
DROP PROCEDURE IF EXISTS sp_GetUserAuctions;
GO
CREATE PROCEDURE sp_GetUserAuctions
    @SellerUserId INT
AS
BEGIN
    SELECT * FROM Auctions 
    WHERE SellerUserId = @SellerUserId;
END
GO

-- Update Auction
DROP PROCEDURE IF EXISTS sp_UpdateAuction;
GO
CREATE PROCEDURE sp_UpdateAuction
    @AuctionId INT,
    @VehicleId INT,
    @SellerUserId INT,
    @Price DECIMAL(18, 2),
    @ClosingDate DATETIME,
    @BuyerUserId INT
AS
BEGIN
    UPDATE Auctions
    SET VehicleId = @VehicleId,
        SellerUserId = @SellerUserId,
        Price = @Price,
        ClosingDate = @ClosingDate,
        BuyerUserId = @BuyerUserId
    WHERE AuctionId = @AuctionId;
END
GO

-- Get Auction by ID
DROP PROCEDURE IF EXISTS sp_GetAuction;
GO
CREATE PROCEDURE sp_GetAuction
    @AuctionId INT
AS
BEGIN
    SELECT * FROM Auctions WHERE AuctionId = @AuctionId;
END
GO

DROP PROCEDURE IF EXISTS AddNormalVehicles;
GO

CREATE PROCEDURE AddNormalVehicles
    @VehicleId INT,
    @NumberOfSeats INT,
    @TrunkDimensions NVARCHAR(50),
    @IsCommercial BIT
AS
BEGIN
    INSERT INTO NormalVehicles (VehicleId, NumberOfSeats, TrunkDimensions, IsCommercial)
    VALUES (@VehicleId, @NumberOfSeats, @TrunkDimensions, @IsCommercial);
    SELECT SCOPE_IDENTITY();
END;
GO

DROP PROCEDURE IF EXISTS GetVehicleByIdAndReturnTypeAndId;
GO

CREATE PROCEDURE GetVehicleByIdAndReturnTypeAndId
    @VehicleId INT
AS
BEGIN
    DECLARE @VehicleType NVARCHAR(50);
    DECLARE @VehicleSubId INT;

    -- Check if the vehicle is a Truck
    IF EXISTS (SELECT 1 FROM Trucks t 
                JOIN HeavyVehicles h ON t.HeavyVehicleId = h.HeavyVehicleId 
                WHERE h.VehicleId = @VehicleId)
    BEGIN
        SELECT @VehicleType = 'Truck', @VehicleSubId = t.TruckId
        FROM Trucks t
        JOIN HeavyVehicles h ON t.HeavyVehicleId = h.HeavyVehicleId
        WHERE h.VehicleId = @VehicleId;
    END
    -- Check if the vehicle is a Bus
    ELSE IF EXISTS (SELECT 1 FROM Buses b 
                    JOIN HeavyVehicles h ON b.HeavyVehicleId = h.HeavyVehicleId 
                    WHERE h.VehicleId = @VehicleId)
    BEGIN
        SELECT @VehicleType = 'Bus', @VehicleSubId = b.BusId
        FROM Buses b
        JOIN HeavyVehicles h ON b.HeavyVehicleId = h.HeavyVehicleId
        WHERE h.VehicleId = @VehicleId;
    END
    -- Check if the vehicle is a Private Vehicle
    ELSE IF EXISTS (SELECT 1 FROM PrivateVehicles p 
                    JOIN NormalVehicles n ON p.NormalVehicleId = n.NormalVehicleId 
                    WHERE n.VehicleId = @VehicleId)
    BEGIN
        SELECT @VehicleType = 'PrivateVehicle', @VehicleSubId = p.PrivateVehicleId
        FROM PrivateVehicles p
        JOIN NormalVehicles n ON p.NormalVehicleId = n.NormalVehicleId
        WHERE n.VehicleId = @VehicleId;
    END
    -- Check if the vehicle is a Commercial Vehicle
    ELSE IF EXISTS (SELECT 1 FROM CommercialVehicles c 
                    JOIN NormalVehicles n ON c.NormalVehicleId = n.NormalVehicleId 
                    WHERE n.VehicleId = @VehicleId)
    BEGIN
        SELECT @VehicleType = 'CommercialVehicle', @VehicleSubId = c.CommercialVehicleId
        FROM CommercialVehicles c
        JOIN NormalVehicles n ON c.NormalVehicleId = n.NormalVehicleId
        WHERE n.VehicleId = @VehicleId;
    END
    ELSE
    BEGIN
        SET @VehicleType = 'Not Found';  -- If no type found
        SET @VehicleSubId = NULL;  -- No sub ID
    END

    -- Return the results
    SELECT @VehicleType AS VehicleType, @VehicleSubId AS VehicleSubId;
END
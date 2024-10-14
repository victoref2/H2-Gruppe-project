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
    @EngineSize DECIMAL,
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

-- GetABus procedure
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
        V.Name AS VehicleName, 
        V.RegistrationNumber, 
        V.KM, 
        V.AgeGroup, 
        V.FuelType, 
        V.EnergyClass
    FROM Trucks T
    JOIN HeavyVehicles HV ON T.HeavyVehicleId = HV.HeavyVehicleId
    JOIN Vehicles V ON HV.VehicleId = V.VehicleId;
END;
GO

-- Edit a Truck and its related Vehicle and Heavy Vehicle details
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

-- Get all Buses with relevant information
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
        V.Name AS VehicleName, 
        V.RegistrationNumber, 
        V.KM, 
        V.AgeGroup, 
        V.FuelType, 
        V.EnergyClass
    FROM Buses B
    JOIN HeavyVehicles HV ON B.HeavyVehicleId = HV.HeavyVehicleId
    JOIN Vehicles V ON HV.VehicleId = V.VehicleId;
END;
GO

-- Edit a Bus and its related Vehicle and Heavy Vehicle details
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
        -- Update Buses table
        UPDATE Buses
        SET Height = @Height, Weight = @Weight, Length = @Length, 
            NumberOfSeats = @NumberOfSeats, NumberOfSleepingPlaces = @NumberOfSleepingPlaces, 
            HasToilet = @HasToilet
        WHERE BusId = @BusId;

        -- Update HeavyVehicles table
        UPDATE HeavyVehicles
        SET MaxLoadCapacity = @MaxLoadCapacity, NumberOfAxles = @NumberOfAxles
        WHERE HeavyVehicleId = (SELECT HeavyVehicleId FROM Buses WHERE BusId = @BusId);

        -- Update Vehicles table
        UPDATE Vehicles
        SET Name = @VehicleName, KM = @KM, RegistrationNumber = @RegistrationNumber, 
            AgeGroup = @AgeGroup, FuelType = @FuelType, EnergyClass = @EnergyClass
        WHERE VehicleId = (SELECT VehicleId FROM HeavyVehicles WHERE HeavyVehicleId = 
            (SELECT HeavyVehicleId FROM Buses WHERE BusId = @BusId));
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Delete a Bus and related records from HeavyVehicles and Vehicles
CREATE PROCEDURE DeleteABus
    @BusId INT
AS
BEGIN
    BEGIN TRY
        -- Delete the Bus, this will also delete associated HeavyVehicle and Vehicle due to cascading delete
        DELETE FROM Buses WHERE BusId = @BusId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;
END;
GO

-- Get all Commercial Vehicles with relevant information
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
CREATE PROCEDURE GetAllPrivateVehicles
AS
BEGIN
    SELECT 
        PV.PrivateVehicleId,
        PV.IsofixMount, 
        NV.NumberOfSeats, 
        NV.TrunkDimensions,
        V.Name AS VehicleName, 
        V.RegistrationNumber, 
        V.KM, 
        V.AgeGroup, 
        V.FuelType, 
        V.EnergyClass
    FROM PrivateVehicles PV
    JOIN NormalVehicles NV ON PV.NormalVehicleId = NV.NormalVehicleId
    JOIN Vehicles V ON NV.VehicleId = V.VehicleId;
END;
GO

-- Edit a Private Vehicle and its related Vehicle and Normal Vehicle details
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
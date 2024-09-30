CREATE TABLE Vehicles (
    VehicleId INT IDENTITY(1,1) PRIMARY KEY, 
    Name VARCHAR(100) NOT NULL,
    KM VARCHAR(20) NOT NULL,
    RegistrationNumber VARCHAR(20) UNIQUE NOT NULL,
    AgeGroup VARCHAR(20) NOT NULL,
    TowHook BIT NOT NULL,
    DriversLicenceClass VARCHAR(5) NOT NULL,
    EngineSize DECIMAL NOT NULL,
    KmL DECIMAL(10, 2) NOT NULL,
    FuelType VARCHAR(20) NOT NULL,
    EnergyClass VARCHAR(10) NOT NULL
);

CREATE TABLE HeavyVehicles (
    HeavyVehicleId INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    MaxLoadCapacity INT NOT NULL,
    NumberOfAxles INT NOT NULL,
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(VehicleId) ON DELETE CASCADE
);


CREATE TABLE Trucks (
    TruckId INT IDENTITY(1,1) PRIMARY KEY,
    HeavyVehicleId INT NOT NULL,
    Height DECIMAL(10, 2) NOT NULL,
    Weight DECIMAL(10, 2) NOT NULL,
    Length DECIMAL(10, 2) NOT NULL,
    LoadCapacity DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (HeavyVehicleId) REFERENCES HeavyVehicles(HeavyVehicleId) ON DELETE CASCADE
);

CREATE TABLE Buses (
    BusId INT IDENTITY(1,1) PRIMARY KEY,
    HeavyVehicleId INT NOT NULL,
    Height DECIMAL(10, 2) NOT NULL,
    Weight DECIMAL(10, 2) NOT NULL,
    Length DECIMAL(10, 2) NOT NULL,
    NumberOfSeats INT NOT NULL,
    NumberOfSleepingPlaces INT NOT NULL,
    HasToilet BIT NOT NULL,
    FOREIGN KEY (HeavyVehicleId) REFERENCES HeavyVehicles(HeavyVehicleId) ON DELETE CASCADE
);

CREATE TABLE NormalVehicles (
    NormalVehicleId INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    NumberOfSeats INT NOT NULL,
    TrunkDimensions VARCHAR(50) NOT NULL,
    IsCommercial BIT NOT NULL,
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(VehicleId) ON DELETE CASCADE
);

CREATE TABLE PrivateVehicles (
    PrivateVehicleId INT IDENTITY(1,1) PRIMARY KEY,
    NormalVehicleId INT NOT NULL,
    IsofixMount BIT NOT NULL,
    FOREIGN KEY (NormalVehicleId) REFERENCES NormalVehicles(NormalVehicleId) ON DELETE CASCADE
);

CREATE TABLE CommercialVehicles (
    CommercialVehicleId INT IDENTITY(1,1) PRIMARY KEY,
    NormalVehicleId INT NOT NULL,
    RollCage BIT NOT NULL,
    LoadCapacity INT NOT NULL CHECK (LoadCapacity >= 0),
    FOREIGN KEY (NormalVehicleId) REFERENCES NormalVehicles(NormalVehicleId) ON DELETE CASCADE
);
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    PassWord NVARCHAR(255) NOT NULL,
    Mail NVARCHAR(100) NOT NULL UNIQUE
);
CREATE TABLE Auctions (
    AuctionId INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    UserId INT NOT NULL,
    Price INT NOT NULL,
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(VehicleId) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE           
);
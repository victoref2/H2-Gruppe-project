-- Drop tables if they exist before creating new ones
DROP TABLE IF EXISTS Auctions;
DROP TABLE IF EXISTS CommercialVehicles;
DROP TABLE IF EXISTS PrivateVehicles;
DROP TABLE IF EXISTS NormalVehicles;
DROP TABLE IF EXISTS Buses;
DROP TABLE IF EXISTS Trucks;
DROP TABLE IF EXISTS HeavyVehicles;
DROP TABLE IF EXISTS Vehicles;

-- Create Vehicles table
CREATE TABLE Vehicles (
    VehicleId INT IDENTITY(1,1) PRIMARY KEY, 
    Name VARCHAR(100) NOT NULL,
    KM VARCHAR(20) NOT NULL,
    RegistrationNumber VARCHAR(20) UNIQUE NOT NULL,
    AgeGroup VARCHAR(20) NOT NULL,
    TowHook BIT NOT NULL,
    DriversLicenceClass VARCHAR(5) NOT NULL,
    EngineSize VARCHAR(10) NOT NULL,
    KmL DECIMAL(10, 2) NOT NULL,
    FuelType VARCHAR(20) NOT NULL,
    EnergyClass VARCHAR(10) NOT NULL
);

-- Create HeavyVehicles table
CREATE TABLE HeavyVehicles (
    HeavyVehicleId INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    MaxLoadCapacity INT NOT NULL,
    NumberOfAxles INT NOT NULL,
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(VehicleId) ON DELETE CASCADE
);

-- Create Trucks table
CREATE TABLE Trucks (
    TruckId INT IDENTITY(1,1) PRIMARY KEY,
    HeavyVehicleId INT NOT NULL,
    Height DECIMAL(10, 2) NOT NULL,
    Weight DECIMAL(10, 2) NOT NULL,
    Length DECIMAL(10, 2) NOT NULL,
    LoadCapacity DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (HeavyVehicleId) REFERENCES HeavyVehicles(HeavyVehicleId) ON DELETE CASCADE
);

-- Create Buses table
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

-- Create NormalVehicles table
CREATE TABLE NormalVehicles (
    NormalVehicleId INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    NumberOfSeats INT NOT NULL,
    TrunkDimensions VARCHAR(50) NOT NULL,
    IsCommercial BIT NOT NULL,
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(VehicleId) ON DELETE CASCADE
);

-- Create PrivateVehicles table
CREATE TABLE PrivateVehicles (
    PrivateVehicleId INT IDENTITY(1,1) PRIMARY KEY,
    NormalVehicleId INT NOT NULL,
    IsofixMount BIT NOT NULL,
    FOREIGN KEY (NormalVehicleId) REFERENCES NormalVehicles(NormalVehicleId) ON DELETE CASCADE
);

-- Create CommercialVehicles table
CREATE TABLE CommercialVehicles (
    CommercialVehicleId INT IDENTITY(1,1) PRIMARY KEY,
    NormalVehicleId INT NOT NULL,
    RollCage BIT NOT NULL,
    LoadCapacity INT NOT NULL CHECK (LoadCapacity >= 0),
    FOREIGN KEY (NormalVehicleId) REFERENCES NormalVehicles(NormalVehicleId) ON DELETE CASCADE
);

-- Create Auctions table
CREATE TABLE Auctions (
    AuctionId INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId INT NOT NULL,
    SellerUserId INT NOT NULL,
    BuyerUserId INT, 
    Price DECIMAL(18,2) NOT NULL,
    ClosingDate DATETIME NOT NULL,

    -- Foreign Key Constraints
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(VehicleId),
    FOREIGN KEY (SellerUserId) REFERENCES Users(UserId),
    FOREIGN KEY (BuyerUserId) REFERENCES Users(UserId)
);

-- Drop table if it exists before creating it
DROP TABLE IF EXISTS Users;
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL UNIQUE, 
    Password NVARCHAR(255) NOT NULL,
    CorporateUser BIT NOT NULL DEFAULT 0,
    Balance DECIMAL(10,2) NOT NULL DEFAULT 0,
    Mail NVARCHAR(100) NOT NULL UNIQUE
);

-- Drop table if it exists before creating it
DROP TABLE IF EXISTS CorporateUsers;
CREATE TABLE CorporateUsers (
    CorporateUserId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Credit DECIMAL(10,2) NOT NULL,
    CVRNumber NVARCHAR(20) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
);

-- Drop table if it exists before creating it
DROP TABLE IF EXISTS PrivateUsers;
CREATE TABLE PrivateUsers (
    PrivateUserId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    CPRNumber NVARCHAR(11) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
 );
-- =============================================
-- VeterinariaDB - Script de creacion y datos de ejemplo
-- Dominio: Propietarios (1) -> Mascotas (N)
-- =============================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'VeterinariaDB')
BEGIN
    CREATE DATABASE VeterinariaDB;
END
GO

USE VeterinariaDB;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Mascotas')
    DROP TABLE Mascotas;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Propietarios')
    DROP TABLE Propietarios;
GO

CREATE TABLE Propietarios (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Nombres     NVARCHAR(80)  NOT NULL,
    Apellidos   NVARCHAR(80)  NOT NULL,
    Dni         CHAR(8)       NOT NULL,
    Telefono    NVARCHAR(9)   NULL,
    Email       NVARCHAR(120) NULL,
    Direccion   NVARCHAR(150) NULL,
    CONSTRAINT UQ_Propietarios_Dni UNIQUE (Dni)
);
GO

CREATE TABLE Mascotas (
    Id                INT IDENTITY(1,1) PRIMARY KEY,
    Nombre            NVARCHAR(60) NOT NULL,
    Especie           NVARCHAR(40) NOT NULL,
    Raza              NVARCHAR(60) NULL,
    FechaNacimiento   DATE NULL,
    PropietarioId     INT NOT NULL,
    CONSTRAINT FK_Mascotas_Propietarios FOREIGN KEY (PropietarioId)
        REFERENCES Propietarios(Id)
);
GO

-- Datos de ejemplo
INSERT INTO Propietarios (Nombres, Apellidos, Dni, Telefono, Email, Direccion) VALUES
('Maria',   'Gonzales', '45123456', '987654321', 'maria.gonzales@example.com', 'Av. Los Pinos 123, Lima'),
('Carlos',  'Ramirez',  '41234567', '987111222', 'carlos.ramirez@example.com', 'Jr. Las Flores 456, Lima'),
('Ana',     'Torres',   '42345678', '987333444', 'ana.torres@example.com',     'Calle Sol 789, Arequipa'),
('Luis',    'Fernandez','43456789', '987555666', 'luis.fernandez@example.com', 'Av. Grau 321, Trujillo'),
('Rosa',    'Mendoza',  '44567890', NULL,         NULL,                         'Jr. Union 654, Cusco');
GO

INSERT INTO Mascotas (Nombre, Especie, Raza, FechaNacimiento, PropietarioId) VALUES
('Toby',    'Perro', 'Labrador',        '2021-03-15', 1),
('Luna',    'Gato',  'Siames',          '2022-07-01', 1),
('Rocky',   'Perro', 'Bulldog',         '2020-11-20', 2),
('Michi',   'Gato',  'Comun Europeo',   '2023-01-10', 3),
('Max',     'Perro', 'Pastor Aleman',   '2019-05-05', 3),
('Pelusa',  'Conejo', 'Holandes',       '2022-09-30', 4),
('Nina',    'Gato',  'Persa',           '2021-12-25', 4);
-- Rosa (Id 5) queda sin mascotas: sirve para probar GET anidado vacio y DELETE sin dependientes (200/204)
-- Carlos (Id 2) tiene 1 mascota: sirve para probar DELETE con dependiente -> 409
GO

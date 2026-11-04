-- =============================================
-- VeterinariaDB - Script de creacion y datos de ejemplo
-- Dominio: Propietarios (1) -> Mascotas (N)
--          Veterinarios (1) -> Citas (N) <- Mascotas (1)
-- =============================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'VeterinariaDB')
BEGIN
    CREATE DATABASE VeterinariaDB;
END
GO

USE VeterinariaDB;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Citas')
    DROP TABLE Citas;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Mascotas')
    DROP TABLE Mascotas;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Propietarios')
    DROP TABLE Propietarios;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Veterinarios')
    DROP TABLE Veterinarios;
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

CREATE TABLE Veterinarios (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Nombres       NVARCHAR(80) NOT NULL,
    Apellidos     NVARCHAR(80) NOT NULL,
    Licencia      VARCHAR(10)  NOT NULL,
    Especialidad  NVARCHAR(60) NULL,
    Telefono      NVARCHAR(9)  NULL,
    CONSTRAINT UQ_Veterinarios_Licencia UNIQUE (Licencia)
);
GO

CREATE TABLE Citas (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    FechaHora       DATETIME2 NOT NULL,
    Motivo          NVARCHAR(200) NOT NULL,
    MascotaId       INT NOT NULL,
    VeterinarioId   INT NOT NULL,
    CONSTRAINT FK_Citas_Mascotas FOREIGN KEY (MascotaId)
        REFERENCES Mascotas(Id),
    CONSTRAINT FK_Citas_Veterinarios FOREIGN KEY (VeterinarioId)
        REFERENCES Veterinarios(Id)
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

INSERT INTO Veterinarios (Nombres, Apellidos, Licencia, Especialidad, Telefono) VALUES
('Jorge',   'Salazar',  'CMVP0001', 'Medicina general', '987222333'),
('Patricia','Vargas',   'CMVP0002', 'Cirugia',          '987444555'),
('Diego',   'Castillo', 'CMVP0003', 'Dermatologia',     NULL);
-- Diego (Id 3) queda sin citas: sirve para probar GET anidado vacio y DELETE sin dependientes (200/204)
GO

INSERT INTO Citas (FechaHora, Motivo, MascotaId, VeterinarioId) VALUES
('2026-11-10 09:00', 'Control anual',       1, 1),
('2026-11-10 10:30', 'Vacuna antirrabica',  2, 1),
('2026-11-11 15:00', 'Cirugia menor',       3, 2),
('2026-11-12 11:00', 'Chequeo general',     4, 1),
('2026-11-12 16:00', 'Desparasitacion',     6, 2);
-- Veterinario 1 (Jorge) tiene varias citas: sirve para probar DELETE con dependientes -> 409
-- y la regla de horario ocupado (mismo VeterinarioId + misma FechaHora -> 409)
GO

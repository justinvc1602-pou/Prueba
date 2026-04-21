CREATE DATABASE Jus;
GO

USE Jus;
GO

CREATE TABLE Usuarios (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    Nombre    NVARCHAR(100) NOT NULL,
    Telefono  NVARCHAR(20)  NOT NULL,
    Celular   NVARCHAR(20)  NULL,
    Email     NVARCHAR(100) NULL
);
GO


INSERT INTO Usuarios (Nombre, Telefono, Celular, Email)
VALUES ('Alejandro', '444444', '444445554444', 'alejand5@gmail.com');
GO

PRINT '✅ Registro insertado correctamente!';
GO


SELECT * FROM Usuarios;
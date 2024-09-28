CREATE DATABASE Practica03
GO
USE Practica03
GO

CREATE TABLE Articulos(
id_articulo INT IDENTITY,
nombre VARCHAR(40) not null,
pre_unitario INT NOT NULL,
CONSTRAINT pk_articulo PRIMARY KEY (id_articulo)
)
GO

CREATE TABLE FormasPago(
id_forma_pago INT IDENTITY,
nombre VARCHAR(50) NOT NULL,
CONSTRAINT pk_FormasPago PRIMARY KEY(id_forma_pago)
)
GO

CREATE TABLE Facturas(
nro_factura INT IDENTITY,
fecha DATE NOT NULL,
id_forma_pago INT NOT NULL,
cliente VARCHAR(40),
CONSTRAINT pk_Facturas PRIMARY KEY (nro_factura),
CONSTRAINT fk_FormasPago FOREIGN KEY (id_forma_pago)
REFERENCES FormasPago(id_forma_pago)
)
GO

CREATE TABLE DetallesFactura(
id_detalle INT IDENTITY,
nro_factura INT NOT NULL,
id_articulo INT NOT NULL,
cantidad INT NOT NULL
CONSTRAINT pk_DetallesFactura PRIMARY KEY (id_detalle),
CONSTRAINT fk_Facturas FOREIGN KEY (nro_factura)
REFERENCES Facturas(nro_factura),
CONSTRAINT fk_Articulos FOREIGN KEY (id_articulo)
REFERENCES Articulos(id_articulo)
)
GO


-------------INSERTS
INSERT INTO FormasPago (nombre)
VALUES 
    ('Efectivo'),
    ('Tarjeta de Crédito'),
    ('Transferencia Bancaria'),
    ('PayPal'),
    ('Cheque');
GO

INSERT INTO Articulos (nombre, pre_unitario)
VALUES 
    ('Laptop', 1200),
    ('Mouse', 25),
    ('Teclado', 50),
    ('Monitor', 300),
    ('Impresora', 150);
GO

INSERT INTO Facturas (fecha, id_forma_pago, cliente)
VALUES 
    ('2024-09-01', 1, 'Juan Pérez'),   -- id_forma_pago = 1
    ('2024-09-02', 2, 'Ana Gómez'),    -- id_forma_pago = 2
    ('2024-09-03', 3, 'Carlos Rodríguez'), -- id_forma_pago = 3
    ('2024-09-04', 4, 'Laura Fernández'), -- id_forma_pago = 4
    ('2024-09-05', 5, 'Pedro Martínez'); -- id_forma_pago = 5
GO

INSERT INTO DetallesFactura (nro_factura, id_articulo, cantidad)
VALUES 
    (1, 1, 2),  -- Laptop, Juan Pérez
    (1, 2, 1),  -- Mouse, Juan Pérez
    (2, 3, 1),  -- Teclado, Ana Gómez
    (3, 4, 1),  -- Monitor, Carlos Rodríguez
    (4, 5, 1);  -- Impresora, Laura Fernández
GO


-------------PROCEDIMIENTOS ALMACENADOS
CREATE PROCEDURE SP_GET_ALL_ARTICULO
AS
BEGIN
SELECT *
FROM Articulos
END
GO


CREATE PROCEDURE SP_DEL_ARTICULO
@id_articulo INT
AS
BEGIN
DELETE Articulos
FROM Articulos
WHERE id_articulo = @id_articulo
END
GO


CREATE PROCEDURE SP_CREATE_ARTICULO
@nombre VARCHAR(40), @pre_unitario INT
AS
BEGIN
INSERT INTO Articulos(nombre, pre_unitario)
VALUES(@nombre, @pre_unitario)
END
GO


CREATE PROCEDURE SP_UPDATE_ARTICULO
@id_articulo INT, @nombre VARCHAR(40), @pre_unitario INT
AS
BEGIN
UPDATE Articulos 
SET nombre = @nombre, pre_unitario = @pre_unitario
WHERE id_articulo = @id_articulo
END
GO



-------------PROCEDIMIENTOS ALMACENADOS

--CREATE PROCEDURE SP_GET_ALL_FACTURA 
--AS
--BEGIN
--SELECT *
--FROM Facturas 
--END
--GO

CREATE PROCEDURE SP_GET_ALL_FACTURA
AS
BEGIN
    SELECT 
        f.nro_factura,
        f.fecha,
        f.id_forma_pago,
        f.cliente,
        df.id_articulo,
        df.cantidad
    FROM 
        Facturas f
    INNER JOIN 
        DetallesFactura df ON f.nro_factura = df.nro_factura
    ORDER BY 
        f.nro_factura, df.id_detalle;
END;
GO


CREATE PROCEDURE SP_CREATE_FACTURA
    @fecha DATE,
    @id_forma_pago INT,
    @cliente VARCHAR(40)
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Inserta los datos en la tabla Facturas
        INSERT INTO Facturas(fecha, id_forma_pago, cliente) 
        VALUES(@fecha, @id_forma_pago, @cliente);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Si ocurre un error, deshacer la transacción
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END
GO



CREATE PROCEDURE SP_CREATE_DETALLE
    @nro_factura INT,
    @id_articulo INT,
    @cantidad INT
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Inserta los detalles en la tabla DetallesFactura
        INSERT INTO DetallesFactura(nro_factura, id_articulo, cantidad) 
        VALUES(@nro_factura, @id_articulo, @cantidad);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Si ocurre un error, deshacer la transacción
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END
GO


CREATE PROCEDURE SP_UPDATE_FACT
    @nro_factura INT,
    @fecha DATE,
    @id_forma_pago INT,
    @cliente VARCHAR(40)
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Actualizar la cabecera de la factura
        UPDATE Facturas
        SET fecha = @fecha, 
            id_forma_pago = @id_forma_pago, 
            cliente = @cliente
        WHERE nro_factura = @nro_factura;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH

        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END
GO


CREATE PROCEDURE SP_UPDATE_DET
    @id_detalle INT,
    @nro_factura INT,
    @id_articulo INT,
    @cantidad INT
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        UPDATE DetallesFactura
        SET id_articulo = @id_articulo, 
            cantidad = @cantidad 
        WHERE id_detalle = @id_detalle AND nro_factura = @nro_factura;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH

        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END
GO


CREATE PROCEDURE SP_GET_PARAM_FACTURA
    @fecha DATE,
    @id_forma_pago INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        f.nro_factura, 
        f.fecha, 
        f.id_forma_pago, 
        f.cliente,
        d.id_articulo,
        d.cantidad
    FROM 
        Facturas f
    JOIN 
        DetallesFactura d ON f.nro_factura = d.nro_factura
    WHERE 
        f.fecha = @fecha OR
        f.id_forma_pago = @id_forma_pago;
END
GO



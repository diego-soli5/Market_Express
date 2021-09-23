USE master;
GO

DROP DATABASE MARKET_EXPRESS;
GO

CREATE DATABASE MARKET_EXPRESS;
GO

USE MARKET_EXPRESS;
GO

CREATE TABLE Usuario(
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Nombre VARCHAR(40) NOT NULL,
	Cedula VARCHAR(12) UNIQUE NOT NULL,
	Email VARCHAR(40) UNIQUE NOT NULL,
	Telefono VARCHAR(40) NOT NULL,
	Clave VARCHAR(80) NOT NULL,
	Tipo VARCHAR(15) NOT NULL,
	Estado VARCHAR(11) NOT NULL,
	Adicionado_Por VARCHAR(12),
	Modificado_Por VARCHAR(12)

	PRIMARY KEY(Id),
	CONSTRAINT CHK_Usuario_Tipo CHECK (Tipo = 'ADMINISTRADOR' OR Tipo = 'CLIENTE'),
	CONSTRAINT CHK_Usuario_Estado CHECK (Estado = 'ACTIVADO' OR Estado = 'DESACTIVADO')
);
GO

CREATE TABLE Permiso(		-- Permisos para crear Roles que ser�n asignados a un usuario
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Nombre VARCHAR(15) NOT NULL,
	Descripcion VARCHAR(50),

	PRIMARY KEY(Id)
);
GO

CREATE TABLE Rol(	-- Roles los cuales definiran los permisos de cada usuario
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Nombre VARCHAR(15) NOT NULL,
	Descripcion VARCHAR(50),

	PRIMARY KEY(Id)	
);
GO

CREATE TABLE Rol_Permiso( -- Para almacenar los permisos que tendra un Rol
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Id_Rol UNIQUEIDENTIFIER NOT NULL,
	Id_Permiso UNIQUEIDENTIFIER NOT NULL,

	PRIMARY KEY(Id),
	FOREIGN KEY(Id_Rol) REFERENCES Rol(Id),
	FOREIGN KEY(Id_Permiso) REFERENCES Permiso(Id)
);
GO

CREATE TABLE Usuario_Rol( --Para almacenar los roles asignados a cada usuario
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Id_Usuario UNIQUEIDENTIFIER NOT NULL,

	PRIMARY KEY(Id),
	FOREIGN KEY(Id_Usuario) REFERENCES Usuario(Id)
);
GO

CREATE TABLE Cliente(
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Id_Usuario UNIQUEIDENTIFIER UNIQUE NOT NULL,
	Cod_Cliente VARCHAR(10), -- Codigo para los clientes registrados en el POS
	Auto_Sinc BIT NOT NULL,

	PRIMARY KEY(Id),
	FOREIGN KEY(Id_Usuario) REFERENCES Usuario(Id)
);
GO

CREATE TABLE Direccion(
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Id_Cliente UNIQUEIDENTIFIER NOT NULL,
	Nombre VARCHAR(10) NOT NULL,
	Detalle VARCHAR(255) NOT NULL,

	PRIMARY KEY(Id)
);
GO

CREATE TABLE Bitacora_Acceso(
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Id_Usuario UNIQUEIDENTIFIER NOT NULL,
	Fecha_Inicio DATETIME NOT NULL,
	Fecha_Salida DATETIME,

	PRIMARY KEY(Id),
	FOREIGN KEY(Id_Usuario) REFERENCES Usuario(Id)
);
GO

CREATE TABLE Bitacora_Movimiento(
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Id_Usuario UNIQUEIDENTIFIER NOT NULL,
	Fecha DATETIME NOT NULL,
	Tipo VARCHAR(10) NOT NULL,
	Detalle Varchar(100) NOT NULL,

	PRIMARY KEY(Id),
	FOREIGN KEY(Id_Usuario) REFERENCES Usuario(Id)
);
GO

CREATE TABLE Inventario_Categoria(
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Nombre VARCHAR(20) NOT NULL,
	Descripcion VARCHAR(200),
	Estado VARCHAR(11) NOT NULL,
	Adicionado_Por VARCHAR(12),
	Modificado_Por VARCHAR(12)

	PRIMARY KEY(Id),
	CONSTRAINT CHK_Categoria_Estado CHECK (Estado = 'ACTIVADO' OR Estado = 'DESACTIVADO')
);
GO

CREATE TABLE Inventario_Articulo(
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Id_Categoria UNIQUEIDENTIFIER,
	Descripcion VARCHAR(255) NOT NULL,
	Codigo_Barras VARCHAR(255) UNIQUE NOT NULL,
	Precio DECIMAL(19,2) NOT NULL,
	Imagen VARCHAR(30),
	Auto_Sinc BIT NOT NULL,
	Estado VARCHAR(11) NOT NULL,
	Adicionado_Por VARCHAR(12),
	Modificado_Por VARCHAR(12)

	PRIMARY KEY(Id),
	CONSTRAINT CHK_Articulo_Estado CHECK (Estado = 'ACTIVADO' OR Estado = 'DESACTIVADO')
);
GO

CREATE TABLE Carrito(
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Id_Cliente UNIQUEIDENTIFIER NOT NULL,
	Fecha_Apertura DATETIME NOT NULL,
	Estado VARCHAR(7) NOT NULL,

	PRIMARY KEY(Id),
	FOREIGN KEY(Id_Cliente) REFERENCES Cliente(Id),
	CONSTRAINT CHK_Carrito_Estado CHECK (Estado = 'ABIERTO' OR Estado = 'CERRADO')
);
GO

CREATE TABLE Carrito_Detalle(
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Id_Carrito UNIQUEIDENTIFIER NOT NULL,
	Id_Articulo UNIQUEIDENTIFIER NOT NULL

	PRIMARY KEY(Id),
	FOREIGN KEY(Id_Carrito) REFERENCES Carrito(Id),
	FOREIGN KEY(Id_Articulo) REFERENCES Inventario_Articulo(Id)
);
GO

CREATE TABLE Pedido(
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Id_Cliente UNIQUEIDENTIFIER NOT NULL,
	Fecha_Creacion DATETIME NOT NULL,
	Total DECIMAL(19,2) NOT NULL,
	Estado VARCHAR(9) NOT NULL,

	PRIMARY KEY(Id),
	CONSTRAINT CHK_Pedido_Estado CHECK (Estado = 'PENDIENTE' OR Estado = 'TERMINADO' OR Estado = 'CANCELADO')
);

CREATE TABLE Pedido_Detalle(
	Id UNIQUEIDENTIFIER DEFAULT newsequentialid(),
	Id_Pedido UNIQUEIDENTIFIER NOT NULL,
	Id_Articulo UNIQUEIDENTIFIER NOT NULL, -- Se agrega para realizar el promedio de art. m�s solicitados
	Descripcion VARCHAR(255) NOT NULL,
	Codigo_Barras VARCHAR(255) NOT NULL,
	Precio DECIMAL(19,2) NOT NULL,

	PRIMARY KEY(Id),
	FOREIGN KEY(Id_Pedido) REFERENCES Pedido(Id),
	FOREIGN KEY(Id_Articulo) REFERENCES Inventario_Articulo(Id)
);
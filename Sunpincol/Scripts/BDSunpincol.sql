
create table Usuario (
    idUsuario int identity(1,1) primary key,
    nombre varchar(100) not null,
    correo varchar(120) not null,
    contrasena varchar(255) not null,
    rol varchar(50) not null,
    estadoActivo bit not null
);

create table Sede (
    idSede int identity(1,1) primary key,
    nombreSede varchar(80) not null,
    estadoSede varchar(20) not null
);

create table Producto (
    codigoProducto varchar(30) primary key,
    nombreProducto varchar(100) not null,
    categoria varchar(60) not null,
    descripcion varchar(255) not null,
    precioUnitario decimal(12,2) not null,
    estadoProducto bit not null
);

create table Empleado (
    idEmpleado int primary key,
    idSedeAsignada int not null,
    cargo varchar(60) not null,
    turno varchar(30) not null,
    foreign key (idEmpleado) references Usuario(idUsuario),
    foreign key (idSedeAsignada) references Sede(idSede)
);

create table Jefe (
    idJefe int identity(1,1) primary key,
    idEmpleado int not null,
    areaSupervision varchar(80) not null,
    nivelAutorizacion numeric(2,0) not null,
    foreign key (idEmpleado) references Empleado(idEmpleado)
);

create table Catalogo (
    idCatalogo int identity(1,1) primary key,
    idSede int not null,
    fechaGeneracion datetime2 not null,
    totalProductosActivos int not null,
    foreign key (idSede) references Sede(idSede)
);

create table Guia (
    idGuia int identity(1,1) primary key,
    idSedeOrigen int not null,
    idSedeDestino int not null,
    referenciaGuia varchar(50) not null,
    fechaEmision datetime2 not null,
    estadoGuia varchar(30) not null,
    foreign key (idSedeOrigen) references Sede(idSede),
    foreign key (idSedeDestino) references Sede(idSede)
);

create table Inventario (
    idInventario int identity(1,1) primary key,
    idSede int not null,
    codigoProducto varchar(30) not null,
    stockActual int not null,
    stockMinimo int not null,
    fechaUltimaActualizacion datetime2 not null,
    foreign key (idSede) references Sede(idSede),
    foreign key (codigoProducto) references Producto(codigoProducto)
);

create table Inconsistencia (
    idInconsistencia int identity(1,1) primary key,
    idSede int not null,
    codigoProducto varchar(30) not null,
    idGuia int not null,
    cantidadSistema int not null,
    cantidadFisica int not null,
    diferencia int not null,
    observacion varchar(255) not null,
    estadoReporte varchar(30) not null,
    fechaReporte datetime2 not null,
    foreign key (idSede) references Sede(idSede),
    foreign key (codigoProducto) references Producto(codigoProducto),
    foreign key (idGuia) references Guia(idGuia)
);

create table Cambios (
    idCambio int identity(1,1) primary key,
    idInventario int not null,
    idUsuarioResponsable int not null,
    cantidadAnterior int not null,
    cantidadNueva int not null,
    motivoCambio varchar(200) not null,
    fechaCambio datetime2 not null,
    foreign key (idInventario) references Inventario(idInventario),
    foreign key (idUsuarioResponsable) references Usuario(idUsuario)
);

declare @idUsuario int;
declare @idSedePrincipal int;
declare @idSedeNorte int;

insert into Usuario (nombre, correo, contrasena, rol, estadoActivo)
values ('Empleado Prueba', 'empleado@sunpincol.com', '123456', 'Empleado', 1);

set @idUsuario = scope_identity();

insert into Sede (nombreSede, estadoSede)
values ('Sede Principal', 'Activa');

set @idSedePrincipal = scope_identity();

insert into Sede (nombreSede, estadoSede)
values ('Sede Norte', 'Activa');

set @idSedeNorte = scope_identity();

insert into Empleado (idEmpleado, idSedeAsignada, cargo, turno)
values (@idUsuario, @idSedePrincipal, 'Auxiliar', 'Mañana');

insert into Producto (codigoProducto, nombreProducto, categoria, descripcion, precioUnitario, estadoProducto)
values
('PROD001', 'Teclado mecanico', 'Perifericos', 'Teclado mecanico', 120000, 1),
('PROD002', 'Mouse inalambrico', 'Perifericos', 'Mouse inalambrico', 65000, 1),
('PROD003', 'Monitor 24 pulgadas', 'Monitores', 'Monitor de 24 pulgadas', 550000, 1);

insert into Inventario (idSede, codigoProducto, stockActual, stockMinimo, fechaUltimaActualizacion)
values
(@idSedePrincipal, 'PROD001', 15, 5, getdate()),
(@idSedePrincipal, 'PROD002', 20, 5, getdate()),
(@idSedePrincipal, 'PROD003', 8, 3, getdate()),
(@idSedeNorte, 'PROD001', 7, 5, getdate()),
(@idSedeNorte, 'PROD002', 12, 5, getdate()),
(@idSedeNorte, 'PROD003', 4, 3, getdate());

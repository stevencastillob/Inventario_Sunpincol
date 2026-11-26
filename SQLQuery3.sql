create table Bodeguero (
    idBodeguero int identity(1,1) primary key,
    idEmpleado int not null,
    idBodegaAsignada int not null,
    zonaAlmacenamiento varchar(60) not null,
    turno varchar(30) not null,
    foreign key (idEmpleado) references Empleado(idEmpleado),
    foreign key (idBodegaAsignada) references Sede(idSede)
);

create table Pedido (
    idPedido int identity(1,1) primary key,
    idSedeSolicitante int not null,
    idEmpleadoSolicitante int not null,
    codigoProducto varchar(30) not null,
    cantidadSolicitada int not null,
    estadoPedido varchar(30) not null,
    fechaSolicitud datetime2 not null,
    foreign key (idSedeSolicitante) references Sede(idSede),
    foreign key (idEmpleadoSolicitante) references Empleado(idEmpleado),
    foreign key (codigoProducto) references Producto(codigoProducto)
);

create table GuiaDespacho (
    idGuiaDespacho int identity(1,1) primary key,
    idPedido int not null,
    idBodegueroEmisor int not null,
    numeroGuia varchar(50) not null,
    fechaEmision datetime2 not null,
    estadoEntrega varchar(30) not null,
    foreign key (idPedido) references Pedido(idPedido),
    foreign key (idBodegueroEmisor) references Bodeguero(idBodeguero)
);

create table Recepcion (
    idRecepcion int identity(1,1) primary key,
    idPedido int not null,
    idGuiaDespacho int not null,
    idEmpleadoReceptor int not null,
    cantidadRecibida int not null,
    conformidad bit not null,
    fechaRecepcion datetime2 not null,
    foreign key (idPedido) references Pedido(idPedido),
    foreign key (idGuiaDespacho) references GuiaDespacho(idGuiaDespacho),
    foreign key (idEmpleadoReceptor) references Empleado(idEmpleado)
);

create table Historial (
    idHistorial int identity(1,1) primary key,
    idPedido int not null,
    idUsuarioResponsable int not null,
    estadoAnterior varchar(30) not null,
    estadoActual varchar(30) not null,
    fechaRegistro datetime2 not null,
    foreign key (idPedido) references Pedido(idPedido),
    foreign key (idUsuarioResponsable) references Usuario(idUsuario)
);
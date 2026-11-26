
declare @sede1 int;
declare @sede2 int;
declare @usuario int;

insert into Usuario (nombre, correo, contrasena, rol, estadoActivo)
values ('Empleado Prueba', 'empleado@sunpincol.com', '123456', 'Empleado', 1);

set @usuario = scope_identity();

insert into Sede (nombreSede, estadoSede)
values ('Sede Principal', 'Activa');

set @sede1 = scope_identity();

insert into Sede (nombreSede, estadoSede)
values ('Sede Norte', 'Activa');

set @sede2 = scope_identity();

insert into Empleado (idEmpleado, idSedeAsignada, cargo, turno)
values (@usuario, @sede1, 'Auxiliar', 'Mañana');

insert into Producto (codigoProducto, nombreProducto, categoria, descripcion, precioUnitario, estadoProducto)
values
('PROD001', 'Teclado mecanico', 'Perifericos', 'Teclado mecanico USB', 120000, 1),
('PROD002', 'Mouse inalambrico', 'Perifericos', 'Mouse inalambrico USB', 65000, 1),
('PROD003', 'Monitor 24 pulgadas', 'Monitores', 'Monitor LED de 24 pulgadas', 550000, 1);

insert into Inventario (idSede, codigoProducto, stockActual, stockMinimo, fechaUltimaActualizacion)
values
(@sede1, 'PROD001', 15, 5, getdate()),
(@sede1, 'PROD002', 20, 5, getdate()),
(@sede1, 'PROD003', 8, 3, getdate()),
(@sede2, 'PROD001', 7, 5, getdate()),
(@sede2, 'PROD002', 12, 5, getdate()),
(@sede2, 'PROD003', 4, 3, getdate());

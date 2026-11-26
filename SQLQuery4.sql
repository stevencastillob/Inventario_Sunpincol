select
    t.name as tabla,
    c.name as columna,
    ty.name as tipo,
    c.precision,
    c.scale,
    c.is_identity
from sys.tables t
inner join sys.columns c on t.object_id = c.object_id
inner join sys.types ty on c.user_type_id = ty.user_type_id
where t.name in ('Usuario', 'Sede', 'Producto', 'Empleado', 'Jefe', 'Catalogo', 'Guia', 'Inventario', 'Inconsistencia', 'Cambios')
and (ty.name in ('decimal', 'numeric') or c.is_identity = 1)
order by t.name, c.column_id;
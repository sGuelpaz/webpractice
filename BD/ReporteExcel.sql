create proc sp_reporte_cliente4(
@FechaInicio varchar(10),
@FechaFin varchar(10)
)
as
begin


select Documento, Nombre, Apellidos, convert(char(10), FechaNac, 103)[FechaNac], Direccion, Celular, Genero, Deporte, Trabaja, Sueldo, Estado, convert(char(10), FechaReg, 103)[FechaRegistro] from Registro
where FechaReg between @FechaInicio and @FechaFin

end

exec sp_reporte_cliente4 '14/09/2022' , '30/09/2022'
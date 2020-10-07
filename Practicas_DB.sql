create database practicas;
use practicas;

create table Tbl_Programas (
Id_Programa int primary key identity,
Nombre varchar(50));

create table Tbl_Fichas(
Id_Ficha int primary key identity,
Numero_Ficha int,
Id_Programa int foreign key references Tbl_Programas(Id_Programa));

create table Tbl_Roles(
Id_Rol int primary key identity,
Rol varchar(15));

create table Tbl_Usuarios(
NombreUsuario varchar(50) primary key,
Contrasena varchar(200),
Id_Rol int foreign key references Tbl_Roles(Id_Rol));

create table Tbl_Instructores(
Numero_Identificacion int primary key,
Nombres varchar(50),
Apellidos varchar(50),
Id_Usuario varchar(50) foreign key references Tbl_Usuarios(NombreUsuario));

create table Tbl_Aprendices(
Numero_Identificacion int primary key,
Nombres varchar(25),
Apellidos varchar(25),
Id_Ficha int foreign key references Tbl_Fichas(Id_Ficha),
Telefono varchar(25),
Correo varchar(50));

create table Tbl_Empresas(
Id_Empresa int primary key identity,
Razon_Social varchar(50),
Nit varchar(25),
Direccion varchar(50),
Nombre_Jefe varchar(25),
Cargo varchar(25),
Telefono varchar(25),
Correo varchar(50),
Id_Aprendiz int foreign key references Tbl_Aprendices(Numero_Identificacion));

create table Tbl_Visitas(
Id_Visita int primary key identity,
Id_Instructor int foreign key references Tbl_Instructores(Numero_Identificacion),
Numero_Ficha int foreign key references Tbl_Fichas(Id_Ficha));

create table Tbl_Planeacion_Etapa_Productiva(
Id_Planeacion int primary key identity,
Actividades_A_Desarrollar text,
Evidencias_Aprendizaje text,
Fecha date,
Lugar varchar(50),
Observaciones text,
Id_Aprendiz int foreign key references Tbl_Aprendices(Numero_Identificacion));

create table Tbl_Actitud_Comportamiento(
Id_Actitud int primary key identity,
Tipo_Informe int,
Relaciones_Interpersonales text,
Valoracion_Relaciones varchar(15),
TrabajoEnEquipo text,
Valoracion_Trabajo varchar(15),
Solucion_De_Problemas text,
Valoracion_Solucion varchar(15),
Cumplimiento text,
Valoracion_Cumplimiento varchar(15),
Organizacion text,
Valoracion_Organizacion varchar(15),
Id_Aprendiz int foreign key references Tbl_Aprendices(Numero_Identificacion));

create table Tbl_Factores_Tecnicos(
Id_Factores_Tecnicos int primary key identity,
Tipo_Informe int,
Transferencia_Conocimiento text,
Valoracion_Conocimiento varchar(15),
Mejora_Continua text,
Valoracion_Mejora varchar(15),
Fortalecimiento_Ocupacional text,
Valoracion_Fortalecimiento varchar(15),
Oportunidad_Calidad text,
Valoracion_Oportunidad varchar(15),
Responsabilidad_Ambiental text,
Valoracion_Ambiental varchar(15),
Administracion_Recursos text,
Valoracion_Administracion varchar(15),
Seguridad_Ocupacional text,
Valoracion_Seguridad varchar(15),
Documentacion_Etapa_Productiva text,
Valoracion_Etapa_Productiva varchar(15),
Observacion_Jefe text,
Observacion_Aprendiz text,
Id_Aprendiz int foreign key references Tbl_Aprendices(Numero_Identificacion));

create table Tbl_Evaluacion_Etapa_Productiva(
Id_Evaluacion int primary key identity,
Juicio_Evaluacion varchar(15),
Reconocimientos text,
Id_Aprendiz int foreign key references Tbl_Aprendices(Numero_Identificacion));

insert into Tbl_Roles values ('Administrador');
insert into Tbl_Roles values ('Instructor');
insert into Tbl_Roles values ('Aprendiz');

insert into Tbl_Usuarios values ('Jasa','SgBhAHMAYQAyADQAMQA=',1);
/*El usuario administrador por defecto es Jasa y su contraseña es Jasa241*/
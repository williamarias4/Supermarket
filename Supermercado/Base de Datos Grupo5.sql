-- INTEGRANTES:
-- Kenneth Ariel Chaves Herrera
-- Randall Sandoval Chavarría
-- William Arias Bermúdez
-- Brandon Rodríguez Rodríguez

ALTER SESSION SET "_Oracle_SCRIPT"=true;

CREATE USER SUPER IDENTIFIED BY superUsuario;

GRANT ALL PRIVILEGES TO SUPER;

CREATE TABLE SUPER.ROL
(
	PK_IDROL NUMBER(10),
	DESCRIPCION VARCHAR2(200),
	CONSTRAINT ROL_PK PRIMARY KEY (PK_IDROL)
);

CREATE TABLE SUPER.AREA
(
	PK_IDAREA NUMBER(10), 
	DESCRIPCION VARCHAR2(200),
	CONSTRAINT AREA_PK PRIMARY KEY (PK_IDAREA)
);

CREATE TABLE SUPER.USUARIO
(
	PK_IDUSUARIO NUMBER(10),
	FK_IDROL NUMBER(10),
	NOMBREUSUARIO VARCHAR2(50),
	CONTRASENA VARCHAR2(500),
	NOMBRE VARCHAR2(50),
	APELLIDO1 VARCHAR2(50),
	APELLIDO2 VARCHAR2(50),
	CONSTRAINT USUARIO_PK PRIMARY KEY (PK_IDUSUARIO),
	CONSTRAINT USUARIO_ROL_FK FOREIGN KEY (FK_IDROL) REFERENCES SUPER.ROL(PK_IDROL)
	
);

CREATE TABLE SUPER.GERENTEAREA
(
	PK_IDGERENTEAREA NUMBER(10),
	FK_IDUSUARIO NUMBER(10),
	FK_IDAREA NUMBER(10),
	CONSTRAINT GERENTEAREA_PK PRIMARY KEY (PK_IDGERENTEAREA),
	CONSTRAINT GERENTEAREA_USUARIO_FK FOREIGN KEY (FK_IDUSUARIO) REFERENCES SUPER.USUARIO(PK_IDUSUARIO),
	CONSTRAINT GERENTEAREA_AREA_FK FOREIGN KEY (FK_IDAREA) REFERENCES SUPER.AREA(PK_IDAREA)
);

CREATE TABLE SUPER.CAJERO
(
	PK_IDCAJERO NUMBER(10),
	FK_IDUSUARIO NUMBER(10),
	NUMEROCAJA NUMBER(2),
	CONSTRAINT CAJERO_PK PRIMARY KEY (PK_IDCAJERO),
	CONSTRAINT CAJERO_USUARIO_FK FOREIGN KEY (FK_IDUSUARIO) REFERENCES SUPER.USUARIO(PK_IDUSUARIO)
);

CREATE TABLE SUPER.PRODUCTO
(
	PK_IDPRODUCTO NUMBER(10),
	FK_IDAREA NUMBER(10),
	EAN NUMBER(13),
	DESCRIPCION VARCHAR2(200),
	PRECIO NUMBER(10, 2),
	CANTIDAD NUMBER(10),
	CONSTRAINT PRODUCTO_PK PRIMARY KEY (PK_IDPRODUCTO),
	CONSTRAINT PRODUCTO_AREA_FK FOREIGN KEY (FK_IDAREA) REFERENCES SUPER.AREA(PK_IDAREA)
);

CREATE TABLE SUPER.PRODUCTOFRESCO
(
	PK_IDPRODUCTOFRESCO NUMBER(10),
	FK_IDPRODUCTO NUMBER(10),
	PLU NUMBER(5),
	PESO NUMBER(5, 2),
	CONSTRAINT PRODUCTOFRESCO_PK PRIMARY KEY (PK_IDPRODUCTOFRESCO),
	CONSTRAINT PRODUCTOFRESCO_PRODUCTO_FK FOREIGN KEY (FK_IDPRODUCTO) REFERENCES SUPER.PRODUCTO(PK_IDPRODUCTO)
);

CREATE TABLE SUPER.FACTURA
(
	PK_IDFACTURA NUMBER(10),
	FK_IDCAJERO NUMBER(10),
	MONTO NUMBER(12, 2),
	FECHA DATE,
	CONSTRAINT FACTURA_PK PRIMARY KEY (PK_IDFACTURA),
	CONSTRAINT FACTURA_CAJERO_FK FOREIGN KEY (FK_IDCAJERO) REFERENCES SUPER.CAJERO(PK_IDCAJERO)
);

CREATE TABLE SUPER.FACTURAPRODUCTO
(
	PK_IDFACTURAPRODUCTO NUMBER(10),
	FK_IDFACTURA NUMBER(10),
	FK_IDPRODUCTO NUMBER(10),
	CANTIDAD NUMBER(10),
	SUBTOTAL NUMBER(10, 2),
	CONSTRAINT FACTURAPRODUCTO_PK PRIMARY KEY (PK_IDFACTURAPRODUCTO),
	CONSTRAINT FACTURAPRODUCTO_FACTURA_FK FOREIGN KEY (FK_IDFACTURA) REFERENCES SUPER.FACTURA(PK_IDFACTURA),
	CONSTRAINT FACTURAPRODUCTO_PRODUCTO_FK FOREIGN KEY (FK_IDPRODUCTO) REFERENCES SUPER.PRODUCTO(PK_IDPRODUCTO)
);

CREATE TABLE SUPER.BITACORAMOVIMIENTOS
(
	PK_IDBITACORAMOVIMIENTOS NUMBER(10),
	IDUSUARIO NUMBER(10),
	NOMBREUSUARIO VARCHAR2(50),
	TABLA VARCHAR2(50),
	TIPOTRANSACCION VARCHAR2(20),
	DESCRIPCION VARCHAR2(300),
	FECHA DATE,
	CONSTRAINT BITACORAMOVIMIENTOS_PK PRIMARY KEY (PK_IDBITACORAMOVIMIENTOS)
);

CREATE TABLE SUPER.BITACORACAJERO
(
	PK_IDBITACORACAJERO NUMBER(10),
	IDUSUARIO NUMBER(10),
	NOMBREUSUARIO VARCHAR2(50),
	NUMEROCAJA NUMBER(2),
	IDFACTURA NUMBER(10),
	FECHA DATE,
	CONSTRAINT BITACORACAJERO_PK PRIMARY KEY (PK_IDBITACORACAJERO)
);

CREATE TABLE SUPER.BITACORAFACTURA
(
	PK_IDBITACORAFACTURA NUMBER(10),
	IDFACTURA NUMBER(10),
	EAN NUMBER(13),
	CANTIDAD NUMBER(10),
	SUBTOTAL NUMBER(10, 2),
	TOTAL NUMBER(12, 2),
	IDUSUARIO NUMBER(10),
	NOMBREUSUARIO VARCHAR2(50),
	FECHA DATE,
	CONSTRAINT BITACORAFACTURA_PK PRIMARY KEY (PK_IDBITACORAFACTURA)
);

--Aqui hay que desconectarse del usuario system y conectarse con el usuario super (PARA QUE LO HAGA EN EL ESQUEMA DE SUPER).

create or replace procedure insertar_producto_sp 
(
  p_pk_idproducto in number 
, p_fk_idarea in number 
, p_ean in number 
, p_descripcion in varchar2 
, p_precio in number 
, p_cantidad in number 
) as 
begin
  insert into SUPER.producto(pk_idproducto, fk_idarea, ean, descripcion, precio, cantidad) values(p_pk_idproducto, p_fk_idarea, p_ean, p_descripcion, p_precio, p_cantidad);
  commit;
end insertar_producto_sp;

create or replace procedure actualizar_producto_sp 
(
p_fk_idarea in number 
, p_ean in number 
, p_descripcion in varchar2 
, p_precio in number 
, p_cantidad in number 
) as 
begin
  update SUPER.producto set fk_idarea = p_fk_idarea, ean = p_ean, descripcion = p_descripcion, precio = p_precio, cantidad = p_cantidad;
  commit;
end actualizar_producto_sp;

create or replace procedure eliminar_producto_sp 
(
  p_pk_idproducto in number 
) as 
begin
  delete from SUPER.producto where  pk_idproducto = p_pk_idproducto;
  commit;
end eliminar_producto_sp;

create or replace procedure insertar_area_sp 
(
  p_pk_idarea in number 
, p_descripcion in varchar2
) as 
begin
  insert into SUPER.area(pk_idarea, descripcion) values(p_pk_idarea, p_descripcion);
  commit;
end insertar_area_sp;

create or replace procedure actualizar_area_sp 
(
p_descripcion in varchar2
) as 
begin
  update SUPER.area set descripcion = p_descripcion;
  commit;
end actualizar_area_sp;

create or replace procedure eliminar_area_sp 
(
  p_pk_idarea in number 
) as 
begin
  delete from SUPER.area where  pk_idarea = p_pk_idarea;
  commit;
end eliminar_area_sp;

create or replace procedure insertar_bitacoracajero_sp 
(
  p_pk_idbitacoracajero in number 
, p_idusuario in number
, p_nombreusuario in VARCHAR2
, p_numerocaja in number
, p_idfactura in number
, p_fecha in date
) as 
begin
  insert into SUPER.bitacoracajero(pk_idbitacoracajero, idusuario, nombreusuario, numerocaja, idfactura, fecha) values(p_pk_idbitacoracajero, p_idusuario, p_nombreusuario, p_numerocaja, p_idfactura, p_fecha);
  commit;
end insertar_bitacoracajero_sp;

create or replace procedure actualizar_bitacoracajero_sp 
(
p_idusuario in number, p_nombreusuario in varchar2, p_numerocaja in number, p_idfactura in number, p_fecha in date
) as 
begin
  update SUPER.bitacoracajero set idusuario = p_idusuario, nombreusuario = p_nombreusuario, numerocaja = p_numerocaja, idfactura = p_idfactura, fecha = p_fecha;
  commit;
end actualizar_bitacoracajero_sp;

create or replace procedure eliminar_bitacoracajero_sp 
(
  p_pk_idbitacoracajero in number 
) as 
begin
  delete from SUPER.bitacoracajero where  pk_idbitacoracajero = p_pk_idbitacoracajero;
  commit;
end eliminar_bitacoracajero_sp;

--Agregar Insertar BITACORACAJERO

create or replace procedure actualizar_bitacorafactura_sp 
(
p_idfactura in number, p_ean in number, p_cantidad in number, p_subtotal in number, p_total in number, p_idusuario in number, p_nombreusuario in varchar2, p_fecha in date
) as 
begin
  update SUPER.bitacorafactura set idfactura = p_idfactura, ean = p_ean, cantidad = p_cantidad, subtotal = p_subtotal, total = p_total, idusuario = p_idusuario, nombreusuario = p_nombreusuario, fecha = p_fecha;
  commit;
end actualizar_bitacorafactura_sp;

create or replace procedure eliminar_bitacorafactura_sp 
(
  p_pk_idbitacorafactura in number 
) as 
begin
  delete from SUPER.bitacorafactura where  pk_idbitacorafactura = p_pk_idbitacorafactura;
  commit;
end eliminar_bitacorafactura_sp;

create or replace procedure insertar_bitacoramovimientos_sp 
(
  p_pk_idbitacoramovimientos in number 
, p_idusuario in number
, p_nombreusuario in varchar2
, p_tabla in varchar2
, p_tipotransaccion in varchar2
, p_descripcion in varchar2
, p_fecha in date
) as 
begin
  insert into SUPER.bitacoramovimientos(pk_idbitacoramovimientos, idusuario, nombreusuario, tabla, tipotransaccion, descripcion, fecha) values(p_pk_idbitacoramovimientos, p_idusuario, p_nombreusuario, p_tabla, p_tipotransaccion, p_descripcion, p_fecha);
  commit;
end insertar_bitacoramovimientos_sp;

create or replace procedure actualizar_bitacoramovimientos_sp 
(
p_idusuario in number, p_nombreusuario in varchar2, p_tabla in varchar2, p_tipotransaccion in varchar2, p_descripcion in varchar2, p_fecha in date
) as 
begin
  update SUPER.bitacoramovimientos set idusuario = p_idusuario, nombreusuario = p_nombreusuario, tabla = p_tabla, tipotransaccion = p_tipotransaccion, descripcion = p_descripcion, fecha = p_fecha;
  commit;
end actualizar_bitacoramovimientos_sp;

create or replace procedure eliminar_bitacoramovimientos_sp 
(
  p_pk_idbitacoramovimientos in number 
) as 
begin
  delete from SUPER.bitacoramovimientos where  pk_idbitacoramovimientos = p_pk_idbitacoramovimientos;
  commit;
end eliminar_bitacoramovimientos_sp;

create or replace procedure insertar_cajero_sp 
(
  p_pk_idcajero in number 
, p_fk_idusuario in number
, p_numerocaja in number
) as 
begin
  insert into SUPER.cajero(pk_idcajero, fk_idusuario, numerocaja) values(p_pk_idcajero, p_fk_idusuario, p_numerocaja);
  commit;
end insertar_cajero_sp;

create or replace procedure actualizar_cajero_sp 
(
p_fk_idusuario in number,  p_numerocaja in number
) as 
begin
  update SUPER.cajero set fk_idusuario = p_fk_idusuario, numerocaja = p_numerocaja;
  commit;
end actualizar_cajero_sp;

create or replace procedure eliminar_cajero_sp 
(
  p_pk_idcajero in number 
) as 
begin
  delete from SUPER.cajero where  pk_idcajero = p_pk_idcajero;
  commit;
end eliminar_cajero_sp;

create or replace procedure insertar_factura_sp 
(
  p_pk_idfactura in number 
, p_fk_idcajero in number
, p_monto in number
, p_fecha in date
) as 
begin
  insert into SUPER.factura(pk_idfactura, fk_idcajero, monto, fecha) values(p_pk_idfactura, p_fk_idcajero, p_monto, p_fecha);
  commit;
end insertar_factura_sp;

create or replace procedure actualizar_factura_sp 
(
p_fk_idcajero in number,  p_monto in number, p_fecha in date
) as 
begin
  update SUPER.factura set fk_idcajero = p_fk_idcajero, monto = p_monto, fecha = p_fecha;
  commit;
end actualizar_factura_sp;

create or replace procedure eliminar_factura_sp 
(
  p_pk_idfactura in number 
) as 
begin
  delete from SUPER.factura where  pk_idfactura = p_pk_idfactura;
  commit;
end eliminar_factura_sp;

create or replace procedure insertar_facturaproducto_sp 
(
  p_pk_idfacturaproducto in number 
, p_fk_idfactura in number
, p_fk_idproducto in number
, p_cantidad in number
, p_subtotal in number
) as 
begin
  insert into SUPER.facturaproducto(pk_idfacturaproducto, fk_idfactura, fk_idproducto, cantidad, subtotal) values(p_pk_idfacturaproducto, p_fk_idfactura, p_fk_idproducto, p_cantidad, p_subtotal);
  commit;
end insertar_facturaproducto_sp;

create or replace procedure actualizar_facturaproducto_sp 
(
p_fk_idfactura in number, p_fk_idproducto in number, p_cantidad in number, p_subtotal in number
) as 
begin
  update SUPER.facturaproducto set fk_idfactura = p_fk_idfactura, fk_idproducto = p_fk_idproducto, cantidad = p_cantidad, subtotal = p_subtotal;
  commit;
end actualizar_facturaproducto_sp;

create or replace procedure eliminar_facturaproducto_sp 
(
  p_pk_idfacturaproducto in number 
) as 
begin
  delete from SUPER.facturaproducto where  pk_idfacturaproducto = p_pk_idfacturaproducto;
  commit;
end eliminar_facturaproducto_sp;

create or replace procedure insertar_gerentearea_sp 
(
  p_pk_idgerentearea in number 
, p_fk_idusuario in number
, p_fk_idarea in number
) as 
begin
  insert into SUPER.gerentearea(pk_idgerentearea, fk_idusuario, fk_idarea) values(p_pk_idgerentearea, p_fk_idusuario, p_fk_idarea);
  commit;
end insertar_gerentearea_sp;

create or replace procedure actualizar_gerentearea_sp 
(
p_fk_idusuario in number, p_fk_idarea in number
) as 
begin
  update SUPER.gerentearea set fk_idusuario = p_fk_idusuario, fk_idarea = p_fk_idarea;
  commit;
end actualizar_gerentearea_sp;

create or replace procedure eliminar_gerentearea_sp 
(
  p_pk_idgerentearea in number 
) as 
begin
  delete from SUPER.gerentearea where  pk_idgerentearea = p_pk_idgerentearea;
  commit;
end eliminar_gerentearea_sp;

create or replace procedure insertar_productofresco_sp 
(
  p_pk_idproductofresco in number 
, p_fk_idproducto in number
, p_plu in number
, p_peso in number
) as 
begin
  insert into SUPER.productofresco(pk_idproductofresco, fk_idproducto, plu, peso) values(p_pk_idproductofresco, p_fk_idproducto, p_plu, p_peso);
  commit;
end insertar_productofresco_sp;

create or replace procedure actualizar_productofresco_sp 
(
p_fk_idproducto in number, p_plu in number, p_peso in number
) as 
begin
  update SUPER.productofresco set fk_idproducto = p_fk_idproducto, plu = p_plu, peso = p_peso;
  commit;
end actualizar_productofresco_sp;

create or replace procedure eliminar_productofresco_sp 
(
  p_pk_idproductofresco in number
) as 
begin
  delete from SUPER.productofresco where  pk_idproductofresco = p_pk_idproductofresco;
  commit;
end eliminar_productofresco_sp;

create or replace procedure insertar_rol_sp 
(
  p_pk_idrol in number 
, p_descripcion in varchar2
) as 
begin
  insert into SUPER.rol(pk_idrol, descripcion) values(p_pk_idrol, p_descripcion);
  commit;
end insertar_rol_sp;

create or replace procedure actualizar_rol_sp 
(
p_descripcion in varchar2
) as 
begin
  update SUPER.rol set descripcion = p_descripcion;
  commit;
end actualizar_rol_sp;

create or replace procedure eliminar_rol_sp 
(
  p_pk_idrol in number
) as 
begin
  delete from SUPER.rol where  pk_idrol = p_pk_idrol;
  commit;
end eliminar_rol_sp;

create or replace procedure insertar_usuario_sp 
(
  p_pk_idusuario in number 
, p_fk_idrol in number
, p_nombreusuario in varchar2
, p_contrasena in varchar2
, p_nombre in varchar2
, p_apellido1 in varchar2
, p_apellido2 in varchar2
) as 
begin
  insert into SUPER.usuario(pk_idusuario, fk_idrol, nombreusuario, contrasena, nombre, apellido1, apellido2) values(p_pk_idusuario, p_fk_idrol, p_nombreusuario, p_contrasena, p_nombre, p_apellido1, p_apellido2);
  commit;
end insertar_usuario_sp;

create or replace procedure actualizar_usuario_sp 
(
p_fk_idrol in number, p_nombreusuario in varchar2, p_contrasena in varchar2, p_nombre in varchar2, p_apellido1 in varchar2, p_apellido2 in varchar2
) as 
begin
  update SUPER.usuario set fk_idrol = p_fk_idrol, nombreusuario = p_nombreusuario, contrasena = p_contrasena, nombre = p_nombre, apellido1 = p_apellido1, apellido2 = p_apellido2;
  commit;
end actualizar_usuario_sp;

create or replace procedure eliminar_usuario_sp 
(
  p_pk_idusuario in number
) as 
begin
  delete from SUPER.usuario where  pk_idusuario = p_pk_idusuario;
  commit;
end eliminar_usuario_sp;
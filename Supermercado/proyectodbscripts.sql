create or replace procedure actualizar_producto_sp 
(
p_pk_idproducto in number, p_fk_idarea in number, p_ean in number, p_descripcion in varchar2, p_precio in number, p_cantidad in number
) as 
begin
  update SUPER.producto set producto.fk_idarea = p_fk_idarea, producto.ean = p_ean, producto.descripcion = p_descripcion, producto.precio = p_precio, producto.cantidad = p_cantidad
  where producto.pk_idproducto = p_pk_idproducto;
  commit;
end actualizar_producto_sp;

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

create or replace procedure eliminar_producto_sp 
(
  p_pk_idproducto in number 
) as 
begin
  delete from SUPER.producto where  pk_idproducto = p_pk_idproducto;
  commit;
end eliminar_producto_sp;

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
p_pk_idproducto in number, p_plu in number, p_peso in number
) as 
begin
  update SUPER.productofresco set productofresco.plu = p_plu, productofresco.peso = p_peso where productofresco.fk_idproducto = p_pk_idproducto;
  commit;
end actualizar_productofresco_sp;

create or replace procedure eliminar_productofresco_sp 
(
  p_pk_idproducto in number
) as 
begin
  delete from SUPER.productofresco where  fk_idproducto = p_pk_idproducto;
  commit;
end eliminar_productofresco_sp;

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
p_pk_idarea in number, p_descripcion in varchar2
) as 
begin
  update SUPER.area set descripcion = p_descripcion where pk_idarea = p_pk_idarea;
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
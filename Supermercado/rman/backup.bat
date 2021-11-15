cd cd:/
sqlplus /nolog
conn super as sysdba
superUsuario
alter session set"_oracle_script"=true;
shutdown immediate
startup mount
alter database archivelog;
alter database open;
exit
rman
connect target super/superUsuario
run{
allocate channel c1 device type DISK format 'c:\respaldos/_xd_%u_%t.back';
backup database current controlfile plus archivelog delete all input;
}



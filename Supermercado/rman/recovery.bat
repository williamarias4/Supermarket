cd cd:/
sqlplus /nolog
conn super as sysdba
superUsuario
alter session set"_oracle_script"=true;
shutdown immediate
startup mount
alter database noarchivelog;
alter database open;
shutdow immediate
startup nomount
exit
rman
set DBID 2964902100
connect target
restore controlfile from "C:\respaldos\_XD_060E5OFE_1088610798.BACK";
startup mount;
restore database;
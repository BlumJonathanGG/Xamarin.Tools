﻿using SQLHelper;
using SQLHelper.Interfaces;
using SyncService.Daemon.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;


namespace SyncService.Daemon.Abstractions
{
    public class Pendientes
    {
        public AccionDemonio Accion { get; set; }
        public object LLave { get; private set; }
        //public string Campo { get; private set; }
        public string Tabla { get; private set; }
        public int Id { get; private set; }
        public Pendientes(AccionDemonio Accion, object LLave, string Tabla, int Id)
        {
            this.Accion = Accion;
            this.LLave = LLave;
            //this.Campo = Campo.ToUpper();
            this.Tabla = Tabla.ToUpper();
            this.Id = Id;
        }

        internal void Sincronizado(DaemonConfig config, DireccionDemonio direccion)
        {
            try
            {
                BaseSQLHelper connection = null;
                switch (direccion)
                {
                    case DireccionDemonio.TO_DESTINY:
                        connection = config.Source;
                        break;
                    case DireccionDemonio.TO_ORIGIN:
                        connection = config.Destination;
                        break;
                }
                if (connection is SQLH SQLH)
                {
                    SQLH.EXEC("INSERT INTO DESCARGAS_VERSIONES(ID_DESCARGA,ID_DISPOSITIVO) VALUES(@ID_DESCARGA,@ID_DISPOSITIVO)"
                        , System.Data.CommandType.Text, false,
                        new SqlParameter("ID_DESCARGA", Id),
                        new SqlParameter("ID_DISPOSITIVO", Daemon.DeviceId));
                }
                else if (connection is SQLHLite SQLHLite)
                {
                    SQLHLite.EXEC($"DELETE FROM VERSION_CONTROL WHERE ID=?", Id);
                }
            }
            catch (Exception ex)
            {
                Log.LogMeDemonio(ex, "Al marcar como finalizada la sincronización");
            }
        }
    }
}

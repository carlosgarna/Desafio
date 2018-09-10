using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace Logs
{
    public class GuardaLogs
    {
        private static bool _enArchivo;
        private static bool _toConsole;
        private static bool _mensajEe;
        private static bool _advertencia;
        private static bool _error;
        private static bool toDataBase;
        public GuardaLogs() : this(true, false, true, false, true, true)
        {
        }
        public GuardaLogs(bool enArchivo, bool enConsola, bool mensaje,
        bool advertencia, bool error, bool EnBaseDeDatos)
        {
            _enArchivo = enArchivo;
            _toConsole = enConsola;
            _mensajEe = mensaje;
            toDataBase = EnBaseDeDatos;
            _advertencia = advertencia;
            _error = error;
        }
        public static void LogMessage(string mensaje, bool message1, bool warning
        , bool error)
        {
            int t = -10;
            string l = "inicializar string";


            mensaje.Trim();
            if (mensaje == null || mensaje.Length == 0)
            {
                return;
            }
            if (!_toConsole && !_enArchivo && !toDataBase)
            {
                throw new Exception("Configuracion invalida");
            }
            if ((!_error && !message1 && !_advertencia) || (!message1 && !warning && !error))
            {
                throw new Exception("Debe especificar el nivel de error");
            }
            
            if (message1 && _mensajEe)
            {
                t = 1;
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (error && _error)
            {
                t = 2;
                Console.ForegroundColor = ConsoleColor.Red;
            }
            if (warning && _advertencia)
            {
                t = 3;
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            string sentenciaSql = "Insert into Logger Values('" + mensaje + "', " + t.ToString() + ")";
            SqlConnection connection = new SqlConnection(CadenaConexion());
            connection.Open();
            SqlCommand command = new SqlCommand(sentenciaSql, connection);
            command.ExecuteNonQuery();

            if(!File.Exists(ArchivoLog()))
            {
                FileStream fs = File.Create(ArchivoLog());
                fs.Dispose();
            }
            else
            {
                l = File.ReadAllText(ArchivoLog());
            }
            
            l = l + FechaActual() + mensaje;
            File.WriteAllText(ArchivoLog(),l);
            Console.WriteLine(FechaActual() + mensaje);
        }

        public static string FechaActual()
        {
            return DateTime.Now.ToString("dd-MM-yyyy");
        }

        public static string ArchivoLog()
        {
            return ConfigurationManager.AppSettings["CarpetaLogs"] + "DocLog" + FechaActual() + ".txt";
        }

        public static string CadenaConexion()
        {
            return ConfigurationManager.AppSettings["CadenaDeConexion"];
        }
    }
}

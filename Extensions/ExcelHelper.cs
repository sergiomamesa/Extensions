using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;


namespace Extensions
{
    public class ExcelHelper
    {
        /// <summary>
        /// Devuelve un datatable a partir de un excel, debemos seleccionar la pestaña e introducir la columna clave para eliminar filas en blanco
        /// </summary>
        /// <param name="strFile">Path al fichero</param>
        /// <param name="sheetName">Pestaña de la hoja de calculos</param>
        /// <param name="keyColumn">Columna clave se eliminaran todas las filas en las que esta casilla no este valorizada</param>
        /// <returns></returns>
        public static System.Data.DataTable GetDataTableFromExcel(string strFile, string sheetName, string keyColumn = "")
        {
            var con = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;TypeGuessRows=0;HDR=Yes;ImportMixedTypes=Text;'", strFile);
            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                var command = new OleDbCommand("Select * from [{0}$]".FormatWith(sheetName), connection);
                if (keyColumn.HasValue())
                    command = new OleDbCommand("Select * from [{0}$] where {1} is not null".FormatWith(sheetName, keyColumn), connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    var table = new System.Data.DataTable();
                    table.Load(dr);
                    return table;
                }
            }
        }
    }
}

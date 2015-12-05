using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AppConsola.ServiciosConsola;

namespace AppConsola
{
    class Program
    {
        static ServiciosConsola.ServiciosConsola soapConsola = new ServiciosConsola.ServiciosConsola();

        static void Main(string[] args)
        {
            string username = "", password = "";
            
            Console.WriteLine("Bienvenido al aplicativo de configración de Ciudad contra la delincuencia");
            Console.WriteLine("Antes de empezar es necesario que proporcione sus credenciales de acceso");
            Console.WriteLine("Nombre de usuario");
            username = Console.ReadLine();
            Console.WriteLine("Contraseña");
            password = getPassword();
            bool uservalid = soapConsola.ValidarUsuarioConsola(username, password);
            while (!uservalid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error al iniciar sesión. Verifique los datos ingresados");
                Console.ResetColor();
                Console.WriteLine("Nombre de usuario");
                username = Console.ReadLine();
                Console.WriteLine("Contraseña");
                password = getPassword();
                uservalid = soapConsola.ValidarUsuarioConsola(username, password);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Acceso concedido. Bienvenido "+ username);
            Console.ResetColor();
            int opcion = mostrarMenu();
            while (opcion <= 4)
            {
                switch (opcion)
                {
                    case 1:
                        configurarParametros();
                        break;
                    case 2:
                        administrarModeradores();
                        break;
                    case 3:
                        administrarSectores();
                        break;
                    case 4:
                        administrarCategorias();
                        break;
                }
                opcion = mostrarMenu();
            }
            Console.WriteLine("Ha salido exitosamente. Presione cualquier recla para continuar");
            Console.ReadKey();
        }

        public static string getPassword()
        {
            string pwd = "";
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Length > 0)
                    {
                        pwd = pwd.Substring(0, pwd.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    pwd += i.KeyChar.ToString();
                    Console.Write("*");
                }
            }
            Console.Write("\n");
            return pwd.ToString();
        }

        public static int mostrarMenu()
        {
            int opc = 0;
            Console.WriteLine("Seleccione la opcion deseada");
            Console.WriteLine("1. Configurar parámetros del sistema");
            Console.WriteLine("2. Administrar moderadores");
            Console.WriteLine("3. Administrar sectores de ciudad");
            Console.WriteLine("4. Administrar categorias de delitos");
            Console.WriteLine("5. Salir");
            do
            {
                Console.WriteLine("Seleccione una opcion valida");
                opc = int.Parse(Console.ReadLine());
            } while (opc <= 0 || opc > 5);
            return opc;
        }

        public static int mostrarSubMenu1()
        {
            int opc = 0;
            Console.WriteLine("Seleccione la opcion deseada");
            Console.WriteLine("1. Crear o editar moderador");
            Console.WriteLine("2. Eliminar moderador");
            Console.WriteLine("3. Volver al menu anterior");
            do
            {
                Console.WriteLine("Seleccione una opcion valida");
                opc = int.Parse(Console.ReadLine());
            } while (opc <= 0 || opc > 3);
            return opc;
        }

        public static int mostrarSubMenu2()
        {
            int opc = 0;
            Console.WriteLine("Seleccione la opcion deseada");
            Console.WriteLine("1. Crear o editar sector");
            Console.WriteLine("2. Eliminar sector");
            Console.WriteLine("3. Volver al menu anterior");
            do
            {
                Console.WriteLine("Seleccione una opcion valida");
                opc = int.Parse(Console.ReadLine());
            } while (opc <= 0 || opc > 3);
            return opc;
        }

        public static int mostrarSubMenu3()
        {
            int opc = 0;
            Console.WriteLine("Seleccione la opcion deseada");
            Console.WriteLine("1. Crear o editar categoria");
            Console.WriteLine("2. Eliminar categoria");
            Console.WriteLine("3. Volver al menu anterior");
            do
            {
                Console.WriteLine("Seleccione una opcion valida");
                opc = int.Parse(Console.ReadLine());
            } while (opc <= 0 || opc > 3);
            return opc;
        }

        public static void configurarParametros()
        {
            int[] actuales = soapConsola.ObtenerParametrosConfiguracion();
            int[] futuros = new int[2];
            Console.WriteLine("Frecuencia de subida de videos en minutos (Actual : "+actuales[0]+" )");
            int p = int.Parse(Console.ReadLine());
            futuros[0] = p >= 0 ? p : actuales[0];
            Console.WriteLine("Frecuencia de backup en horas (Actual : " + actuales[1] + " )");
            p = int.Parse(Console.ReadLine());
            futuros[1] = p >= 0 ? p : actuales[1];
            if (soapConsola.ActualizarParametros(futuros))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Los parámetros se han actualizado correctamente. Para aplicar debe reiniciar los servidores");
                Console.ResetColor();
            }
            else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hubo un error al actulizar. Intente más tarde");
                Console.ResetColor();
            }
            Console.WriteLine("Presione cualquier tecla para continuar");
            Console.ReadKey();
        }

        public static void administrarModeradores()
        {
            int opcion = mostrarSubMenu1();
            switch (opcion)
            {
                case 1:
                    crearModerador();
                    break;
                case 2:
                    eliminarModerador();
                    break;
            }
            Console.WriteLine("Presione cualquier tecla para continuar");
            Console.ReadKey();
        }

        public static void crearModerador()
        {
            string nick = "", email = "";
            Console.WriteLine("Digite el nickname del moderador");
            nick = Console.ReadLine();

            string emailactual = "";
            DataTable dt = soapConsola.ObtenerInfoUsuario(nick);
            if (dt.Rows.Count > 0)
            {
                emailactual = "(Actual: " + dt.Rows[0]["email"].ToString() + " )";
            }

            Console.WriteLine("Digite el email del moderador " + emailactual);
            email = Console.ReadLine();

            

            bool respo = soapConsola.CrearEditarModerador(nick, email);

            if (respo)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("El moderador se ha creado/actualizado correctamente");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hubo un error en el servidor. Intente más tarde");
                Console.ResetColor();
            }
        }

        public static void eliminarModerador()
        {
            string nick = "";
            Console.WriteLine("Digite el nickname del moderador a eliminar");
            nick = Console.ReadLine();

            bool respo = soapConsola.EliminarModerador(nick);

            if (respo)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("El moderador se ha eliminado correctamente");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hubo un error en el servidor. Intente más tarde");
                Console.ResetColor();
            }
        }

        public static void administrarSectores()
        {
            int opcion = mostrarSubMenu2();
            switch (opcion)
            {
                case 1:
                    crearSector();
                    break;
                case 2:
                    eliminarSector();
                    break;
            }
            Console.WriteLine("Presione cualquier tecla para continuar");
            Console.ReadKey();
        }

        public static void crearSector()
        {
            string nombre = "";
            double[] p1 = new double[2], p2 = new double[2], p3 = new double[2], p4 = new double[2];

            Console.WriteLine("Digite el nombre del sector");
            nombre = Console.ReadLine();
            DataTable dt = soapConsola.ObtenerInfoSector(nombre);
            string actualx1 = "", actualy1 = "", actualx2 = "", actualy2 = "", actualx3 = "", actualy3 = "", actualx4 = "", actualy4 = "";
            if (dt.Rows.Count > 0)
            {
                actualx1 = "(Actual: " +dt.Rows[0]["cord11"].ToString()+ " )";
                actualy1 = "(Actual: " + dt.Rows[0]["cord12"].ToString() + " )";
                actualx2 = "(Actual: " + dt.Rows[0]["cord21"].ToString() + " )";
                actualy2 = "(Actual: " + dt.Rows[0]["cord22"].ToString() + " )";
                actualx3 = "(Actual: " + dt.Rows[0]["cord31"].ToString() + " )";
                actualy3 = "(Actual: " + dt.Rows[0]["cord32"].ToString() + " )";
                actualx4 = "(Actual: " + dt.Rows[0]["cord41"].ToString() + " )";
                actualy4 = "(Actual: " + dt.Rows[0]["cord42"].ToString() + " )";
            }

            Console.WriteLine("Digite la latitud de la esquina superior izquierda del sector" + actualx1);
            p1[0] = double.Parse(Console.ReadLine());

            Console.WriteLine("Digite la longitud de la esquina superior izquierda del sector" + actualy1);
            p1[1] = double.Parse(Console.ReadLine());

            Console.WriteLine("Digite la latitud de la esquina superior derecha del sector" + actualx2);
            p2[0] = double.Parse(Console.ReadLine());

            Console.WriteLine("Digite la longitud de la esquina superior derecha del sector" + actualy2);
            p2[1] = double.Parse(Console.ReadLine());

            Console.WriteLine("Digite la latitud de la esquina inferior izquierda del sector" + actualx3);
            p3[0] = double.Parse(Console.ReadLine());

            Console.WriteLine("Digite la longitud de la esquina inferior izquierda del sector" + actualy3);
            p3[1] = double.Parse(Console.ReadLine());

            Console.WriteLine("Digite la latitud de la esquina inferior derecha del sector" + actualx4);
            p4[0] = double.Parse(Console.ReadLine());

            Console.WriteLine("Digite la longitud de la esquina inferior derecha del sector" + actualy4);
            p4[1] = double.Parse(Console.ReadLine());

            bool respo = soapConsola.CrearEditarSector(nombre, p1[0], p1[1], p2[0], p2[1], p3[0], p3[1], p4[0], p4[1]);

            if (respo)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("El sector se ha creado/actualizado correctamente");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hubo un error en el servidor. Intente más tarde");
                Console.ResetColor();
            }
        }

        public static void eliminarSector()
        {
            string nombre = "";
            Console.WriteLine("Digite el nombre del sector a eliminar");
            nombre = Console.ReadLine();

            bool respo = soapConsola.EliminarSector(nombre);

            if (respo)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("El sector se ha eliminado correctamente");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hubo un error en el servidor. Intente más tarde");
                Console.ResetColor();
            }
        }

        public static void administrarCategorias()
        {
            int opcion = mostrarSubMenu3();
            switch (opcion)
            {
                case 1:
                    crearCategoria();
                    break;
                case 2:
                    eliminarCategoria();
                    break;
            }
            Console.WriteLine("Presione cualquier tecla para continuar");
            Console.ReadKey();
        }

        public static void crearCategoria()
        {
            string nombre = "", nombreedit ="";
            Console.WriteLine("Digite el nombre de la categoria");
            nombre = Console.ReadLine();
            DataTable dt = soapConsola.ObtenerInfoCategoria(nombre);

            if (dt.Rows.Count > 0)
            {
                Console.WriteLine("Digite el nuevo nombre de la categoría ( Valor actual : " + dt.Rows[0]["nombreCategoria"].ToString() + " )");
                nombreedit = Console.ReadLine();
            }
            
            bool respo = soapConsola.CrearEditarCategoria(nombre, nombreedit);

            if (respo)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("La categoría se ha creado/actualizado correctamente");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hubo un error en el servidor. Intente más tarde");
                Console.ResetColor();
            }
        }

        public static void eliminarCategoria()
        {
            string nombre = "";
            Console.WriteLine("Digite el nombre de la categoria a eliminar");
            nombre = Console.ReadLine();

            bool respo = soapConsola.EliminarCategoria(nombre);

            if (respo)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("La categoría se ha eliminado correctamente");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hubo un error en el servidor. Intente más tarde");
                Console.ResetColor();
            }
        }
    }
}

using Dominio;

namespace InterfazConsola
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Sistema Sis = Sistema.GetInstancia();

            int opcionMenu = -1;

            while (opcionMenu != 0)
            {
                DateTime? fecha = null;
                Console.WriteLine(fecha);

                Console.WriteLine("Bienvenido a la red social PixelMinds\n\n" +
                    "¿Que desea hacer?\n\n" +
                    "1 - Ingresar un nuevo miembro\n" +
                    "2 - Listas todas las publicaciones de un miembro por mail\n" +
                    "3 - Listar los post en los que un miembro haya hecho comentarios\n" +
                    "4 - Listas todos los post realizados dentro de un rango de fechas\n" +
                    "5 - Obtener el/los miembro/s con más publicaciones realizadas\n" +
                    "0 - Salir");

                opcionMenu = int.Parse(Console.ReadLine());

                Console.Clear();
                switch (opcionMenu)
                {
                    case 1:
                        Console.WriteLine("Sección de alta de miembro, por favor ingrese los datos que serán solicitados\n\n" +
                            "Email:");
                        string email = Console.ReadLine();
                        Console.WriteLine("Contraseña:");
                        string contra = Console.ReadLine();
                        Console.WriteLine("Nombre:");
                        string nombre = Console.ReadLine();
                        Console.WriteLine("Apellido:");
                        string apellido = Console.ReadLine();
                        Console.WriteLine("Fecha de nacimiento (yyyy-MM-dd):");
                        DateTime fechaNac = DateTime.Parse(Console.ReadLine());

                        Miembro m = new Miembro(email, contra, nombre, apellido, fechaNac);

                        try
                        {
                            Sis.AltaUsuario(m);
                            Console.Clear();
                            Console.WriteLine("Agregado con éxito");
                        }
                        catch (Exception e)
                        {
                            Console.Clear();
                            Console.WriteLine("Hubo un error al agregar el nuevo miembro\n\n" + e.Message);
                        }
                        break;
                    case 2:
                        Console.WriteLine("Sección de busqueda de publicaciones por email\n\n");
                        string emailConsultaXPublicaciones = Console.ReadLine();

                        try
                        {
                            Console.Clear();
                            Console.WriteLine("Estas son las publicaciones realizadas por el ususario " + emailConsultaXPublicaciones + "\n");
                            foreach (Publicacion p in Sis.GetPublicacionesXEmail(emailConsultaXPublicaciones))
                            {
                                Console.WriteLine($"{p}\n");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.Clear();
                            Console.WriteLine(e.Message);

                        }
                        break;
                    case 3:
                        Console.WriteLine("Sección de busqueda de Post en los que un usuario haya realizado un comentario\n" +
                            "Ingrese el email de un miembro");
                        string emailConsultaXPost = Console.ReadLine();
                        try
                        {
                            Console.Clear();
                            Console.WriteLine($"Estos son los Post donde el ususario {emailConsultaXPost} ha hecho comentarios\n\n");
                            foreach (Post post in Sis.GetPostXMail(emailConsultaXPost))
                            {
                                Console.WriteLine($"{post}\n");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.Clear();
                            Console.WriteLine(e);

                        }
                        break;
                    case 4:
                        Console.WriteLine("Sección de buscado de Posts por lapso de fecha \n" +
                            "Ingrese la primera fecha (yyyy-MM-dd)");

                        try
                        {
                            DateTime fecha1 = DateTime.Parse(Console.ReadLine());
                            Console.WriteLine("Ingrese la segunda fecha");
                            DateTime fecha2 = DateTime.Parse(Console.ReadLine());
                            try
                            {
                                Console.Clear();
                                Console.WriteLine($"Estas son los Post publicados entre {fecha1.ToString("dd-MM-yyyy")} y {fecha2.ToString("dd-MM-yyyy")}\n");
                                foreach (Post post in Sis.GetPostsXFechas(fecha1, fecha2))
                                {
                                    Console.WriteLine($"Id del Post: {post.Id} || Fecha de publicación: {post.Fecha.ToString("dd-MM-yyyy")} || Titulo: {post.Titulo}\n" +
                                        $"Contenido: {post.Contenido.Substring(0, 50)}...");
                                }
                            }
                            catch (Exception e)
                            {
                                Console.Clear();
                                Console.WriteLine(e.Message);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: Una de las fechas no fue ingresada de forma correcta");
                        }
                        break;
                    case 5:
                        Console.WriteLine("Sección de buscado de ususarios con mas publicaciones realizadas\n" +
                            "Estos son los usuarios que han realizado mas publicaciones\n\n");
                        foreach (Miembro mi in Sis.GetMiembrosConMasPublicaciones())
                        {
                            Console.WriteLine($"Miembro: {mi.Nombre} {mi.Apellido} || Email: {mi.Email} \nCantidad de publicaciones: {Sis.GetPublicacionesXEmail(mi.Email).Count}\n");
                        }

                        break;
                    case 0:
                        Console.WriteLine("Saliendo... Pulse una tecla para continuar");
                        break;
                }
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
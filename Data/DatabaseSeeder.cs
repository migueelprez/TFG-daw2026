using TFG.Models;

namespace TFG.Data
{
    public static class DatabaseSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // --- Usuarios iniciales ---
            if (!context.Usuarios.Any())
            {
                context.Usuarios.AddRange(
                    new ApplicationUser
                    {
                        Nombre = "Administrador",
                        Email = "admin@senpro.es",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                        Rol = "Admin",
                        FechaRegistro = new DateTime(2025, 1, 1),
                        Activo = true
                    },
                    new ApplicationUser
                    {
                        Nombre = "Usuario Demo",
                        Email = "usuario@senpro.es",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Rol = "Usuario",
                        FechaRegistro = new DateTime(2025, 1, 1),
                        Activo = true
                    }
                );
                context.SaveChanges();
            }

            // --- Productos ---
            if (!context.Productos.Any())
            {
                context.Productos.AddRange(

                    // ESPINILLERAS
                    new ProductoModel { Nombre = "Protector de Espinilla Ref. 007008",       Categoria = "Espinilleras", Precio = 14.95, ImagenUrl = "protector_espinilla/004DM107008 copia.png",  Descripcion = "Protector anatómico con relleno de espuma de alta densidad. Especialmente diseñado para pieles sensibles y frágiles." },
                    new ProductoModel { Nombre = "Protector de Espinilla Ref. 007012",       Categoria = "Espinilleras", Precio = 16.50, ImagenUrl = "protector_espinilla/005DM107012 copia.png",  Descripcion = "Confeccionado en tejido transpirable con cierre de velcro ajustable. Previene hematomas y lesiones por impacto." },
                    new ProductoModel { Nombre = "Protector de Espinilla Largo Ref. 107046", Categoria = "Espinilleras", Precio = 19.95, ImagenUrl = "protector_espinilla/015DM107046 copia.png",  Descripcion = "Cobertura extendida hasta el tobillo para mayor protección. Indicado para usuarios con alta actividad." },
                    new ProductoModel { Nombre = "Espinillera SENPRO Premium",               Categoria = "Espinilleras", Precio = 24.90, ImagenUrl = "protector_espinilla/espinillera.png",          Descripcion = "Modelo insignia con tecnología de absorción de choque multicapa. Tejido interior suave e hipoalergénico." },

                    // EMPAPADORES
                    new ProductoModel { Nombre = "Empapador Desechable 60×90 Ref. 107110", Categoria = "Empapadores", Precio =  9.90, ImagenUrl = "empapadores/032DM107110 copia.png", Descripcion = "Alta capacidad de absorción con núcleo superabsorbente. Superficie no tejida suave para la piel del residente." },
                    new ProductoModel { Nombre = "Empapador Desechable 60×60 Ref. 107115", Categoria = "Empapadores", Precio =  7.50, ImagenUrl = "empapadores/033DM107115 copia.png", Descripcion = "Con tratamiento de aloe vera para proteger la integridad cutánea. Barrera impermeable de alta fiabilidad." },

                    // LENCERÍA
                    new ProductoModel { Nombre = "Ropa Interior Adaptada Ref. 107098", Categoria = "Lencería", Precio = 12.95, ImagenUrl = "lenceria/029DM107098 copia.png", Descripcion = "Apertura total para facilitar la higiene diaria de personas con movilidad reducida. Tejido 100% algodón." },
                    new ProductoModel { Nombre = "Ropa Interior Adaptada Ref. 107100", Categoria = "Lencería", Precio = 14.50, ImagenUrl = "lenceria/030DM107100 copia.png", Descripcion = "Resistente al lavado industrial a 90 °C. Cintura elástica cómoda y adaptable a distintas tallas." },
                    new ProductoModel { Nombre = "Sábana Bajera Impermeable",           Categoria = "Lencería", Precio = 18.90, ImagenUrl = "lenceria/3.20_01.jpg",             Descripcion = "Acabado transpirable que protege el colchón articulado. Elástico perimetral reforzado de fácil colocación." },
                    new ProductoModel { Nombre = "Protector de Colchón Ref. 3.24",      Categoria = "Lencería", Precio = 22.50, ImagenUrl = "lenceria/3.24_03.jpg",             Descripcion = "Sistema de fijación seguro adaptable a camas articuladas geriátricas. Lavable hasta 90 °C." },

                    // MOBILIARIO
                    new ProductoModel { Nombre = "Cama Geriátrica Articulada", Categoria = "Mobiliario", Precio = 890.00, ImagenUrl = "mobiliario/Captura de pantalla 2025-09-27 a las 17.09.10.png", Descripcion = "Regulación eléctrica de altura, cabecero y piecero. Barandillas abatibles de seguridad incluidas." },
                    new ProductoModel { Nombre = "Sillón Relax Modelo Milán",  Categoria = "Mobiliario", Precio = 450.00, ImagenUrl = "mobiliario/sillon-relax-modelo-milan.jpg",                      Descripcion = "Mecanismo reclinable de fácil manejo. Estructura reforzada con tapizado resistente al uso continuado en residencias." },

                    // MOVILIDAD
                    new ProductoModel { Nombre = "Andador de Aluminio con Asiento", Categoria = "Movilidad", Precio =  89.00, ImagenUrl = "movilidad/3.jpg",                           Descripcion = "Ligero y plegable con asiento acolchado y cesta de transporte. Ruedas giratorias con freno de seguridad." },
                    new ProductoModel { Nombre = "Silla de Ruedas Modelo Bulnes",   Categoria = "Movilidad", Precio = 350.00, ImagenUrl = "movilidad/BULNES.jpg",                       Descripcion = "Estructura de acero resistente con reposapiés abatibles y reposabrazos desmontables. Plegable para transporte." },
                    new ProductoModel { Nombre = "Bastón de Apoyo Ergonómico",       Categoria = "Movilidad", Precio =  19.90, ImagenUrl = "movilidad/_MG_8970_01.jpg",                 Descripcion = "Mango anatómico antideslizante regulable en altura. Punta de goma intercambiable de alta durabilidad." },
                    new ProductoModel { Nombre = "Muleta Axilar Regulable",          Categoria = "Movilidad", Precio =  28.50, ImagenUrl = "movilidad/_MG_9234_01.jpg",                 Descripcion = "Altura ajustable con sistema de seguridad de doble pasador. Apoyo axilar acolchado para uso prolongado." },
                    new ProductoModel { Nombre = "Grúa de Transferencia",            Categoria = "Movilidad", Precio = 490.00, ImagenUrl = "movilidad/white-Photoroom-Photoroom-4.png", Descripcion = "Para traslados seguros entre cama, silla y baño. Capacidad hasta 150 kg. Arnés incluido." },

                    // ANTIESCARAS
                    new ProductoModel { Nombre = "Cojín Antiescaras Viscoelástico Ref. 107078", Categoria = "Antiescaras", Precio =  45.00, ImagenUrl = "sistemas antiescaras/023DM107078 copia.png", Descripcion = "Distribuye la presión de forma homogénea. Funda lavable con tratamiento antibacteriano." },
                    new ProductoModel { Nombre = "Colchón Antiescaras Estático Ref. 107086",    Categoria = "Antiescaras", Precio = 120.00, ImagenUrl = "sistemas antiescaras/026DM107086 copia.png", Descripcion = "Espuma de alta especificación para prevención de úlceras por presión de grado I y II. Funda impermeable." },
                    new ProductoModel { Nombre = "Sistema Otur con Compresor",                   Categoria = "Antiescaras", Precio = 195.00, ImagenUrl = "sistemas antiescaras/OTUR CON COMPRESOR.jpg",Descripcion = "Colchón de alternancia de presión con compresor silencioso. Indicado para úlceras de grado III y IV." },

                    // SUJECIÓN
                    new ProductoModel { Nombre = "Cinturón de Sujeción Abdominal Ref. 106998", Categoria = "Sujeción", Precio = 22.50, ImagenUrl = "sistemas de sujeccion/001DM106998 copia.png",           Descripcion = "Fijación segura al respaldo de la silla con sistema de apertura magnética de seguridad." },
                    new ProductoModel { Nombre = "Arnés Pélvico de Sujeción Ref. 107003",      Categoria = "Sujeción", Precio = 28.90, ImagenUrl = "sistemas de sujeccion/003DM107003 copia.png",           Descripcion = "Distribución ergonómica de la carga. Compatible con la mayoría de sillas y sillones geriátricos." },
                    new ProductoModel { Nombre = "Chaleco de Sujeción Ref. 107072",            Categoria = "Sujeción", Precio = 34.50, ImagenUrl = "sistemas de sujeccion/021DM107072 copia.png",           Descripcion = "Tejido transpirable y ajustable que mantiene la postura correcta en sedestación prolongada." },
                    new ProductoModel { Nombre = "Barrera Lateral de Cama",                    Categoria = "Sujeción", Precio = 38.00, ImagenUrl = "sistemas de sujeccion/n-Photoroom-Photoroom copia.png",  Descripcion = "Protección anti-caídas de fácil instalación. Compatible con camas articuladas de 80 y 90 cm." },
                    new ProductoModel { Nombre = "Sujeción para Silla de Ruedas",              Categoria = "Sujeción", Precio = 18.90, ImagenUrl = "sistemas de sujeccion/n-Photoroom-Photoroom.png",        Descripcion = "Cinturón de seguridad con ajuste rápido y apertura de emergencia para silla de ruedas." }
                );
                context.SaveChanges();
            }
        }
    }
}

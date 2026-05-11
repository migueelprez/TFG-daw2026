# SENPRO — Plataforma de Suministros Geriátricos

> Trabajo de Fin de Grado · DAW 2026  
> Alumno: Miguel Pérez · miguel.perez@espiralms.com

---

## Descripción del proyecto

**SENPRO** es una aplicación web de comercio electrónico B2B desarrollada en **ASP.NET Core MVC (.NET 10)** orientada a la venta de productos de protección y cuidado geriátrico a residencias de la tercera edad.

La plataforma permite a los centros geriátricos explorar el catálogo completo de productos, realizar pedidos, gestionar su cuenta y solicitar presupuestos personalizados por volumen. Dispone de un panel de administración completo para gestionar productos, usuarios, pedidos y solicitudes de presupuesto.

---

## Tecnologías utilizadas

| Capa | Tecnología |
|------|-----------|
| Framework | ASP.NET Core MVC 10 |
| Lenguaje | C# 13 |
| Base de datos | SQLite (via Entity Framework Core 10) |
| Autenticación | Cookie Authentication (Microsoft.AspNetCore.Authentication.Cookies) |
| Cifrado de contraseñas | BCrypt.Net-Next 4.0.3 |
| Pasarela de pago | Stripe Checkout Sessions (Stripe.net 47.0.0) |
| Frontend | HTML5 · CSS3 · JavaScript (Vanilla) |
| Iconos | SVG inline (sin dependencias externas) |
| Sesiones | ISession (carrito de compra en JSON) |

---

## Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Visual Studio 2022 / VS Code / cualquier editor compatible
- Cuenta de [Stripe](https://stripe.com) (para pagos; modo test gratuito)

---

## Instalación y arranque

```bash
# 1. Clonar / abrir la carpeta del proyecto
cd TFG

# 2. Restaurar dependencias
dotnet restore

# 3. Ejecutar la aplicación
dotnet run
# o con recarga en caliente:
dotnet watch run
```

La primera vez que arranca, la aplicación **crea automáticamente** la base de datos SQLite (`senpro.db`) y la puebla con datos de ejemplo (usuarios y productos).

> ⚠️ **Nota:** En modo desarrollo, cada reinicio borra y recrea la BD para aplicar cambios de esquema. Esto es intencionado durante el desarrollo. Para producción, cambiar `EnsureDeleted()` por migraciones de EF.

Accede en: `https://localhost:7XXX` (el puerto exacto aparece en la consola al arrancar).

---

## Configuración

### Stripe (pagos)

Edita `appsettings.json` y sustituye las claves de ejemplo por las tuyas (modo test):

```json
"Stripe": {
  "SecretKey": "sk_test_TU_CLAVE_SECRETA",
  "PublishableKey": "pk_test_TU_CLAVE_PUBLICA"
}
```

Las claves se obtienen en [dashboard.stripe.com](https://dashboard.stripe.com) → Developers → API Keys.  
Para simular pagos usa la tarjeta de prueba: **4242 4242 4242 4242** · Fecha: cualquiera futura · CVC: cualquiera.

### Cadena de conexión

Por defecto usa SQLite con el archivo `senpro.db` en la raíz del proyecto:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=senpro.db"
}
```

---

## Cuentas de acceso predeterminadas

| Rol | Email | Contraseña |
|-----|-------|-----------|
| Administrador | admin@senpro.es | Admin123! |
| Usuario | usuario@senpro.es | User123! |

> Estas cuentas se crean automáticamente al iniciar la aplicación por primera vez mediante el `DatabaseSeeder`.

---

## Estructura del proyecto

```
TFG/
├── Controllers/
│   ├── HomeController.cs          # Inicio, Quiénes Somos, Contacto, Presupuesto
│   ├── AccountController.cs       # Login, Registro, Logout, MisPedidos, Recuperar contraseña
│   ├── ProductosController.cs     # Catálogo, Categorías, Espinillera
│   ├── CarritoController.cs       # Carrito, Pago con Stripe, Confirmación
│   └── AdminController.cs         # Panel de administración completo
│
├── Models/
│   ├── ApplicationUser.cs         # Entidad usuario (Id, Nombre, Email, Rol, PasswordHash…)
│   ├── ProductoModel.cs           # Entidad producto (Nombre, Categoria, Precio, Stock…)
│   ├── Pedido.cs                  # Cabecera de pedido (Total, Estado, StripeSessionId…)
│   ├── LineaPedido.cs             # Líneas de detalle de pedido
│   ├── CarritoItem.cs             # Ítem de sesión del carrito
│   ├── SolicitudPresupuesto.cs    # Solicitud de presupuesto B2B
│   ├── PasswordResetToken.cs      # Token de recuperación de contraseña
│   ├── ContactoModel.cs           # Formulario de contacto
│   ├── LoginViewModel.cs          # ViewModel de login
│   └── RegisterViewModel.cs       # ViewModel de registro
│
├── Data/
│   ├── ApplicationDbContext.cs    # DbContext de Entity Framework
│   └── DatabaseSeeder.cs         # Datos iniciales (usuarios + 25 productos)
│
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml         # Layout principal (nav + footer)
│   │   └── _Breadcrumb.cshtml     # Partial de migas de pan
│   ├── Home/                      # Inicio, QuienesSomos, Contacto, Presupuesto
│   ├── Productos/                 # Catálogo, Categoria, Espinillera
│   ├── Carrito/                   # Carrito, Confirmación de pago
│   ├── Account/                   # Login, Register, MisPedidos, ForgotPassword, ResetPassword
│   └── Admin/                     # Dashboard, Productos, Usuarios, Pedidos, Presupuestos, Estadísticas
│
├── wwwroot/
│   ├── css/
│   │   ├── site.css               # Estilos globales (nav, footer, breadcrumb)
│   │   ├── home.css               # Página de inicio
│   │   ├── productos.css          # Catálogo y categorías
│   │   ├── espinillera.css        # Landing del producto estrella
│   │   ├── contacto.css           # Formulario de contacto
│   │   ├── presupuesto.css        # Formulario de presupuesto
│   │   ├── quienes-somos.css      # Página corporativa
│   │   ├── auth.css               # Login, registro y recuperación
│   │   ├── admin.css              # Panel de administración
│   │   ├── pago.css               # Carrito y confirmación de pago
│   │   └── mis-pedidos.css        # Historial de pedidos
│   └── assets/images/             # Imágenes y fotografías de productos
│
├── appsettings.json               # Configuración (BD, Stripe)
├── Program.cs                     # Bootstrap, DI, middlewares
├── README.md                      # Este archivo
└── TFG.csproj                     # Definición del proyecto y paquetes NuGet
```

---

## Funcionalidades

### Públicas (sin login)

| Funcionalidad | Ruta |
|--------------|------|
| Página de inicio | `/` |
| Quiénes Somos | `/Home/QuienesSomos` |
| Catálogo de productos | `/Productos/Productos` |
| Filtrado por categoría | `/Productos/Categoria/{id}` |
| Landing Protector de Espinilla | `/Productos/Espinillera` |
| Buscador en tiempo real | Integrado en el catálogo |
| Formulario de contacto | `/Home/Contacto` |
| Solicitud de presupuesto | `/Home/Presupuesto` |
| Iniciar sesión | `/Account/Login` |
| Registro de cuenta | `/Account/Register` |
| Recuperar contraseña | `/Account/ForgotPassword` |

### Usuarios autenticados

| Funcionalidad | Ruta |
|--------------|------|
| Añadir productos al carrito | Botón en cualquier producto |
| Ver y gestionar carrito | `/Carrito/Index` |
| Pagar con Stripe | Desde el carrito |
| Confirmación de pago | `/Carrito/Confirmacion` |
| Historial de pedidos propios | `/Account/MisPedidos` |

### Administradores

| Funcionalidad | Ruta |
|--------------|------|
| Dashboard con estadísticas | `/Admin/Index` |
| Gestión de productos (CRUD) | `/Admin/Productos` |
| Control de stock | Integrado en edición de producto |
| Gestión de usuarios | `/Admin/Usuarios` |
| Listado completo de pedidos | `/Admin/Pedidos` |
| Detalle de pedido + exportar PDF | `/Admin/DetallePedido/{id}` |
| Gestión de presupuestos B2B | `/Admin/Presupuestos` |
| Estadísticas y métricas | `/Admin/Estadisticas` |

---

## Catálogo de productos

La base de datos incluye **25 productos** distribuidos en **7 categorías**:

| Categoría | Productos |
|-----------|-----------|
| Protectores de Espinilla | 4 referencias |
| Empapadores | 2 referencias |
| Lencería Adaptada | 4 referencias |
| Mobiliario Geriátrico | 2 referencias |
| Movilidad y Ayudas | 5 referencias |
| Sistemas Antiescaras | 3 referencias |
| Sistemas de Sujeción | 5 referencias |

---

## Flujo de compra

```
1. El usuario navega el catálogo y hace clic en "Añadir al carrito"
   └─ Si no está autenticado → redirige a Login

2. En el carrito puede:
   ├─ Cambiar cantidades (+ / -)
   ├─ Eliminar productos individuales
   └─ Vaciar el carrito completo

3. Al pulsar "Pagar con Stripe":
   ├─ Se crea un Pedido en BD con estado "Pendiente"
   └─ Se redirige a Stripe Checkout (sesión segura)

4. Stripe redirige a /Carrito/Confirmacion:
   ├─ Si PaymentStatus == "paid" → Pedido marcado como "Pagado", carrito vaciado
   └─ Si hubo error → Se muestra mensaje y el pedido queda en "Pendiente"
```

---

## Sistema de roles

```
Rol "Admin"
├─ Acceso a /Admin/* (protegido con [Authorize(Roles = "Admin")])
├─ CRUD de productos y control de stock
├─ Gestión de usuarios (activar/desactivar, cambiar rol)
├─ Visualización y exportación de pedidos a PDF
└─ Gestión de solicitudes de presupuesto

Rol "Usuario"
├─ Añadir productos al carrito y pagar
└─ Ver su historial de pedidos propios
```

---

## Recuperación de contraseña

El flujo funciona con tokens de un solo uso almacenados en BD, con **caducidad de 2 horas**.

En el entorno de desarrollo (sin servidor SMTP configurado), el enlace de restablecimiento se muestra directamente en pantalla dentro de un cuadro amarillo de depuración. En producción habría que conectar un servicio SMTP (SendGrid, SMTP propio, etc.) en el método `ForgotPassword` del `AccountController`.

---

## Control de stock

Cada producto tiene un campo `Stock` (entero). La lógica aplicada en el catálogo:

| Stock | Comportamiento |
|-------|---------------|
| 0 | Badge rojo "Agotado" · Botón desactivado |
| 1–5 | Badge naranja "Últimas X ud." · Se puede comprar |
| > 5 | Sin badge · Funcionamiento normal |

El stock se gestiona desde el panel de administración (`/Admin/Productos` → Editar).

---

## Exportación de pedidos a PDF

Desde `/Admin/DetallePedido/{id}` se muestra una vista con diseño de documento profesional (cabecera azul con datos de empresa, tabla de líneas, total con IVA incluido).

El botón **"Descargar PDF"** ejecuta `window.print()`. Los estilos `@media print` ocultan la barra de acciones y eliminan sombras, generando un PDF limpio al imprimir/guardar desde el navegador.

---

## Migas de pan (Breadcrumbs)

El sistema de navegación secundaria está implementado mediante el partial `_Breadcrumb.cshtml`. Cada vista configura su ruta en el bloque `@{ }`:

```csharp
ViewData["Breadcrumbs"] = new List<(string, string?)>
{
    ("Inicio", "/"),
    ("Productos", "/Productos/Productos"),
    ("Espinillera", null)  // null = página actual (no enlazada)
};
```

El partial solo se renderiza si hay más de un nivel (la página de inicio no muestra breadcrumb).

---

## Guía rápida — Administrador

1. Iniciar sesión con `admin@senpro.es` / `Admin123!`
2. Serás redirigido automáticamente al **Dashboard** (`/Admin`)
3. Desde la barra lateral accedes a todas las secciones
4. Para añadir un producto: **Productos → Nuevo Producto**
5. Para ver un pedido en detalle y descargarlo: **Pedidos → Ver / PDF**
6. Para gestionar solicitudes de presupuesto: **Presupuestos**
7. Para desactivar un usuario: **Usuarios → toggle Activo/Inactivo**

---

## Guía rápida — Usuario de residencia

1. Crear cuenta en `/Account/Register` o iniciar sesión
2. Navegar el catálogo desde el menú **Productos**
3. Usar el **buscador** para filtrar por nombre o descripción
4. Añadir productos al carrito y proceder al pago (Stripe)
5. Consultar pedidos anteriores en el menú de usuario → **Mis pedidos**
6. Para grandes volúmenes, solicitar presupuesto en `/Home/Presupuesto`

---

## Decisiones técnicas destacadas

**SQLite sobre SQL Server** — No requiere instalación de servidor. La BD se crea automáticamente al arrancar la aplicación, lo que simplifica el despliegue y la corrección del TFG.

**Stripe Checkout Sessions (redirect)** — Flujo de pago más seguro y sencillo que Stripe Elements embebido. Stripe gestiona toda la UI del formulario de tarjeta.

**CSS puro sin frameworks** — El diseño está íntegramente hecho a mano con CSS Grid y Flexbox, sin Bootstrap ni Tailwind, para demostrar dominio del estándar.

**Dropdown hover con puente CSS** — El menú desplegable usa el pseudo-elemento `::before` para crear un área invisible que mantiene el estado `:hover` mientras el cursor se desplaza entre el botón y el panel, sin necesidad de JavaScript.

**EnsureDeleted + EnsureCreated en desarrollo** — Garantiza que cualquier cambio en los modelos de EF se aplique automáticamente sin necesidad de migraciones durante el desarrollo.

---

## Autor y contexto

Proyecto desarrollado como Trabajo de Fin de Grado del ciclo **Desarrollo de Aplicaciones Web (DAW)**, curso 2025–2026.

| | |
|--|--|
| **Alumno** | Miguel Pérez |
| **Contacto** | miguel.perez@espiralms.com |
| **Empresa simulada** | SENPRO — Suministros geriátricos profesionales |
| **Año** | 2026 |

# Funcionalidades de la API

Este directorio contiene la implementación de todas las funcionalidades principales de la API, organizadas en módulos específicos.

## Estructura del Proyecto

- `Auth`: Contiene la lógica para la autenticación de usuarios, incluyendo endpoints para login y registro.
- `Carrito`: Implementa las funcionalidades relacionadas con el carrito de compras, como agregar productos, eliminarlos y calcular el total.
- `Categoria`: Gestiona las categorías de productos, permitiendo agregar, editar y eliminar categorías.
- `HistorialCompra`: Maneja el historial de compras de los usuarios, incluyendo la visualización y búsqueda de compras anteriores.
- `Producto`: Contiene las operaciones para gestionar productos, como agregar nuevos productos, editar información existente y eliminar productos.
- `Usuario`: Implementa las funcionalidades para gestionar usuarios, incluyendo la creación de nuevos usuarios, edición de información personal y eliminación de cuentas.

Cada módulo tiene su propio archivo de endpoints (`Endpoints.cs`) que define las rutas y los manejadores de solicitudes para las operaciones CRUD y otras funcionalidades específicas.

## Módulos Principales

### 🔐 Autenticación (Auth)
- Manejo de login y generación de tokens JWT
- Validación de credenciales
- Control de acceso basado en roles

### 🛒 Carritos
- CRUD de carritos de compra
- Cálculo de totales
- Gestión de estados (entregado/eliminado)
- Búsqueda de carritos por producto

### 📁 Categorías
- CRUD de categorías
- Búsqueda por nombre
- Gestión de productos por categoría
- Marcado lógico de eliminación

### 📋 Historial de Compras
- Registro de compras realizadas
- Consulta de historial
- Actualización y eliminación de registros

### 📦 Productos
- CRUD de productos
- Validación de permisos (Vendedor/Administrador)
- Gestión de stock

### 👥 Roles
- Administración de roles del sistema
- Permisos específicos por rol
- Validación de acceso administrativo

### 👤 Usuarios
- Registro y gestión de usuarios
- Asignación de roles
- Actualización de perfiles
- Eliminación lógica de cuentas

## Características Principales

- Autenticación mediante JWT
- Validación de permisos por rol
- Eliminación lógica en entidades relevantes
- Manejo de relaciones entre entidades
- Endpoints RESTful para cada funcionalidad

## Seguridad

- Encriptación de contraseñas con BCrypt
- Validación de tokens JWT
- Control de acceso basado en roles
- Protección de endpoints sensibles

## Servicios Disponibles

Cada módulo implementa su propia interfaz de servicio con operaciones específicas:

- `IAuthService`: Servicios de autenticación
- `ICarritoService`: Gestión de carritos
- `ICategoriaService`: Administración de categorías
- `IHistorialCompraServices`: Registro de compras
- `IProductoService`: Gestión de productos
- `IRolService`: Administración de roles
- `IUsuarioService`: Gestión de usuarios

## Configuración

Los servicios se registran en el `ServiceManager` para su inyección de dependencias:

```csharp
services.AddScoped<IAuthService, AuthService>();
```

## Documentación de API

Cada endpoint está documentado con:
- Tags para agrupación en Swagger
- Códigos de respuesta HTTP
- Requisitos de autorización
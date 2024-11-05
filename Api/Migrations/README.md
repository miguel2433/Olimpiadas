# Carpeta de Migraciones

## Descripción
La carpeta `Migrations` contiene los archivos de migración de Entity Framework Core que definen la evolución del esquema de la base de datos MySQL. Estas migraciones permiten mantener y versionar los cambios en la estructura de la base de datos.

## Migraciones Disponibles

### 1. InitialMigration
- **Archivo**: `20241010145403_InitialMigration.cs`
- **Descripción**: Migración inicial que establece la estructura base de la base de datos.
- **Tablas creadas**:
  - Categoria
  - Usuario
  - Carrito
  - HistorialCompra
  - Producto
  - CategoriaProducto

### 2. CreateTableRol
- **Archivo**: `20241015130120_CreateTableRol.cs`
- **Descripción**: Agrega la tabla Rol y la relación con Usuario.
- **Cambios principales**:
  - Creación de la tabla Rol
  - Adición de RolId en la tabla Usuario
  - Configuración de la relación Usuario-Rol

### 3. ActualizacionCategoriaYHistorialCompra
- **Archivo**: `20241015132111_ActualizacionCategoriaYHistorialCompra.cs`
- **Descripción**: Actualiza las propiedades de Categoria y HistorialCompra.
- **Cambios principales**:
  - Aumento de longitud máxima en campos de Categoria
  - Renombre de campo Fecha a FechaCompra en HistorialCompra

### 4. ChangePasswordMaxLenght
- **Archivo**: `20241024135128_ChangePasswordMaxLenght.cs`
- **Descripción**: Modifica la longitud máxima del campo Password.
- **Cambios principales**:
  - Cambio del tipo de dato de Password a longtext
  - Actualización de longitudes máximas en tabla Rol

## Estructura de las Tablas

### Usuario
- Id (Guid)
- Nombre (varchar(50))
- NombreUsuario (varchar(50))
- Apellido (varchar(50))
- Email (varchar(50))
- Password (longtext)
- Telefono (varchar(50))
- RolId (Guid)
- Eliminado (bool)

### Producto
- Id (Guid)
- Nombre (varchar(50))
- Descripcion (varchar(255))
- Precio (decimal)
- Stock (int)
- UrlImagen (varchar(255))
- VendedorId (Guid)
- CarritoId (Guid, nullable)
- Eliminado (bool)

### Carrito
- Id (Guid)
- UsuarioId (Guid)
- Total (decimal)
- Entregado (bool)
- Eliminado (bool)

### Categoria
- Id (Guid)
- Nombre (varchar(100))
- Descripcion (varchar(500))
- Eliminado (bool)

### Rol
- Id (Guid)
- Nombre (varchar(100))
- Descripcion (varchar(500))

### HistorialCompra
- Id (Guid)
- FechaCompra (DateTime)
- CarritoId (Guid)
- Eliminado (bool)

## Relaciones
- Usuario -> Carrito (1:N)
- Usuario -> Rol (N:1)
- Producto -> Usuario (N:1, como Vendedor)
- Producto -> Carrito (N:1)
- Producto <-> Categoria (N:N)
- HistorialCompra -> Carrito (N:1)

## Notas Importantes
- Todas las tablas principales utilizan Guid como tipo de dato para sus IDs
- Se implementa borrado lógico mediante el campo "Eliminado" en la mayoría de las entidades
- Las relaciones están configuradas con borrado en cascada donde es apropiado
- Se utiliza UTF8MB4 como conjunto de caracteres para todos los campos de texto
## Diagrama Beta de la Aplicacion

```mermaid

erDiagram
    Usuario {
        int IdUsuario PK
        string Nombre
        string Apellido
        string Correo
        string Contrasena
        string Direccion
    }

    Categoria {
        int IdCategoria PK
        string Nombre
    }

    Vendedor {
        int IdVendedor PK
        string Nombre
        string Correo
        string Direccion
    }

    Producto {
        int IdProducto PK
        string Nombre
        string Descripcion
        double Precio
        int Stock
        int IdCategoria FK
        int IdVendedor FK
    }

    Orden {
        int IdOrden PK
        int IdUsuario FK
        string Estado
        double Total
        string FechaCreacion
        string FechaEnvio
    }

    DetalleOrden {
        int IdDetalleOrden PK
        int IdOrden FK
        int IdProducto FK
        int Cantidad
        double PrecioUnitario
    }

    Pago {
        int IdPago PK
        int IdOrden FK
        string Metodo
        string Estado
        string FechaPago
    }

    Envio {
        int IdEnvio PK
        int IdOrden FK
        string DireccionEnvio
        string EstadoEnvio
        string FechaEnvio
    }

    Usuario ||--o{ Orden : "hace"
    Orden ||--|{ DetalleOrden : "contiene"
    Producto ||--o{ DetalleOrden : "asociado a"
    Categoria ||--o{ Producto : "clasifica"
    Vendedor ||--o{ Producto : "vende"
    Orden ||--o{ Pago : "paga"
    Orden ||--o{ Envio : "tiene"

```

## Diagram de Clases

<img src="/Doc/Driagrama de Clases.png">

```mermaid

classDiagram

    class Usuario {
        +int UsuarioId
        +string Nombre
        +string Apellido
        +string Correo
        +string Contrasena
        +string Direccion
    }

    class Categoria {
        +int CategoriaId
        +string Nombre
    }

    class Vendedor {
        +int VendedorId
        +string Nombre
        +string Correo
        +string Direccion
    }

    class Producto {
        +int ProductoId
        +string Nombre
        +string Descripcion
        +decimal Precio
        +int Stock
        +int CategoriaId
        +int VendedorId
        +Categoria Categoria
        +Vendedor Vendedor
    }

    class Orden {
        +int OrdenId
        +int UsuarioId
        +string Estado
        +decimal Total
        +DateTime FechaCreacion
        +DateTime? FechaEnvio
        +Usuario Usuario
        +List~DetalleOrden~ DetalleOrdenes
    }

    class DetalleOrden {
        +int DetalleOrdenId
        +int OrdenId
        +int ProductoId
        +int Cantidad
        +decimal PrecioUnitario
        +Orden Orden
        +Producto Producto
    }

    class Pago {
        +int PagoId
        +int OrdenId
        +string Metodo
        +string Estado
        +DateTime FechaPago
        +Orden Orden
    }

    class Envio {
        +int EnvioId
        +int OrdenId
        +string DireccionEnvio
        +string EstadoEnvio
        +DateTime FechaEnvio
        +Orden Orden
    }

    
    Usuario "1" --> "0..*" Orden : realiza
    Vendedor "1" --> "0..*" Producto : vende
    Categoria "1" --> "0..*" Producto : categoriza
    Producto "1" --> "0..*" DetalleOrden : contiene
    Orden "1" --> "0..*" DetalleOrden : tiene
    Orden "1" --> "1" Pago : tiene
    Orden "1" --> "1" Envio : tiene
```
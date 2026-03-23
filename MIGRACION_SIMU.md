# Migración Simu - De Java/JSF a .NET Blazor

## 📋 Descripción General

Este proyecto contiene la migración completa del sistema **Simu** (Sistema de Gestión de Motocicletas) de una arquitectura Java/JSF a una moderna arquitectura .NET 8 con Blazor.

## 🏗️ Estructura del Proyecto

### **Solución: `Simu.slnx`**

La solución está organizada en 5 proyectos principales:

```
Adsi/
├── Simu.slnx                     # Archivo de solución
├── Simu.Core/                    # Capa de Dominio
│   └── Entities/                 # Modelos de datos (EF Core)
│       ├── Usuario.cs
│       ├── Moto.cs
│       ├── Transaccion.cs
│       ├── Producto.cs
│       ├── Reparacion.cs
│       ├── Accesorio.cs
│       └── ... (otras entidades)
│
├── Simu.Data/                    # Capa de Acceso a Datos
│   ├── Context/
│   │   └── SimuDbContext.cs      # DbContext de Entity Framework
│   └── Repositories/
│       ├── BaseRepository.cs     # Repositorio base con CRUD
│       ├── UsuarioRepository.cs
│       ├── MotoRepository.cs
│       ├── TransaccionRepository.cs
│       └── ProductoRepository.cs
│
├── Simu.Business/                # Capa de Lógica de Negocio
│   └── Services/
│       ├── IBaseService.cs       # Interfaz del servicio base
│       ├── BaseService.cs        # Implementación del servicio base
│       ├── IUsuarioService.cs
│       ├── UsuarioService.cs
│       ├── IMotoService.cs
│       ├── MotoService.cs
│       └── ... (otros servicios)
│
├── Simu.API/                     # Capa de Presentación (REST API)
│   ├── Program.cs
│   ├── Controllers/
│   │   ├── MotosController.cs
│   │   ├── ProductosController.cs
│   │   └── UsuariosController.cs
│   └── appsettings.json
│
└── Simu.Blazor.Web/             # Frontend (Blazor WebAssembly/Server)
    ├── Program.cs
    ├── Components/
    │   ├── Layout/
    │   │   └── MainLayout.razor
    │   ├── Pages/
    │   │   ├── Index.razor
    │   │   ├── Motos.razor
    │   │   └── Productos.razor
    │   └── Routes.razor
    └── wwwroot/
        ├── index.html
        ├── app.css
        └── bootstrap/
```

---

## 🗺️ Mapeo de Tecnologías

### **Antes (Java)**
| Aspecto | Tecnología |
|--------|-----------|
| Frontend | JSF (JavaServer Faces) |
| Templates | Facelets (.xhtml) |
| Controllers | Managed Beans |
| ORM | JPA/Hibernate |
| Database Access | DAO Pattern + Facades |
| Web Framework | BootsFaces |

### **Después (.NET)**
| Aspecto | Tecnología |
|--------|-----------|
| Frontend | **Blazor WebAssembly** |
| Components | **Razor Components** |
| Business Logic | **Service Layer** |
| ORM | **Entity Framework Core** |
| Data Access | **Repository Pattern** |
| Web Framework | **ASP.NET Core** |

---

## 📦 Proyectos Detallados

### 1. **Simu.Core** - Capa de Dominio
- Contiene todas las entidades que fueron mapeadas desde Java:
  - `Usuario`, `Moto`, `Transaccion`, `Producto`, `Reparacion`
  - Entidades de referencia: `Rol`, `Marca`, `TipoProducto`, etc.
- Propiedades mapeadas fielmente desde JPA
- Relaciones One-to-Many y Many-to-Many configuradas

**Ejemplo de entidad:**
```csharp
public class Moto
{
    public int IdMoto { get; set; }
    public string Placa { get; set; }
    public int Cilindraje { get; set; }
    public virtual Marca Marca { get; set; }
    public virtual ICollection<Transaccion> Transacciones { get; set; }
}
```

### 2. **Simu.Data** - Capa de Acceso a Datos
- **SimuDbContext**: DbContext de EF Core con configuración completa
  - Relaciones entre entidades
  - Precisión de decimales para precios
  - Comportamiento de eliminación en cascada
  
- **BaseRepository<T>**: Implementación genérica de CRUD
  - `GetByIdAsync(int id)`
  - `GetAllAsync()`
  - `AddAsync(T entity)`
  - `Update(T entity)`
  - `SaveChangesAsync()`

- **Repositorios específicos**:
  - `UsuarioRepository`: Búsqueda por username, email, rol
  - `MotoRepository`: Búsqueda por placa, marca, activos
  - `TransaccionRepository`: Rango de fechas, por moto, por usuario
  - `ProductoRepository`: Búsqueda por referencia, bajo stock

### 3. **Simu.Business** - Capa de Lógica de Negocio
- Interfaz `IBaseService<T>` que define operaciones comunes
- `BaseService<T>` implementación abstracta
- Servicios específicos:
  - **UsuarioService**: Autenticación, validación, hashing de contraseñas
  - **MotoService**: Validaciones de motos
  - **TransaccionService**: Cálculos de totales, filtrados
  - **ProductoService**: Gestión de stock
  - **ReparacionService**: Cierre de reparaciones

**Ejemplo de servicio:**
```csharp
public class UsuarioService : BaseService<Usuario>, IUsuarioService
{
    public async Task<Usuario?> AuthenticateAsync(string username, string password)
    {
        var usuario = await _usuarioRepository.GetByUsernameAsync(username);
        if (usuario != null && VerifyPassword(password, usuario.Contrasena))
            return usuario;
        return null;
    }
}
```

### 4. **Simu.API** - API REST
- Controladores ASP.NET Core estándar
- Endpoints RESTful para cada recurso:
  - `GET /api/motos` - Obtener todas las motos
  - `GET /api/motos/{id}` - Obtener moto por ID
  - `POST /api/motos` - Crear nueva moto
  - `PUT /api/motos/{id}` - Actualizar moto
  - `DELETE /api/motos/{id}` - Eliminar moto

- Controladores implementados:
  - `MotosController`
  - `ProductosController`
  - `UsuariosController` (con login)

### 5. **Simu.Blazor.Web** - Frontend
- **Componentes Razor** (no JSF)
- **Bootstrap 5** para estilos
- **Font Awesome** para iconos
- Páginas implementadas:
  - `Index.razor` - Dashboard principal
  - `Motos.razor` - Gestión de motos
  - `Productos.razor` - Gestión de productos
  - `MainLayout.razor` - Layout principal con navbar

---

## 🚀 Próximos Pasos para Completar la Migración

### **Fase 2: Configuración e Integración**

1. **Configurar conexión SQL Server**
   ```csharp
   // En Program.cs del API
   builder.Services.AddDbContext<SimuDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
   ```

2. **Crear migraciones EF Core**
   ```bash
   cd Simu.Data
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

3. **Registrar servicios en DI**
   ```csharp
   builder.Services.AddScoped<IUsuarioService, UsuarioService>();
   builder.Services.AddScoped<IMotoService, MotoService>();
   // ... más servicios
   ```

4. **Configurar CORS para Blazor**
   ```csharp
   builder.Services.AddCors(options =>
   {
       options.AddPolicy("AllowBlazor", builder =>
           builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
   });
   ```

### **Fase 3: Completar Frontend**

1. Implementar componentes de edición/creación
2. Agregar validaciones en cliente
3. Consumir APIs desde Blazor con `HttpClient`
4. Implementar autenticación y autorización
5. Agregar notificaciones y toasts

### **Fase 4: Testing y Validación**

1. Tests unitarios para servicios
2. Tests de integración para repositorios
3. Tests E2E para componentes Blazor

---

## 📊 Mapeo de Entidades Java → C#

| Java Entity | C# Entity | Cambios |
|------------|----------|---------|
| `Usuario` | `Usuario` | Sin cambios mayores |
| `Moto` | `Moto` | Sin cambios mayores |
| `Transaccion` | `Transaccion` | Sin cambios mayores |
| `Producto` | `Producto` | Sin cambios mayores |
| `Reparacion` | `Reparacion` | Sin cambios mayores |
| DAO Pattern | Repository Pattern | Abstracción más limpia |
| Managed Beans | Services | Inyección de dependencias |
| JSF Views | Razor Components | Componentes reutilizables |

---

## 🔧 Configuración de Base de Datos

### **Cadena de conexión (appsettings.json)**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Simu;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### **Crear BD desde código**
```csharp
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SimuDbContext>();
    context.Database.Migrate();
}
```

---

## 📚 Librerías Principales

### **Entity Framework Core**
- `Microsoft.EntityFrameworkCore` - ORM
- `Microsoft.EntityFrameworkCore.SqlServer` - Proveedor SQL Server
- `Microsoft.EntityFrameworkCore.Tools` - Herramientas para migraciones

### **ASP.NET Core**
- `Microsoft.AspNetCore.Mvc` - Controllers y APIs
- `Microsoft.AspNetCore.Components.WebAssembly` - Blazor
- `Swashbuckle.AspNetCore` - Swagger/OpenAPI

### **Frontend**
- Bootstrap 5
- Font Awesome 6.4.0

---

## 🎯 Ventajas de esta Arquitectura

✅ **Separación de responsabilidades** clara (Core, Data, Business, API, UI)  
✅ **Reutilización de código** con patrones genéricos  
✅ **Testing facilitado** con inyección de dependencias  
✅ **Escalabilidad** con servicios desacoplados  
✅ **Mantenibilidad** mejorada con código C# moderno  
✅ **Performance** superior con .NET 8  
✅ **UI moderna** con Blazor interactivo  

---

## 📝 Notas Importantes

1. **Migración de datos**: Los datos existentes en Java deben ser migrados usando herramientas como SQL Server Integration Services (SSIS) o scripts personalizados.

2. **Seguridad**: 
   - Las contraseñas se hashean con SHA256 (puede mejorarse con Bcrypt)
   - Agregar JWT para autenticación en API
   - Implementar CORS apropiadamente

3. **Performance**:
   - Usar lazy loading con `.Include()` en repositorios
   - Implementar caching si es necesario
   - Paginar resultados en listas grandes

4. **Compatibilidad**:
   - .NET 8 es soporte a largo plazo (LTS)
   - SQL Server 2019+ recomendado
   - Navegadores modernos para Blazor

---

## 🤝 Contribución

Para contribuir a la migración:
1. Completar los servicios faltantes
2. Implementar más componentes Blazor
3. Agregar tests
4. Documentar cambios

---

**Estado**: ✅ Estructura base completada - Listo para configuración e integración

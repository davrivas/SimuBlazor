# 🚀 Guía Rápida de Inicio - Simu .NET

## Requisitos Previos

- **.NET 8 SDK** → [Descargar](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** 2019+ o **SQL Server Express**
- **Visual Studio 2022** (Community edition o superior) o **Visual Studio Code**
- **Git**

---

## ⚡ Setup Inicial (5 minutos)

### 1. Abrir la solución
```bash
cd Adsi
# Abrir con Visual Studio
start Simu.slnx
```

### 2. Configurar la cadena de conexión

**Archivo: `Simu.API/appsettings.json`**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=SimuDB;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

**Ejemplos:**
- Local: `Server=(localdb)\\mssqllocaldb;Database=SimuDB;...`
- Express: `Server=.\\SQLEXPRESS;Database=SimuDB;...`
- Remoto: `Server=192.168.1.100;Database=SimuDB;User Id=sa;Password=YourPassword;...`

### 3. Crear la base de datos

Opción A: Con Entity Framework (automático)
```bash
cd Simu.Data
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Opción B: SQL Server Management Studio
- Crear BD vacía con nombre `SimuDB`
- Las tablas se crearán con el primer `SaveChangesAsync()`

### 4. Restaurar dependencias
```bash
dotnet restore
```

### 5. Compilar la solución
```bash
dotnet build
```

---

## 🏃 Ejecutar la Aplicación

### **Opción 1: Desde Visual Studio**

1. Establecer `Simu.API` como proyecto de inicio
2. Presionar `F5` o `Ctrl+F5`
3. Se abrirá Swagger en `https://localhost:5001/swagger`

### **Opción 2: Desde Terminal**

```bash
cd Simu.API
dotnet run
```

Swagger disponible en: `https://localhost:5001/swagger`

### **Opción 3: Ejecutar Frontend y API por separado**

**Terminal 1 - API:**
```bash
cd Simu.API
dotnet run --configuration Release
```

**Terminal 2 - Frontend (Blazor):**
```bash
cd Simu.Blazor.Web
dotnet run
```

Acceder a: `https://localhost:5001` (o el puerto indicado)

---

## 🧪 Probar con Swagger

Una vez que la API esté corriendo:

1. Ir a `https://localhost:5001/swagger`
2. Expandir los controladores (Motos, Productos, Usuarios)
3. Hacer clic en "Try it out"
4. Ejecutar peticiones de prueba

### Ejemplos de Peticiones

**Obtener todas las motos:**
```
GET /api/motos
```

**Crear una nueva moto:**
```
POST /api/motos
Content-Type: application/json

{
  "placa": "ABC-123",
  "cilindraje": 250,
  "color": "Rojo",
  "modelo": 2023,
  "idMarca": 1,
  "idEstadoMoto": 1
}
```

**Autenticar usuario:**
```
POST /api/usuarios/login
Content-Type: application/json

{
  "username": "admin",
  "password": "password123"
}
```

---

## 📁 Estructura de Archivos Importante

```
Adsi/
├── Simu.slnx                          # ✅ Solución principal
├── MIGRACION_SIMU.md                  # 📖 Documentación completa
├── QUICK_START.md                     # 📖 Este archivo
│
├── Simu.Core/
│   ├── Simu.Core.csproj               # ✅ Proyecto de dominio
│   └── Entities/
│       └── *.cs                       # ✅ Modelos de datos
│
├── Simu.Data/
│   ├── Simu.Data.csproj               # ✅ Proyecto de acceso a datos
│   ├── Context/
│   │   └── SimuDbContext.cs           # ✅ DbContext configurado
│   └── Repositories/
│       └── *.cs                       # ✅ Repositorios
│
├── Simu.Business/
│   ├── Simu.Business.csproj           # ✅ Proyecto de lógica
│   └── Services/
│       └── *.cs                       # ✅ Servicios de negocio
│
├── Simu.API/
│   ├── Simu.API.csproj                # ✅ Proyecto REST API
│   ├── Program.cs                     # ⚠️ EDITAR: Registrar servicios
│   ├── Controllers/
│   │   └── *.cs                       # ✅ Controladores REST
│   └── appsettings.json               # ⚠️ EDITAR: Cadena de conexión
│
└── Simu.Blazor.Web/
    ├── Simu.Blazor.Web.csproj         # ⚠️ EDITAR: Agregar cliente HTTP
    ├── Program.cs                     # ⚠️ EDITAR: Registrar servicios Blazor
    ├── Components/
    │   ├── Pages/                     # 🚧 COMPLETAR: Más páginas
    │   └── Layout/
    └── wwwroot/
        └── *.css                      # ✅ Estilos

Leyenda:
✅ Completado
⚠️  Necesita configuración
🚧 Necesita desarrollo
```

---

## ⚙️ Configuración Adicional Necesaria

### **1. Registrar Servicios en Program.cs (API)**

**Archivo: `Simu.API/Program.cs`**

Agregar después de `builder.Services.AddSwaggerGen();`:

```csharp
// Agregar DbContext
builder.Services.AddDbContext<SimuDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar Repositorios
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<MotoRepository>();
builder.Services.AddScoped<TransaccionRepository>();
builder.Services.AddScoped<ProductoRepository>();

// Registrar Servicios
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<ITransaccionService, TransaccionService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IReparacionService, ReparacionService>();

// Agregar CORS si es necesario
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});
```

Agregar en el pipeline:
```csharp
app.UseCors("AllowAll");
```

### **2. Configurar Cliente HTTP en Blazor (Opcional)**

Si Blazor hace llamadas al API:

```csharp
// En Simu.Blazor.Web/Program.cs
builder.Services.AddScoped(sp => 
    new HttpClient { BaseAddress = new Uri("https://localhost:5001") });
```

---

## 🐛 Troubleshooting Común

### ❌ Error: "Cannot connect to database"

**Solución:**
1. Verificar SQL Server está corriendo
2. Revisar cadena de conexión en `appsettings.json`
3. Ejecutar: `dotnet ef database update`

### ❌ Error: "Type not found"

**Solución:**
1. Compilar solución: `dotnet build`
2. Limpiar: `dotnet clean && dotnet build`
3. Verificar referencias entre proyectos

### ❌ Error: "Port is already in use"

**Solución:**
Cambiar puerto en `launchSettings.json`:
```json
"profiles": {
  "https": {
    "commandName": "Project",
    "launchBrowser": true,
    "applicationUrl": "https://localhost:5002;http://localhost:5003"
  }
}
```

### ❌ Error: "Migrations not found"

**Solución:**
```bash
cd Simu.Data
dotnet ef migrations add InitialCreate --startup-project ../Simu.API
dotnet ef database update --startup-project ../Simu.API
```

---

## 📚 Archivos a Estudiar Primero

1. **Entities** (Simu.Core/Entities/)
   - Entender la estructura de datos

2. **DbContext** (Simu.Data/Context/SimuDbContext.cs)
   - Configuración de relaciones y mapeos

3. **BaseRepository** (Simu.Data/Repositories/BaseRepository.cs)
   - Patrón de acceso a datos

4. **Services** (Simu.Business/Services/)
   - Lógica de negocio

5. **Controllers** (Simu.API/Controllers/)
   - Endpoints REST

6. **Pages Razor** (Simu.Blazor.Web/Components/Pages/)
   - Frontend

---

## 🎓 Próximos Pasos de Aprendizaje

1. **Completar componentes Blazor** faltantes
2. **Agregar autenticación JWT**
3. **Implementar validaciones** con Fluent Validation
4. **Agregar tests unitarios**
5. **Deployment en Azure/IIS**

---

## 📞 Comandos Útiles

```bash
# Build
dotnet build

# Run
dotnet run

# Clean
dotnet clean

# Migrations
dotnet ef migrations add <NombreMigracion>
dotnet ef migrations remove
dotnet ef database update
dotnet ef database drop

# NuGet
dotnet add package <NombrePaquete>
dotnet restore

# Test
dotnet test

# Publish
dotnet publish -c Release -o ./publish
```

---

## ✨ Checklist de Implementación

- [ ] Configurar cadena de conexión
- [ ] Crear base de datos
- [ ] Registrar servicios en Program.cs
- [ ] Ejecutar migraciones EF Core
- [ ] Probar API con Swagger
- [ ] Completar componentes Blazor faltantes
- [ ] Conectar Blazor con API
- [ ] Agregar autenticación
- [ ] Implementar validaciones
- [ ] Tests unitarios
- [ ] Documentar cambios
- [ ] Deploy a producción

---

**¡Listo para empezar!** 🎉

Ante dudas, revisar `MIGRACION_SIMU.md` para documentación completa.

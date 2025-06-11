# 馃摌 FlujosApp API

**FlujosApp** es una API REST desarrollada en ASP.NET Core 6 para gestionar flujos de procesos, pasos y sus dependencias. Permite definir relaciones entre pasos y controlar su ejecuci贸n en funci贸n de dependencias cumplidas.

---

## 馃殌 Tecnolog铆as utilizadas

- .NET 6 (ASP.NET Core Web API)
- Entity Framework Core
- SQL Server
- Swagger / OpenAPI
- LINQ / EF Navigation Properties

---

## 馃摝 Instalaci贸n y ejecuci贸n local

### 1锔忊儯 Requisitos

- .NET 6 SDK
- SQL Server (Local o en red)
- Visual Studio 2022+ o Visual Studio Code
- [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)


### 2锔忊儯 Clonar el proyecto

```bash
git clone https://github.com/tuusuario/FlujosApp.git
cd FlujosApp
```

### 3锔忊儯 Configuraci贸n de `appsettings.json`

Edita el archivo `appsettings.json` y configura tu cadena de conexi贸n:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\SQLEXPRESS;Database=FlujosDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```


## 🛠 Restauración de la Base de Datos

Se incluye un archivo de respaldo SQL Server en la ruta:


### Pasos para restaurar:

1. Abre **SQL Server Management Studio**.
2. Crea una nueva base de datos (por ejemplo, `FlujosAppDB`).
3. Haz clic derecho sobre la base → `Tareas` → `Restaurar base de datos`.
4. Selecciona **Dispositivo**, busca el archivo `.bak` incluido.
5. Asegúrate de marcar la opción **"Sobrescribir la base de datos existente"** si es necesario.
6. Aplica los cambios y verifica que la restauración fue exitosa.

⚠ Luego, actualiza tu cadena de conexión en `appsettings.json` apuntando a esta base de datos.


### 4锔忊儯 Restaurar dependencias y compilar

```bash
dotnet restore
dotnet build
```

---

## 馃洜锔?Migraciones y base de datos

Si es la **primera vez** que trabajas con la base de datos, ejecuta:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> Aseg煤rate de tener instalada la herramienta de EF Core CLI:
```bash
dotnet tool install --global dotnet-ef
```

---

## 鈻讹笍 Ejecutar el proyecto

```bash
dotnet run
```

Una vez iniciado, abre tu navegador en:

```
http://localhost:5072/swagger/index.html
```

All铆 ver谩s la documentaci贸n completa y probador interactivo de la API generado con Swagger.

---

## 馃摎 Documentaci贸n Swagger

La documentaci贸n Swagger se genera autom谩ticamente desde los comentarios XML del c贸digo. Aseg煤rate de que estas l铆neas est茅n en tu `.csproj`:

```xml
<GenerateDocumentationFile>true</GenerateDocumentationFile>
<NoWarn>$(NoWarn);1591</NoWarn>
```

Y que en `Program.cs` incluyas:

```csharp
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
```

---

## 馃殌 Despliegue

### 馃搶 Opci贸n 1: Publicar y usar en IIS

```bash
dotnet publish -c Release -o ./publish
```

Luego, configura IIS apuntando a la carpeta `./publish`. Aseg煤rate de:

- Tener el .NET 6 Hosting Bundle instalado en el servidor.
- Configurar el `web.config` (se genera solo).
- Habilitar puertos/firewall si es remoto.

### 馃惓 Opci贸n 2: Docker (opcional)

#### `Dockerfile`

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FlujosApp.dll"]
```

#### Comandos

```bash
docker build -t flujosapp .
docker run -d -p 8080:80 --name flujosapp-container flujosapp
```

---

## 馃檵鈥嶁檪锔?Autor

**Orlando Henao Cespedes**  
Desarrollador Full Stack .NET | Java | Angular  
[LinkedIn](https://www.linkedin.com/in/orlando-henao-cespedes)

---

## 馃摑 Licencia

Este proyecto est谩 bajo la licencia MIT - ver el archivo [LICENSE](LICENSE) para mas detalles.

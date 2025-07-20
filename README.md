# 🚀 velocist.MySqlDataAccess

<p align="center">
  <img src="https://img.shields.io/badge/License-LGPL%20v3-blue.svg" alt="License: LGPL v3">
  <img src="https://img.shields.io/badge/Author-velocist-green.svg" alt="Author: velocist">
  <img src="https://img.shields.io/badge/.NET-9.0-blueviolet" alt=".NET 9.0">
</p>
<p align="center">
  <img src="https://img.shields.io/badge/Microsoft.AspNetCore.Identity.UI-9.0.6-blue" alt="Microsoft.AspNetCore.Identity.UI 9.0.6">
  <img src="https://img.shields.io/badge/Microsoft.Extensions.Logging.Log4Net.AspNetCore-8.0.0-blue" alt="Microsoft.Extensions.Logging.Log4Net.AspNetCore 8.0.0">
  <img src="https://img.shields.io/badge/System.Diagnostics.PerformanceCounter-9.0.6-blue" alt="System.Diagnostics.PerformanceCounter 9.0.6">
  <img src="https://img.shields.io/badge/MySql.Data-6.6.4-yellow" alt="MySql.Data custom">
</p>

> **Biblioteca para acceso a MySQL con patrón Unit of Work y repositorio genérico en .NET**

---

## 📑 Tabla de Contenidos
- [Descripción](#descripción)
- [Características](#características)
- [Instalación y Uso](#instalación-y-uso)
  - [1. Referencia la DLL](#1-referencia-la-dll-en-tu-proyecto)
  - [2. Dependencias](#2-agrega-las-dependencias-necesarias)
  - [3. Cadena de conexión](#3-configura-la-cadena-de-conexión)
  - [4. Registro de servicios (DI)](#4-registro-de-servicios-si-usas-dependency-injection)
  - [5. Uso manual](#5-uso-básico-sin-di-instanciación-manual)
  - [6. Ejemplo CRUD](#6-ejemplo-de-uso-típico)
  - [7. Transacciones](#7-manejo-de-transacciones)
- [Notas adicionales](#notas-adicionales)
- [Licencia](#licencia)
- [Autor](#autor)

---

## 📝 Descripción
Contiene clases para soportar la conexión a base de datos MySQL, patrón Unit of Work y repositorio genérico. Ideal para proyectos .NET que requieran acceso robusto y desacoplado a MySQL.

## ✨ Características
- Acceso a MySQL simplificado
- Patrón Unit of Work
- Repositorio genérico tipado
- Soporte para Dependency Injection
- Documentación XML en el código
- Ejemplos de uso incluidos

---

## 🚦 Instalación y Uso

### 1. Referencia la DLL en tu proyecto
- Agrega el proyecto `velocist.MySqlDataAccess` como referencia en tu solución **o**
- Añade la DLL compilada (`velocist.MySqlDataAccess.dll` y dependencias) como referencia en tu proyecto destino.

### 2. Agrega las dependencias necesarias
Asegúrate de tener:
- `MySql.Data` (driver oficial de MySQL para .NET)
- `Microsoft.Extensions.Logging` (para logging)

Instala con NuGet si es necesario:
```sh
dotnet add package MySql.Data
dotnet add package Microsoft.Extensions.Logging
```

### 3. Configura la cadena de conexión
```csharp
string connectionString = "Server=localhost;Database=mi_db;User Id=usuario;Password=contraseña;";
```

### 4. Registro de servicios (si usas Dependency Injection)
```csharp
using velocist.MySqlDataAccess.Core;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddServicesMySql(connectionString);
```
Esto registra:
- `IBaseUnitOfWork` → `UnitOfWork`
- `IBaseConnector` → `MySqlConnector`
- `IBaseRepository<T>` → `Repository<T>`

### 5. Uso básico sin DI (instanciación manual)
```csharp
using velocist.MySqlDataAccess.Core;
using velocist.MySqlDataAccess;
using velocist.MySqlDataAccess.Interfaces;

var connector = new MySqlConnector(connectionString);
var unitOfWork = new UnitOfWork(connector);
var repository = new Repository<Usuario>(unitOfWork, "Usuarios");
var usuarios = repository.List();
```

### 6. Ejemplo de uso típico
```csharp
public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
}

// Insertar
var nuevoUsuario = new Usuario { Nombre = "Ana", Email = "ana@email.com" };
repository.Insert(nuevoUsuario);

// Listar
var lista = repository.List();

// Buscar por filtro
var usuario = repository.Get(new Usuario { Id = 1 }, whereProperties: new[] { "Id" });

// Actualizar
usuario.Nombre = "Ana Actualizada";
repository.Update(usuario, whereProperties: new[] { "Id" });

// Eliminar
repository.Delete(usuario, whereProperties: new[] { "Id" });
```

### 7. Manejo de transacciones
```csharp
unitOfWork.BeginTransaction();
try
{
    repository.Insert(nuevoUsuario);
    unitOfWork.Commit();
}
catch
{
    unitOfWork.Rollback();
    throw;
}
```

---

## ℹ️ Notas adicionales
- El nombre de la tabla se pasa como string al crear el repositorio.
- Los métodos permiten excluir o filtrar propiedades para operaciones SQL.
- Si configuras un logger, tendrás trazas de las operaciones.

---

## 📝 Licencia

Este proyecto está licenciado bajo la **GNU Lesser General Public License v3.0 (LGPL-3.0)**. Consulta el archivo [LICENSE.txt](./LICENSE.txt) para más detalles.

---

## 👤 Autor

**velocist**

¿Dudas o sugerencias? Abre un issue o revisa la documentación XML en el código fuente para más detalles.

# 🚀 velocist.MySqlDataAccess

<p align="center">
  <img src="https://img.shields.io/badge/License-LGPL%20v3-blue.svg" alt="License: LGPL v3">
  <img src="https://img.shields.io/badge/Author-velocist-green.svg" alt="Author: velocist">
  <img src="https://img.shields.io/badge/.NET-9.0-blueviolet" alt=".NET 9.0">
</p>
<p align="center">
  <img src="https://img.shields.io/badge/MySql.Data-6.6.4-yellow" alt="MySql.Data custom">
</p>

> **Biblioteca para acceso a MySQL con patrón Unit of Work y repositorio genérico en .NET**

---

## 📑 Tabla de Contenidos
- [Descripción](#descripcion)
- [Características](#caracteristicas)
- [Instalación y Uso](#instalacion-y-uso)
  - [1. Referencia la DLL](#1)
  - [2. Dependencias](#2)
  - [3. Cadena de conexión](#3)
  - [4. Registro de servicios (DI)](#4)
  - [5. Uso manual](#5)
  - [6. Ejemplo CRUD](#6)
  - [7. Transacciones](#7)
- [Notas adicionales](#notas-adicionales)
- [Licencia](#licencia)
- [Autor](#autor)

---

## 📝 Descripción<a name="descripcion"></a>
Contiene clases para soportar la conexión a base de datos MySQL, patrón Unit of Work y repositorio genérico. Ideal para proyectos .NET que requieran acceso robusto y desacoplado a MySQL.

## ✨ Características<a name="caracteristicas"></a>
- Acceso a MySQL simplificado
- Patrón Unit of Work
- Repositorio genérico tipado
- Soporte para Dependency Injection
- Documentación XML en el código
- Ejemplos de uso incluidos

---

## 🚦 Instalación y Uso<a name="instalacion-y-uso"></a>

### 1. Referencia la DLL en tu proyecto<a name="1"></a>
- Agrega el proyecto `velocist.MySqlDataAccess` como referencia en tu solución **o**
- Añade la DLL compilada (`velocist.MySqlDataAccess.dll` y dependencias) como referencia en tu proyecto destino.

### 2. Agrega las dependencias necesarias<a name=""></a>
Asegúrate de tener:
- `MySql.Data` (driver oficial de MySQL para .NET)
- `Microsoft.Extensions.Logging` (para logging)

Instala con NuGet si es necesario:
```sh
dotnet add package MySql.Data
dotnet add package Microsoft.Extensions.Logging
```

### 3. Configura la cadena de conexión<a name="3"></a>
```csharp
string connectionString = "Server=localhost;Database=mi_db;User Id=usuario;Password=contraseña;";
```

### 4. Registro de servicios (si usas Dependency Injection)<a name="4"></a>
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

### 5. Uso básico sin DI (instanciación manual)<a name="5"></a>
```csharp
using velocist.MySqlDataAccess.Core;
using velocist.MySqlDataAccess;
using velocist.MySqlDataAccess.Interfaces;

var connector = new MySqlConnector(connectionString);
var unitOfWork = new UnitOfWork(connector);
var repository = new Repository<Usuario>(unitOfWork, "Usuarios");
var usuarios = repository.List();
```

### 6. Ejemplo de uso típico<a name="6"></a>
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

### 7. Manejo de transacciones<a name="7"></a>
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

## ℹ️ Notas adicionales<a name="notas-adicionales"></a>
- El nombre de la tabla se pasa como string al crear el repositorio.
- Los métodos permiten excluir o filtrar propiedades para operaciones SQL.
- Si configuras un logger, tendrás trazas de las operaciones.

---

## 📝 Licencia<a name="licencia"></a>

Este proyecto está licenciado bajo la **GNU Lesser General Public License v3.0 (LGPL-3.0)**. Consulta el archivo [LICENSE.txt](./LICENSE.txt) para más detalles.

---

## 👤 Autor<a name="autor"></a>

**velocist**

¿Dudas o sugerencias? Abre un issue o revisa la documentación XML en el código fuente para más detalles.
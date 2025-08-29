using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace velocist.MySqlDataAccess.Core;
/// <summary>
/// Configure services for MySql database access.
/// </summary>
public static class AccessServiceExtensions {

	/// <summary>
	/// Adds the services my SQL <see cref="ServiceCollection"/>.
	/// </summary>
	/// <param name="services">The services.</param>
	/// <param name="connectionString">The connection string.</param>
	public static void AddServicesMySql(this IServiceCollection services, string connectionString) {
		_ = services.AddScoped(typeof(IBaseUnitOfWork), typeof(UnitOfWork));
		_ = services.AddSingleton<IBaseConnector>(new MySqlConnector(connectionString));
		_ = services.AddScoped(typeof(IBaseRepository<>), typeof(Repository<>));
	}

	/// <summary>
	/// Registers MySQL server with <see cref="Autofac"/>.
	/// </summary>
	/// <param name="builder">The builder.</param>
	/// <param name="nameConnection"></param>
	public static void RegisterMySqlServer(this ContainerBuilder builder, string nameConnection) {
		//var connectionString = AccessConfiguration.GetConnectionString(nameConnection);
		_ = builder.RegisterType<MySqlConnector>().As<IBaseConnector>().WithParameter("connectionString", nameConnection).InstancePerLifetimeScope();
		_ = builder.RegisterType<UnitOfWork>().As<IBaseUnitOfWork>().InstancePerLifetimeScope();
		//builder.RegisterType<Repository<IEntity>>().As<IRepository<IEntity>>().InstancePerLifetimeScope();
	}
}

namespace velocist.MySqlDataAccess.Interfaces;

/// <summary>
/// Interface for a store which manages TEntity.
/// </summary>
/// <typeparam name="TEntity">The type encapsulating a TEntity</typeparam>
public interface IRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class {

	/// <summary>
	/// Gets the specified filter.
	/// </summary>
	/// <param name="filter">The filter.</param>
	/// <param name="orderBy">The order by.</param>
	/// <param name="includeProperties">The include properties.</param>
	/// <returns></returns>
	IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");

	/// <summary>
	/// Gets the by identifier.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns></returns>
	TEntity GetByID(object id);

	/// <summary>
	/// Inserts the specified entity.
	/// </summary>
	/// <param name="entity">The entity.</param>
	/// <returns></returns>
	TEntity Insert(TEntity entity);

	/// <summary>
	/// Updates the specified entity to update.
	/// </summary>
	/// <param name="entityToUpdate">The entity to update.</param>
	void Update(TEntity entityToUpdate);

	/// <summary>
	/// Deletes the specified identifier.
	/// </summary>
	/// <param name="id">The identifier.</param>
	void Delete(object id);

	/// <summary>
	/// Deletes the specified entity to delete.
	/// </summary>
	/// <param name="entityToDelete">The entity to delete.</param>
	void Delete(TEntity entityToDelete);

	/// <summary>
	/// Drops the table.
	/// </summary>
	void DropTable();

	/// <summary>
	/// RemoveAll entity
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="whereProperties"></param>
	/// <returns></returns>
	int Delete(TEntity entity, string[] whereProperties = null);

	/// <summary>
	/// Add entity
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="excludeProperties"></param>
	/// <returns></returns>
	int Insert(TEntity entity, string[] excludeProperties = null);

	/// <summary>
	/// EditAll entity
	/// </summary>
	/// <param name="entity">Entity to update</param>
	/// <param name="excludeProperties">Exclude columns to update</param>
	/// <param name="whereProperties">Where columns of the entity</param>
	/// <returns></returns>
	bool Update(TEntity entity, string[] excludeProperties = null, string[] whereProperties = null);

	/// <summary>
	/// GetById an entity
	/// </summary>
	/// <param name="entity">Entity to get</param>
	/// <param name="excludeProperties">Exclude columns query</param>
	/// <param name="whereProperties">Where columns of the query</param>
	/// <returns>Return entity</returns>
	TEntity Get(TEntity entity, string[] excludeProperties = null, string[] whereProperties = null);

	/// <summary>
	/// GetById list of entities
	/// </summary>
	/// <param name="filter">Filter of the query</param>
	/// <param name="orderBy">Order by of the query</param>
	/// <param name="excludeProperties">Exclude columns query</param>
	/// <returns>Return list of entities</returns>
	IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string[] excludeProperties = null);
}

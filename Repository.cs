using velocist.Services.Core;
using velocist.Services.Json.Serialization;
using velocist.Services.Sql;

namespace velocist.MySqlDataAccess {

	/// <summary>
	/// Read repository class of TEntity
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public class Repository<TEntity> : Item, IRepository<TEntity> where TEntity : class {

		private readonly ILogger _logger;

		private string Table { get; }

		/// <summary>
		/// IUnitOfWork for the connection
		/// </summary>
		public readonly IBaseUnitOfWork _unitOfWork;

		/// <summary>
		/// Constructor for the repository
		/// </summary>
		/// <param name="unitOfWork">UnitOfWork for repository</param>
		/// <param name="table">Table for repository</param>
		public Repository(IBaseUnitOfWork unitOfWork, string table) {
			ItemGuid = Guid.NewGuid();
			_logger = GetStaticLogger<Repository<TEntity>>();

			_unitOfWork = unitOfWork;
			Table = table;

			_unitOfWork.Connect();
		}

		///<inheritdoc/>
		public int Delete(TEntity entity, string[]? whereProperties = null) {
			try {
				var ColumnsWhere = EntityClassHelper<TEntity>.GetColumnsWithValues(entity, whereProperties, null);
				var lsSql = SqlHelper.Delete(Table, ColumnsWhere);
				return _unitOfWork.ExecuteSql(lsSql);
			} catch (Exception ex) {
				ErrorCollector.AddError(nameof(Repository<TEntity>), nameof(Delete), ex);
				throw;
			}
		}

		///<inheritdoc/>
		public int Insert(TEntity entity, string[]? excludeProperties = null) {
			try {
				var ColumnsInsert = EntityClassHelper<TEntity>.GetColumnsWithValues(entity, null, excludeProperties);
				//string table = EntityClassHelper<TEntity>.GetTableEntity(entity);
				var lsSql = SqlHelper.Insert(Table, ColumnsInsert);
				return _unitOfWork.ExecuteSql(lsSql);
			} catch (Exception ex) {
				ErrorCollector.AddError(nameof(Repository<TEntity>), nameof(Insert), ex);
				throw;
			}
		}

		///<inheritdoc/>
		public bool Update(TEntity entity, string[]? whereProperties = null, string[]? excludeProperties = null) {
			try {
				var ColumnsUpdate = EntityClassHelper<TEntity>.GetColumnsWithValues(entity, null, excludeProperties);
				var ColumnsWhere = EntityClassHelper<TEntity>.GetColumnsWithValues(entity, whereProperties, null);
				var lsSql = SqlHelper.Update(Table, ColumnsUpdate, ColumnsWhere);
				var result = _unitOfWork.ExecuteSql(lsSql);
				if (result > 0) {
					return true;
				}
			} catch (Exception ex) {
				ErrorCollector.AddError(nameof(Repository<TEntity>), nameof(Update), ex);
				throw;
			}

			return false;
		}

		///<inheritdoc/>
		public TEntity Get(TEntity entity, string[]? whereProperties = null, string[]? excludeProperties = null) {
			try {
				var ColumnsWhere = EntityClassHelper<TEntity>.GetColumnsWithValues(entity, whereProperties, excludeProperties);
				var lsSql = SqlHelper.Select(Table, null, ColumnsWhere);
				//JArray dt = await _connector.ReadV3(lsSql);
				var dt = _unitOfWork.Read(lsSql);
				return dt != null && dt.Length > 0 ? JsonHelper<TEntity>.DeserializeToEnumerable(dt, true).First() : null;
			} catch (Exception ex) {
				ErrorCollector.AddError(nameof(Repository<TEntity>), nameof(Get), ex);
				throw;
			}
		}

		///<inheritdoc/>
		public IEnumerable<TEntity>? List(
			Expression<Func<TEntity, bool>>? filter = null,
			Func<IQueryable<TEntity>,
			IOrderedQueryable<TEntity>>? orderBy = null,
			string[]? excludeProperties = null) {

			try {
				string? lsSql = null;
				//string table = EntityClassHelper<TEntity>.GetTableEntity(entity);
				lsSql = SqlHelper.Select(Table);
				//JArray dt = await _connector.ReadV3(lsSql);
				var dt = _unitOfWork.Read(lsSql);
				if (dt != null) {
					var query = JsonHelper<TEntity>.DeserializeToEnumerable(dt, true).AsQueryable();
					if (filter != null) {
						query = query.Where(filter);
					}

					return orderBy != null ? orderBy(query).AsEnumerable() : query.AsEnumerable();
				}

				return null;
			} catch (Exception ex) {
				ErrorCollector.AddError(nameof(Repository<TEntity>), nameof(List), ex);
				throw;
			}
		}

		/// <summary>
		/// GetAll 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="whereProperties"></param>
		/// <param name="excludeProperties"></param>
		/// <param name="groupByProperties"></param>
		/// <param name="orderByProperties"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public IEnumerable<TEntity> List(
			TEntity entity,
			string[]? whereProperties = null,
			string[]? excludeProperties = null,
			string[]? groupByProperties = null,
			string[]? orderByProperties = null,
			Expression<Func<TEntity, bool>>? filter = null) {

			try {
				var Columns = EntityClassHelper<TEntity>.GetColumnsWithAlias(entity, excludeProperties);
				var ColumnsWhere = EntityClassHelper<TEntity>.GetColumnsWithValues(entity, whereProperties, excludeProperties);
				var GroupBy = EntityClassHelper<TEntity>.GetColumnsGroupBy(entity, groupByProperties);
				var OrderBy = EntityClassHelper<TEntity>.GetColumnsOrderBy(entity, orderByProperties);

				var lsSql = SqlHelper.Select(Table, Columns, ColumnsWhere, GroupBy, OrderBy);
				//JArray dt = (await _connector.ReadV3(lsSql));
				var dt = _unitOfWork.Read(lsSql);
				if (dt != null) {
					var query = JsonHelper<TEntity>.DeserializeToEnumerable(dt, true).AsQueryable();
					if (filter != null) {
						query = query.Where(filter);
					}

					return query.AsEnumerable();
				}

				return Enumerable.Empty<TEntity>();
			} catch (Exception ex) {
				ErrorCollector.AddError(nameof(Repository<TEntity>), nameof(List), ex);
				throw;
			}
		}

		/// <summary>
		/// EditAll column active to false
		/// </summary>
		/// <param name="entity">Entity to update</param>
		/// <param name="whereProperties">Where for sql</param>
		public void Disable(TEntity entity, string[]? whereProperties = null) {
			try {
				var ColumnsUpdate = EntityClassHelper<TEntity>.CreateColumnWithValue(0, "Active");
				var ColumnsWhere = EntityClassHelper<TEntity>.GetColumnsWithValues(entity, whereProperties, null);
				var lsSql = SqlHelper.Update(Table, ColumnsUpdate, ColumnsWhere);
				var result = _unitOfWork.ExecuteSql(lsSql);
			} catch (Exception ex) {
				ErrorCollector.AddError(nameof(Repository<TEntity>), nameof(Disable), ex);
				throw;
			}
		}

		/// <summary>
		/// GetAll with string sql
		/// </summary>
		/// <param name="filter">Filter of the select sql</param>
		/// <param name="groupBy">Group by for the select sql</param>
		/// <param name="orderBy">Order by for the select sql</param>
		/// <returns></returns>
		public IEnumerable<TEntity>? List(string filter, string groupBy, string orderBy) {
			try {
				var lsSql = SqlHelper.Select(Table);

				var auxSql = string.Concat(" ", filter ?? "", groupBy ?? "", orderBy ?? "", ";");
				lsSql = lsSql.Replace(";", auxSql);

				//JArray dt = await _connector.ReadV3(lsSql);
				var dt = _unitOfWork.Read(lsSql);
				return dt != null ? JsonHelper<TEntity>.DeserializeToEnumerable(dt, true) : null;
			} catch (Exception ex) {
				ErrorCollector.AddError(nameof(Repository<TEntity>), nameof(List), ex);
				throw;
			}
		}

		public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "") => throw new NotImplementedException();
		public TEntity GetByID(object id) => throw new NotImplementedException();
		public TEntity Insert(TEntity entity) => throw new NotImplementedException();
		public void Update(TEntity entityToUpdate) => throw new NotImplementedException();
		public void Delete(object id) => throw new NotImplementedException();
		public void Delete(TEntity entityToDelete) => throw new NotImplementedException();
		public void DropTable() => throw new NotImplementedException();

		//public int Count(
		//    Expression<Func<TEntity, bool>> filter = null) {
		//    try {
		//        //string table = EntityClassHelper<TEntity>.GetTableEntity(entity);
		//        string lsSql = SqlHelper.Select(Table);
		//        //JArray dt = await _connector.ReadV3(lsSql);
		//        string dt = _connector.Read(lsSql);
		//        if (dt != null) {
		//            IQueryable<TEntity> query = JsonConvertHelper<TEntity>.DeserializeToEnumerable(dt).AsQueryable();
		//            if (filter != null) {
		//                query = query.Where(filter);
		//            }
		//            return query.Count();
		//        }
		//        return 0;
		//    } catch (Exception ex) {
		//        _logger.LogError(MessageStrings.SELECT_ALL_KO_DB, ex);
		//        throw;
		//    }
		//}

	}
}

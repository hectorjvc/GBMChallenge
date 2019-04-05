using GeoPosition.API.Contracts;
using GeoPosition.API.Domain;
using GeoPosition.API.Infrastructure.DataStore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GeoPosition.API.Infrastructure.Repository
{
    /// <summary>
    /// Repositorio base, hereda de IRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {

        private readonly DataContext _ctx; 
        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ctx"></param>
        protected BaseRepository(DataContext ctx)
        {
            _ctx = ctx;
        }


        /// <summary>
        /// Almacena el histórico en cada Insercion o Actualizacion
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task RegistraHistorial(LocalizacionHistorica entity)
        {
            _ctx.Add(entity);
            await SaveChanges();
        }

        /// <summary>
        /// Agrega una entidad
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task Add(T entity)
        {
            _ctx.Set<T>().Add(entity);
            await SaveChanges();
        }

        /// <summary>
        /// Elimina una entidad
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task Remove(T entity)
        {
            _ctx.Set<T>().Remove(entity);
            await SaveChanges();
        }

        /// <summary>
        /// Almacena cambios realizados a entidades
        /// </summary>
        /// <returns></returns>
        public async virtual Task<int> SaveChanges()
        {
            int returnValue = 0;
            try
            {
                returnValue = _ctx.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Could not SaveChanges in BaseRepository : {ex.Message}");
            }
            catch (Exception ex)
            {
                //var traveredMessage = ExceptionHandlingUtilties.TraverseException(ex);
                //throw new Exception($"Could not SaveChanges in BaseRepository : {traveredMessage}");
                throw new Exception($"Could not SaveChanges in BaseRepository : {ex.Message}");
            }

            //_ctx.ChangeTracker.DetectChanges();
            return returnValue;
        }

        /// <summary>
        /// Actualiza una entidad
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task Update(T entity)
        {
            EntityEntry entry = _ctx.Entry(entity);

            if (entry != null)
                switch (entry.State)
                {
                    case EntityState.Detached:
                        AttachAndMarkModfied(entity);
                        break;

                    case EntityState.Deleted:
                        // CurrentValues returns current property values for given Entity Class<T>. 
                        // CurrentValues is exposed as DbPropertyValue class, which is a 
                        // collection of all properties for the underlying object.
                        // DbPropertyValues.setValues() sets value for dictionary property
                        // collection by reading values from another dictionary.
                        // Source dictaionary must be the same type as target dictionary,
                        // or type derived from this dictionary.
                        entry.CurrentValues.SetValues(entity);
                        entry.State = EntityState.Modified;
                        break;

                    default:
                        entry.CurrentValues.SetValues(entity);
                        break;
                }
            else
                AttachAndMarkModfied(entity);

            await SaveChanges();
        }

        /// <summary>
        /// Método Get para IQueryable
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<T> Get()
        {
            return _ctx.Set<T>().AsQueryable();
        }

        /// <summary>
        /// Método Find del Repositorio
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        protected async virtual Task<IQueryable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return _ctx.Set<T>().Where(predicate).AsQueryable();
        }

        /// <summary>
        /// Método Find del Encontrar Por del Repositorio
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        protected async virtual Task<T> FindById(int entityId)
        {
            return _ctx.Set<T>().Find(entityId);
        }

        /// <summary>
        /// Método para validación de actualizaciones
        /// </summary>
        /// <param name="entity"></param>
        private void AttachAndMarkModfied(T entity)
        {
            _ctx.Set<T>().Attach(entity);
            _ctx.Entry(entity).State = EntityState.Modified;
        }
    }
}

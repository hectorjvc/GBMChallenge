using System.Threading.Tasks;

namespace GeoPosition.API.Contracts
{
    /// <summary>
    /// Interfaz general de los repositorios
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {

        /// <summary>
        /// Registra el historial
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task RegistraHistorial(Domain.LocalizacionHistorica entity);

        /// <summary>
        /// Agrega una entidad a la base de datos
        /// </summary>
        /// <param name="entity"></param> 
        /// <returns></returns>
        Task Add(T entity);

        /// <summary>
        /// Elimina una entidad en la base de datos
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Remove(T entity);

        /// <summary>
        /// Actualiza una entidad en la base de datos
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Update(T entity);

        /// <summary>
        /// Método general para hacer commit a la base de datos los cambios
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChanges();
    }
}

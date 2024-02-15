﻿using Proyecto_Final.Models;

namespace Proyecto_Final.Repository.IRepository
{
    public interface IRepositoryProducto : IRepositoryGeneric<Producto>
    {
        Task<Producto?> ObtenerPorNombre(string nombre);
    }
}

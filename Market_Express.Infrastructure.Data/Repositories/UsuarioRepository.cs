using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        private const string _Sp_Usuario_GetPermisos = "Sp_Usuario_GetPermisos";

        public UsuarioRepository(MARKET_EXPRESSContext context, IConfiguration configuration)
            : base(context, configuration)
        { }

        public async Task<List<Permiso>> GetPermisosAsync(Guid id)
        {
            List<Permiso> lstPermissions = new();

            var arrParams = new[]
            {
                new SqlParameter("@Id",id)
            };

            var oDTResult = await ExecuteQuery(_Sp_Usuario_GetPermisos, arrParams);

            foreach (DataRow oRow in oDTResult.Rows)
            {
                lstPermissions.Add(new Permiso
                {
                    Id = (Guid)oRow["Id"],
                    Nombre = oRow["Nombre"].ToString(),
                    Descripcion = oRow["Descripcion"].ToString()
                });
            }

            return lstPermissions;
        }
    }
}

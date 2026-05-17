using p_ProveedorStreaming.Clases.strUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using static p_ProveedorStreaming.Clases.strUsuario.ReglasNegocioUsuario;

namespace p_ProveedorStreaming.Clases.strUsuario
{
    public class UsuarioService
    {
        private List<Usuario> _usuarios = new();

        public void Agregar(Usuario usuario)
        {
            if (usuario != null)
            {
                _usuarios.Add(usuario);
            }
        }

        public List<Usuario> ObtenerTodos() => _usuarios;

        public Usuario BuscarPorId(ulong id)
        {
            var usuario = _usuarios.FirstOrDefault(u => (ulong)u.Id_interno == id);
            if (usuario == null)
            {
                throw new Exception($"[Error] El usuario con ID {id} no existe en el sistema.");
            }
            return usuario;
        }

        public Usuario BuscarPorNombre(string nombre)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (usuario == null)
            {
                throw new Exception($"[Error] El usuario con nombre '{nombre}' no fue encontrado.");
            }
            return usuario;
        }

        public ulong ObtenerPuntaje(ulong id)
        {
            // Reutilizamos BuscarPorId, que ya lanza la excepción si no lo encuentra
            return BuscarPorId(id).Puntos;
        }

        public l_categorias ObtenerCategoria(ulong id)
        {
            // Si el ID no existe, BuscarPorId lanzará el error 
            return BuscarPorId(id).Categoria;
        }
    }
}
namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface ITokenService
    {
        string GenerarToken(string username, string rol);
    }
}

namespace MyFavoriteWeb.Models.Singletons
{
    public sealed class UsuarioLogado
    {
        private static string nome;

        public static string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        private static string email;

        public static string Email
        {
            get { return email; }
            set { email = value; }
        }

        private static string senha;

        public static string Senha
        {
            get { return senha; }
            set { senha = value; }
        }

        public UsuarioLogado(Usuario usuario)
        {
            if (usuario == null)
            {
                email = string.Empty;
                nome = string.Empty;
                senha = string.Empty;
            }
            else
            {
                email = usuario.Email;
                nome = usuario.Nome;
                senha = usuario.Senha;
            }
        }
    }
}

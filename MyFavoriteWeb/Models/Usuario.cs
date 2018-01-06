using System.ComponentModel.DataAnnotations;

namespace MyFavoriteWeb.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Avatar { get; set; }
    }
}

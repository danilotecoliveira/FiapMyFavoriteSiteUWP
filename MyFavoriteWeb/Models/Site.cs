using System.ComponentModel.DataAnnotations;

namespace MyFavoriteWeb.Models
{
    public class Site
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Url { get; set; }

        public int UsuarioId { get; set; }
        //public virtual Usuario Usuario { get; set; }
}
}

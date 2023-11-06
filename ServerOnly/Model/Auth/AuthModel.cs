using System.ComponentModel.DataAnnotations;

namespace ServerOnly.Model.Auth
{
    public class AuthModel
    {
        [Required(ErrorMessage = "Имя пользователя обязательно!")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Пароль обязателен!")]
        public string? Password { get; set; }
    }
}

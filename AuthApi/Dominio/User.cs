using AuthApi.Util;
using Flunt.Notifications;
using Flunt.Validations;
using Newtonsoft.Json;

namespace AuthApi.Dominio
{
    public class User : Notifiable
    {
        public int Id { get; }
        public string Name { get; }
        public string Email { get; }

        [JsonIgnore]
        public Password Password { get; private set; }

        protected User() { }

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = new Password(password);
        }

        public void ChangePassword(string newPassword, string newPasswordConfirmation)
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(newPassword, nameof(newPassword), "A senha não pode ficar vazia")
                .IsNotNullOrEmpty(newPasswordConfirmation, nameof(newPasswordConfirmation), "A confirmação de senha não pode ficar vazia")
                .AreEquals(newPassword, newPasswordConfirmation, "PasswordConfirmation", "As senhas não conferem")
            );

            Password = new Password(newPassword);
        }
    }
}

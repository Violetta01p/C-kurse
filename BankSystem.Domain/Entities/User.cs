// ============================================================
// User.cs — Сутність "Користувач банку"
// Кожен User має роль (Client, Manager, Admin), яка визначає
// до яких функцій системи він має доступ.
// IsBlocked захищає систему від доступу заблокованих користувачів.
// ============================================================
namespace BankSystem.Domain.Entities;

public class User
{
    // Унікальний ідентифікатор користувача
    public Guid Id { get; private set; }

    // Повне ім'я (ПІБ) — відображається у CRM менеджера
    public string FullName { get; private set; }

    // Логін для входу в систему
    public string Login { get; private set; }

    // Хеш пароля (в реальному проєкті тут має бути bcrypt/SHA256)
    public string PasswordHash { get; private set; }

    // Роль визначає, які сторінки та API доступні користувачу
    public Role Role { get; private set; }

    // Заблокований користувач не може увійти в систему
    public bool IsBlocked { get; private set; }

    public User(Guid id, string fullName, string login, string passwordHash, Role role = Role.Client)
    {
        Id = id;
        FullName = fullName;
        Login = login;
        PasswordHash = passwordHash;
        Role = role;
        IsBlocked = false;
    }

    // Заблокувати користувача (виклик від BlockUserUseCase)
    public void Block()
    {
        IsBlocked = true;
    }

    // Розблокувати користувача
    public void Unblock()
    {
        IsBlocked = false;
    }
}

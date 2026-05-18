// ============================================================
// Navbar.tsx — Навігаційна панель (верхнє меню)
// Показує різні посилання залежно від ролі користувача:
//   - Client бачить посилання на свій кабінет
//   - Manager/Admin бачать CRM та глобальну історію транзакцій
// ============================================================
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

export const Navbar = () => {
    // Отримуємо поточного користувача та функцію виходу з контексту
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    // При натисканні "Logout": очищаємо токен і перекидаємо на сторінку входу
    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    // Якщо користувач не авторизований — навбар не відображається
    if (!user) return null;

    return (
        <nav style={{ display: 'flex', justifyContent: 'space-between', padding: '1rem', background: '#111', marginBottom: '2rem', borderRadius: '8px' }}>
            <div style={{ display: 'flex', gap: '1rem', alignItems: 'center' }}>
                <strong style={{ fontSize: '1.2em' }}>BankSystem</strong>

                {/* Посилання для клієнта */}
                {user.role === 'Client' && <Link to="/dashboard" style={{ color: 'white', textDecoration: 'none' }}>My Account</Link>}

                {/* Посилання для менеджера та адміна */}
                {(user.role === 'Manager' || user.role === 'Admin') && (
                    <>
                        <Link to="/manager" style={{ color: 'white', textDecoration: 'none' }}>CRM (Users)</Link>
                        <Link to="/transactions" style={{ color: 'white', textDecoration: 'none' }}>Global Transactions</Link>
                    </>
                )}
            </div>
            <div>
                <button onClick={handleLogout} style={{ padding: '0.4rem 1rem' }}>Logout</button>
            </div>
        </nav>
    );
};

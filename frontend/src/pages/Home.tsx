// ============================================================
// Home.tsx — Головна сторінка (Landing Page)
// Це перша сторінка, яку бачить користувач при переході на сайт.
// Вона містить презентацію можливостей банку та кнопки входу/реєстрації.
// ============================================================
import { Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

export const Home = () => {
    const { user } = useAuth();

    return (
        <div style={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            minHeight: '80vh',
            textAlign: 'center',
            padding: '2rem',
            color: '#fff',
            boxSizing: 'border-box'
        }}>
            <div style={{
                background: 'linear-gradient(135deg, #1e1e38 0%, #0f0f1a 100%)',
                padding: '3rem',
                borderRadius: '24px',
                boxShadow: '0 20px 40px rgba(0, 0, 0, 0.4)',
                border: '1px solid rgba(255, 255, 255, 0.1)',
                maxWidth: '800px',
                width: '100%',
                backdropFilter: 'blur(10px)'
            }}>
                <h1 style={{
                    fontSize: '3.5rem',
                    fontWeight: 800,
                    marginBottom: '1rem',
                    background: 'linear-gradient(90deg, #646cff, #ff4aff)',
                    WebkitBackgroundClip: 'text',
                    WebkitTextFillColor: 'transparent',
                    letterSpacing: '-1px'
                }}>
                    BankSystem
                </h1>
                
                <p style={{
                    fontSize: '1.25rem',
                    color: '#ccc',
                    marginBottom: '2.5rem',
                    lineHeight: '1.6'
                }}>
                    Сучасна, надійна та швидка банківська екосистема з рольовою моделлю доступу. 
                    Керуйте своїми фінансами безпечно та ефективно.
                </p>

                <div style={{
                    display: 'grid',
                    gridTemplateColumns: 'repeat(auto-fit, minmax(220px, 1fr))',
                    gap: '1.5rem',
                    marginBottom: '3rem',
                    textAlign: 'left'
                }}>
                    <div style={{
                        background: 'rgba(255, 255, 255, 0.05)',
                        padding: '1.5rem',
                        borderRadius: '16px',
                        border: '1px solid rgba(255, 255, 255, 0.05)'
                    }}>
                        <h3 style={{ margin: '0 0 0.5rem 0', color: '#646cff' }}>🛡️ Надійність</h3>
                        <p style={{ margin: 0, fontSize: '0.9rem', color: '#aaa' }}>Авторизація на базі JWT токенів та захист кожної операції.</p>
                    </div>
                    <div style={{
                        background: 'rgba(255, 255, 255, 0.05)',
                        padding: '1.5rem',
                        borderRadius: '16px',
                        border: '1px solid rgba(255, 255, 255, 0.05)'
                    }}>
                        <h3 style={{ margin: '0 0 0.5rem 0', color: '#ff4aff' }}>⚡ Швидкість</h3>
                        <p style={{ margin: 0, fontSize: '0.9rem', color: '#aaa' }}>Миттєві перекази між рахунками та автоматичне оновлення балансу.</p>
                    </div>
                    <div style={{
                        background: 'rgba(255, 255, 255, 0.05)',
                        padding: '1.5rem',
                        borderRadius: '16px',
                        border: '1px solid rgba(255, 255, 255, 0.05)'
                    }}>
                        <h3 style={{ margin: '0 0 0.5rem 0', color: '#4aff74' }}>📊 Прозорість</h3>
                        <p style={{ margin: 0, fontSize: '0.9rem', color: '#aaa' }}>Детальна історія транзакцій для клієнтів та CRM для менеджерів.</p>
                    </div>
                </div>

                {user ? (
                    <div>
                        <p style={{ marginBottom: '1rem', color: '#4aff74' }}>
                            Ви вже увійшли в систему як <strong>{user.role}</strong>
                        </p>
                        <Link to={user.role === 'Client' ? '/dashboard' : '/manager'}>
                            <button style={{
                                padding: '0.8rem 2rem',
                                fontSize: '1.1rem',
                                fontWeight: 'bold',
                                borderRadius: '12px',
                                border: 'none',
                                background: 'linear-gradient(90deg, #646cff, #ff4aff)',
                                color: '#fff',
                                cursor: 'pointer',
                                transition: 'transform 0.2s',
                                boxShadow: '0 8px 20px rgba(100, 108, 255, 0.4)'
                            }}
                            onMouseOver={(e) => e.currentTarget.style.transform = 'scale(1.05)'}
                            onMouseOut={(e) => e.currentTarget.style.transform = 'scale(1)'}
                            >
                                Перейти до кабінету
                            </button>
                        </Link>
                    </div>
                ) : (
                    <div style={{ display: 'flex', gap: '1rem', justifyContent: 'center' }}>
                        <Link to="/login">
                            <button style={{
                                padding: '0.8rem 2rem',
                                fontSize: '1.1rem',
                                fontWeight: 'bold',
                                borderRadius: '12px',
                                border: 'none',
                                background: '#646cff',
                                color: '#fff',
                                cursor: 'pointer',
                                transition: 'transform 0.2s',
                                boxShadow: '0 8px 20px rgba(100, 108, 255, 0.3)'
                            }}
                            onMouseOver={(e) => e.currentTarget.style.transform = 'scale(1.05)'}
                            onMouseOut={(e) => e.currentTarget.style.transform = 'scale(1)'}
                            >
                                Увійти
                            </button>
                        </Link>
                        <Link to="/register">
                            <button style={{
                                padding: '0.8rem 2rem',
                                fontSize: '1.1rem',
                                fontWeight: 'bold',
                                borderRadius: '12px',
                                border: '1px solid rgba(255, 255, 255, 0.2)',
                                background: 'rgba(255, 255, 255, 0.05)',
                                color: '#fff',
                                cursor: 'pointer',
                                transition: 'background-color 0.2s, transform 0.2s'
                            }}
                            onMouseOver={(e) => {
                                e.currentTarget.style.background = 'rgba(255, 255, 255, 0.1)';
                                e.currentTarget.style.transform = 'scale(1.05)';
                            }}
                            onMouseOut={(e) => {
                                e.currentTarget.style.background = 'rgba(255, 255, 255, 0.05)';
                                e.currentTarget.style.transform = 'scale(1)';
                            }}
                            >
                                Реєстрація
                            </button>
                        </Link>
                    </div>
                )}
            </div>
        </div>
    );
};

// ============================================================
// AuthContext.tsx — Контекст авторизації (глобальний стан)
// Зберігає JWT токен та дані про поточного користувача.
// Всі компоненти можуть отримати ці дані через хук useAuth().
// Токен зберігається в localStorage, щоб не зникати після оновлення сторінки.
// ============================================================
import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';

// Структура даних користувача після входу
interface UserData {
    userId: string;
    role: string;       // 'Client' | 'Manager' | 'Admin'
    accountId?: string; // ID рахунку (є тільки у клієнтів)
}

// Тип контексту: що саме доступно через useAuth()
interface AuthContextType {
    user: UserData | null;
    token: string | null;
    login: (token: string, user: UserData) => void;  // Функція для збереження даних після входу
    logout: () => void;                               // Функція для виходу та очищення даних
}

const AuthContext = createContext<AuthContextType>(null!);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [user, setUser] = useState<UserData | null>(null);
    const [token, setToken] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);

    // При завантаженні сторінки перевіряємо, чи є збережений токен
    useEffect(() => {
        const storedToken = localStorage.getItem('token');
        const storedUser = localStorage.getItem('user');
        if (storedToken && storedUser) {
            setToken(storedToken);
            setUser(JSON.parse(storedUser));
        }
        setLoading(false);
    }, []);

    // Зберігаємо токен і дані користувача після успішного входу
    const login = (newToken: string, newUser: UserData) => {
        setToken(newToken);
        setUser(newUser);
        localStorage.setItem('token', newToken);
        localStorage.setItem('user', JSON.stringify(newUser));
    };

    // Очищаємо всі дані при виході
    const logout = () => {
        setToken(null);
        setUser(null);
        localStorage.removeItem('token');
        localStorage.removeItem('user');
    };

    // Поки перевіряємо localStorage — показуємо заглушку
    if (loading) return <div>Loading...</div>;

    return (
        <AuthContext.Provider value={{ user, token, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

// Хук для зручного доступу до контексту авторизації
export const useAuth = () => useContext(AuthContext);

// ============================================================
// api.ts — Допоміжний модуль для запитів до бекенду
// API_URL — базова адреса нашого .NET API.
// fetchWithAuth — обгортка над fetch(), яка автоматично:
//   1. Додає JWT токен в заголовок Authorization
//   2. Перенаправляє на /login при помилці 401 (токен протермінований)
// ============================================================

// Базова адреса нашого .NET Web API в хмарі Render
export const API_URL = 'https://c-kurse.onrender.com/api';

// Відправляє HTTP запит до API з автоматичним додаванням токена авторизації
export const fetchWithAuth = async (url: string, options: RequestInit = {}) => {
    // Читаємо JWT токен зі сховища браузера
    const token = localStorage.getItem('token');
    const headers = new Headers(options.headers || {});

    // Якщо токен є — додаємо його в заголовок (Bearer схема авторизації)
    if (token) {
        headers.set('Authorization', `Bearer ${token}`);
    }

    // Встановлюємо Content-Type для JSON запитів (якщо не вказано вручну)
    if (!headers.has('Content-Type') && !(options.body instanceof FormData)) {
        headers.set('Content-Type', 'application/json');
    }

    const response = await fetch(`${API_URL}${url}`, {
        ...options,
        headers
    });

    // Якщо сервер повернув 401 (неавторизований) — виходимо та перенаправляємо на логін
    if (response.status === 401) {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        window.location.href = '/login';
    }

    return response;
};

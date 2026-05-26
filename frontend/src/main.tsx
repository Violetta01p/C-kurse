// ============================================================
// main.tsx — Точка входу застосунку
// Тут ми "монтуємо" (підключаємо) весь React до HTML-елементу #root.
// AuthProvider — надає інформацію про авторизацію всім компонентам.
// BrowserRouter — дозволяє React Router визначати, яку сторінку показувати.
// ============================================================
import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import { AuthProvider } from './contexts/AuthContext';
import './index.css';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    {/* AuthProvider зберігає JWT токен та дані користувача у пам'яті */}
    <AuthProvider>
      {/* BrowserRouter дозволяє переходити між сторінками без перезавантаження */}
      {/* Додано basename="/C-kurse" для коректної роботи роутингу на GitHub Pages */}
      <BrowserRouter basename="/C-kurse">
        <App />
      </BrowserRouter>
    </AuthProvider>
  </React.StrictMode>
);

// ============================================================
// App.tsx — Головний компонент застосунку з маршрутизацією
// Тут визначено, які сторінки доступні і хто може їх бачити.
// ProtectedRoute захищає сторінки від неавторизованих користувачів.
// ============================================================
import { Routes, Route, Navigate } from 'react-router-dom';
import { Navbar } from './components/Navbar';
import { ProtectedRoute } from './components/ProtectedRoute';
import { Login } from './pages/Login';
import { Register } from './pages/Register';
import { ClientDashboard } from './pages/ClientDashboard';
import { ManagerDashboard } from './pages/ManagerDashboard';
import { Transactions } from './pages/Transactions';
import './App.css';

function App() {
  return (
    <div className="container">
      {/* Навігаційна панель — з'являється на всіх сторінках після входу */}
      <Navbar />
      <Routes>
        {/* Спільні сторінки — доступні без авторизації */}
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />

        {/* Сторінки для клієнтів — тільки з роллю Client */}
        <Route path="/dashboard" element={
          <ProtectedRoute allowedRoles={['Client']}>
            <ClientDashboard />
          </ProtectedRoute>
        } />

        {/* Сторінки для менеджерів/адміністраторів */}
        <Route path="/manager" element={
          <ProtectedRoute allowedRoles={['Manager', 'Admin']}>
            <ManagerDashboard />
          </ProtectedRoute>
        } />

        <Route path="/transactions" element={
          <ProtectedRoute allowedRoles={['Manager', 'Admin']}>
            <Transactions />
          </ProtectedRoute>
        } />

        {/* Якщо сторінка не знайдена — перенаправляємо на логін */}
        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    </div>
  );
}

export default App;

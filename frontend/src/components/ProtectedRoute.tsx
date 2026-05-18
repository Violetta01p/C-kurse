// ============================================================
// ProtectedRoute.tsx — Захист маршрутів від неавторизованих користувачів
// Якщо користувач не авторизований — перекидає на /login.
// Якщо роль не відповідає дозволеним — перекидає на свою домашню сторінку.
// ============================================================
import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

export const ProtectedRoute = ({ children, allowedRoles }: { children: React.ReactNode, allowedRoles?: string[] }) => {
    const { user } = useAuth();

    // Не авторизований — на сторінку входу
    if (!user) {
        return <Navigate to="/login" replace />;
    }

    // Авторизований, але роль не підходить — на свою сторінку
    if (allowedRoles && !allowedRoles.includes(user.role)) {
        return <Navigate to={user.role === 'Client' ? '/dashboard' : '/manager'} replace />;
    }

    // Все гаразд — показуємо вміст сторінки
    return <>{children}</>;
};

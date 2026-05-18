import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { API_URL } from '../api';

export const Login = () => {
    const [loginStr, setLoginStr] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const res = await fetch(`${API_URL}/auth/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ login: loginStr, password })
            });
            const data = await res.json();
            if (res.ok) {
                login(data.token, { userId: data.userId, role: data.role, accountId: data.accountId });
                if (data.role === 'Client') navigate('/dashboard');
                else navigate('/manager');
            } else {
                setError(data.error || 'Login failed');
            }
        } catch {
            setError('Network error');
        }
    };

    return (
        <div style={{ display: 'flex', justifyContent: 'center' }}>
            <div className="card" style={{ width: '400px' }}>
                <h2>Login</h2>
                <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
                    <input value={loginStr} onChange={e => setLoginStr(e.target.value)} placeholder="Login" required />
                    <input type="password" value={password} onChange={e => setPassword(e.target.value)} placeholder="Password" required />
                    <button type="submit">Sign In</button>
                </form>
                {error && <p className="error">{error}</p>}
                <p style={{ marginTop: '1rem' }}>Don't have an account? <Link to="/register">Register</Link></p>
                
                <div style={{ marginTop: '2rem', fontSize: '0.8em', color: '#888' }}>
                    <p>Test Accounts:</p>
                    <p>Client: client1 / client1</p>
                    <p>Manager: manager / manager</p>
                </div>
            </div>
        </div>
    );
};

import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { API_URL } from '../api';

export const Register = () => {
    const [fullName, setFullName] = useState('');
    const [loginStr, setLoginStr] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const res = await fetch(`${API_URL}/auth/register`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ fullName, login: loginStr, password, role: 0 })
            });
            const data = await res.json();
            if (res.ok) {
                alert('Registered successfully! You can now login.');
                navigate('/login');
            } else {
                setError(data.error || 'Registration failed');
            }
        } catch {
            setError('Network error');
        }
    };

    return (
        <div style={{ display: 'flex', justifyContent: 'center' }}>
            <div className="card" style={{ width: '400px' }}>
                <h2>Register</h2>
                <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
                    <input value={fullName} onChange={e => setFullName(e.target.value)} placeholder="Full Name" required />
                    <input value={loginStr} onChange={e => setLoginStr(e.target.value)} placeholder="Login" required />
                    <input type="password" value={password} onChange={e => setPassword(e.target.value)} placeholder="Password" required />
                    <button type="submit">Sign Up</button>
                </form>
                {error && <p className="error">{error}</p>}
                <p style={{ marginTop: '1rem' }}>Already have an account? <Link to="/login">Login</Link></p>
            </div>
        </div>
    );
};

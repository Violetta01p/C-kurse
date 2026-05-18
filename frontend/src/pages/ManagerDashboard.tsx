import React, { useState, useEffect } from 'react';
import { fetchWithAuth } from '../api';

export const ManagerDashboard = () => {
    const [users, setUsers] = useState<any[]>([]);
    
    const fetchUsers = async () => {
        const res = await fetchWithAuth('/users');
        if (res.ok) setUsers(await res.json());
    };

    useEffect(() => {
        fetchUsers();
    }, []);

    const blockUser = async (id: string) => {
        if (!window.confirm('Are you sure you want to block this user?')) return;
        const res = await fetchWithAuth(`/users/${id}/block`, { method: 'POST' });
        if (res.ok) fetchUsers();
        else alert('Failed to block user');
    };

    return (
        <div className="card" style={{ width: '100%', maxWidth: '1000px' }}>
            <h2>Client Management (CRM)</h2>
            <table style={{ width: '100%', textAlign: 'left', borderCollapse: 'collapse' }}>
                <thead>
                    <tr>
                        <th style={{ borderBottom: '1px solid #555', padding: '0.5rem' }}>Name</th>
                        <th style={{ borderBottom: '1px solid #555', padding: '0.5rem' }}>Login</th>
                        <th style={{ borderBottom: '1px solid #555', padding: '0.5rem' }}>Role</th>
                        <th style={{ borderBottom: '1px solid #555', padding: '0.5rem' }}>Status</th>
                        <th style={{ borderBottom: '1px solid #555', padding: '0.5rem' }}>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map(u => (
                        <tr key={u.id}>
                            <td style={{ padding: '0.5rem', borderBottom: '1px solid #333' }}>{u.fullName}</td>
                            <td style={{ padding: '0.5rem', borderBottom: '1px solid #333' }}>{u.login}</td>
                            <td style={{ padding: '0.5rem', borderBottom: '1px solid #333' }}>{u.role === 0 ? 'Client' : u.role === 1 ? 'Manager' : 'Admin'}</td>
                            <td style={{ padding: '0.5rem', borderBottom: '1px solid #333', color: u.isBlocked ? 'red' : 'green' }}>
                                {u.isBlocked ? 'Blocked' : 'Active'}
                            </td>
                            <td style={{ padding: '0.5rem', borderBottom: '1px solid #333' }}>
                                {!u.isBlocked && u.role === 0 && (
                                    <button onClick={() => blockUser(u.id)} style={{ background: '#ff4a4a', color: 'white', padding: '0.3rem 0.6rem', fontSize: '0.8em', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>Block</button>
                                )}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

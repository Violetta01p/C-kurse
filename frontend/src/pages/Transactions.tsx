import { useState, useEffect } from 'react';
import { fetchWithAuth } from '../api';

export const Transactions = () => {
    const [transactions, setTransactions] = useState<any[]>([]);

    useEffect(() => {
        const fetchTx = async () => {
            const res = await fetchWithAuth('/transactions');
            if (res.ok) setTransactions(await res.json());
        };
        fetchTx();
    }, []);

    return (
        <div className="card" style={{ width: '100%', maxWidth: '1000px' }}>
            <h2>Global Transaction History</h2>
            <table style={{ width: '100%', textAlign: 'left', borderCollapse: 'collapse' }}>
                <thead>
                    <tr>
                        <th style={{ borderBottom: '1px solid #555', padding: '0.5rem' }}>Date</th>
                        <th style={{ borderBottom: '1px solid #555', padding: '0.5rem' }}>Account ID</th>
                        <th style={{ borderBottom: '1px solid #555', padding: '0.5rem' }}>Type</th>
                        <th style={{ borderBottom: '1px solid #555', padding: '0.5rem' }}>Amount</th>
                        <th style={{ borderBottom: '1px solid #555', padding: '0.5rem' }}>Description</th>
                    </tr>
                </thead>
                <tbody>
                    {transactions.map(tx => (
                        <tr key={tx.id}>
                            <td style={{ padding: '0.5rem', borderBottom: '1px solid #333' }}>{new Date(tx.date).toLocaleString()}</td>
                            <td style={{ padding: '0.5rem', borderBottom: '1px solid #333' }}><code style={{ fontSize: '0.8em', background: '#222', padding: '0.2em' }}>{tx.accountId}</code></td>
                            <td style={{ padding: '0.5rem', borderBottom: '1px solid #333' }}>
                                {tx.type === 0 ? 'Deposit' : tx.type === 1 ? 'Withdrawal' : 'Transfer'}
                            </td>
                            <td style={{ padding: '0.5rem', borderBottom: '1px solid #333' }}>
                                {tx.amount.amount} {tx.amount.currency}
                            </td>
                            <td style={{ padding: '0.5rem', borderBottom: '1px solid #333' }}>{tx.description}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

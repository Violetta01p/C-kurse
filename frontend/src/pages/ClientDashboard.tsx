import React, { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { fetchWithAuth } from '../api';

export const ClientDashboard = () => {
    const { user } = useAuth();
    const [account, setAccount] = useState<any>(null);
    const [transactions, setTransactions] = useState<any[]>([]);
    const [amount, setAmount] = useState<number>(0);
    const [transferTo, setTransferTo] = useState('');
    const [transferAmount, setTransferAmount] = useState<number>(0);
    const [error, setError] = useState('');

    const fetchData = async () => {
        if (!user?.accountId) return;
        const res = await fetchWithAuth(`/accounts/${user.accountId}`);
        if (res.ok) setAccount(await res.json());

        const txRes = await fetchWithAuth(`/accounts/${user.accountId}/transactions`);
        if (txRes.ok) setTransactions(await txRes.json());
    };

    useEffect(() => {
        fetchData();
    }, []);

    const handleDeposit = async () => {
        try {
            const res = await fetchWithAuth(`/accounts/${user?.accountId}/deposit`, {
                method: 'POST',
                body: JSON.stringify({ accountId: user?.accountId, amount, currency: 'USD' })
            });
            if (res.ok) { setAmount(0); fetchData(); }
            else { const d = await res.json(); setError(d.error); }
        } catch { setError('Error depositing'); }
    };

    const handleTransfer = async () => {
        try {
            const res = await fetchWithAuth(`/transfers`, {
                method: 'POST',
                body: JSON.stringify({ fromAccountId: user?.accountId, toAccountId: transferTo, amount: transferAmount, currency: 'USD' })
            });
            if (res.ok) { setTransferTo(''); setTransferAmount(0); fetchData(); setError(''); }
            else { const d = await res.json(); setError(d.error); }
        } catch { setError('Error transferring'); }
    };

    if (account?.isBlocked) return <h2>Your account has been blocked by an administrator.</h2>;
    if (!account) return <div>Loading account details...</div>;

    return (
        <div>
            <div className="card" style={{ marginBottom: '2rem' }}>
                <h2>My Account</h2>
                <p>Account ID: <code style={{ userSelect: 'all', background: '#222', padding: '0.2em 0.4em', borderRadius: '4px' }}>{account.id}</code></p>
                <div className="balance" style={{ fontSize: '3em' }}>{account.balance} {account.currency}</div>
                
                <div style={{ display: 'flex', gap: '1rem', marginTop: '2rem', justifyContent: 'center' }}>
                    <input type="number" value={amount} onChange={e => setAmount(Number(e.target.value))} placeholder="Amount to deposit" />
                    <button onClick={handleDeposit}>Deposit Cash</button>
                </div>
            </div>

            <div className="transfer-box" style={{ marginBottom: '2rem' }}>
                <h3>Transfer Funds</h3>
                <div className="input-group">
                    <input value={transferTo} onChange={e => setTransferTo(e.target.value)} placeholder="Recipient Account ID" style={{ width: '300px' }} />
                    <input type="number" value={transferAmount} onChange={e => setTransferAmount(Number(e.target.value))} placeholder="Amount" />
                    <button onClick={handleTransfer}>Send</button>
                </div>
                {error && <p className="error">{error}</p>}
            </div>

            <div className="card">
                <h3>Transaction History</h3>
                <table style={{ width: '100%', textAlign: 'left', borderCollapse: 'collapse' }}>
                    <thead>
                        <tr>
                            <th style={{ borderBottom: '1px solid #555', padding: '0.5rem' }}>Date</th>
                            <th style={{ borderBottom: '1px solid #555', padding: '0.5rem' }}>Type</th>
                            <th style={{ borderBottom: '1px solid #555', padding: '0.5rem' }}>Amount</th>
                            <th style={{ borderBottom: '1px solid #555', padding: '0.5rem' }}>Description</th>
                        </tr>
                    </thead>
                    <tbody>
                        {transactions.map(tx => (
                            <tr key={tx.id}>
                                <td style={{ padding: '0.5rem', borderBottom: '1px solid #333' }}>{new Date(tx.date).toLocaleString()}</td>
                                <td style={{ padding: '0.5rem', borderBottom: '1px solid #333' }}>
                                    {tx.type === 0 ? 'Deposit' : tx.type === 1 ? 'Withdrawal' : 'Transfer'}
                                </td>
                                <td style={{ padding: '0.5rem', borderBottom: '1px solid #333', color: tx.type === 1 || (tx.type === 2 && tx.description.includes('Transfer to')) ? '#ff4a4a' : '#4aff74' }}>
                                    {tx.amount.amount} {tx.amount.currency}
                                </td>
                                <td style={{ padding: '0.5rem', borderBottom: '1px solid #333' }}>{tx.description}</td>
                            </tr>
                        ))}
                        {transactions.length === 0 && <tr><td colSpan={4} style={{ textAlign: 'center', padding: '1rem' }}>No transactions yet.</td></tr>}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

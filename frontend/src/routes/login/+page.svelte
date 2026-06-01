<script lang="ts">
    import { api, setToken } from '$lib/api';
    import { goto } from '$app/navigation';

    let username = $state('');
    let password = $state('');
    let error = $state('');

    async function handleLogin() {
        error = '';

        const res = await api('/api/auth/login', {
            method: 'POST',
            body: JSON.stringify({ username, password })
        });

        if (!res.ok) {
            error = 'Invalid username or password';
            return;
        }

        const data = await res.json();
        setToken(data.token);
        goto('/dashboard');
    }
</script>

<h1>Login</h1>

{#if error}
    <p style="color: red;">{error}</p>
{/if}

<form onsubmit={handleLogin}>
    <label>
        Username
        <input bind:value={username} required />
    </label>
    <label>
        Password
        <input type="password" bind:value={password} required />
    </label>
    <button type="submit">Login</button>
</form>

<p>Don't have an account? <a href="/register">Register</a></p>
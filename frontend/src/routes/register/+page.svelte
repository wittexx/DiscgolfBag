<script lang="ts">
    import { api, setToken } from '$lib/api';
    import { goto } from '$app/navigation';

    let username = $state('');
    let email = $state('');
    let password = $state('');
    let displayName = $state('');
    let error = $state('');

    async function handleRegister() {
        error = '';

        const res = await api('/api/auth/register', {
            method: 'POST',
            body: JSON.stringify({ username, email, password, displayName })
        });

        if (!res.ok) {
            const data = await res.json();
            error = Array.isArray(data) 
                ? data.map((e: any) => e.description).join(', ') 
                : 'Registration failed';
            return;
        }

        // Automatically log in after successful registration
        const loginRes = await api('/api/auth/login', {
            method: 'POST',
            body: JSON.stringify({ username, password })
        });

        if (loginRes.ok) {
            const loginData = await loginRes.json();
            setToken(loginData.token);
            goto('/dashboard');
        } else {
            goto('/login');
        }
    }
</script>

<h1>Register</h1>

{#if error}
    <p style="color: red;">{error}</p>
{/if}

<form on:submit|preventDefault={handleRegister}>
    <label>
        Username
        <input bind:value={username} required />
    </label>
    <label>
        Display Name
        <input bind:value={displayName} required />
    </label>
    <label>
        Email
        <input type="email" bind:value={email} required />
    </label>
    <label>
        Password
        <input type="password" bind:value={password} required />
    </label>
    <button type="submit">Register</button>
</form>

<p>Already have an account? <a href="/login">Login</a></p>
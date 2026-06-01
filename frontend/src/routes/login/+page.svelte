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

<div class="max-w-md mx-auto">
	<h1 class="text-3xl font-bold text-cyan mb-6">Login</h1>

	{#if error}
		<p class="bg-red-500/20 text-red-400 p-3 rounded mb-4">{error}</p>
	{/if}

	<form onsubmit={handleLogin} class="space-y-4">
		<div>
			<label class="block text-sm text-silver mb-1">Username</label>
			<input bind:value={username} required class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
		</div>
		<div>
			<label class="block text-sm text-silver mb-1">Password</label>
			<input type="password" bind:value={password} required class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
		</div>
		<button type="submit" class="w-full bg-cyan text-dark font-medium py-3 rounded hover:bg-teal transition">Login</button>
	</form>

	<p class="mt-4 text-silver">Don't have an account? <a href="/register" class="text-cyan hover:text-teal">Register</a></p>
</div>

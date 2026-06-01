<script lang="ts">
    import { api, setToken } from '$lib/api';
    import { goto } from '$app/navigation';

    let username = $state('');
    let email = $state('');
    let password = $state('');
    let displayName = $state('');
    let error = $state('');

    async function handleRegister(e: Event) {
        e.preventDefault();
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

<div class="max-w-md mx-auto">
	<h1 class="text-3xl font-bold text-cyan mb-6">Register</h1>

	{#if error}
		<p class="bg-red-500/20 text-red-400 p-3 rounded mb-4">{error}</p>
	{/if}

	<form onsubmit={handleRegister} class="space-y-4">
		<div>
			<label class="block text-sm text-silver mb-1">Username</label>
			<input bind:value={username} required class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
		</div>
		<div>
			<label class="block text-sm text-silver mb-1">Display Name</label>
			<input bind:value={displayName} required class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
		</div>
		<div>
			<label class="block text-sm text-silver mb-1">Email</label>
			<input type="email" bind:value={email} required class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
		</div>
		<div>
			<label class="block text-sm text-silver mb-1">Password</label>
			<input type="password" bind:value={password} required class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
		</div>
		<button type="submit" class="w-full bg-cyan text-dark font-medium py-3 rounded hover:bg-teal transition">Register</button>
	</form>

	<p class="mt-4 text-silver">Already have an account? <a href="/login" class="text-cyan hover:text-teal">Login</a></p>
</div>

<script lang="ts">
	import '../app.css';
	import favicon from '$lib/assets/favicon.svg';
	import { getToken, removeToken, api } from '$lib/api';
	import { goto, afterNavigate } from '$app/navigation';

	let { children } = $props();
	let loggedIn = $state(false);
	let displayName = $state('');

	function checkAuth() {
		loggedIn = !!getToken();
		if (loggedIn && !displayName) {
			api('/api/auth/me').then(res => {
				if (res.ok) res.json().then(data => displayName = data.displayName);
			});
		}
		if (!loggedIn) displayName = '';
	}

	afterNavigate(() => {
		checkAuth();
	});

	function logout() {
		removeToken();
		loggedIn = false;
		goto('/login');
	}
</script>

<svelte:head>
	<link rel="icon" href={favicon} />
</svelte:head>

<div class="min-h-screen bg-dark text-silver">
	<nav class="flex items-center gap-6 px-6 py-4 bg-dark-light shadow-lg border-b border-teal/20">
		<a href="/" class="text-xl font-bold text-cyan">🥏 DiscGolf Bag</a>
		{#if loggedIn}
			<a href="/dashboard" class="hover:text-cyan transition">My Bag</a>
			<a href="/friends" class="hover:text-cyan transition">Friends</a>
			<span class="ml-auto font-medium text-teal">{displayName}</span>
			<button onclick={logout} class="bg-teal/20 hover:bg-teal/30 text-cyan px-3 py-1 rounded transition">Logout</button>
		{:else}
			<div class="ml-auto flex gap-4">
				<a href="/login" class="hover:text-cyan transition">Login</a>
				<a href="/register" class="bg-cyan text-dark px-3 py-1 rounded font-medium hover:bg-teal transition">Register</a>
			</div>
		{/if}
	</nav>

	<main class="max-w-4xl mx-auto px-4 py-8">
		{@render children()}
	</main>
</div>

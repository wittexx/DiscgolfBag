<script lang="ts">
	import { getToken, api } from '$lib/api';
	import { onMount } from 'svelte';

	let loggedIn = $state(false);
	let displayName = $state('');

	onMount(async () => {
		if (getToken()) {
			const res = await api('/api/auth/me');
			if (res.ok) {
				const data = await res.json();
				displayName = data.displayName;
				loggedIn = true;
			}
		}
	});
</script>

<div class="text-center py-20">
	<h1 class="text-5xl font-bold mb-4 text-cyan">The DiscGolf Bag</h1>
	<p class="text-xl text-silver mb-8">Track your discs, share your bag, connect with friends.</p>

	{#if loggedIn}
		<p class="text-2xl text-teal mb-6">Welcome back, <span class="text-cyan font-bold">{displayName}</span>!</p>
		<div class="flex justify-center gap-4">
			<a href="/dashboard" class="bg-cyan text-dark px-6 py-3 rounded-lg text-lg font-medium hover:bg-teal transition">Go to My Bag</a>
			<a href="/friends" class="border-2 border-cyan text-cyan px-6 py-3 rounded-lg text-lg font-medium hover:bg-cyan/10 transition">Friends</a>
		</div>
	{:else}
		<div class="flex justify-center gap-4">
			<a href="/register" class="bg-cyan text-dark px-6 py-3 rounded-lg text-lg font-medium hover:bg-teal transition">Get Started</a>
			<a href="/login" class="border-2 border-cyan text-cyan px-6 py-3 rounded-lg text-lg font-medium hover:bg-cyan/10 transition">Login</a>
		</div>
	{/if}

	<div class="grid grid-cols-1 md:grid-cols-3 gap-8 mt-16 text-left">
		<div class="p-6 rounded-lg bg-dark-light border border-teal/20">
			<h3 class="text-lg font-bold mb-2 text-cyan">Upload Your Discs</h3>
			<p class="text-silver">Add photos, flight numbers, and details for every disc in your bag.</p>
		</div>
		<div class="p-6 rounded-lg bg-dark-light border border-teal/20">
			<h3 class="text-lg font-bold mb-2 text-cyan">Connect With Friends</h3>
			<p class="text-silver">Add friends and check out what they're throwing.</p>
		</div>
		<div class="p-6 rounded-lg bg-dark-light border border-teal/20">
			<h3 class="text-lg font-bold mb-2 text-cyan">Manage Your Bag</h3>
			<p class="text-silver">Keep track of up to 45 discs in your bag.</p>
		</div>
	</div>
</div>

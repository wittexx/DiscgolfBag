<script lang="ts">
    import { api, getToken } from '$lib/api';
    import { goto } from '$app/navigation';
    import { page } from '$app/state';
    import { onMount } from 'svelte';

    let profile = $state<any>(null);
    let error = $state('');

    const username = page.params.username;

    onMount(async () => {
        if (!getToken()) { goto('/login'); return; }

        const res = await api(`/api/profiles/${username}/discs`);
        if (res.ok) {
            profile = await res.json();
        } else if (res.status === 403) {
            error = 'You need to be friends to view this bag.';
        } else {
            error = 'User not found.';
        }
    });
</script>

{#if error}
	<p class="bg-red-500/20 text-red-400 p-3 rounded">{error}</p>
{:else if profile}
	<h1 class="text-3xl font-bold text-cyan mb-6">{profile.displayName}'s Bag <span class="text-teal text-lg">({profile.discs.length} discs)</span></h1>

	<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
		{#each profile.discs as disc}
			<div class="bg-dark-light rounded-lg border border-teal/20 overflow-hidden">
				{#if disc.imageUrl}
					<img src="http://localhost:5167{disc.imageUrl}" alt={disc.name} class="w-full h-48 object-cover" />
				{:else}
					<div class="w-full h-48 bg-dark flex items-center justify-center text-4xl">🥏</div>
				{/if}
				<div class="p-4">
					<h3 class="text-lg font-bold text-cyan">{disc.name}</h3>
					<p class="text-silver text-sm">{disc.manufacturer} • {disc.plastic}</p>
					<div class="flex gap-3 mt-2 text-sm">
						<span class="bg-teal/20 text-teal px-2 py-1 rounded">S{disc.speed}</span>
						<span class="bg-teal/20 text-teal px-2 py-1 rounded">G{disc.glide}</span>
						<span class="bg-teal/20 text-teal px-2 py-1 rounded">T{disc.turn}</span>
						<span class="bg-teal/20 text-teal px-2 py-1 rounded">F{disc.fade}</span>
					</div>
				</div>
			</div>
		{/each}
	</div>
{:else}
	<p class="text-silver text-center py-10">Loading...</p>
{/if}

<a href="/friends" class="block mt-6 text-teal hover:text-cyan transition">← Back to friends</a>

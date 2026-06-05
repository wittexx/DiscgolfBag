<script lang="ts">
    import { api, getToken } from '$lib/api';
    import { goto } from '$app/navigation';
    import { onMount } from 'svelte';

    let discs = $state<any[]>([]);
    let stats = $state<any>(null);
    let error = $state('');

    onMount(async () => {
        if (!getToken()) {
            goto('/login');
            return;
        }
        await loadDiscs();
    });

    async function loadDiscs() {
        const [discsRes, statsRes] = await Promise.all([
            api('/api/discs'),
            api('/api/discs/stats')
        ]);
        if (discsRes.ok) discs = await discsRes.json();
        if (statsRes.ok) stats = await statsRes.json();
    }

    async function deleteDisc(id: number) {
        const res = await api(`/api/discs/${id}`, { method: 'DELETE' });
        if (res.ok) {
            await loadDiscs();
        }
    }
</script>

{#if stats && stats.totalDiscs > 0}
<div class="bg-dark-light rounded-lg border border-teal/20 p-5 mb-6">
	<h2 class="text-lg font-bold text-silver mb-3">Bag Analysis</h2>
	<div class="grid grid-cols-2 md:grid-cols-4 gap-3 mb-4 text-center">
		<div class="bg-dark rounded p-3">
			<p class="text-2xl font-bold text-cyan">{stats.putters}</p>
			<p class="text-xs text-silver">Putters</p>
		</div>
		<div class="bg-dark rounded p-3">
			<p class="text-2xl font-bold text-cyan">{stats.midranges}</p>
			<p class="text-xs text-silver">Midranges</p>
		</div>
		<div class="bg-dark rounded p-3">
			<p class="text-2xl font-bold text-cyan">{stats.fairwayDrivers}</p>
			<p class="text-xs text-silver">Fairways</p>
		</div>
		<div class="bg-dark rounded p-3">
			<p class="text-2xl font-bold text-cyan">{stats.distanceDrivers}</p>
			<p class="text-xs text-silver">Drivers</p>
		</div>
	</div>
	<div class="flex flex-wrap gap-4 text-sm mb-3">
		<span class="text-silver">Avg Speed: <span class="text-teal font-medium">{stats.averageSpeed}</span></span>
		<span class="text-silver">Avg Turn: <span class="text-teal font-medium">{stats.averageTurn}</span></span>
		<span class="text-silver">Avg Fade: <span class="text-teal font-medium">{stats.averageFade}</span></span>
		<span class="text-silver">Stability: <span class="text-cyan font-medium">{stats.stability}</span></span>
	</div>
	<div class="space-y-1">
		{#each stats.suggestions as tip}
			<p class="text-sm text-teal">💡 {tip}</p>
		{/each}
	</div>
</div>
{/if}

<div class="flex items-center justify-between mb-6">
	<h1 class="text-3xl font-bold text-cyan">My Bag <span class="text-teal text-lg">({discs.length} / 45)</span></h1>
	<a href="/discs/add" class="bg-cyan text-dark px-4 py-2 rounded font-medium hover:bg-teal transition">+ Add Disc</a>
</div>

{#if discs.length === 0}
	<p class="text-silver text-center py-10">No discs yet. Add your first disc!</p>
{:else}
	<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
		{#each discs as disc}
			<div class="bg-dark-light rounded-lg border border-teal/20 overflow-hidden">
				{#if disc.imageUrl}
					<img src="http://localhost:5167{disc.imageUrl}" alt={disc.name} class="w-full h-48 object-cover" />
				{:else}
					<div class="w-full h-48 bg-dark flex items-center justify-center text-4xl"></div>
				{/if}
				<div class="p-4">
					<h3 class="text-lg font-bold text-cyan">{disc.name}</h3>
					<p class="text-silver text-sm">{disc.manufacturer} • {disc.plastic}</p>
				{#if disc.description}
					<p class="text-silver text-sm mt-2">{disc.description}</p>
				{/if}
					<div class="flex gap-3 mt-2 text-sm">
						<span class="bg-teal/20 text-teal px-2 py-1 rounded">S{disc.speed}</span>
						<span class="bg-teal/20 text-teal px-2 py-1 rounded">G{disc.glide}</span>
						<span class="bg-teal/20 text-teal px-2 py-1 rounded">T{disc.turn}</span>
						<span class="bg-teal/20 text-teal px-2 py-1 rounded">F{disc.fade}</span>
					</div>
					<div class="flex justify-between items-center mt-3">
						<span class="text-silver text-sm">{disc.weight}g</span>
						<button onclick={() => deleteDisc(disc.id)} class="text-red-400 hover:text-red-300 text-sm transition">Delete</button>
					</div>
				</div>
			</div>
		{/each}
	</div>
{/if}

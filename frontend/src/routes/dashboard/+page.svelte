<script lang="ts">
    import { api, getToken } from '$lib/api';
    import { goto } from '$app/navigation';
    import { onMount } from 'svelte';

    let discs = $state<any[]>([]);
    let error = $state('');

    onMount(async () => {
        if (!getToken()) {
            goto('/login');
            return;
        }
        await loadDiscs();
    });

    async function loadDiscs() {
        const res = await api('/api/discs');
        if (res.ok) {
            discs = await res.json();
        }
    }

    async function deleteDisc(id: number) {
        const res = await api(`/api/discs/${id}`, { method: 'DELETE' });
        if (res.ok) {
            discs = discs.filter(d => d.id !== id);
        }
    }
</script>

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

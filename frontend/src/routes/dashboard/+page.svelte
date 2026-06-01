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

<h1>My Bag ({discs.length} / 45)</h1>
<a href="/discs/add">+ Add Disc</a>

{#if discs.length === 0}
    <p>No discs yet. Add your first disc!</p>
{:else}
    {#each discs as disc}
        <div style="border: 1px solid #ccc; padding: 10px; margin: 10px 0;">
            {#if disc.imageUrl}
<img src="http://localhost:5167{disc.imageUrl}" alt={disc.name} width="150" />
            {/if}
            <h3>{disc.name}</h3>
            <p>{disc.manufacturer} - {disc.plastic}</p>
            <p>Speed: {disc.speed} | Glide: {disc.glide} | Turn: {disc.turn} | Fade: {disc.fade}</p>
            <p>Type: {disc.type} | Weight: {disc.weight}g</p>
            <button onclick={() => deleteDisc(disc.id)}>Delete</button>
        </div>
    {/each}
{/if}
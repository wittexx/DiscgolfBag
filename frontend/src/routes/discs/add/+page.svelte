<script lang="ts">
    import { api, getToken } from '$lib/api';
    import { goto } from '$app/navigation';
    import { onMount } from 'svelte';

    let name = $state('');
    let manufacturer = $state('');
    let plastic = $state('');
    let type = $state('Putter');
    let speed = $state(0);
    let glide = $state(0);
    let turn = $state(0);
    let fade = $state(0);
    let weight = $state(0);
    let color = $state('');
    let image: File | null = $state(null);
    let error = $state('');

    onMount(() => {
        if (!getToken()) goto('/login');
    });

    function handleFileChange(e: Event) {
        const input = e.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            image = input.files[0];
        }
    }

    async function handleSubmit() {
        error = '';
        const formData = new FormData();
        formData.append('name', name);
        formData.append('manufacturer', manufacturer);
        formData.append('plastic', plastic);
        formData.append('type', type);
        formData.append('speed', speed.toString());
        formData.append('glide', glide.toString());
        formData.append('turn', turn.toString());
        formData.append('fade', fade.toString());
        formData.append('weight', weight.toString());
        formData.append('color', color);
        if (image) formData.append('image', image);

        const res = await api('/api/discs', {
            method: 'POST',
            body: formData
        });

        if (res.ok) {
            goto('/dashboard');
        } else {
            const data = await res.text();
            error = data || 'Failed to add disc';
        }
    }
</script>

<h1>Add Disc</h1>

{#if error}
    <p style="color: red;">{error}</p>
{/if}

<form onsubmit={handleSubmit}>
    <label>Name <input bind:value={name} required /></label>
    <label>Manufacturer <input bind:value={manufacturer} required /></label>
    <label>Plastic <input bind:value={plastic} /></label>
    <label>Type
        <select bind:value={type}>
            <option>Putter</option>
            <option>Midrange</option>
            <option>Fairway</option>
            <option>DistanceDriver</option>
        </select>
    </label>
    <label>Speed <input type="number" bind:value={speed} step="0.5" min="1" max="14" required /></label>
    <label>Glide <input type="number" bind:value={glide} step="0.5" min="1" max="7" required /></label>
    <label>Turn <input type="number" bind:value={turn} step="0.5" min="-5" max="1" required /></label>
    <label>Fade <input type="number" bind:value={fade} step="0.5" min="0" max="5" required /></label>
    <label>Weight (g) <input type="number" bind:value={weight} min="100" max="200" /></label>
    <label>Color <input bind:value={color} /></label>
    <label>Image <input type="file" accept="image/*" onchange={handleFileChange} /></label>
    <button type="submit">Add Disc</button>
</form>

<a href="/dashboard">Back to bag</a>
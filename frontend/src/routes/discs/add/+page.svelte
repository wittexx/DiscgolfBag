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
    let description = $state('');
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
        formData.append('description', description);
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

<div class="max-w-lg mx-auto">
	<h1 class="text-3xl font-bold text-cyan mb-6">Add Disc</h1>

	{#if error}
		<p class="bg-red-500/20 text-red-400 p-3 rounded mb-4">{error}</p>
	{/if}

	<form onsubmit={handleSubmit} class="space-y-4">
		<div>
			<label class="block text-sm text-silver mb-1">Name</label>
			<input bind:value={name} required class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
		</div>
		<div class="grid grid-cols-2 gap-4">
			<div>
				<label class="block text-sm text-silver mb-1">Manufacturer</label>
				<input bind:value={manufacturer} required class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
			</div>
			<div>
				<label class="block text-sm text-silver mb-1">Plastic</label>
				<input bind:value={plastic} class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
			</div>
		</div>
		<div>
			<label class="block text-sm text-silver mb-1">Type</label>
			<select bind:value={type} class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none">
				<option>Putter</option>
				<option>Midrange</option>
				<option>Fairway</option>
				<option>DistanceDriver</option>
			</select>
		</div>
		<div class="grid grid-cols-4 gap-3">
			<div>
				<label class="block text-sm text-silver mb-1">Speed</label>
				<input type="number" bind:value={speed} step="0.5" min="1" max="14" required class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
			</div>
			<div>
				<label class="block text-sm text-silver mb-1">Glide</label>
				<input type="number" bind:value={glide} step="0.5" min="1" max="7" required class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
			</div>
			<div>
				<label class="block text-sm text-silver mb-1">Turn</label>
				<input type="number" bind:value={turn} step="0.5" min="-5" max="1" required class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
			</div>
			<div>
				<label class="block text-sm text-silver mb-1">Fade</label>
				<input type="number" bind:value={fade} step="0.5" min="0" max="5" required class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
			</div>
		</div>
		<div class="grid grid-cols-2 gap-4">
			<div>
				<label class="block text-sm text-silver mb-1">Weight (g)</label>
				<input type="number" bind:value={weight} min="100" max="200" class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
			</div>
			<div>
				<label class="block text-sm text-silver mb-1">Color</label>
				<input bind:value={color} class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
			</div>
		</div>
		<div>
			<label class="block text-sm text-silver mb-1">Description</label>
			<textarea bind:value={description} rows="4" class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver focus:border-cyan focus:outline-none"></textarea>
		</div>
		<div>
			<label class="block text-sm text-silver mb-1">Image</label>
			<input type="file" accept="image/*" onchange={handleFileChange} class="w-full p-3 rounded bg-dark-light border border-teal/30 text-silver file:bg-teal/20 file:text-teal file:border-0 file:rounded file:px-3 file:py-1 file:mr-3" />
		</div>
		<button type="submit" class="w-full bg-cyan text-dark font-medium py-3 rounded hover:bg-teal transition">Add Disc</button>
	</form>

	<a href="/dashboard" class="block mt-4 text-teal hover:text-cyan transition">← Back to bag</a>
</div>

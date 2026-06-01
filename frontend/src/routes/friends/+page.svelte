<script lang="ts">
    import { api, getToken } from '$lib/api';
    import { goto } from '$app/navigation';
    import { onMount } from 'svelte';

    let friends = $state<any[]>([]);
    let requests = $state<any[]>([]);
    let addUsername = $state('');
    let message = $state('');
    let error = $state('');

    onMount(async () => {
        if (!getToken()) { goto('/login'); return; }
        await loadData();
    });

    async function loadData() {
        const [friendsRes, requestsRes] = await Promise.all([
            api('/api/friends'),
            api('/api/friends/requests')
        ]);
        if (friendsRes.ok) friends = await friendsRes.json();
        if (requestsRes.ok) requests = await requestsRes.json();
    }

    async function sendRequest(e: Event) {
        e.preventDefault();
        error = '';
        message = '';
        const res = await api(`/api/friends/request/${addUsername}`, { method: 'POST' });
        if (res.ok) {
            message = `Friend request sent to ${addUsername}!`;
            addUsername = '';
        } else {
            const data = await res.text();
            error = data || 'Failed to send request';
        }
    }

    async function acceptRequest(id: number) {
        const res = await api(`/api/friends/request/${id}/accept`, { method: 'PUT' });
        if (res.ok) await loadData();
    }

    async function declineRequest(id: number) {
        const res = await api(`/api/friends/request/${id}/decline`, { method: 'PUT' });
        if (res.ok) await loadData();
    }
</script>

<h1 class="text-3xl font-bold text-cyan mb-6">Friends</h1>

<div class="bg-dark-light rounded-lg border border-teal/20 p-6 mb-6">
	<h2 class="text-lg font-bold text-silver mb-3">Add Friend</h2>
	<form onsubmit={sendRequest} class="flex gap-3">
		<input bind:value={addUsername} placeholder="Enter their username" required class="flex-1 p-3 rounded bg-dark border border-teal/30 text-silver focus:border-cyan focus:outline-none" />
		<button type="submit" class="bg-cyan text-dark px-4 py-2 rounded font-medium hover:bg-teal transition">Send Request</button>
	</form>
	{#if message}<p class="text-teal mt-2">{message}</p>{/if}
	{#if error}<p class="text-red-400 mt-2">{error}</p>{/if}
</div>

{#if requests.length > 0}
	<div class="mb-6">
		<h2 class="text-lg font-bold text-silver mb-3">Pending Requests ({requests.length})</h2>
		{#each requests as req}
			<div class="flex items-center justify-between bg-dark-light rounded-lg border border-teal/20 p-4 mb-2">
				<span class="text-silver">{req.displayName} <span class="text-teal">@{req.username}</span></span>
				<div class="flex gap-2">
					<button onclick={() => acceptRequest(req.id)} class="bg-cyan text-dark px-3 py-1 rounded text-sm font-medium hover:bg-teal transition">Accept</button>
					<button onclick={() => declineRequest(req.id)} class="bg-red-500/20 text-red-400 px-3 py-1 rounded text-sm hover:bg-red-500/30 transition">Decline</button>
				</div>
			</div>
		{/each}
	</div>
{/if}

<h2 class="text-lg font-bold text-silver mb-3">My Friends ({friends.length})</h2>
{#if friends.length === 0}
	<p class="text-silver">No friends yet. Add someone above!</p>
{:else}
	{#each friends as friend}
		<div class="flex items-center justify-between bg-dark-light rounded-lg border border-teal/20 p-4 mb-2">
			<span class="text-silver">{friend.displayName} <span class="text-teal">@{friend.username}</span></span>
			<a href="/profile/{friend.username}" class="text-cyan hover:text-teal transition">View Bag →</a>
		</div>
	{/each}
{/if}

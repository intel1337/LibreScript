<script>
// Ce composant doit être utilisé à l'intérieur d'un <tbody> d'une table globale

import { goto } from '$app/navigation';

export let id;
export let title;
export let language;
export let languageIcon;
export let status;
export let votes;
export let replies = 0;
export let userId;
export let author = null;
export let currentUser = null;

function navigateToPost() {
    goto(`/post/${id}`);
}
</script>

<tr class="post-row" on:click={navigateToPost}>
    <td class="title-cell">{title}</td>
    <td class="author-cell">{author?.username || 'Unknown'}</td>
    <td><span class="lang-icon">{languageIcon}</span> {language}</td>
    <td><span class="status {status === 'Solved' ? 'solved' : 'needhelp'}">{status}</span></td>
    <td>{votes}</td>
    <td>{replies}</td>
</tr>

<style>
.post-row {
    border-bottom: 1px solid #f0f0f0;
    cursor: pointer;
    transition: background-color 0.2s ease;
}

.post-row:hover {
    background-color: #f8f9fa;
}

td {
    font-size: 1.01rem;
    color: #222;
    padding: 12px 10px;
    text-align: left;
}

.title-cell {
    font-weight: 500;
    color: #0969da;
}

.title-cell:hover {
    text-decoration: underline;
}

.author-cell {
    color: #0969da;
    font-weight: 500;
}

.lang-icon {
    margin-right: 6px;
}
.status.solved {
    background: #e6f7ec;
    color: #1a7f37;
    border-radius: 8px;
    padding: 4px 12px;
    font-weight: 600;
    font-size: 0.95rem;
}
.status.needhelp {
    background: #fff3e6;
    color: #b26a00;
    border-radius: 8px;
    padding: 4px 12px;
    font-weight: 600;
    font-size: 0.95rem;
}


</style>
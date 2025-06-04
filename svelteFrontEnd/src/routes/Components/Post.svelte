<script>
// Ce composant doit être utilisé à l'intérieur d'un <tbody> d'une table globale
// Exemple d'utilisation :
// <table><thead>...</thead><tbody>{#each posts as post}<Post {...post} /></tbody></table>
export let title;
export let language;
export let languageIcon;
export let status;
export let votes;
export let answers;
export let views;
</script>

<tr>
    <td>{title}</td>
    <td><span class="lang-icon">{languageIcon}</span> {language}</td>
    <td><span class="status {status === 'Solved' ? 'solved' : 'needhelp'}">{status}</span></td>
    <td>{votes}</td>
    <td>{answers}</td>
    <td>{views}</td>
</tr>

<style>
tr {
    border-bottom: 1px solid #f0f0f0;
}
td {
    font-size: 1.01rem;
    color: #222;
    padding: 12px 10px;
    text-align: left;
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
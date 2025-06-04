export async function getPosts() {
    const data = await fetch('http://localhost:5028/api/post'); 
    if (!data.ok) throw new Error('API error: ' + data.status);
    return await data.json();
}
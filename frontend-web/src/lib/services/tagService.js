export async function getTags() {
      const data = await fetch('http://192.168.10.106:5028/api/category'); // minuscule
  if (!data.ok) throw new Error('API error: ' + data.status);
  return await data.json();
}
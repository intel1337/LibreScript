import { API_BASE_URL } from '$lib/config.js';

export async function getTags() {
      const data = await fetch(`${API_BASE_URL}/api/category`); // minuscule
  if (!data.ok) throw new Error('API error: ' + data.status);
  return await data.json();
}
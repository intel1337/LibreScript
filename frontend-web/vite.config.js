import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig } from 'vite';

export default defineConfig({
	plugins: [sveltekit()],
	server: {
		host: true,           // écoute sur 0.0.0.0
		port: 5173,           // ou un autre port si tu veux
	  }
});
